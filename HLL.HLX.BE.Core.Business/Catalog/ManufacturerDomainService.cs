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
    public  class ManufacturerDomainService : DomainService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : manufacturer ID
        /// </remarks>
        private const string MANUFACTURERS_BY_ID_KEY = "Hlx.manufacturer.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : manufacturer ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current customer ID
        /// {5} : store ID
        /// </remarks>
        private const string PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY = "Hlx.productmanufacturer.allbymanufacturerid-{0}-{1}-{2}-{3}-{4}-{5}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : product ID
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        private const string PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY = "Hlx.productmanufacturer.allbyproductid-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string MANUFACTURERS_PATTERN_KEY = "Hlx.manufacturer.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTMANUFACTURERS_PATTERN_KEY = "Hlx.productmanufacturer.";

        private const string CACHE_NAME_MANUFACTURERS = "cache.manufacturer";
        private const string CACHE_NAME_PRODUCTMANUFACTURERS = "cache.productmanufacturer";

        #endregion

        #region Fields

        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IProductManufacturerRepository _productManufacturerRepository;
        private readonly IProductRepository _productRepository;
        //private readonly IRepository<AclRecord> _aclRepository;
        private readonly IStoreMappingRepository _storeMappingRepository;
        //private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        //private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly SettingDomainService _settingDomainService;


        private  CatalogSettings _catalogSettings1;
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
        /// <param name="manufacturerRepository">Category repository</param>
        /// <param name="productManufacturerRepository">ProductCategory repository</param>
        /// <param name="productRepository">Product repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="eventPublisher">Event published</param>
        public ManufacturerDomainService(ICacheManager cacheManager,
            IManufacturerRepository manufacturerRepository,
            IProductManufacturerRepository productManufacturerRepository,
            IProductRepository productRepository,
            //IRepository<AclRecord> aclRepository,
            IStoreMappingRepository storeMappingRepository,
            //IWorkContext workContext,
            IStoreContext storeContext
            //,CatalogSettings catalogSettings
           // ,IEventPublisher eventPublisher
           , SettingDomainService settingDomainService
            )
        {
            this._cacheManager = cacheManager;
            this._manufacturerRepository = manufacturerRepository;
            this._productManufacturerRepository = productManufacturerRepository;
            this._productRepository = productRepository;
            //this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            //this._workContext = workContext;
            this._storeContext = storeContext;
            //this._catalogSettings = catalogSettings;
            //this._eventPublisher = eventPublisher;
            _settingDomainService = settingDomainService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Deletes a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public virtual void DeleteManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");

            manufacturer.Deleted = true;
            UpdateManufacturer(manufacturer);
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="manufacturerName">Manufacturer name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturers</returns>
        public virtual IPagedList<Manufacturer> GetAllManufacturers(string manufacturerName = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            var query = _manufacturerRepository.GetAll();
            if (!showHidden)
                query = query.Where(m => m.Published);
            if (!String.IsNullOrWhiteSpace(manufacturerName))
                query = query.Where(m => m.Name.Contains(manufacturerName));
            query = query.Where(m => !m.Deleted);
            query = query.OrderBy(m => m.DisplayOrder);

            if (!showHidden && (!CatalogSettings.IgnoreAcl || !CatalogSettings.IgnoreStoreLimitations))
            {
                //if (!CatalogSettings.IgnoreAcl)
                //{
                //    //ACL (access control list)
                //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                //    query = from m in query
                //            join acl in _aclRepository.Table
                //            on new { c1 = m.Id, c2 = "Manufacturer" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into m_acl
                //            from acl in m_acl.DefaultIfEmpty()
                //            where !m.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                //            select m;
                //}
                if (!CatalogSettings.IgnoreStoreLimitations)
                {
                    //Store mapping
                    var currentStoreId = _storeContext.CurrentStore.Id;
                    query = from m in query
                            join sm in _storeMappingRepository.GetAll()
                            on new { c1 = m.Id, c2 = "Manufacturer" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into m_sm
                            from sm in m_sm.DefaultIfEmpty()
                            where !m.LimitedToStores || currentStoreId == sm.StoreId
                            select m;
                }
                //only distinct manufacturers (group by ID)
                query = from m in query
                        group m by m.Id
                            into mGroup
                        orderby mGroup.Key
                        select mGroup.FirstOrDefault();
                query = query.OrderBy(m => m.DisplayOrder);
            }

            return new PagedList<Manufacturer>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>Manufacturer</returns>
        public virtual Manufacturer GetManufacturerById(int manufacturerId)
        {
            if (manufacturerId == 0)
                return null;

            string key = string.Format(MANUFACTURERS_BY_ID_KEY, manufacturerId);
            return _cacheManager.GetCache(CACHE_NAME_MANUFACTURERS).Get(key, () => _manufacturerRepository.FirstOrDefault(manufacturerId));
        }

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public virtual void InsertManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");

            _manufacturerRepository.Insert(manufacturer);

            //cache
            _cacheManager.GetCache(CACHE_NAME_MANUFACTURERS).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTMANUFACTURERS).Clear();

            //event notification
            //_eventPublisher.EntityInserted(manufacturer);
        }

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public virtual void UpdateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");

            _manufacturerRepository.Update(manufacturer);

            //cache
            _cacheManager.GetCache(CACHE_NAME_MANUFACTURERS).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTMANUFACTURERS).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(manufacturer);
        }


        /// <summary>
        /// Deletes a product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturer">Product manufacturer mapping</param>
        public virtual void DeleteProductManufacturer(ProductManufacturer productManufacturer)
        {
            if (productManufacturer == null)
                throw new ArgumentNullException("productManufacturer");

            _productManufacturerRepository.Delete(productManufacturer);

            //cache
            _cacheManager.GetCache(CACHE_NAME_MANUFACTURERS).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTMANUFACTURERS).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(productManufacturer);
        }

        /// <summary>
        /// Gets product manufacturer collection
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer collection</returns>
        public virtual IPagedList<ProductManufacturer> GetProductManufacturersByManufacturerId(int manufacturerId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (manufacturerId == 0)
                return new PagedList<ProductManufacturer>(new List<ProductManufacturer>(), pageIndex, pageSize);

            //string key = string.Format(PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY, showHidden, manufacturerId, pageIndex, pageSize, _workContext.CurrentCustomer.Id, _storeContext.CurrentStore.Id);
            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY, showHidden, manufacturerId, pageIndex, pageSize, manufacturerId, _storeContext.CurrentStore.Id);
            return _cacheManager.GetCache(CACHE_NAME_PRODUCTMANUFACTURERS).Get(key, () =>
            {
                var query = from pm in _productManufacturerRepository.GetAll()
                            join p in _productRepository.GetAll() on pm.ProductId equals p.Id
                            where pm.ManufacturerId == manufacturerId &&
                                  !p.Deleted &&
                                  (showHidden || p.Published)
                            orderby pm.DisplayOrder
                            select pm;

                if (!showHidden && (!CatalogSettings.IgnoreAcl || !CatalogSettings.IgnoreStoreLimitations))
                {
                    //if (!_catalogSettings.IgnoreAcl)
                    //{
                    //    //ACL (access control list)
                    //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                    //    query = from pm in query
                    //            join m in _manufacturerRepository.Table on pm.ManufacturerId equals m.Id
                    //            join acl in _aclRepository.Table
                    //            on new { c1 = m.Id, c2 = "Manufacturer" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into m_acl
                    //            from acl in m_acl.DefaultIfEmpty()
                    //            where !m.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                    //            select pm;
                    //}
                    if (!CatalogSettings.IgnoreStoreLimitations)
                    {
                        //Store mapping
                        var currentStoreId = _storeContext.CurrentStore.Id;
                        query = from pm in query
                                join m in _manufacturerRepository.GetAll() on pm.ManufacturerId equals m.Id
                                join sm in _storeMappingRepository.GetAll()
                                on new { c1 = m.Id, c2 = "Manufacturer" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into m_sm
                                from sm in m_sm.DefaultIfEmpty()
                                where !m.LimitedToStores || currentStoreId == sm.StoreId
                                select pm;
                    }

                    //only distinct manufacturers (group by ID)
                    query = from pm in query
                            group pm by pm.Id
                            into pmGroup
                            orderby pmGroup.Key
                            select pmGroup.FirstOrDefault();
                    query = query.OrderBy(pm => pm.DisplayOrder);
                }

                var productManufacturers = new PagedList<ProductManufacturer>(query, pageIndex, pageSize);
                return productManufacturers;
            });
        }

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public virtual IList<ProductManufacturer> GetProductManufacturersByProductId(int productId, bool showHidden = false)
        {
            if (productId == 0)
                return new List<ProductManufacturer>();

            //string key = string.Format(PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY, showHidden, productId, _workContext.CurrentCustomer.Id, _storeContext.CurrentStore.Id);
            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY, showHidden, productId, productId, _storeContext.CurrentStore.Id);
            return _cacheManager.GetCache(CACHE_NAME_PRODUCTMANUFACTURERS).Get(key, () =>
            {
                var query = from pm in _productManufacturerRepository.GetAll()
                            join m in _manufacturerRepository.GetAll() on pm.ManufacturerId equals m.Id
                            where pm.ProductId == productId &&
                                !m.Deleted &&
                                (showHidden || m.Published)
                            orderby pm.DisplayOrder
                            select pm;


                if (!showHidden && (!CatalogSettings.IgnoreAcl || !CatalogSettings.IgnoreStoreLimitations))
                {
                    //if (!_catalogSettings.IgnoreAcl)
                    //{
                    //    //ACL (access control list)
                    //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                    //    query = from pm in query
                    //            join m in _manufacturerRepository.Table on pm.ManufacturerId equals m.Id
                    //            join acl in _aclRepository.Table
                    //            on new { c1 = m.Id, c2 = "Manufacturer" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into m_acl
                    //            from acl in m_acl.DefaultIfEmpty()
                    //            where !m.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                    //            select pm;
                    //}

                    if (!CatalogSettings.IgnoreStoreLimitations)
                    {
                        //Store mapping
                        var currentStoreId = _storeContext.CurrentStore.Id;
                        query = from pm in query
                                join m in _manufacturerRepository.GetAll() on pm.ManufacturerId equals m.Id
                                join sm in _storeMappingRepository.GetAll()
                                on new { c1 = m.Id, c2 = "Manufacturer" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into m_sm
                                from sm in m_sm.DefaultIfEmpty()
                                where !m.LimitedToStores || currentStoreId == sm.StoreId
                                select pm;
                    }

                    //only distinct manufacturers (group by ID)
                    query = from pm in query
                            group pm by pm.Id
                            into mGroup
                            orderby mGroup.Key
                            select mGroup.FirstOrDefault();
                    query = query.OrderBy(pm => pm.DisplayOrder);
                }

                var productManufacturers = query.ToList();
                return productManufacturers;
            });
        }

        /// <summary>
        /// Gets a product manufacturer mapping 
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifier</param>
        /// <returns>Product manufacturer mapping</returns>
        public virtual ProductManufacturer GetProductManufacturerById(int productManufacturerId)
        {
            if (productManufacturerId == 0)
                return null;

            return _productManufacturerRepository.FirstOrDefault(productManufacturerId);
        }

        /// <summary>
        /// Inserts a product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturer">Product manufacturer mapping</param>
        public virtual void InsertProductManufacturer(ProductManufacturer productManufacturer)
        {
            if (productManufacturer == null)
                throw new ArgumentNullException("productManufacturer");

            _productManufacturerRepository.Insert(productManufacturer);

            //cache
            _cacheManager.GetCache(CACHE_NAME_MANUFACTURERS).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTMANUFACTURERS).Clear();

            //event notification
            //_eventPublisher.EntityInserted(productManufacturer);
        }

        /// <summary>
        /// Updates the product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturer">Product manufacturer mapping</param>
        public virtual void UpdateProductManufacturer(ProductManufacturer productManufacturer)
        {
            if (productManufacturer == null)
                throw new ArgumentNullException("productManufacturer");

            _productManufacturerRepository.Update(productManufacturer);

            //cache
            _cacheManager.GetCache(CACHE_NAME_MANUFACTURERS).Clear();
            _cacheManager.GetCache(CACHE_NAME_PRODUCTMANUFACTURERS).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(productManufacturer);
        }

        #endregion
    }
}
