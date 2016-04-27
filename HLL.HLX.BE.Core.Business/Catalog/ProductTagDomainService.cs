using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Common;

namespace HLL.HLX.BE.Core.Business.Catalog
{
    public class ProductTagDomainService : DomainService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string PRODUCTTAG_COUNT_KEY = "Hlx.producttag.count-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTTAG_PATTERN_KEY = "Hlx.producttag.";

        private const string PRODUCTTAG_CACHENAME = "cache.producttag.";

        #endregion

        #region Fields

        private readonly IRepository<ProductTag> _productTagRepository;
        //private readonly IDataProvider _dataProvider;
        //private readonly IDbContext _dbContext;
        
        private readonly ICacheManager _cacheManager;
        //private readonly IEventPublisher _eventPublisher;
        private readonly IStoreContext _storeContext;
        private readonly SettingDomainService _settingDomainService;


        private  CommonSettings _commonSettings1;
        public CommonSettings CommonSettings
        {
            get
            {
                if (_commonSettings1 == null)
                {
                    _commonSettings1 = _settingDomainService.LoadSetting<CommonSettings>(_storeContext.CurrentStore.Id);
                }
                return _commonSettings1;
            }
        }


        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="productTagRepository">Product tag repository</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event published</param>
        public ProductTagDomainService(IRepository<ProductTag> productTagRepository,
            //IDataProvider dataProvider,
            //IDbContext dbContext,
            //CommonSettings commonSettings,
            ICacheManager cacheManager
            //,IEventPublisher eventPublisher
            ,IStoreContext storeContext
            ,SettingDomainService settingDomainService)
        {
            this._productTagRepository = productTagRepository;
            //this._dataProvider = dataProvider;
            //this._dbContext = dbContext;
            //this._commonSettings = commonSettings;
            this._cacheManager = cacheManager;
            //this._eventPublisher = eventPublisher;

            this._storeContext = storeContext;
            _settingDomainService = settingDomainService;
        }

        #endregion

        #region Nested classes

        private class ProductTagWithCount
        {
            public int ProductTagId { get; set; }
            public int ProductCount { get; set; }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get product count for each of existing product tag
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Dictionary of "product tag ID : product count"</returns>
        //private Dictionary<int, int> GetProductCount(int storeId)
        //{
        //    string key = string.Format(PRODUCTTAG_COUNT_KEY, storeId);
        //    return _cacheManager.GetCache(PRODUCTTAG_CACHENAME).Get(key, () =>
        //    {

        //        if (CommonSettings.UseStoredProceduresIfSupported 
        //        //&& _dataProvider.StoredProceduredSupported
        //        )
        //        {
        //            //stored procedures are enabled and supported by the database. 
        //            //It's much faster than the LINQ implementation below 

        //            #region Use stored procedure

        //            //prepare parameters
        //            var pStoreId = _dataProvider.GetParameter();
        //            pStoreId.ParameterName = "StoreId";
        //            pStoreId.Value = storeId;
        //            pStoreId.DbType = DbType.Int32;


        //            //invoke stored procedure
        //            var result = _dbContext.SqlQuery<ProductTagWithCount>(
        //                "Exec ProductTagCountLoadAll @StoreId",
        //                pStoreId);

        //            var dictionary = new Dictionary<int, int>();
        //            foreach (var item in result)
        //                dictionary.Add(item.ProductTagId, item.ProductCount);
        //            return dictionary;

        //            #endregion
        //        }
        //        else
        //        {
        //            //stored procedures aren't supported. Use LINQ
        //            #region Search products
        //            var query = from pt in _productTagRepository.Table
        //                        select new
        //                        {
        //                            Id = pt.Id,
        //                            ProductCount = pt.Products
        //                                //published and not deleted products
        //                                .Count(p => !p.Deleted && p.Published)
        //                        };

        //            var dictionary = new Dictionary<int, int>();
        //            foreach (var item in query)
        //                dictionary.Add(item.Id, item.ProductCount);
        //            return dictionary;

        //            #endregion

        //        }
        //    });
        //}

        #endregion

        #region Methods

        /// <summary>
        /// Delete a product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        public virtual void DeleteProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException("productTag");

            _productTagRepository.Delete(productTag);

            //cache
            _cacheManager.GetCache(PRODUCTTAG_CACHENAME).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(productTag);
        }

        /// <summary>
        /// Gets all product tags
        /// </summary>
        /// <returns>Product tags</returns>
        public virtual IList<ProductTag> GetAllProductTags()
        {
            var query = _productTagRepository.GetAll();
            var productTags = query.ToList();
            return productTags;
        }

        /// <summary>
        /// Gets product tag
        /// </summary>
        /// <param name="productTagId">Product tag identifier</param>
        /// <returns>Product tag</returns>
        public virtual ProductTag GetProductTagById(int productTagId)
        {
            if (productTagId == 0)
                return null;

            return _productTagRepository.FirstOrDefault(productTagId);
        }

        /// <summary>
        /// Gets product tag by name
        /// </summary>
        /// <param name="name">Product tag name</param>
        /// <returns>Product tag</returns>
        public virtual ProductTag GetProductTagByName(string name)
        {
            var query = from pt in _productTagRepository.GetAll()
                        where pt.Name == name
                        select pt;

            var productTag = query.FirstOrDefault();
            return productTag;
        }

        /// <summary>
        /// Inserts a product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        public virtual void InsertProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException("productTag");

            _productTagRepository.Insert(productTag);

            //cache
            _cacheManager.GetCache(PRODUCTTAG_CACHENAME).Clear();

            //event notification
            //_eventPublisher.EntityInserted(productTag);
        }

        /// <summary>
        /// Updates the product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        public virtual void UpdateProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException("productTag");

            _productTagRepository.Update(productTag);

            //cache
            _cacheManager.GetCache(PRODUCTTAG_CACHENAME).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(productTag);
        }

        ///// <summary>
        ///// Get number of products
        ///// </summary>
        ///// <param name="productTagId">Product tag identifier</param>
        ///// <param name="storeId">Store identifier</param>
        ///// <returns>Number of products</returns>
        //public virtual int GetProductCount(int productTagId, int storeId)
        //{
        //    var dictionary = GetProductCount(storeId);
        //    if (dictionary.ContainsKey(productTagId))
        //        return dictionary[productTagId];

        //    return 0;
        //}

        #endregion
    }
}
