using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.Core.Business.Stores
{
    public class StoreDomainService : DomainService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string STORES_ALL_KEY = "Hlx.stores.all";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string STORES_BY_ID_KEY = "Hlx.stores.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string STORES_PATTERN_KEY = "Hlx.stores.";
        /// <summary>
        /// Cache name of product
        /// </summary>
        private const string CACHE_NAME_STORE = "Cache.store";

        #endregion

        #region Fields

        private readonly IStoreRepository _storeRepository;
        //private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="storeRepository">Store repository</param>
        /// <param name="eventPublisher">Event published</param>
        public StoreDomainService(ICacheManager cacheManager,
            IStoreRepository storeRepository
            //,IEventPublisher eventPublisher
            )
        {
            this._cacheManager = cacheManager;
            this._storeRepository = storeRepository;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void DeleteStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            var allStores = GetAllStores();
            if (allStores.Count == 1)
                throw new Exception("You cannot delete the only configured store");

            _storeRepository.Delete(store);

            _cacheManager.GetCache(CACHE_NAME_STORE).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(store);
        }

        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <returns>Stores</returns>
        public virtual IList<Store> GetAllStores()
        {
            string key = STORES_ALL_KEY;
            return _cacheManager.GetCache(CACHE_NAME_STORE).Get(key, () =>
            {
                var query = from s in _storeRepository.GetAll()
                            orderby s.DisplayOrder, s.Id
                            select s;
                var stores = query.ToList();
                return stores;
            });
        }

        /// <summary>
        /// Gets a store 
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Store</returns>
        public virtual Store GetStoreById(int storeId)
        {
            if (storeId == 0)
                return null;

            string key = string.Format(STORES_BY_ID_KEY, storeId);
            return _cacheManager.GetCache(CACHE_NAME_STORE).Get(key, () => _storeRepository.FirstOrDefault(storeId));
        }

        /// <summary>
        /// Inserts a store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void InsertStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            _storeRepository.Insert(store);

            _cacheManager.GetCache(CACHE_NAME_STORE).Clear();

            //event notification
            //_eventPublisher.EntityInserted(store);
        }

        /// <summary>
        /// Updates the store
        /// </summary>
        /// <param name="store">Store</param>
        public virtual void UpdateStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            _storeRepository.Update(store);

            _cacheManager.GetCache(CACHE_NAME_STORE).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(store);
        }

        #endregion
    }
}
