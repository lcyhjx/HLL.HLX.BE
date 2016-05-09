using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.Core.Business.Orders
{
    public class CheckoutAttributeDomainService : DomainService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// {1} : >A value indicating whether we should exlude shippable attributes
        /// </remarks>
        private const string CHECKOUTATTRIBUTES_ALL_KEY = "Hlx.checkoutattribute.all-{0}-{1}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute ID
        /// </remarks>
        private const string CHECKOUTATTRIBUTES_BY_ID_KEY = "Hlx.checkoutattribute.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute ID
        /// </remarks>
        private const string CHECKOUTATTRIBUTEVALUES_ALL_KEY = "Hlx.checkoutattributevalue.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute value ID
        /// </remarks>
        private const string CHECKOUTATTRIBUTEVALUES_BY_ID_KEY = "Hlx.checkoutattributevalue.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CHECKOUTATTRIBUTES_PATTERN_KEY = "Hlx.checkoutattribute.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CHECKOUTATTRIBUTEVALUES_PATTERN_KEY = "Hlx.checkoutattributevalue.";

        private const string CACHE_NAME_CHECKOUTATTRIBUTE = "cache.checkoutattribute";
        private const string CACHE_NAME_CHECKOUTATTRIBUTEVALUE = "cache.checkoutattributevalue";

        #endregion

        #region Fields

        private readonly ICheckoutAttributeRepository _checkoutAttributeRepository;
        private readonly ICheckoutAttributeValueRepository _checkoutAttributeValueRepository;
        private readonly StoreMappingDomainService _storeMappingService;
        //private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="checkoutAttributeRepository">Checkout attribute repository</param>
        /// <param name="checkoutAttributeValueRepository">Checkout attribute value repository</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="eventPublisher">Event published</param>
        public CheckoutAttributeDomainService(ICacheManager cacheManager,
            ICheckoutAttributeRepository checkoutAttributeRepository,
            ICheckoutAttributeValueRepository checkoutAttributeValueRepository,
            StoreMappingDomainService storeMappingService
            //IEventPublisher eventPublisher
            )
        {
            this._cacheManager = cacheManager;
            this._checkoutAttributeRepository = checkoutAttributeRepository;
            this._checkoutAttributeValueRepository = checkoutAttributeValueRepository;
            this._storeMappingService = storeMappingService;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Checkout attributes

        /// <summary>
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void DeleteCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Delete(checkoutAttribute);

            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Clear();
            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(checkoutAttribute);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="excludeShippableAttributes">A value indicating whether we should exlude shippable attributes</param>
        /// <returns>Checkout attributes</returns>
        public virtual IList<CheckoutAttribute> GetAllCheckoutAttributes(int storeId = 0, bool excludeShippableAttributes = false)
        {
            string key = string.Format(CHECKOUTATTRIBUTES_ALL_KEY, storeId, excludeShippableAttributes);
            return _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Get(key, () =>
            {
                var query = from ca in _checkoutAttributeRepository.GetAll()
                            orderby ca.DisplayOrder
                            select ca;
                var checkoutAttributes = query.ToList();
                if (storeId > 0)
                {
                    //store mapping
                    checkoutAttributes = checkoutAttributes.Where(ca => _storeMappingService.Authorize(ca)).ToList();
                }
                if (excludeShippableAttributes)
                {
                    //remove attributes which require shippable products
                    checkoutAttributes = checkoutAttributes.RemoveShippableAttributes().ToList();
                }
                return checkoutAttributes;
            });
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <returns>Checkout attribute</returns>
        public virtual CheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId)
        {
            if (checkoutAttributeId == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTES_BY_ID_KEY, checkoutAttributeId);
            return _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Get(key, () => _checkoutAttributeRepository.FirstOrDefault(checkoutAttributeId));
        }

        /// <summary>
        /// Inserts a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void InsertCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Insert(checkoutAttribute);

            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Clear();
            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Clear();

            //event notification
            //_eventPublisher.EntityInserted(checkoutAttribute);
        }

        /// <summary>
        /// Updates the checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void UpdateCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Update(checkoutAttribute);

            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Clear();
            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(checkoutAttribute);
        }

        #endregion

        #region Checkout attribute values

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void DeleteCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException("checkoutAttributeValue");

            _checkoutAttributeValueRepository.Delete(checkoutAttributeValue);

            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Clear();
            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(checkoutAttributeValue);
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <returns>Checkout attribute values</returns>
        public virtual IList<CheckoutAttributeValue> GetCheckoutAttributeValues(int checkoutAttributeId)
        {
            string key = string.Format(CHECKOUTATTRIBUTEVALUES_ALL_KEY, checkoutAttributeId);
            return _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Get(key, () =>
            {
                var query = from cav in _checkoutAttributeValueRepository.GetAll()
                            orderby cav.DisplayOrder
                            where cav.CheckoutAttributeId == checkoutAttributeId
                            select cav;
                var checkoutAttributeValues = query.ToList();
                return checkoutAttributeValues;
            });
        }

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <returns>Checkout attribute value</returns>
        public virtual CheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId)
        {
            if (checkoutAttributeValueId == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTEVALUES_BY_ID_KEY, checkoutAttributeValueId);
            return _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Get(key, () => _checkoutAttributeValueRepository.FirstOrDefault(checkoutAttributeValueId));
        }

        /// <summary>
        /// Inserts a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void InsertCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException("checkoutAttributeValue");

            _checkoutAttributeValueRepository.Insert(checkoutAttributeValue);

            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Clear();
            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Clear();

            //event notification
            //_eventPublisher.EntityInserted(checkoutAttributeValue);
        }

        /// <summary>
        /// Updates the checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void UpdateCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException("checkoutAttributeValue");

            _checkoutAttributeValueRepository.Update(checkoutAttributeValue);

            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTE).Clear();
            _cacheManager.GetCache(CACHE_NAME_CHECKOUTATTRIBUTEVALUE).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(checkoutAttributeValue);
        }

        #endregion

        #endregion
    }
}
