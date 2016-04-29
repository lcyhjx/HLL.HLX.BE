using System;
using System.Globalization;
using Abp.UI;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Directory;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Directory;
using HLL.HLX.BE.Core.Model.Localization;
using HLL.HLX.BE.Core.Model.Tax;

namespace HLL.HLX.BE.Core.Business.Catalog
{
    /// <summary>
    /// Price formatter
    /// </summary>
    public partial class PriceFormatter : IPriceFormatter
    {
        #region Fields

        //private readonly IWorkContext _workContext;
        private readonly CurrencyDomainService _currencyService;
        //private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly SettingDomainService _settingDomainService;

        private  TaxSettings _taxSettings1;
        public TaxSettings TaxSettings
        {
            get
            {
                if (_taxSettings1 == null)
                {
                    _taxSettings1 = _settingDomainService.LoadSetting<TaxSettings>(_storeContext.CurrentStore.Id);
                }
                return _taxSettings1;
            }
        }
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

        #region Constructors

        public PriceFormatter(
            //IWorkContext workContext,
            CurrencyDomainService currencyService
            //ILocalizationService localizationService,
            //TaxSettings taxSettings,
            //CurrencySettings currencySettings
            ,IStoreContext storeContext
            ,SettingDomainService settingDomainService
            )
        {
            //this._workContext = workContext;
            this._currencyService = currencyService;
            //this._localizationService = localizationService;
            //this._taxSettings = taxSettings;
            //this._currencySettings = currencySettings;
            this._storeContext = storeContext;
            _settingDomainService = settingDomainService;
        }

        #endregion

        #region Utilities

        ///// <summary>
        ///// Gets currency string
        ///// </summary>
        ///// <param name="amount">Amount</param>
        ///// <returns>Currency string without exchange rate</returns>
        //protected virtual string GetCurrencyString(decimal amount)
        //{
        //    return GetCurrencyString(amount, true, _workContext.WorkingCurrency);
        //}

