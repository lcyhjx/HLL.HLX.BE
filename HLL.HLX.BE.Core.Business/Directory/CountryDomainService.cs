using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Directory;
using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.Core.Business.Directory
{
    public class CountryDomainService : DomainService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        private const string COUNTRIES_ALL_KEY = "Hlx.country.all-{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string COUNTRIES_PATTERN_KEY = "Hlx.country.";
        private const string CACHE_NAME_COUNTRY = "cache.country";
        #endregion

        #region Fields

        private readonly ICountryRepository _countryRepository;
        private readonly IStoreMappingRepository _storeMappingRepository;
        private readonly IStoreContext _storeContext;
        private readonly SettingDomainService _settingDomainService;

        //private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

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
        /// <param name="countryRepository">Country repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="eventPublisher">Event published</param>
        public CountryDomainService(ICacheManager cacheManager,
            ICountryRepository countryRepository,
            IStoreMappingRepository storeMappingRepository,
            IStoreContext storeContext,
            SettingDomainService settingDomainService
            //,IEventPublisher eventPublisher
            )
        {
            this._cacheManager = cacheManager;
            this._countryRepository = countryRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._storeContext = storeContext;
            //this._catalogSettings = catalogSettings;
            _settingDomainService = settingDomainService;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void DeleteCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Delete(country);

            _cacheManager.GetCache(CACHE_NAME_COUNTRY).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(country);
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountries(int languageId = 0, bool showHidden = false)
        {
            string key = string.Format(COUNTRIES_ALL_KEY, languageId, showHidden);
            return _cacheManager.GetCache(CACHE_NAME_COUNTRY).Get(key, () =>
            {
                var query = _countryRepository.GetAll();
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

                if (!showHidden && ! CatalogSettings.IgnoreStoreLimitations)
                {
                    //Store mapping
                    var currentStoreId = _storeContext.CurrentStore.Id;
                    query = from c in query
                            join sc in _storeMappingRepository.GetAll()
                            on new { c1 = c.Id, c2 = "Country" } equals new { c1 = sc.EntityId, c2 = sc.EntityName } into c_sc
                            from sc in c_sc.DefaultIfEmpty()
                            where !c.LimitedToStores || currentStoreId == sc.StoreId
                            select c;

                    //only distinct entities (group by ID)
                    query = from c in query
                            group c by c.Id
                                into cGroup
                            orderby cGroup.Key
                            select cGroup.FirstOrDefault();
                    query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);
                }

                var countries = query.ToList();

                if (languageId > 0)
                {
                    //we should sort countries by localized names when they have the same display order
                    countries = countries
                        .OrderBy(c => c.DisplayOrder)
                        .ThenBy(x => x.Name)
                        .ToList();
                }
                return countries;
            });
        }

        /// <summary>
        /// Gets all countries that allow billing
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountriesForBilling(int languageId = 0, bool showHidden = false)
        {
            return GetAllCountries(languageId, showHidden).Where(c => c.AllowsBilling).ToList();
        }

        /// <summary>
        /// Gets all countries that allow shipping
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountriesForShipping(int languageId = 0, bool showHidden = false)
        {
            return GetAllCountries(languageId, showHidden).Where(c => c.AllowsShipping).ToList();
        }

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryById(int countryId)
        {
            if (countryId == 0)
                return null;

            return _countryRepository.FirstOrDefault(countryId);
        }

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="countryIds">Country identifiers</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetCountriesByIds(int[] countryIds)
        {
            if (countryIds == null || countryIds.Length == 0)
                return new List<Country>();

            var query = from c in _countryRepository.GetAll()
                        where countryIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Country>();
            foreach (int id in countryIds)
            {
                var country = countries.Find(x => x.Id == id);
                if (country != null)
                    sortedCountries.Add(country);
            }
            return sortedCountries;
        }

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="twoLetterIsoCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            if (String.IsNullOrEmpty(twoLetterIsoCode))
                return null;

            var query = from c in _countryRepository.GetAll()
                        where c.TwoLetterIsoCode == twoLetterIsoCode
                        select c;
            var country = query.FirstOrDefault();
            return country;
        }

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="threeLetterIsoCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryByThreeLetterIsoCode(string threeLetterIsoCode)
        {
            if (String.IsNullOrEmpty(threeLetterIsoCode))
                return null;

            var query = from c in _countryRepository.GetAll()
                        where c.ThreeLetterIsoCode == threeLetterIsoCode
                        select c;
            var country = query.FirstOrDefault();
            return country;
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void InsertCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Insert(country);

            _cacheManager.GetCache(CACHE_NAME_COUNTRY).Clear();

            //event notification
            //_eventPublisher.EntityInserted(country);
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void UpdateCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Update(country);

            _cacheManager.GetCache(CACHE_NAME_COUNTRY).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(country);
        }

        #endregion
    }
}
