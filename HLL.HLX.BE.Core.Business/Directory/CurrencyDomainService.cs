using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using Abp.UI;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.Core.Business.Directory
{
    public class CurrencyDomainService : DomainService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : currency ID
        /// </remarks>
        private const string CURRENCIES_BY_ID_KEY = "Hlx.currency.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string CURRENCIES_ALL_KEY = "Hlx.currency.all-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CURRENCIES_PATTERN_KEY = "Hlx.currency.";
        private const string CACHE_NAME_CURRENCY = "cache.currency";
        #endregion

        #region Fields

        private readonly ICurrencyRepository _currencyRepository;
        private readonly StoreMappingDomainService _storeMappingService;
        private readonly IStoreContext _storeContext;
        private readonly SettingDomainService _settingDomainService;
        private readonly ICacheManager _cacheManager;

        //private readonly IPluginFinder _pluginFinder;
        //private readonly IEventPublisher _eventPublisher;

        private  CurrencySettings _currencySettings1;
        public CurrencySettings CurrencySettings
        {
            get
            {
                if (_currencySettings1 == null)
                {
                    _currencySettings1 = _settingDomainService.LoadSetting<CurrencySettings>(_storeContext.CurrentStore.Id);
                }
                return _currencySettings1;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="currencyRepository">Currency repository</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="currencySettings">Currency settings</param>
        /// <param name="pluginFinder">Plugin finder</param>
        /// <param name="eventPublisher">Event published</param>
        public CurrencyDomainService(ICacheManager cacheManager,
            ICurrencyRepository currencyRepository,
            StoreMappingDomainService storeMappingService,
             IStoreContext storeContext,
            SettingDomainService settingDomainService
            //CurrencySettings currencySettings
            //IPluginFinder pluginFinder,
            //IEventPublisher eventPublisher
            )
        {
            this._cacheManager = cacheManager;
            this._currencyRepository = currencyRepository;
            this._storeMappingService = storeMappingService;
            this._storeContext = storeContext;            
            _settingDomainService = settingDomainService;
            //this._currencySettings = currencySettings;
            //this._pluginFinder = pluginFinder;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>Exchange rates</returns>
        //public virtual IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode)
        //{
        //    var exchangeRateProvider = LoadActiveExchangeRateProvider();
        //    if (exchangeRateProvider == null)
        //        throw new Exception("Active exchange rate provider cannot be loaded");
        //    return exchangeRateProvider.GetCurrencyLiveRates(exchangeRateCurrencyCode);
        //}

        /// <summary>
        /// Deletes currency
        /// </summary>
        /// <param name="currency">Currency</param>
        public virtual void DeleteCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException("currency");

            _currencyRepository.Delete(currency);

            _cacheManager.GetCache(CACHE_NAME_CURRENCY).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(currency);
        }

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <returns>Currency</returns>
        public virtual Currency GetCurrencyById(int currencyId)
        {
            if (currencyId == 0)
                return null;

            string key = string.Format(CURRENCIES_BY_ID_KEY, currencyId);
            return _cacheManager.GetCache(CACHE_NAME_CURRENCY).Get(key, () => _currencyRepository.FirstOrDefault(currencyId));
        }

        /// <summary>
        /// Gets a currency by code
        /// </summary>
        /// <param name="currencyCode">Currency code</param>
        /// <returns>Currency</returns>
        public virtual Currency GetCurrencyByCode(string currencyCode)
        {
            if (String.IsNullOrEmpty(currencyCode))
                return null;
            return GetAllCurrencies(true).FirstOrDefault(c => c.CurrencyCode.ToLower() == currencyCode.ToLower());
        }

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Currencies</returns>
        public virtual IList<Currency> GetAllCurrencies(bool showHidden = false, int storeId = 0)
        {
            string key = string.Format(CURRENCIES_ALL_KEY, showHidden);
            var currencies = _cacheManager.GetCache(CACHE_NAME_CURRENCY).Get(key, () =>
            {
                var query = _currencyRepository.GetAll();
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.OrderBy(c => c.DisplayOrder);
                return query.ToList();
            });

            //store mapping
            if (storeId > 0)
            {
                currencies = currencies
                    .Where(c => _storeMappingService.Authorize(c, storeId))
                    .ToList();
            }
            return currencies;
        }

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="currency">Currency</param>
        public virtual void InsertCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException("currency");

            _currencyRepository.Insert(currency);

            _cacheManager.GetCache(CACHE_NAME_CURRENCY).Clear();

            //event notification
            //_eventPublisher.EntityInserted(currency);
        }

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="currency">Currency</param>
        public virtual void UpdateCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException("currency");

            _currencyRepository.Update(currency);

            _cacheManager.GetCache(CACHE_NAME_CURRENCY).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(currency);
        }



        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Currency exchange rate</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertCurrency(decimal amount, decimal exchangeRate)
        {
            if (amount != decimal.Zero && exchangeRate != decimal.Zero)
                return amount * exchangeRate;
            return decimal.Zero;
        }

        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertCurrency(decimal amount, Currency sourceCurrencyCode, Currency targetCurrencyCode)
        {
            if (targetCurrencyCode == null)
                throw new ArgumentNullException("sourceCurrencyCode");

            if (targetCurrencyCode == null)
                throw new ArgumentNullException("targetCurrencyCode");

            decimal result = amount;
            if (sourceCurrencyCode.Id == targetCurrencyCode.Id)
                return result;
            if (result != decimal.Zero && sourceCurrencyCode.Id != targetCurrencyCode.Id)
            {
                result = ConvertToPrimaryExchangeRateCurrency(result, sourceCurrencyCode);
                result = ConvertFromPrimaryExchangeRateCurrency(result, targetCurrencyCode);
            }
            return result;
        }

        /// <summary>
        /// Converts to primary exchange rate currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertToPrimaryExchangeRateCurrency(decimal amount, Currency sourceCurrencyCode)
        {
            if (sourceCurrencyCode == null)
                throw new ArgumentNullException("sourceCurrencyCode");

            var primaryExchangeRateCurrency = GetCurrencyById(CurrencySettings.PrimaryExchangeRateCurrencyId);
            if (primaryExchangeRateCurrency == null)
                throw new Exception("Primary exchange rate currency cannot be loaded");

            decimal result = amount;
            if (result != decimal.Zero && sourceCurrencyCode.Id != primaryExchangeRateCurrency.Id)
            {
                decimal exchangeRate = sourceCurrencyCode.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new UserFriendlyException(string.Format("Exchange rate not found for currency [{0}]", sourceCurrencyCode.Name));
                result = result / exchangeRate;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary exchange rate currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertFromPrimaryExchangeRateCurrency(decimal amount, Currency targetCurrencyCode)
        {
            if (targetCurrencyCode == null)
                throw new ArgumentNullException("targetCurrencyCode");

            var primaryExchangeRateCurrency = GetCurrencyById(CurrencySettings.PrimaryExchangeRateCurrencyId);
            if (primaryExchangeRateCurrency == null)
                throw new Exception("Primary exchange rate currency cannot be loaded");

            decimal result = amount;
            if (result != decimal.Zero && targetCurrencyCode.Id != primaryExchangeRateCurrency.Id)
            {
                decimal exchangeRate = targetCurrencyCode.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new UserFriendlyException(string.Format("Exchange rate not found for currency [{0}]", targetCurrencyCode.Name));
                result = result * exchangeRate;
            }
            return result;
        }

        /// <summary>
        /// Converts to primary store currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertToPrimaryStoreCurrency(decimal amount, Currency sourceCurrencyCode)
        {
            if (sourceCurrencyCode == null)
                throw new ArgumentNullException("sourceCurrencyCode");

            var primaryStoreCurrency = GetCurrencyById(CurrencySettings.PrimaryStoreCurrencyId);
            if (primaryStoreCurrency == null)
                throw new Exception("Primary store currency cannot be loaded");

            decimal result = amount;
            if (result != decimal.Zero && sourceCurrencyCode.Id != primaryStoreCurrency.Id)
            {
                decimal exchangeRate = sourceCurrencyCode.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new UserFriendlyException(string.Format("Exchange rate not found for currency [{0}]", sourceCurrencyCode.Name));
                result = result / exchangeRate;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary store currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertFromPrimaryStoreCurrency(decimal amount, Currency targetCurrencyCode)
        {
            var primaryStoreCurrency = GetCurrencyById(CurrencySettings.PrimaryStoreCurrencyId);
            var result = ConvertCurrency(amount, primaryStoreCurrency, targetCurrencyCode);
            return result;
        }


        ///// <summary>
        ///// Load active exchange rate provider
        ///// </summary>
        ///// <returns>Active exchange rate provider</returns>
        //public virtual IExchangeRateProvider LoadActiveExchangeRateProvider()
        //{
        //    var exchangeRateProvider = LoadExchangeRateProviderBySystemName(_currencySettings.ActiveExchangeRateProviderSystemName);
        //    if (exchangeRateProvider == null)
        //        exchangeRateProvider = LoadAllExchangeRateProviders().FirstOrDefault();
        //    return exchangeRateProvider;
        //}

        ///// <summary>
        ///// Load exchange rate provider by system name
        ///// </summary>
        ///// <param name="systemName">System name</param>
        ///// <returns>Found exchange rate provider</returns>
        //public virtual IExchangeRateProvider LoadExchangeRateProviderBySystemName(string systemName)
        //{
        //    var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<IExchangeRateProvider>(systemName);
        //    if (descriptor != null)
        //        return descriptor.Instance<IExchangeRateProvider>();

        //    return null;
        //}

        ///// <summary>
        ///// Load all exchange rate providers
        ///// </summary>
        ///// <returns>Exchange rate providers</returns>
        //public virtual IList<IExchangeRateProvider> LoadAllExchangeRateProviders()
        //{
        //    var exchangeRateProviders = _pluginFinder.GetPlugins<IExchangeRateProvider>();
        //    return exchangeRateProviders
        //        .OrderBy(tp => tp.PluginDescriptor)
        //        .ToList();
        //}
        #endregion
    }
}
