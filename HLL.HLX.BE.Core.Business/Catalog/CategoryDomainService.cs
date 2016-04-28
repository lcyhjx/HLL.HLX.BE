using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.Core.Business.Catalog
{
    public class CategoryDomainService : DomainService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : category ID
        /// </remarks>
        private const string CATEGORIES_BY_ID_KEY = "Hlx.category.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// {1} : show hidden records?
        /// {2} : current customer ID
        /// {3} : store ID
        /// {3} : include all levels (child)
        /// </remarks>
        private const string CATEGORIES_BY_PARENT_CATEGORY_ID_KEY = "Hlx.category.byparent-{0}-{1}-{2}-{3}-{4}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : category ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current customer ID
        /// {5} : store ID
        /// </remarks>
        private const string PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY = "Hlx.productcategory.allbycategoryid-{0}-{1}-{2}-{3}-{4}-{5}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : product ID
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        private const string PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY = "Hlx.productcategory.allbyproductid-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CATEGORIES_PATTERN_KEY = "Hlx.category.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTCATEGORIES_PATTERN_KEY = "Hlx.productcategory.";

        private const string CACHE_NAME_CATEGORIES = "cache.category";
        private const string CACHE_NAME_PRODUCTCATEGORIES = "cache.productcategory";

        #endregion

        #region Fields

        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        //private readonly IRepository<AclRecord> _aclRepository;
        private readonly IStoreMappingRepository _storeMappingRepository;
        //private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        //private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly StoreMappingDomainService _storeMappingService;
        //private readonly IAclService _aclService;
        private readonly SettingDomainService _settingDomainService;

        private CatalogSettings _catalogSettings1;
        public CatalogSettings CatalogSettings
        {
            get
            {
                if (_catalogSettings1 == null)
                {
                    _catalogSettings1 = _settingDomainService.LoadSetting<CatalogSettings>(_storeContext.CurrentStore.Id);
                }
                return _catalogSettings1;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="categoryRepository">Category repository</param>
        /// <param name="productCategoryRepository">ProductCategory repository</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="catalogSettings">Catalog settings</param>
        public CategoryDomainService(ICacheManager cacheManager,
            ICategoryRepository categoryRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository,
            //IRepository<AclRecord> aclRepository,
            IStoreMappingRepository storeMappingRepository,
            //IWorkContext workContext,
            IStoreContext storeContext,
            //IEventPublisher eventPublisher,
            StoreMappingDomainService storeMappingService,
            //IAclService aclService,
            SettingDomainService settingDomainService)
        {
            this._cacheManager = cacheManager;
            this._categoryRepository = categoryRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._productRepository = productRepository;
            //this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            //this._workContext = workContext;
            this._storeContext = storeContext;
            //this._eventPublisher = eventPublisher;
            this._storeMappingService = storeMappingService;
            //this._aclService = aclService;
            this._settingDomainService = settingDomainService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void DeleteCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            category.Deleted = true;
            UpdateCategory(category);

            //reset a "Parent category" property of all child subcategories
            var subcategories = GetAllCategoriesByParentCategoryId(category.Id, true);
            foreach (var subcategory in subcategories)
            {
                subcategory.ParentCategoryId = 0;
                UpdateCategory(subcategory);
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IPagedList<Category> GetAllCategories(string categoryName = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _categoryRepository.GetAll();
            if (!showHidden)
                query = query.Where(c => c.Published);
            if (!String.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder);

            if (!showHidden && (!CatalogSettings.IgnoreAcl || !CatalogSettings.IgnoreStoreLimitations))
            {
                //if (!CatalogSettings.IgnoreAcl)
                //{
                //    //ACL (access control list)
                //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                //    query = from c in query
                //            join acl in _aclRepository.Table
                //            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
                //            from acl in c_acl.DefaultIfEmpty()
                //            where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                //            select c;
                //}
                if (!CatalogSettings.IgnoreStoreLimitations)
                {
                    //Store mapping
                    var currentStoreId = _storeContext.CurrentStore.Id;
                    query = from c in query
                            join sm in _storeMappingRepository.GetAll()
                            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                            from sm in c_sm.DefaultIfEmpty()
                            where !c.LimitedToStores || currentStoreId == sm.StoreId
                            select c;
                }

                //only distinct categories (group by ID)
                query = from c in query
                        group c by c.Id
                        into cGroup
                        orderby cGroup.Key
                        select cGroup.FirstOrDefault();
                query = query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder);
            }

            var unsortedCategories = query.ToList();

            //sort categories
            var sortedCategories = unsortedCategories.SortCategoriesForTree();

            //paging
            return new PagedList<Category>(sortedCategories, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="includeAllLevels">A value indicating whether we should load all child levels</param>
        /// <returns>Categories</returns>
        public virtual IList<Category> GetAllCategoriesByParentCategoryId(int parentCategoryId, //int currentCustomerId,
            bool showHidden = false, bool includeAllLevels = false)
        {
            //string key = string.Format(CATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentCategoryId, showHidden, currentCustomerId, _storeContext.CurrentStore.Id, includeAllLevels);
            string key = string.Format(CATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentCategoryId, showHidden, parentCategoryId, _storeContext.CurrentStore.Id, includeAllLevels);
            return _cacheManager.GetCache(CACHE_NAME_CATEGORIES).Get(key, () =>
            {
                var query = _categoryRepository.GetAll();
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.Where(c => c.ParentCategoryId == parentCategoryId);
                query = query.Where(c => !c.Deleted);
                query = query.OrderBy(c => c.DisplayOrder);

                if (!showHidden && (!CatalogSettings.IgnoreAcl || !CatalogSettings.IgnoreStoreLimitations))
                {
                    //if (!CatalogSettings.IgnoreAcl)
                    //{
                    //    //ACL (access control list)
                    //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                    //    query = from c in query
                    //            join acl in _aclRepository.Table
                    //            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
                    //            from acl in c_acl.DefaultIfEmpty()
                    //            where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                    //            select c;
                    //}
                    if (!CatalogSettings.IgnoreStoreLimitations)
                    {
                        //Store mapping
                        var currentStoreId = _storeContext.CurrentStore.Id;
                        query = from c in query
                                join sm in _storeMappingRepository.GetAll()
                                on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                                from sm in c_sm.DefaultIfEmpty()
                                where !c.LimitedToStores || currentStoreId == sm.StoreId
                                select c;
                    }
                    //only distinct categories (group by ID)
                    query = from c in query
                            group c by c.Id
                            into cGroup
                            orderby cGroup.Key
                            select cGroup.FirstOrDefault();
                    query = query.OrderBy(c => c.DisplayOrder);
                }

                var categories = query.ToList();
                if (includeAllLevels)
                {
                    var childCategories = new List<Category>();
                    //add child levels
                    foreach (var category in categories)
                    {
                        childCategories.AddRange(GetAllCategoriesByParentCategoryId(category.Id,showHidden, includeAllLevels));
                    }
                    categories.AddRange(childCategories);
                }
                return categories;
            });
        }

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IList<Category> GetAllCategoriesDisplayedOnHomePage(bool showHidden = false)
        {
            var query = from c in _categoryRepository.GetAll()
                        orderby c.DisplayOrder
                        where c.Published &&
                        !c.Deleted &&
                        c.ShowOnHomePage
                        select c;

            var categories = query.ToList();
            //if (!showHidden)
            //{
            //    categories = categories
            //        .Where(c => _aclService.Authorize(c) && _storeMappingService.Authorize(c))
            //        .ToList();
            //}

            return categories;
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        public virtual Category GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return null;

            string key = string.Format(CATEGORIES_BY_ID_KEY, categoryId);
            return _cacheManager.GetCache(CACHE_NAME_CATEGORIES).Get(key, () => _categoryRepository.FirstOrDefault(categoryId));
        }

        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void InsertCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            _categoryRepository.Insert(category);

            //cache
            _cacheManager.GetCache(CACHE_NAME_CATEGORIES).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTCATEGORIES).Clear();

            //event notification
            //_eventPublisher.EntityInserted(category);
        }

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            //validate category hierarchy
            var parentCategory = GetCategoryById(category.ParentCategoryId);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentCategoryId = 0;
                    break;
                }
                parentCategory = GetCategoryById(parentCategory.ParentCategoryId);
            }

            _categoryRepository.Update(category);

            //cache
            _cacheManager.GetCache(CACHE_NAME_CATEGORIES).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTCATEGORIES).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(category);
        }


        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="productCategory">Product category</param>
        public virtual void DeleteProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Delete(productCategory);

            //cache
            _cacheManager.GetCache(CACHE_NAME_CATEGORIES).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTCATEGORIES).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(productCategory);
        }

        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product a category mapping collection</returns>
        public virtual IPagedList<ProductCategory> GetProductCategoriesByCategoryId(int categoryId,int currentCustomerId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (categoryId == 0)
                return new PagedList<ProductCategory>(new List<ProductCategory>(), pageIndex, pageSize);

            string key = string.Format(PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY, showHidden, categoryId, pageIndex, pageSize, currentCustomerId, _storeContext.CurrentStore.Id);
            return _cacheManager.GetCache(CACHE_NAME_PRODUCTCATEGORIES).Get(key, () =>
            {
                var query = from pc in _productCategoryRepository.GetAll()
                            join p in _productRepository.GetAll() on pc.ProductId equals p.Id
                            where pc.CategoryId == categoryId &&
                                  !p.Deleted &&
                                  (showHidden || p.Published)
                            orderby pc.DisplayOrder
                            select pc;

                if (!showHidden && (!CatalogSettings.IgnoreAcl || !CatalogSettings.IgnoreStoreLimitations))
                {
                    //if (!_catalogSettings.IgnoreAcl)
                    //{
                    //    //ACL (access control list)
                    //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                    //    query = from pc in query
                    //            join c in _categoryRepository.Table on pc.CategoryId equals c.Id
                    //            join acl in _aclRepository.Table
                    //            on new { c1 = c.Id, c2 = "Category" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
                    //            from acl in c_acl.DefaultIfEmpty()
                    //            where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                    //            select pc;
                    //}
                    if (!CatalogSettings.IgnoreStoreLimitations)
                    {
                        //Store mapping
                        var currentStoreId = _storeContext.CurrentStore.Id;
                        query = from pc in query
                                join c in _categoryRepository.GetAll() on pc.CategoryId equals c.Id
                                join sm in _storeMappingRepository.GetAll()
                                on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                                from sm in c_sm.DefaultIfEmpty()
                                where !c.LimitedToStores || currentStoreId == sm.StoreId
                                select pc;
                    }
                    //only distinct categories (group by ID)
                    query = from c in query
                            group c by c.Id
                            into cGroup
                            orderby cGroup.Key
                            select cGroup.FirstOrDefault();
                    query = query.OrderBy(pc => pc.DisplayOrder);
                }

                var productCategories = new PagedList<ProductCategory>(query, pageIndex, pageSize);
                return productCategories;
            });
        }

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Product category mapping collection</returns>
        public virtual IList<ProductCategory> GetProductCategoriesByProductId(int productId, bool showHidden = false)
        {
            return GetProductCategoriesByProductId(productId, _storeContext.CurrentStore.Id, showHidden);
        }
        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="storeId">Store identifier (used in multi-store environment). "showHidden" parameter should also be "true"</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Product category mapping collection</returns>
        public virtual IList<ProductCategory> GetProductCategoriesByProductId(int productId, int storeId//,int currentCustomerId
            , bool showHidden = false)
        {
            if (productId == 0)
                return new List<ProductCategory>();

            //string key = string.Format(PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY, showHidden, productId, currentCustomerId, storeId);
            string key = string.Format(PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY, showHidden, productId, productId, storeId);
            return _cacheManager.GetCache(CACHE_NAME_PRODUCTCATEGORIES).Get(key, () =>
            {
                var query = from pc in _productCategoryRepository.GetAll()
                            join c in _categoryRepository.GetAll() on pc.CategoryId equals c.Id
                            where pc.ProductId == productId &&
                                  !c.Deleted &&
                                  (showHidden || c.Published)
                            orderby pc.DisplayOrder
                            select pc;

                var allProductCategories = query.ToList();
                var result = new List<ProductCategory>();
                //if (!showHidden)
                //{
                //    foreach (var pc in allProductCategories)
                //    {
                //        //ACL (access control list) and store mapping
                //        var category = pc.Category;
                //        if (_aclService.Authorize(category) && _storeMappingService.Authorize(category, storeId))
                //            result.Add(pc);
                //    }
                //}
                //else
                {
                    //no filtering
                    result.AddRange(allProductCategories);
                }
                return result;
            });
        }

        /// <summary>
        /// Gets a product category mapping 
        /// </summary>
        /// <param name="productCategoryId">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public virtual ProductCategory GetProductCategoryById(int productCategoryId)
        {
            if (productCategoryId == 0)
                return null;

            return _productCategoryRepository.FirstOrDefault(productCategoryId);
        }

        /// <summary>
        /// Inserts a product category mapping
        /// </summary>
        /// <param name="productCategory">>Product category mapping</param>
        public virtual void InsertProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Insert(productCategory);

            //cache
            _cacheManager.GetCache(CACHE_NAME_CATEGORIES).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTCATEGORIES).Clear();

            //event notification
            //_eventPublisher.EntityInserted(productCategory);
        }

        /// <summary>
        /// Updates the product category mapping 
        /// </summary>
        /// <param name="productCategory">>Product category mapping</param>
        public virtual void UpdateProductCategory(ProductCategory productCategory)
        {
            if (productCategory == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryRepository.Update(productCategory);

            //cache
            _cacheManager.GetCache(CACHE_NAME_CATEGORIES).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTCATEGORIES).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(productCategory);
        }

        #endregion
    }
}
