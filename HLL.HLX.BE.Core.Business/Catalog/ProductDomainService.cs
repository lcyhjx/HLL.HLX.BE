using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Core.Business.Catalog
{
    /// <summary>
    /// Product domain service
    /// </summary>
    public class ProductDomainService : DomainService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        private const string PRODUCTS_BY_ID_KEY = "Hlx.product.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>        
        private const string PRODUCTS_PATTERN_KEY = "Hlx.product.";
        /// <summary>
        /// Cache name of product
        /// </summary>
        private const string CACHE_NAME_PRODUCT = "Cache.product";
        #endregion

        #region Fields
        private readonly IProductRepository _productRepository;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public ProductDomainService(IProductRepository productRepository
            , ICacheManager cacheManager
            )
        {
            _productRepository = productRepository;
            _cacheManager = cacheManager;
        }
        #endregion


        #region Methods

        #region Products
        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product</returns>
        public virtual Product GetProductById(int productId)
        {
           if (productId == 0)
                return null;

            string key = string.Format(PRODUCTS_BY_ID_KEY, productId);
            var product = _cacheManager.GetCache("PRODUCTS_cache_name")
                .Get(key, () => _productRepository.FirstOrDefault(x => x.Id == productId));
            //var product = _productRepository.FirstOrDefault(x => x.Id == productId);
            //return _cacheManager.Get(key, () => _productRepository.FirstOrDefault(x=>x.Id == productId));
            return product;
        }
        #endregion

        #endregion
    }


}
