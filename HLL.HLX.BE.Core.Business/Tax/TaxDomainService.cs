using System;
using System.Text.RegularExpressions;
using Abp.Domain.Services;
using HLL.HLX.BE.Core.Business.Common;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Directory;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Common;
using HLL.HLX.BE.Core.Model.Orders;
using HLL.HLX.BE.Core.Model.Tax;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Business.Tax
{
    public class TaxDomainService : DomainService
    {

        #region Fields

        private readonly AddressDomainService _addressService;
        //private readonly IWorkContext _workContext;

        //private readonly IPluginFinder _pluginFinder;
        //private readonly IGeoLookupService _geoLookupService;
        private readonly CountryDomainService _countryService;
        //private readonly CustomerSettings _customerSettings;
        //private readonly AddressSettings _addressSettings;

        private readonly IStoreContext _storeContext;
        private readonly SettingDomainService _settingDomainService;



        private TaxSettings _taxSettings1;
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

        #endregion


        #region Ctor

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="addressService">Address service</param>
        /// <param name="workContext">Work context</param>
        /// <param name="taxSettings">Tax settings</param>
        /// <param name="pluginFinder">Plugin finder</param>
        /// <param name="geoLookupService">GEO lookup service</param>
        /// <param name="countryService">Country service</param>
        /// <param name="customerSettings">Customer settings</param>
        /// <param name="addressSettings">Address settings</param>
        public TaxDomainService(
            IStoreContext storeContext,
            SettingDomainService settingDomainService
            , AddressDomainService addressService
            //IWorkContext workContext,
            //TaxSettings taxSettings,
            //IPluginFinder pluginFinder,
            //IGeoLookupService geoLookupService,
            , CountryDomainService countryService
            //CustomerSettings customerSettings,
            //AddressSettings addressSettings
            )
        {
            _addressService = addressService;
            //this._workContext = workContext;
            //this._taxSettings = taxSettings;
            //this._pluginFinder = pluginFinder;
            //this._geoLookupService = geoLookupService;
            _countryService = countryService;
            //this._customerSettings = customerSettings;
            //this._addressSettings = addressSettings;
            _storeContext = storeContext;
            _settingDomainService = settingDomainService;
        }

        #endregion




        #region Utilities

        /// <summary>
        ///     Get a value indicating whether a customer is consumer (a person, not a company) located in Europe Union
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Result</returns>
        protected virtual bool IsEuConsumer(User customer)
        {
            return false;
        }

        /// <summary>
        ///     Create request for tax calculation
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="customer">Customer</param>
        /// <returns>Package for tax calculation</returns>
        protected virtual CalculateTaxRequest CreateCalculateTaxRequest(Product product,
            int taxCategoryId, User customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var calculateTaxRequest = new CalculateTaxRequest();
            calculateTaxRequest.Customer = customer;
            if (taxCategoryId > 0)
            {
                calculateTaxRequest.TaxCategoryId = taxCategoryId;
            }
            else
            {
                if (product != null)
                    calculateTaxRequest.TaxCategoryId = product.TaxCategoryId;
            }

            var basedOn = TaxSettings.TaxBasedOn;
            //new EU VAT rules starting January 1st 2015
            //find more info at http://ec.europa.eu/taxation_customs/taxation/vat/how_vat_works/telecom/index_en.htm#new_rules
            //EU VAT enabled?
            if (TaxSettings.EuVatEnabled)
            {
                //telecommunications, broadcasting and electronic services?
                if (product != null && product.IsTelecommunicationsOrBroadcastingOrElectronicServices)
                {
                    //January 1st 2015 passed?
                    if (DateTime.UtcNow > new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                    {
                        //Europe Union consumer?
                        if (IsEuConsumer(customer))
                        {
                            //We must charge VAT in the EU country where the customer belongs (not where the business is based)
                            basedOn = TaxBasedOn.BillingAddress;
                        }
                    }
                }
            }
            if (basedOn == TaxBasedOn.BillingAddress && customer.BillingAddress == null)
                basedOn = TaxBasedOn.DefaultAddress;
            if (basedOn == TaxBasedOn.ShippingAddress && customer.ShippingAddress == null)
                basedOn = TaxBasedOn.DefaultAddress;

            Address address;

            switch (basedOn)
            {
                case TaxBasedOn.BillingAddress:
                {
                    address = customer.BillingAddress;
                }
                    break;
                case TaxBasedOn.ShippingAddress:
                {
                    address = customer.ShippingAddress;
                }
                    break;
                case TaxBasedOn.DefaultAddress:
                default:
                {
                    address = _addressService.GetAddressById(TaxSettings.DefaultTaxAddressId);
                }
                    break;
            }

            calculateTaxRequest.Address = address;
            return calculateTaxRequest;
        }

        /// <summary>
        ///     Calculated price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="percent">Percent</param>
        /// <param name="increase">Increase</param>
        /// <returns>New price</returns>
        protected virtual decimal CalculatePrice(decimal price, decimal percent, bool increase)
        {
            if (percent == decimal.Zero)
                return price;

            decimal result;
            if (increase)
            {
                result = price*(1 + percent/100);
            }
            else
            {
                result = price - (price)/(100 + percent)*percent;
            }
            return result;
        }

        /// <summary>
        ///     Gets tax rate
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="customer">Customer</param>
        /// <param name="taxRate">Calculated tax rate</param>
        /// <param name="isTaxable">A value indicating whether a request is taxable</param>
        protected virtual void GetTaxRate(Product product, int taxCategoryId,
            User customer, out decimal taxRate, out bool isTaxable)
        {
            taxRate = decimal.Zero;
            isTaxable = true;

            ////active tax provider
            //var activeTaxProvider = LoadActiveTaxProvider();
            //if (activeTaxProvider == null)
            //    return;

            ////tax request
            //var calculateTaxRequest = CreateCalculateTaxRequest(product, taxCategoryId, customer);

            ////tax exempt
            //if (IsTaxExempt(product, calculateTaxRequest.Customer))
            //{
            //    isTaxable = false;
            //}
            ////make EU VAT exempt validation (the European Union Value Added Tax)
            //if (isTaxable &&
            //    TaxSettings.EuVatEnabled &&
            //    IsVatExempt(calculateTaxRequest.Address, calculateTaxRequest.Customer))
            //{
            //    //VAT is not chargeable
            //    isTaxable = false;
            //}

            ////get tax rate
            //var calculateTaxResult = activeTaxProvider.GetTaxRate(calculateTaxRequest);
            //if (calculateTaxResult.Success)
            //{
            //    //ensure that tax is equal or greater than zero
            //    if (calculateTaxResult.TaxRate < decimal.Zero)
            //        calculateTaxResult.TaxRate = decimal.Zero;

            //    taxRate = calculateTaxResult.TaxRate;
            //}
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// Load active tax provider
        ///// </summary>
        ///// <returns>Active tax provider</returns>
        //public virtual ITaxProvider LoadActiveTaxProvider()
        //{
        //    var taxProvider = LoadTaxProviderBySystemName(_taxSettings.ActiveTaxProviderSystemName);
        //    if (taxProvider == null)
        //        taxProvider = LoadAllTaxProviders().FirstOrDefault();
        //    return taxProvider;
        //}

        ///// <summary>
        ///// Load tax provider by system name
        ///// </summary>
        ///// <param name="systemName">System name</param>
        ///// <returns>Found tax provider</returns>
        //public virtual ITaxProvider LoadTaxProviderBySystemName(string systemName)
        //{
        //    var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<ITaxProvider>(systemName);
        //    if (descriptor != null)
        //        return descriptor.Instance<ITaxProvider>();

        //    return null;
        //}

        ///// <summary>
        ///// Load all tax providers
        ///// </summary>
        ///// <returns>Tax providers</returns>
        //public virtual IList<ITaxProvider> LoadAllTaxProviders()
        //{
        //    return _pluginFinder.GetPlugins<ITaxProvider>().ToList();
        //}


        ///// <summary>
        ///// Gets price
        ///// </summary>
        ///// <param name="product">Product</param>
        ///// <param name="price">Price</param>
        ///// <param name="taxRate">Tax rate</param>
        ///// <returns>Price</returns>
        //public virtual decimal GetProductPrice(Product product, decimal price,
        //    out decimal taxRate)
        //{
        //    var customer = _workContext.CurrentCustomer;
        //    return GetProductPrice(product, price, customer, out taxRate);
        //}

        /// <summary>
        ///     Gets price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetProductPrice(Product product, decimal price,
            User customer, out decimal taxRate)
        {
            //bool includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;
            var includingTax = true;
            return GetProductPrice(product, price, includingTax, customer, out taxRate);
        }

        /// <summary>
        ///     Gets price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetProductPrice(Product product, decimal price,
            bool includingTax, User customer, out decimal taxRate)
        {
            var priceIncludesTax = TaxSettings.PricesIncludeTax;
            var taxCategoryId = 0;
            return GetProductPrice(product, taxCategoryId, price, includingTax,
                customer, priceIncludesTax, out taxRate);
        }

        /// <summary>
        ///     Gets price
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="priceIncludesTax">A value indicating whether price already includes tax</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetProductPrice(Product product, int taxCategoryId,
            decimal price, bool includingTax, User customer,
            bool priceIncludesTax, out decimal taxRate)
        {
            //no need to calculate tax rate if passed "price" is 0
            if (price == decimal.Zero)
            {
                taxRate = decimal.Zero;
                return taxRate;
            }


            bool isTaxable;
            GetTaxRate(product, taxCategoryId, customer, out taxRate, out isTaxable);

            if (priceIncludesTax)
            {
                //"price" already includes tax
                if (includingTax)
                {
                    //we should calculate price WITH tax
                    if (!isTaxable)
                    {
                        //but our request is not taxable
                        //hence we should calculate price WITHOUT tax
                        price = CalculatePrice(price, taxRate, false);
                    }
                }
                else
                {
                    //we should calculate price WITHOUT tax
                    price = CalculatePrice(price, taxRate, false);
                }
            }
            else
            {
                //"price" doesn't include tax
                if (includingTax)
                {
                    //we should calculate price WITH tax
                    //do it only when price is taxable
                    if (isTaxable)
                    {
                        price = CalculatePrice(price, taxRate, true);
                    }
                }
            }


            if (!isTaxable)
            {
                //we return 0% tax rate in case a request is not taxable
                taxRate = decimal.Zero;
            }

            //allowed to support negative price adjustments
            //if (price < decimal.Zero)
            //    price = decimal.Zero;

            return price;
        }


        /// <summary>
        ///     Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public virtual decimal GetShippingPrice(decimal price, User customer)
        {
            //bool includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;

            var taxDisplayType = TaxSettings.TaxDisplayType;
            var includingTax = taxDisplayType == TaxDisplayType.IncludingTax;

            return GetShippingPrice(price, includingTax, customer);
        }

        /// <summary>
        ///     Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public virtual decimal GetShippingPrice(decimal price, bool includingTax, User customer)
        {
            decimal taxRate;
            return GetShippingPrice(price, includingTax, customer, out taxRate);
        }

        /// <summary>
        ///     Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetShippingPrice(decimal price, bool includingTax, User customer, out decimal taxRate)
        {
            taxRate = decimal.Zero;

            if (!TaxSettings.ShippingIsTaxable)
            {
                return price;
            }
            int taxClassId = TaxSettings.ShippingTaxClassId;
            bool priceIncludesTax = TaxSettings.ShippingPriceIncludesTax;
            return GetProductPrice(null, taxClassId, price, includingTax, customer,
                priceIncludesTax, out taxRate);
        }


        /// <summary>
        ///     Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public virtual decimal GetPaymentMethodAdditionalFee(decimal price, User customer)
        {
            //var includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;

            var taxDisplayType = TaxSettings.TaxDisplayType;
            var includingTax = taxDisplayType == TaxDisplayType.IncludingTax;

            return GetPaymentMethodAdditionalFee(price, includingTax, customer);
        }

        /// <summary>
        ///     Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public virtual decimal GetPaymentMethodAdditionalFee(decimal price, bool includingTax, User customer)
        {
            decimal taxRate;
            return GetPaymentMethodAdditionalFee(price, includingTax,
                customer, out taxRate);
        }

        /// <summary>
        ///     Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetPaymentMethodAdditionalFee(decimal price, bool includingTax, User customer,
            out decimal taxRate)
        {
            taxRate = decimal.Zero;

            if (!TaxSettings.PaymentMethodAdditionalFeeIsTaxable)
            {
                return price;
            }
            int taxClassId = TaxSettings.PaymentMethodAdditionalFeeTaxClassId;
            bool priceIncludesTax = TaxSettings.PaymentMethodAdditionalFeeIncludesTax;
            return GetProductPrice(null, taxClassId, price, includingTax, customer,
                priceIncludesTax, out taxRate);
        }


        ///// <summary>
        /////     Gets checkout attribute value price
        ///// </summary>
        ///// <param name="cav">Checkout attribute value</param>
        ///// <returns>Price</returns>
        //public virtual decimal GetCheckoutAttributePrice(CheckoutAttributeValue cav)
        //{
        //    var customer = _workContext.CurrentCustomer;
        //    return GetCheckoutAttributePrice(cav, customer);
        //}

        /// <summary>
        ///     Gets checkout attribute value price
        /// </summary>
        /// <param name="cav">Checkout attribute value</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public virtual decimal GetCheckoutAttributePrice(CheckoutAttributeValue cav, User customer)
        {
            //var includingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax;

            var taxDisplayType = TaxSettings.TaxDisplayType;
            var includingTax = taxDisplayType == TaxDisplayType.IncludingTax;

            return GetCheckoutAttributePrice(cav, includingTax, customer);
        }

        /// <summary>
        ///     Gets checkout attribute value price
        /// </summary>
        /// <param name="cav">Checkout attribute value</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public virtual decimal GetCheckoutAttributePrice(CheckoutAttributeValue cav,
            bool includingTax, User customer)
        {
            decimal taxRate;
            return GetCheckoutAttributePrice(cav, includingTax, customer, out taxRate);
        }

        /// <summary>
        ///     Gets checkout attribute value price
        /// </summary>
        /// <param name="cav">Checkout attribute value</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="taxRate">Tax rate</param>
        /// <returns>Price</returns>
        public virtual decimal GetCheckoutAttributePrice(CheckoutAttributeValue cav,
            bool includingTax, User customer, out decimal taxRate)
        {
            if (cav == null)
                throw new ArgumentNullException("cav");

            taxRate = decimal.Zero;

            var price = cav.PriceAdjustment;
            if (cav.CheckoutAttribute.IsTaxExempt)
            {
                return price;
            }

            bool priceIncludesTax = TaxSettings.PricesIncludeTax;
            var taxClassId = cav.CheckoutAttribute.TaxCategoryId;
            return GetProductPrice(null, taxClassId, price, includingTax, customer,
                priceIncludesTax, out taxRate);
        }


        ///// <summary>
        /////     Gets VAT Number status
        ///// </summary>
        ///// <param name="fullVatNumber">Two letter ISO code of a country and VAT number (e.g. GB 111 1111 111)</param>
        ///// <returns>VAT Number status</returns>
        //public virtual VatNumberStatus GetVatNumberStatus(string fullVatNumber)
        //{
        //    string name, address;
        //    return GetVatNumberStatus(fullVatNumber, out name, out address);
        //}

        ///// <summary>
        /////     Gets VAT Number status
        ///// </summary>
        ///// <param name="fullVatNumber">Two letter ISO code of a country and VAT number (e.g. GB 111 1111 111)</param>
        ///// <param name="name">Name (if received)</param>
        ///// <param name="address">Address (if received)</param>
        ///// <returns>VAT Number status</returns>
        //public virtual VatNumberStatus GetVatNumberStatus(string fullVatNumber,
        //    out string name, out string address)
        //{
        //    name = string.Empty;
        //    address = string.Empty;

        //    if (String.IsNullOrWhiteSpace(fullVatNumber))
        //        return VatNumberStatus.Empty;
        //    fullVatNumber = fullVatNumber.Trim();

        //    //GB 111 1111 111 or GB 1111111111
        //    //more advanced regex - http://codeigniter.com/wiki/European_Vat_Checker
        //    var r = new Regex(@"^(\w{2})(.*)");
        //    var match = r.Match(fullVatNumber);
        //    if (!match.Success)
        //        return VatNumberStatus.Invalid;
        //    var twoLetterIsoCode = match.Groups[1].Value;
        //    var vatNumber = match.Groups[2].Value;

        //    return GetVatNumberStatus(twoLetterIsoCode, vatNumber, out name, out address);
        //}

        ///// <summary>
        /////     Gets VAT Number status
        ///// </summary>
        ///// <param name="twoLetterIsoCode">Two letter ISO code of a country</param>
        ///// <param name="vatNumber">VAT number</param>
        ///// <returns>VAT Number status</returns>
        //public virtual VatNumberStatus GetVatNumberStatus(string twoLetterIsoCode, string vatNumber)
        //{
        //    string name, address;
        //    return GetVatNumberStatus(twoLetterIsoCode, vatNumber, out name, out address);
        //}

        ///// <summary>
        /////     Gets VAT Number status
        ///// </summary>
        ///// <param name="twoLetterIsoCode">Two letter ISO code of a country</param>
        ///// <param name="vatNumber">VAT number</param>
        ///// <param name="name">Name (if received)</param>
        ///// <param name="address">Address (if received)</param>
        ///// <returns>VAT Number status</returns>
        //public virtual VatNumberStatus GetVatNumberStatus(string twoLetterIsoCode, string vatNumber,
        //    out string name, out string address)
        //{
        //    name = string.Empty;
        //    address = string.Empty;

        //    if (String.IsNullOrEmpty(twoLetterIsoCode))
        //        return VatNumberStatus.Empty;

        //    if (String.IsNullOrEmpty(vatNumber))
        //        return VatNumberStatus.Empty;

        //    if (TaxSettings.EuVatAssumeValid)
        //        return VatNumberStatus.Valid;

        //    if (!TaxSettings.EuVatUseWebService)
        //        return VatNumberStatus.Unknown;

        //    Exception exception;
        //    return DoVatCheck(twoLetterIsoCode, vatNumber, out name, out address, out exception);
        //}

        ///// <summary>
        /////     Performs a basic check of a VAT number for validity
        ///// </summary>
        ///// <param name="twoLetterIsoCode">Two letter ISO code of a country</param>
        ///// <param name="vatNumber">VAT number</param>
        ///// <param name="name">Company name</param>
        ///// <param name="address">Address</param>
        ///// <param name="exception">Exception</param>
        ///// <returns>VAT number status</returns>
        //public virtual VatNumberStatus DoVatCheck(string twoLetterIsoCode, string vatNumber,
        //    out string name, out string address, out Exception exception)
        //{
        //    name = string.Empty;
        //    address = string.Empty;

        //    if (vatNumber == null)
        //        vatNumber = string.Empty;
        //    vatNumber = vatNumber.Trim().Replace(" ", "");

        //    if (twoLetterIsoCode == null)
        //        twoLetterIsoCode = string.Empty;
        //    if (!String.IsNullOrEmpty(twoLetterIsoCode))
        //        //The service returns INVALID_INPUT for country codes that are not uppercase.
        //        twoLetterIsoCode = twoLetterIsoCode.ToUpper();

        //    EuropaCheckVatService.checkVatService s = null;

        //    try
        //    {
        //        bool valid;

        //        s = new EuropaCheckVatService.checkVatService();
        //        s.checkVat(ref twoLetterIsoCode, ref vatNumber, out valid, out name, out address);
        //        exception = null;
        //        return valid ? VatNumberStatus.Valid : VatNumberStatus.Invalid;
        //    }
        //    catch (Exception ex)
        //    {
        //        name = address = string.Empty;
        //        exception = ex;
        //        return VatNumberStatus.Unknown;
        //    }
        //    finally
        //    {
        //        if (name == null)
        //            name = string.Empty;

        //        if (address == null)
        //            address = string.Empty;

        //        if (s != null)
        //            s.Dispose();
        //    }
        //}


        /// <summary>
        ///     Gets a value indicating whether a product is tax exempt
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="customer">Customer</param>
        /// <returns>A value indicating whether a product is tax exempt</returns>
        public virtual bool IsTaxExempt(Product product, User customer)
        {
            if (customer != null)
            {
                if (customer.IsTaxExempt)
                    return true;

                //if (customer.CustomerRoles.Where(cr => cr.Active).Any(cr => cr.TaxExempt))
                //    return true;
            }

            if (product == null)
            {
                return false;
            }

            if (product.IsTaxExempt)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets a value indicating whether EU VAT exempt (the European Union Value Added Tax)
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="customer">Customer</param>
        /// <returns>Result</returns>
        public virtual bool IsVatExempt(Address address, User customer)
        {
            if (!TaxSettings.EuVatEnabled)
                return false;

            if (address == null || address.Country == null || customer == null)
                return false;


            if (!address.Country.SubjectToVat)
                // VAT not chargeable if shipping outside VAT zone
                return true;

            // VAT not chargeable if address, customer and config meet our VAT exemption requirements:
            // returns true if this customer is VAT exempt because they are shipping within the EU but outside our shop country, they have supplied a validated VAT number, and the shop is configured to allow VAT exemption
            //var customerVatStatus =
            //    (VatNumberStatus) customer.GetAttribute<int>(SystemCustomerAttributeNames.VatNumberStatusId);
            //return address.CountryId != _taxSettings.EuVatShopCountryId &&
            //       customerVatStatus == VatNumberStatus.Valid &&
            //       _taxSettings.EuVatAllowVatExemption;
            return false;
        }

        #endregion
    }
}