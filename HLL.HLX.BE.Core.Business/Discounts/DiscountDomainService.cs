using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Common.Infrastructure;
using HLL.HLX.BE.Core.Business.Orders;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Discounts;
using HLL.HLX.BE.Core.Model.Orders;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Business.Discounts
{
    public class DiscountDomainService : DomainService
    {

        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : discont ID
        /// </remarks>
        private const string DISCOUNTS_BY_ID_KEY = "Hlx.discount.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : coupon code
        /// {2} : discount name
        /// </remarks>
        private const string DISCOUNTS_ALL_KEY = "Hlx.discount.all-{0}-{1}-{2}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string DISCOUNTS_PATTERN_KEY = "Hlx.discount.";

        private const string CACHE_NAME_DISCOUNTS = "cache.discount";
        #endregion

        #region Fields

        private readonly IDiscountRepository _discountRepository;
        private readonly IDiscountRequirementRepository _discountRequirementRepository;
        private readonly IDiscountUsageHistoryRepository _discountUsageHistoryRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        //private readonly IGenericAttributeService _genericAttributeService;
        //private readonly ILocalizationService _localizationService;
        //private readonly IPluginFinder _pluginFinder;
        //private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="discountRepository">Discount repository</param>
        /// <param name="discountRequirementRepository">Discount requirement repository</param>
        /// <param name="discountUsageHistoryRepository">Discount usage history repository</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="pluginFinder">Plugin finder</param>
        /// <param name="eventPublisher">Event published</param>
        public DiscountDomainService(ICacheManager cacheManager,
            IDiscountRepository discountRepository,
            IDiscountRequirementRepository discountRequirementRepository,
            IDiscountUsageHistoryRepository discountUsageHistoryRepository,
            IStoreContext storeContext
            //IGenericAttributeService genericAttributeService,
            //ILocalizationService localizationService,
            //IPluginFinder pluginFinder,
            //IEventPublisher eventPublisher
            )
        {
            this._cacheManager = cacheManager;
            this._discountRepository = discountRepository;
            this._discountRequirementRepository = discountRequirementRepository;
            this._discountUsageHistoryRepository = discountUsageHistoryRepository;
            this._storeContext = storeContext;
            //this._genericAttributeService = genericAttributeService;
            //this._localizationService = localizationService;
            //this._pluginFinder = pluginFinder;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class DiscountRequirementForCaching
        {
            public int Id { get; set; }
            public string SystemName { get; set; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete discount
        /// </summary>
        /// <param name="discount">Discount</param>
        public virtual void DeleteDiscount(Discount discount)
        {
            if (discount == null)
                throw new ArgumentNullException("discount");

            _discountRepository.Delete(discount);

            _cacheManager.GetCache(CACHE_NAME_DISCOUNTS).Clear();

            //event notification
            //_eventPublisher.EntityDeleted(discount);
        }

        /// <summary>
        /// Gets a discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <returns>Discount</returns>
        public virtual Discount GetDiscountById(int discountId)
        {
            if (discountId == 0)
                return null;

            string key = string.Format(DISCOUNTS_BY_ID_KEY, discountId);
            return _cacheManager.GetCache(CACHE_NAME_DISCOUNTS).Get(key, () => _discountRepository.FirstOrDefault(discountId));
        }

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="discountType">Discount type; null to load all discount</param>
        /// <param name="couponCode">Coupon code to find (exact match)</param>
        /// <param name="discountName">Discount name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discounts</returns>
        public virtual IList<Discount> GetAllDiscounts(DiscountType? discountType,
            string couponCode = "", string discountName = "", bool showHidden = false)
        {
            //we load all discounts, and filter them by passed "discountType" parameter later
            //we do it because we know that this method is invoked several times per HTTP request with distinct "discountType" parameter
            //that's why let's access the database only once
            string key = string.Format(DISCOUNTS_ALL_KEY, showHidden, couponCode, discountName);
            var result = _cacheManager.GetCache(CACHE_NAME_DISCOUNTS).Get(key, () =>
            {
                var query = _discountRepository.GetAll();
                if (!showHidden)
                {
                    //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
                    //That's why we pass the date value
                    var nowUtc = DateTime.UtcNow;
                    query = query.Where(d =>
                        (!d.StartDateUtc.HasValue || d.StartDateUtc <= nowUtc)
                        && (!d.EndDateUtc.HasValue || d.EndDateUtc >= nowUtc)
                        );
                }
                if (!String.IsNullOrEmpty(couponCode))
                {
                    query = query.Where(d => d.CouponCode == couponCode);
                }
                if (!String.IsNullOrEmpty(discountName))
                {
                    query = query.Where(d => d.Name.Contains(discountName));
                }
                query = query.OrderBy(d => d.Name);

                var discounts = query.ToList();
                return discounts;
            });
            //we know that this method is usually inkoved multiple times
            //that's why we filter discounts by type on the application layer
            if (discountType.HasValue)
            {
                result = result.Where(d => d.DiscountType == discountType.Value).ToList();
            }
            return result;
        }

        /// <summary>
        /// Inserts a discount
        /// </summary>
        /// <param name="discount">Discount</param>
        public virtual void InsertDiscount(Discount discount)
        {
            if (discount == null)
                throw new ArgumentNullException("discount");

            _discountRepository.Insert(discount);

            _cacheManager.GetCache(CACHE_NAME_DISCOUNTS).Clear();

            //event notification
            //_eventPublisher.EntityInserted(discount);
        }

        /// <summary>
        /// Updates the discount
        /// </summary>
        /// <param name="discount">Discount</param>
        public virtual void UpdateDiscount(Discount discount)
        {
            if (discount == null)
                throw new ArgumentNullException("discount");

            _discountRepository.Update(discount);

            _cacheManager.GetCache(CACHE_NAME_DISCOUNTS).Clear();

            //event notification
            //_eventPublisher.EntityUpdated(discount);
        }

        /// <summary>
        /// Delete discount requirement
        /// </summary>
        /// <param name="discountRequirement">Discount requirement</param>
        public virtual void DeleteDiscountRequirement(DiscountRequirement discountRequirement)
        {
            if (discountRequirement == null)
                throw new ArgumentNullException("discountRequirement");

            _discountRequirementRepository.Delete(discountRequirement);

            _cacheManager.GetCache(CACHE_NAME_DISCOUNTS);

            //event notification
            //_eventPublisher.EntityDeleted(discountRequirement);
        }

        ///// <summary>
        ///// Load discount requirement rule by system name
        ///// </summary>
        ///// <param name="systemName">System name</param>
        ///// <returns>Found discount requirement rule</returns>
        //public virtual IDiscountRequirementRule LoadDiscountRequirementRuleBySystemName(string systemName)
        //{
        //    var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<IDiscountRequirementRule>(systemName);
        //    if (descriptor != null)
        //        return descriptor.Instance<IDiscountRequirementRule>();

        //    return null;
        //}

        ///// <summary>
        ///// Load all discount requirement rules
        ///// </summary>
        ///// <returns>Discount requirement rules</returns>
        //public virtual IList<IDiscountRequirementRule> LoadAllDiscountRequirementRules()
        //{
        //    var rules = _pluginFinder.GetPlugins<IDiscountRequirementRule>();
        //    return rules.ToList();
        //}

        /// <summary>
        /// Get discount by coupon code
        /// </summary>
        /// <param name="couponCode">Coupon code</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount</returns>
        public virtual Discount GetDiscountByCouponCode(string couponCode, bool showHidden = false)
        {
            if (String.IsNullOrWhiteSpace(couponCode))
                return null;

            var discount = GetAllDiscounts(null, couponCode, null, showHidden).FirstOrDefault();
            return discount;
        }

        /// <summary>
        /// Validate discount
        /// </summary>
        /// <param name="discount">Discount</param>
        /// <param name="customer">Customer</param>
        /// <returns>Discount validation result</returns>
        public virtual DiscountValidationResult ValidateDiscount(Discount discount, User customer)
        {
            if (discount == null)
                throw new ArgumentNullException("discount");

            var couponCodeToValidate = "";
            //if (customer != null)
            //    couponCodeToValidate = customer.GetAttribute<string>(SystemCustomerAttributeNames.DiscountCouponCode, _genericAttributeService);

            return ValidateDiscount(discount, customer, couponCodeToValidate);
        }

        /// <summary>
        /// Validate discount
        /// </summary>
        /// <param name="discount">Discount</param>
        /// <param name="customer">Customer</param>
        /// <param name="couponCodeToValidate">Coupon code to validate</param>
        /// <returns>Discount validation result</returns>
        public virtual DiscountValidationResult ValidateDiscount(Discount discount, User customer, string couponCodeToValidate)
        {
            if (discount == null)
                throw new ArgumentNullException("discount");

            if (customer == null)
                throw new ArgumentNullException("customer");

            //invalid by default
            var result = new DiscountValidationResult();

            //check coupon code
            if (discount.RequiresCouponCode)
            {
                if (String.IsNullOrEmpty(discount.CouponCode))
                    return result;
                if (!discount.CouponCode.Equals(couponCodeToValidate, StringComparison.InvariantCultureIgnoreCase))
                    return result;
            }

            //Do not allow discounts applied to order subtotal or total when a customer has gift cards in the cart.
            //Otherwise, this customer can purchase gift cards with discount and get more than paid ("free money").
            if (discount.DiscountType == DiscountType.AssignedToOrderSubTotal ||
                discount.DiscountType == DiscountType.AssignedToOrderTotal)
            {
                var cart = customer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();

                var hasGiftCards = cart.Any(x => x.Product.IsGiftCard);
                if (hasGiftCards)
                {
                    result.UserError = InfoMsg.ShoppingCart_Discount_CannotBeUsedWithGiftCards;
                        
                    return result;
                }
            }

            //check date range
            DateTime now = DateTime.UtcNow;
            if (discount.StartDateUtc.HasValue)
            {
                DateTime startDate = DateTime.SpecifyKind(discount.StartDateUtc.Value, DateTimeKind.Utc);
                if (startDate.CompareTo(now) > 0)
                {
                    result.UserError = InfoMsg.ShoppingCart_Discount_NotStartedYet;
                        
                    return result;
                }
            }
            if (discount.EndDateUtc.HasValue)
            {
                DateTime endDate = DateTime.SpecifyKind(discount.EndDateUtc.Value, DateTimeKind.Utc);
                if (endDate.CompareTo(now) < 0)
                {
                    result.UserError = InfoMsg.ShoppingCart_Discount_Expired;
                    return result;
                }
            }

            //discount limitation
            switch (discount.DiscountLimitation)
            {
                case DiscountLimitationType.NTimesOnly:
                    {
                        var usedTimes = GetAllDiscountUsageHistory(discount.Id, null, null, 0, 1).TotalCount;
                        if (usedTimes >= discount.LimitationTimes)
                            return result;
                    }
                    break;
                case DiscountLimitationType.NTimesPerCustomer:
                    {
                        //if (customer.IsRegistered())
                        {
                            var usedTimes = GetAllDiscountUsageHistory(discount.Id, (int)customer.Id, null, 0, 1).TotalCount;
                            if (usedTimes >= discount.LimitationTimes)
                            {
                                result.UserError = InfoMsg.ShoppingCart_Discount_CannotBeUsedAnymore;
                                return result;
                            }
                        }
                    }
                    break;
                case DiscountLimitationType.Unlimited:
                default:
                    break;
            }

            //discount requirements
            //UNDONE we should inject static cache manager into constructor. we we already have "per request" cache manager injected. better way to do it?
            //we cache meta info of rdiscount requirements. this way we should not load them for each HTTP request
            //var staticCacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
            //string key = string.Format(DiscountRequirementEventConsumer.DISCOUNT_REQUIREMENT_MODEL_KEY, discount.Id);
            //var requirements = discount.DiscountRequirements;
            //var requirements = staticCacheManager.Get(key, () =>
            //{
                var cachedRequirements = new List<DiscountRequirementForCaching>();
                foreach (var dr in discount.DiscountRequirements)
                    cachedRequirements.Add(new DiscountRequirementForCaching
                    {
                        Id = dr.Id,
                        SystemName = dr.DiscountRequirementRuleSystemName
                    });
                //return cachedRequirements;
                var requirements = cachedRequirements;
            //});
            foreach (var req in requirements)
            {
                //load a plugin
                //var requirementRulePlugin = LoadDiscountRequirementRuleBySystemName(req.SystemName);
                //if (requirementRulePlugin == null)
                //    continue;
                //if (!_pluginFinder.AuthenticateStore(requirementRulePlugin.PluginDescriptor, _storeContext.CurrentStore.Id))
                //    continue;

                var ruleRequest = new DiscountRequirementValidationRequest
                {
                    DiscountRequirementId = req.Id,
                    Customer = customer,
                    Store = _storeContext.CurrentStore
                };
                //var ruleResult = requirementRulePlugin.CheckRequirement(ruleRequest);
                //if (!ruleResult.IsValid)
                //{
                //    result.UserError = ruleResult.UserError;
                //    return result;
                //}
            }

            result.IsValid = true;
            return result;
        }

        /// <summary>
        /// Gets a discount usage history record
        /// </summary>
        /// <param name="discountUsageHistoryId">Discount usage history record identifier</param>
        /// <returns>Discount usage history</returns>
        public virtual DiscountUsageHistory GetDiscountUsageHistoryById(int discountUsageHistoryId)
        {
            if (discountUsageHistoryId == 0)
                return null;

            return _discountUsageHistoryRepository.FirstOrDefault(discountUsageHistoryId);
        }

        /// <summary>
        /// Gets all discount usage history records
        /// </summary>
        /// <param name="discountId">Discount identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Discount usage history records</returns>
        public virtual IPagedList<DiscountUsageHistory> GetAllDiscountUsageHistory(int? discountId = null,
            int? customerId = null, int? orderId = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _discountUsageHistoryRepository.GetAll();
            if (discountId.HasValue && discountId.Value > 0)
                query = query.Where(duh => duh.DiscountId == discountId.Value);
            if (customerId.HasValue && customerId.Value > 0)
                query = query.Where(duh => duh.Order != null && duh.Order.CustomerId == customerId.Value);
            if (orderId.HasValue && orderId.Value > 0)
                query = query.Where(duh => duh.OrderId == orderId.Value);
            query = query.OrderByDescending(c => c.CreatedOnUtc);
            return new PagedList<DiscountUsageHistory>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Insert discount usage history record
        /// </summary>
        /// <param name="discountUsageHistory">Discount usage history record</param>
        public virtual void InsertDiscountUsageHistory(DiscountUsageHistory discountUsageHistory)
        {
            if (discountUsageHistory == null)
                throw new ArgumentNullException("discountUsageHistory");

            _discountUsageHistoryRepository.Insert(discountUsageHistory);

            _cacheManager.GetCache(CACHE_NAME_DISCOUNTS).Clear();

            //event notification
            //_eventPublisher.EntityInserted(discountUsageHistory);
        }

        /// <summary>
        /// Update discount usage history record
        /// </summary>
        /// <param name="discountUsageHistory">Discount usage history record</param>
        public virtual void UpdateDiscountUsageHistory(DiscountUsageHistory discountUsageHistory)
        {
            if (discountUsageHistory == null)
                throw new ArgumentNullException("discountUsageHistory");

            _discountUsageHistoryRepository.Update(discountUsageHistory);

            _cacheManager.GetCache(CACHE_NAME_DISCOUNTS);

            //event notification
            //_eventPublisher.EntityUpdated(discountUsageHistory);
        }

        /// <summary>
        /// Delete discount usage history record
        /// </summary>
        /// <param name="discountUsageHistory">Discount usage history record</param>
        public virtual void DeleteDiscountUsageHistory(DiscountUsageHistory discountUsageHistory)
        {
            if (discountUsageHistory == null)
                throw new ArgumentNullException("discountUsageHistory");

            _discountUsageHistoryRepository.Delete(discountUsageHistory);

            _cacheManager.GetCache(CACHE_NAME_DISCOUNTS);

            //event notification
            //_eventPublisher.EntityDeleted(discountUsageHistory);
        }

        #endregion
    }
}