        /// <summary>
        /// Gets currency string
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <returns>Currency string without exchange rate</returns>
        protected virtual string GetCurrencyString(decimal amount,
            bool showCurrency, Currency targetCurrency)
        {
            if (targetCurrency == null)
                throw new ArgumentNullException("targetCurrency");

            string result;
            if (!String.IsNullOrEmpty(targetCurrency.CustomFormatting))
            {
                //custom formatting specified by a store owner
                result = amount.ToString(targetCurrency.CustomFormatting);
            }
            else
            {
                if (!String.IsNullOrEmpty(targetCurrency.DisplayLocale))
                {
                    //default behavior
                    result = amount.ToString("C", new CultureInfo(targetCurrency.DisplayLocale));
                }
                else
                {
                    //not possible because "DisplayLocale" should be always specified
                    //but anyway let's just handle this behavior
                    result = String.Format("{0} ({1})", amount.ToString("N"), targetCurrency.CurrencyCode);
                    return result;
                }
            }

            //display currency code?
            if (showCurrency && CurrencySettings.DisplayCurrencyLabel)
                result = String.Format("{0} ({1})", result, targetCurrency.CurrencyCode);
            return result;
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// Formats the price
        ///// </summary>
        ///// <param name="price">Price</param>
        ///// <returns>Price</returns>
        //public virtual string FormatPrice(decimal price)
        //{
        //    //return FormatPrice(price, true, _workContext.WorkingCurrency);
        //    throw new UserFriendlyException("Not implemented");
        //}

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <returns>Price</returns>
        public virtual string FormatPrice(decimal price, bool showCurrency, Currency targetCurrency)
        {
            //bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            var taxDisplayType = TaxSettings.TaxDisplayType;
            bool priceIncludesTax = taxDisplayType == TaxDisplayType.IncludingTax;
            return FormatPrice(price, showCurrency, targetCurrency, null, priceIncludesTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public virtual string FormatPrice(decimal price, bool showCurrency, bool showTax,Currency targetCurrency)
        {
            //bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;

            var taxDisplayType = TaxSettings.TaxDisplayType;
            bool priceIncludesTax = taxDisplayType == TaxDisplayType.IncludingTax;

            return FormatPrice(price, showCurrency, targetCurrency , null, priceIncludesTax, showTax);
          
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <param name="language">Language</param>
        /// <returns>Price</returns>
        public virtual string FormatPrice(decimal price, bool showCurrency,
            string currencyCode, bool showTax, Language language)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode);
            if (currency == null)
            {
                currency = new Currency();
                currency.CurrencyCode = currencyCode;
            }
            //bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            var taxDisplayType = TaxSettings.TaxDisplayType;
            bool priceIncludesTax = taxDisplayType == TaxDisplayType.IncludingTax;
            return FormatPrice(price, showCurrency, currency, language, priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public virtual string FormatPrice(decimal price, bool showCurrency,
            string currencyCode, Language language, bool priceIncludesTax)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode) 
                ?? new Currency
                   {
                       CurrencyCode = currencyCode
                   };
            return FormatPrice(price, showCurrency, currency, language, priceIncludesTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public virtual string FormatPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax)
        {
            return FormatPrice(price, showCurrency, targetCurrency, language, 
                priceIncludesTax, TaxSettings.DisplayTaxSuffix);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public virtual string FormatPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax, bool showTax)
        {
            //we should round it no matter of "ShoppingCartSettings.RoundPricesDuringCalculation" setting
            price = RoundingHelper.RoundPrice(price);
            
            string currencyString = GetCurrencyString(price, showCurrency, targetCurrency);
            if (showTax)
            {
                //show tax suffix
                string formatStr;
                if (priceIncludesTax)
                {
                    formatStr = InfoMsg.Products_InclTaxSuffix;
                    if (String.IsNullOrEmpty(formatStr))
                        formatStr = "{0} incl tax";
                }
                else
                {
                    formatStr = InfoMsg.Products_ExclTaxSuffix;
                    if (String.IsNullOrEmpty(formatStr))
                        formatStr = "{0} excl tax";
                }
                return string.Format(formatStr, currencyString);
            }
            
            return currencyString;
        }

        /// <summary>
        /// Formats the price of rental product (with rental period)
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="price">Price</param>
        /// <returns>Rental product price with period</returns>
        public virtual string FormatRentalProductPeriod(Product product, string price)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            if (!product.IsRental)
                return price;

            if (String.IsNullOrWhiteSpace(price))
                return price;

            string result;
            switch (product.RentalPricePeriod)
            {
                case RentalPricePeriod.Days:
                    result = string.Format(InfoMsg.Products_Price_Rental_Days, price, product.RentalPriceLength);
                    break;
                case RentalPricePeriod.Weeks:
                    result = string.Format(InfoMsg.Products_Price_Rental_Weeks, price, product.RentalPriceLength);
                    break;
                case RentalPricePeriod.Months:
                    result = string.Format(InfoMsg.Products_Price_Rental_Months, price, product.RentalPriceLength);
                    break;
                case RentalPricePeriod.Years:
                    result = string.Format(InfoMsg.Products_Price_Rental_Years, price, product.RentalPriceLength);
                    break;
                default:
                    throw new UserFriendlyException("Not supported rental period");
            }

            return result;
        }


        ///// <summary>
        ///// Formats the shipping price
        ///// </summary>
        ///// <param name="price">Price</param>
        ///// <param name="showCurrency">A value indicating whether to show a currency</param>
        ///// <returns>Price</returns>
        //public virtual string FormatShippingPrice(decimal price, bool showCurrency)
        //{
        //    //bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
        //    var taxDisplayType = TaxSettings.TaxDisplayType;
        //    bool priceIncludesTax = taxDisplayType == TaxDisplayType.IncludingTax;
        //    return FormatShippingPrice(price, showCurrency, _workContext.WorkingCurrency, _workContext.WorkingLanguage, priceIncludesTax);
        //}

        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public virtual string FormatShippingPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax)
        {
            bool showTax = TaxSettings.ShippingIsTaxable && TaxSettings.DisplayTaxSuffix;

            return FormatShippingPrice(price, showCurrency, targetCurrency, language, priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public virtual string FormatShippingPrice(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax, bool showTax)
        {
            return FormatPrice(price, showCurrency, targetCurrency, language, priceIncludesTax, showTax);
        }
        
        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public virtual string FormatShippingPrice(decimal price, bool showCurrency, 
            string currencyCode, Language language, bool priceIncludesTax)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode) 
                ?? new Currency
                   {
                       CurrencyCode = currencyCode
                   };
            return FormatShippingPrice(price, showCurrency, currency, language, priceIncludesTax);
        }



        ///// <summary>
        ///// Formats the payment method additional fee
        ///// </summary>
        ///// <param name="price">Price</param>
        ///// <param name="showCurrency">A value indicating whether to show a currency</param>
        ///// <returns>Price</returns>
        //public virtual string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency)
        //{
        //    bool priceIncludesTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
        //    return FormatPaymentMethodAdditionalFee(price, showCurrency, _workContext.WorkingCurrency, 
        //        _workContext.WorkingLanguage, priceIncludesTax);
        //}

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public virtual string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency,
            Currency targetCurrency, Language language, bool priceIncludesTax)
        {
            bool showTax = TaxSettings.PaymentMethodAdditionalFeeIsTaxable && TaxSettings.DisplayTaxSuffix;
            return FormatPaymentMethodAdditionalFee(price, showCurrency, targetCurrency, language, priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="targetCurrency">Target currency</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="showTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public virtual string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency, 
            Currency targetCurrency, Language language, bool priceIncludesTax, bool showTax)
        {
            return FormatPrice(price, showCurrency, targetCurrency, language, 
                priceIncludesTax, showTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="showCurrency">A value indicating whether to show a currency</param>
        /// <param name="currencyCode">Currency code</param>
        /// <param name="language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public virtual string FormatPaymentMethodAdditionalFee(decimal price, bool showCurrency, 
            string currencyCode, Language language, bool priceIncludesTax)
        {
            var currency = _currencyService.GetCurrencyByCode(currencyCode)
                ?? new Currency
                   {
                       CurrencyCode = currencyCode
                   };
            return FormatPaymentMethodAdditionalFee(price, showCurrency, currency, 
                language, priceIncludesTax);
        }



        /// <summary>
        /// Formats a tax rate
        /// </summary>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Formatted tax rate</returns>
        public virtual string FormatTaxRate(decimal taxRate)
        {
            return taxRate.ToString("G29");
        }

        #endregion
    }
}
