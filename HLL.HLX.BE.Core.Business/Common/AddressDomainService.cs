using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Directory;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Common;

namespace HLL.HLX.BE.Core.Business.Common
{
    public  class AddressDomainService : DomainService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address ID
        /// </remarks>
        private const string ADDRESSES_BY_ID_KEY = "Hlx.address.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ADDRESSES_PATTERN_KEY = "Hlx.address.";

        private const string CACHE_NAME_ADDRESS = "cache.address";
        #endregion

        #region Fields

        private readonly IAddressRepository _addressRepository;
        private readonly CountryDomainService _countryService;
        private readonly StateProvinceDomainService _stateProvinceService;
        private readonly AddressAttributeDomainService _addressAttributeService;
        //private readonly IEventPublisher _eventPublisher;
        
        private readonly ICacheManager _cacheManager;

        private readonly IStoreContext _storeContext;
        private readonly SettingDomainService _settingDomainService;


        private  AddressSettings _addressSettings1;
        public AddressSettings AddressSettings
        {
            get
            {
                if (_addressSettings1 == null)
                {
                    _addressSettings1 = _settingDomainService.LoadSetting<AddressSettings>(_storeContext.CurrentStore.Id);
                }
                return _addressSettings1;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="addressRepository">Address repository</param>
        /// <param name="countryService">Country service</param>
        /// <param name="stateProvinceService">State/province service</param>
        /// <param name="addressAttributeService">Address attribute service</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="addressSettings">Address settings</param>
        public AddressDomainService(ICacheManager cacheManager,
            IAddressRepository addressRepository,
            CountryDomainService countryService,
            StateProvinceDomainService stateProvinceService,
            AddressAttributeDomainService addressAttributeService,
            //IEventPublisher eventPublisher,
            IStoreContext storeContext,
            SettingDomainService settingDomainService)
        {
            this._cacheManager = cacheManager;
            this._addressRepository = addressRepository;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._addressAttributeService = addressAttributeService;
            //this._eventPublisher = eventPublisher;
            //this._addressSettings = addressSettings;
            this._storeContext = storeContext;
            _settingDomainService = settingDomainService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an address
        /// </summary>
        /// <param name="address">Address</param>
        public virtual void DeleteAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            _addressRepository.Delete(address);

            //cache
            _cacheManager.GetCache(CACHE_NAME_ADDRESS).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(address);
        }

        /// <summary>
        /// Gets total number of addresses by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Number of addresses</returns>
        public virtual int GetAddressTotalByCountryId(int countryId)
        {
            if (countryId == 0)
                return 0;

            var query = from a in _addressRepository.GetAll()
                        where a.CountryId == countryId
                        select a;
            return query.Count();
        }

        /// <summary>
        /// Gets total number of addresses by state/province identifier
        /// </summary>
        /// <param name="stateProvinceId">State/province identifier</param>
        /// <returns>Number of addresses</returns>
        public virtual int GetAddressTotalByStateProvinceId(int stateProvinceId)
        {
            if (stateProvinceId == 0)
                return 0;

            var query = from a in _addressRepository.GetAll()
                        where a.StateProvinceId == stateProvinceId
                        select a;
            return query.Count();
        }

        /// <summary>
        /// Gets an address by address identifier
        /// </summary>
        /// <param name="addressId">Address identifier</param>
        /// <returns>Address</returns>
        public virtual Address GetAddressById(int addressId)
        {
            if (addressId == 0)
                return null;

            string key = string.Format(ADDRESSES_BY_ID_KEY, addressId);
            return _cacheManager.GetCache(CACHE_NAME_ADDRESS).Get(key, () => _addressRepository.FirstOrDefault(addressId));
        }

        /// <summary>
        /// Inserts an address
        /// </summary>
        /// <param name="address">Address</param>
        public virtual void InsertAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            address.CreatedOnUtc = DateTime.UtcNow;

            //some validation
            if (address.CountryId == 0)
                address.CountryId = null;
            if (address.StateProvinceId == 0)
                address.StateProvinceId = null;

            _addressRepository.Insert(address);

            //cache
            _cacheManager.GetCache(CACHE_NAME_ADDRESS).Clear();

            //event notification
            //_eventPublisher.EntityInserted(address);
        }

        /// <summary>
        /// Updates the address
        /// </summary>
        /// <param name="address">Address</param>
        public virtual void UpdateAddress(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            //some validation
            if (address.CountryId == 0)
                address.CountryId = null;
            if (address.StateProvinceId == 0)
                address.StateProvinceId = null;

            _addressRepository.Update(address);

            //cache
            _cacheManager.GetCache(CACHE_NAME_ADDRESS).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(address);
        }

        /// <summary>
        /// Gets a value indicating whether address is valid (can be saved)
        /// </summary>
        /// <param name="address">Address to validate</param>
        /// <returns>Result</returns>
        public virtual bool IsAddressValid(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            if (String.IsNullOrWhiteSpace(address.FirstName))
                return false;

            if (String.IsNullOrWhiteSpace(address.LastName))
                return false;

            if (String.IsNullOrWhiteSpace(address.Email))
                return false;

            if (AddressSettings.CompanyEnabled &&
                AddressSettings.CompanyRequired &&
                String.IsNullOrWhiteSpace(address.Company))
                return false;

            if (AddressSettings.StreetAddressEnabled &&
                AddressSettings.StreetAddressRequired &&
                String.IsNullOrWhiteSpace(address.Address1))
                return false;

            if (AddressSettings.StreetAddress2Enabled &&
                AddressSettings.StreetAddress2Required &&
                String.IsNullOrWhiteSpace(address.Address2))
                return false;

            if (AddressSettings.ZipPostalCodeEnabled &&
                AddressSettings.ZipPostalCodeRequired &&
                String.IsNullOrWhiteSpace(address.ZipPostalCode))
                return false;


            if (AddressSettings.CountryEnabled)
            {
                if (address.CountryId == null || address.CountryId.Value == 0)
                    return false;

                var country = _countryService.GetCountryById(address.CountryId.Value);
                if (country == null)
                    return false;

                if (AddressSettings.StateProvinceEnabled)
                {
                    var states = _stateProvinceService.GetStateProvincesByCountryId(country.Id);
                    if (states.Count > 0)
                    {
                        if (address.StateProvinceId == null || address.StateProvinceId.Value == 0)
                            return false;

                        var state = states.FirstOrDefault(x => x.Id == address.StateProvinceId.Value);
                        if (state == null)
                            return false;
                    }
                }
            }

            if (AddressSettings.CityEnabled &&
                AddressSettings.CityRequired &&
                String.IsNullOrWhiteSpace(address.City))
                return false;

            if (AddressSettings.PhoneEnabled &&
                AddressSettings.PhoneRequired &&
                String.IsNullOrWhiteSpace(address.PhoneNumber))
                return false;

            if (AddressSettings.FaxEnabled &&
                AddressSettings.FaxRequired &&
                String.IsNullOrWhiteSpace(address.FaxNumber))
                return false;

            var attributes = _addressAttributeService.GetAllAddressAttributes();
            if (attributes.Any(x => x.IsRequired))
                return false;

            return true;
        }

        #endregion
    }
}
