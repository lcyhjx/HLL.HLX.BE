using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abp.Authorization;
using Abp.UI;
using HLL.HLX.BE.Application.Common;
using HLL.HLX.BE.Application.Common.Dto;
using HLL.HLX.BE.Application.MobilityH5.Catalog.Dto;
using HLL.HLX.BE.Application.MobilityH5.Orders.Dto;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Common.Util;
using HLL.HLX.BE.Core.Business.Catalog;
using HLL.HLX.BE.Core.Business.Common;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Directory;
using HLL.HLX.BE.Core.Business.Discounts;
using HLL.HLX.BE.Core.Business.Media;
using HLL.HLX.BE.Core.Business.Orders;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Business.Tax;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Common;
using HLL.HLX.BE.Core.Model.Directory;
using HLL.HLX.BE.Core.Model.Discounts;
using HLL.HLX.BE.Core.Model.Media;
using HLL.HLX.BE.Core.Model.Orders;
using HLL.HLX.BE.Core.Model.Shipping;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Application.MobilityH5.Orders
{
    public class ShoppingCartAppService : HlxBeAppServiceBase, IShoppingCartAppService
    {
        #region Fields

        private readonly ProductDomainService _productDomainService;
        private readonly SettingDomainService _settingDomainService;
        private readonly CurrencyDomainService _currencyDomainService;
        private readonly ShoppingCartDomainService _shoppingCartDomainService;
        private readonly ProductAttributeDomainService _productAttributeDomainService;
        private readonly GenericAttributeDomianService _genericAttributeDomianService;
        private readonly CheckoutAttributeDomainService _checkoutAttributeDomainService;
        private readonly DiscountDomainService _discountDomainService;
        private readonly TaxDomainService _taxDomainService;
        private readonly CountryDomainService _countryDomainService;
        private readonly StateProvinceDomainService _stateProvinceDomainService;
        private readonly PriceCalculationDomainService _priceCalculationDomainService;
        private readonly PictureDomainService _pictureDomainService;

        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly ICheckoutAttributeFormatter _checkoutAttributeFormatter;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Properties

        private ShoppingCartSettings _shoppingCartSettings1;
        public ShoppingCartSettings ShoppingCartSettings
        {
            get
            {
                if (_shoppingCartSettings1 == null)
                {
                    _shoppingCartSettings1 =
                        _settingDomainService.LoadSetting<ShoppingCartSettings>(_storeContext.CurrentStore.Id);
                }
                return _shoppingCartSettings1;
            }
        }

        private OrderSettings _orderSettings1;
        public OrderSettings OrderSettings
        {
            get
            {
                if (_orderSettings1 == null)
                {
                    _orderSettings1 =
                        _settingDomainService.LoadSetting<OrderSettings>(_storeContext.CurrentStore.Id);
                }
                return _orderSettings1;
            }
        }

        private CatalogSettings _catalogSettings1;
        public CatalogSettings CatalogSettings
        {
            get
            {
                if (_catalogSettings1 == null)
                {
                    _catalogSettings1 =
                        _settingDomainService.LoadSetting<CatalogSettings>(_storeContext.CurrentStore.Id);
                }
                return _catalogSettings1;
            }
        }


        private ShippingSettings _shippingSettings1;
        public ShippingSettings ShippingSettings
        {
            get
            {
                if (_shippingSettings1 == null)
                {
                    _shippingSettings1 =
                        _settingDomainService.LoadSetting<ShippingSettings>(_storeContext.CurrentStore.Id);
                }
                return _shippingSettings1;
            }
        }

        private MediaSettings _mediaSettings1;
        public MediaSettings MediaSettings
        {
            get
            {
                if (_mediaSettings1 == null)
                {
                    _mediaSettings1 =
                        _settingDomainService.LoadSetting<MediaSettings>(_storeContext.CurrentStore.Id);
                }
                return _mediaSettings1;
            }
        }


        private AddressSettings _addressSettings1;
        public AddressSettings AddressSettings
        {
            get
            {
                if (_addressSettings1 == null)
                {
                    _addressSettings1 =
                        _settingDomainService.LoadSetting<AddressSettings>(_storeContext.CurrentStore.Id);
                }
                return _addressSettings1;
            }
        }


        private Currency _cachedCurrency;
        /// <summary>
        ///     Get or set current user working currency
        /// </summary>
        public virtual Currency WorkingCurrency
        {
            get
            {
                if (_cachedCurrency != null)
                    return _cachedCurrency;

                //it not specified, then return the first found one
                _cachedCurrency = _currencyDomainService.GetAllCurrencies().FirstOrDefault();
                return _cachedCurrency;
            }
        }

        #endregion

        #region Constructors

        public ShoppingCartAppService(ProductDomainService productDomainService
            , SettingDomainService settingDomainService
            , CurrencyDomainService currencyDomainService
            , ShoppingCartDomainService shoppingCartDomainService
            , ProductAttributeDomainService productAttributeDomainService
            , GenericAttributeDomianService genericAttributeDomianService
            , CheckoutAttributeDomainService checkoutAttributeDomainService
            , DiscountDomainService discountDomainService
            , TaxDomainService taxDomainService
            , CountryDomainService countryDomainService
            , StateProvinceDomainService stateProvinceDomainService
            , PriceCalculationDomainService priceCalculationDomainService
            , PictureDomainService pictureDomainService

            , IProductAttributeParser productAttributeParser
            , IProductAttributeFormatter productAttributeFormatter
            , ICheckoutAttributeFormatter checkoutAttributeFormatter
            , ICheckoutAttributeParser checkoutAttributeParser
            , IPriceFormatter priceFormatter
            , IAddressAttributeFormatter addressAttributeFormatter
            , IStoreContext storeContext)
        {
            _productDomainService = productDomainService;
            _settingDomainService = settingDomainService;
            _currencyDomainService = currencyDomainService;
            _shoppingCartDomainService = shoppingCartDomainService;
            _productAttributeDomainService = productAttributeDomainService;
            _genericAttributeDomianService = genericAttributeDomianService;
            _checkoutAttributeDomainService = checkoutAttributeDomainService;
            _discountDomainService = discountDomainService;
            _taxDomainService = taxDomainService;
            _countryDomainService = countryDomainService;
            _stateProvinceDomainService = stateProvinceDomainService;
            _priceCalculationDomainService = priceCalculationDomainService;
            _pictureDomainService = pictureDomainService;

            _productAttributeParser = productAttributeParser;
            _productAttributeFormatter = productAttributeFormatter;
            _checkoutAttributeFormatter = checkoutAttributeFormatter;
            _checkoutAttributeParser = checkoutAttributeParser;
            _priceFormatter = priceFormatter;
            _addressAttributeFormatter = addressAttributeFormatter;
            _storeContext = storeContext;
        }

        #endregion

        #region Shopping cart

        /// <summary>
        /// 添加产品到购物车- currently we use this method on the product details pages
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public AddProductToCartForDetailsOutput AddProductToCartForDetails(AddProductToCartForDetailsInput input)
        {
            var productId = input.ProductId.GetValueOrDefault();

            var product = _productDomainService.GetProductById(productId);
            if (product == null)
            {
                throw new UserFriendlyException(string.Format("商品(Id:{0})不存在", productId));
            }

            //we can add only simple products
            if (product.ProductType != ProductType.SimpleProduct)
            {
                throw new UserFriendlyException("Only simple products could be added to the cart");
            }

            #region Update existing shopping cart item?

            var updatecartitemid = 0;
            //foreach (string formKey in form.AllKeys)
            //    if (formKey.Equals(string.Format("addtocart_{0}.UpdatedShoppingCartItemId", productId), StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        int.TryParse(form[formKey], out updatecartitemid);
            //        break;
            //    }
            ShoppingCartItem updatecartitem = null;
            if (ShoppingCartSettings.AllowCartItemEditing && updatecartitemid > 0)
            {
                var cart = CurrentUser.ShoppingCartItems
                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                updatecartitem = cart.FirstOrDefault(x => x.Id == updatecartitemid);
                //not found?
                if (updatecartitem == null)
                {
                    throw new UserFriendlyException("No shopping cart item found to update");
                }
                //is it this product?
                if (product.Id != updatecartitem.ProductId)
                {
                    throw new UserFriendlyException("This product does not match a passed shopping cart item identifier");
                }
            }

            #endregion

            #region Customer entered price

            var customerEnteredPriceConverted = decimal.Zero;
            if (product.CustomerEntersPrice && input.CustomerEnteredPrice != null)
            {
                customerEnteredPriceConverted =
                    _currencyDomainService.ConvertToPrimaryStoreCurrency(input.CustomerEnteredPrice.Value,
                        WorkingCurrency);
            }

            #endregion

            #region Quantity

            var quantity = 1;
            if (input.Quantity != null && input.Quantity > 0)
            {
                quantity = input.Quantity.Value;
            }

            #endregion

            //product and gift card attributes
            var attributes = ParseProductAttributes(product, input.CartItemAttributes, input.CartItemGiftCard);

            //rental attributes
            DateTime? rentalStartDate = null;
            DateTime? rentalEndDate = null;
            if (product.IsRental)
            {
                rentalStartDate = input.RentalStartDate;
                rentalEndDate = input.RentalEndDate;
            }

            //save item
            var addToCartWarnings = new List<string>();
            var cartType = input.ShoppingCartTypeId;
            if (updatecartitem == null)
            {
                //add to the cart
                addToCartWarnings.AddRange(_shoppingCartDomainService.AddToCart(CurrentUser,
                    product, cartType, _storeContext.CurrentStore.Id,
                    attributes, customerEnteredPriceConverted,
                    rentalStartDate, rentalEndDate, quantity, true));
            }
            else
            {
                var cart = CurrentUser.ShoppingCartItems
                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                var otherCartItemWithSameParameters = _shoppingCartDomainService.FindShoppingCartItemInTheCart(
                    cart, cartType, product, attributes, customerEnteredPriceConverted,
                    rentalStartDate, rentalEndDate);
                if (otherCartItemWithSameParameters != null &&
                    otherCartItemWithSameParameters.Id == updatecartitem.Id)
                {
                    //ensure it's other shopping cart cart item
                    otherCartItemWithSameParameters = null;
                }
                //update existing item
                addToCartWarnings.AddRange(_shoppingCartDomainService.UpdateShoppingCartItem(CurrentUser,
                    updatecartitem.Id, attributes, customerEnteredPriceConverted,
                    rentalStartDate, rentalEndDate, quantity, true));
                if (otherCartItemWithSameParameters != null && addToCartWarnings.Count == 0)
                {
                    //delete the same shopping cart item (the other one)
                    _shoppingCartDomainService.DeleteShoppingCartItem(otherCartItemWithSameParameters);
                }
            }

            #region Return result

            if (addToCartWarnings.Count > 0)
            {
                //cannot be added to the cart/wishlist
                //let's display warnings                
                throw new UserFriendlyException(string.Join(";", addToCartWarnings));
            }

            ////added to the cart/wishlist
            //switch (cartType)
            //{
            //    case ShoppingCartType.Wishlist:
            //        {
            //            //activity log
            //            _customerActivityService.InsertActivity("PublicStore.AddToWishlist", _localizationService.GetResource("ActivityLog.PublicStore.AddToWishlist"), product.Name);

            //            if (_shoppingCartSettings.DisplayWishlistAfterAddingProduct)
            //            {
            //                //redirect to the wishlist page
            //                return Json(new
            //                {
            //                    redirect = Url.RouteUrl("Wishlist"),
            //                });
            //            }

            //            //display notification message and update appropriate blocks
            //            var updatetopwishlistsectionhtml = string.Format(_localizationService.GetResource("Wishlist.HeaderQuantity"),
            //            CurrentUser.ShoppingCartItems
            //            .Where(sci => sci.ShoppingCartType == ShoppingCartType.Wishlist)
            //            .LimitPerStore(_storeContext.CurrentStore.Id)
            //            .ToList()
            //            .GetTotalProducts());

            //            return Json(new
            //            {
            //                success = true,
            //                message = string.Format(_localizationService.GetResource("Products.ProductHasBeenAddedToTheWishlist.Link"), Url.RouteUrl("Wishlist")),
            //                updatetopwishlistsectionhtml = updatetopwishlistsectionhtml,
            //            });
            //        }
            //    case ShoppingCartType.ShoppingCart:
            //    default:
            //        {
            //            //activity log
            //            _customerActivityService.InsertActivity("PublicStore.AddToShoppingCart", _localizationService.GetResource("ActivityLog.PublicStore.AddToShoppingCart"), product.Name);

            //            if (_shoppingCartSettings.DisplayCartAfterAddingProduct)
            //            {
            //                //redirect to the shopping cart page
            //                return Json(new
            //                {
            //                    redirect = Url.RouteUrl("ShoppingCart"),
            //                });
            //            }

            //            //display notification message and update appropriate blocks
            //            var updatetopcartsectionhtml = string.Format(_localizationService.GetResource("ShoppingCart.HeaderQuantity"),
            //            _workContext.CurrentCustomer.ShoppingCartItems
            //            .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
            //            .LimitPerStore(_storeContext.CurrentStore.Id)
            //            .ToList()
            //            .GetTotalProducts());

            //            var updateflyoutcartsectionhtml = _shoppingCartSettings.MiniShoppingCartEnabled
            //                ? this.RenderPartialViewToString("FlyoutShoppingCart", PrepareMiniShoppingCartModel())
            //                : "";

            //            return Json(new
            //            {
            //                success = true,
            //                message = string.Format(_localizationService.GetResource("Products.ProductHasBeenAddedToTheCart.Link"), Url.RouteUrl("ShoppingCart")),
            //                updatetopcartsectionhtml = updatetopcartsectionhtml,
            //                updateflyoutcartsectionhtml = updateflyoutcartsectionhtml
            //            });
            //        }
            //}

            return new AddProductToCartForDetailsOutput();

            #endregion
        }

        #endregion

        #region Utilities

        /// <summary>
        ///     Parse product attributes on the product details page
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="form">Form</param>
        /// <returns>Parsed attributes</returns>
        protected virtual string ParseProductAttributes(Product product, List<CartItemAttributeDto> cartItemAttributes,
            CartItemGiftCardDto cartItemGiftCard)
        {
            var attributesXml = "";

            if (cartItemAttributes == null)
            {
                cartItemAttributes = new List<CartItemAttributeDto>();
            }
            var dicAttributes = new Dictionary<int, string>();
            foreach (var item in cartItemAttributes)
            {
                if (!dicAttributes.ContainsKey(item.AttributeId))
                {
                    dicAttributes.Add(item.AttributeId, item.Values);
                }
            }

            #region Product attributes

            var productAttributes = _productAttributeDomainService.GetProductAttributeMappingsByProductId(product.Id);
            foreach (var attribute in productAttributes)
            {
                //string controlId = string.Format("product_attribute_{0}", attribute.Id);
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                    case AttributeControlType.ColorSquares:
                        {
                            var ctrlAttributes = dicAttributes.ContainsKey(attribute.Id)
                                ? dicAttributes[attribute.Id]
                                : string.Empty;
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                var selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.Checkboxes:
                        {
                            var ctrlAttributes = dicAttributes.ContainsKey(attribute.Id)
                                ? dicAttributes[attribute.Id]
                                : string.Empty;
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                foreach (
                                    var item in ctrlAttributes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    var selectedAttributeId = int.Parse(item);
                                    if (selectedAttributeId > 0)
                                        attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                            attribute, selectedAttributeId.ToString());
                                }
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //load read-only (already server-side selected) values
                            var attributeValues = _productAttributeDomainService.GetProductAttributeValues(attribute.Id);
                            foreach (var selectedAttributeId in attributeValues
                                .Where(v => v.IsPreSelected)
                                .Select(v => v.Id)
                                .ToList())
                            {
                                attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            var ctrlAttributes = dicAttributes.ContainsKey(attribute.Id)
                                ? dicAttributes[attribute.Id]
                                : string.Empty;
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                var enteredText = ctrlAttributes.Trim();
                                attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                    attribute, enteredText);
                            }
                        }
                        break;
                    case AttributeControlType.Datepicker:
                        {
                            var ctrlAttributes = dicAttributes.ContainsKey(attribute.Id)
                                ? dicAttributes[attribute.Id]
                                : string.Empty;
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                            }
                            DateTime? selectedDate = null;
                            try
                            {
                                selectedDate = DateTime.Parse(ctrlAttributes);
                            }
                            catch
                            {
                            }
                            if (selectedDate.HasValue)
                            {
                                attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                    attribute, selectedDate.Value.ToString("D"));
                            }
                        }
                        break;
                    case AttributeControlType.FileUpload:
                        {
                            //Guid downloadGuid;
                            //var ctrlAttributes = dicAttributes.ContainsKey(attribute.Id) ? dicAttributes[attribute.Id] : string.Empty;
                            //Guid.TryParse(ctrlAttributes, out downloadGuid);
                            //var download = _downloadService.GetDownloadByGuid(downloadGuid);
                            //if (download != null)
                            //{
                            //    attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                            //            attribute, download.DownloadGuid.ToString());
                            //}
                        }
                        break;
                    default:
                        break;
                }
            }
            //validate conditional attributes (if specified)
            foreach (var attribute in productAttributes)
            {
                var conditionMet = _productAttributeParser.IsConditionMet(attribute, attributesXml);
                if (conditionMet.HasValue && !conditionMet.Value)
                {
                    attributesXml = _productAttributeParser.RemoveProductAttribute(attributesXml, attribute);
                }
            }

            #endregion

            #region Gift cards

            if (product.IsGiftCard && cartItemGiftCard != null)
            {
                var recipientName = cartItemGiftCard.RecipientName;
                var recipientEmail = cartItemGiftCard.RecipientEmail;
                var senderName = cartItemGiftCard.SenderName;
                var senderEmail = cartItemGiftCard.SenderEmail;
                var giftCardMessage = cartItemGiftCard.Message;

                attributesXml = _productAttributeParser.AddGiftCardAttribute(attributesXml,
                    recipientName, recipientEmail, senderName, senderEmail, giftCardMessage);
            }

            #endregion

            return attributesXml;
        }

        /// <summary>
        /// Prepare shopping cart model
        /// </summary>
        /// <param name="model">Model instance</param>
        /// <param name="cart">Shopping cart</param>
        /// <param name="isEditable">A value indicating whether cart is editable</param>
        /// <param name="validateCheckoutAttributes">A value indicating whether we should validate checkout attributes when preparing the model</param>
        /// <param name="prepareEstimateShippingIfEnabled">A value indicating whether we should prepare "Estimate shipping" model</param>
        /// <param name="setEstimateShippingDefaultAddress">A value indicating whether we should prefill "Estimate shipping" model with the default customer address</param>
        /// <param name="prepareAndDisplayOrderReviewData">A value indicating whether we should prepare review data (such as billing/shipping address, payment or shipping data entered during checkout)</param>
        /// <returns>Model</returns>        
        protected virtual void PrepareShoppingCartModel(ShoppingCartModel model,
            IList<ShoppingCartItem> cart, bool isEditable = true,
            bool validateCheckoutAttributes = false,
            bool prepareEstimateShippingIfEnabled = true, bool setEstimateShippingDefaultAddress = true,
            bool prepareAndDisplayOrderReviewData = false)
        {
            if (cart == null)
                throw new UserFriendlyException("cart is null");

            if (model == null)
                throw new UserFriendlyException("model is null");

            model.OnePageCheckoutEnabled = OrderSettings.OnePageCheckoutEnabled;

            if (cart.Count == 0)
                return;

            #region Simple properties

            model.IsEditable = isEditable;
            model.ShowProductImages = ShoppingCartSettings.ShowProductImagesOnShoppingCart;
            model.ShowSku = CatalogSettings.ShowProductSku;

            var checkoutAttributesXml = CurrentUser.GetAttribute<string>(SystemCustomerAttributeNames.CheckoutAttributes, _genericAttributeDomianService, _storeContext.CurrentStore.Id);
            model.CheckoutAttributeInfo = _checkoutAttributeFormatter.FormatAttributes(checkoutAttributesXml, CurrentUser, WorkingCurrency);
            //bool minOrderSubtotalAmountOk = _orderProcessingService.ValidateMinOrderSubtotalAmount(cart);
            //if (!minOrderSubtotalAmountOk)
            //{
            //    decimal minOrderSubtotalAmount = _currencyDomainService.ConvertFromPrimaryStoreCurrency(OrderSettings.MinOrderSubtotalAmount, WorkingCurrency);
            //    model.MinOrderSubtotalWarning = string.Format(_localizationService.GetResource("Checkout.MinOrderSubtotalAmount"), _priceFormatter.FormatPrice(minOrderSubtotalAmount, true, false, WorkingCurrency));
            //}

            model.TermsOfServiceOnShoppingCartPage = OrderSettings.TermsOfServiceOnShoppingCartPage;
            model.TermsOfServiceOnOrderConfirmPage = OrderSettings.TermsOfServiceOnOrderConfirmPage;
            model.DisplayTaxShippingInfo = CatalogSettings.DisplayTaxShippingInfoShoppingCart;

            //gift card and gift card boxes
            model.DiscountBox.Display = ShoppingCartSettings.ShowDiscountBox;
            var discountCouponCode = CurrentUser.GetAttribute<string>(SystemCustomerAttributeNames.DiscountCouponCode, _genericAttributeDomianService);

            var discount = _discountDomainService.GetDiscountByCouponCode(discountCouponCode);
            if (discount != null &&
                discount.RequiresCouponCode &&
                _discountDomainService.ValidateDiscount(discount, CurrentUser).IsValid)
                model.DiscountBox.CurrentCode = discount.CouponCode;
            model.GiftCardBox.Display = ShoppingCartSettings.ShowGiftCardBox;

            //cart warnings            
            var cartWarnings = _shoppingCartDomainService.GetShoppingCartWarnings(cart, checkoutAttributesXml, validateCheckoutAttributes);
            foreach (var warning in cartWarnings)
                model.Warnings.Add(warning);

            #endregion

            #region Checkout attributes

            var checkoutAttributes = _checkoutAttributeDomainService.GetAllCheckoutAttributes(_storeContext.CurrentStore.Id, !cart.RequiresShipping());
            foreach (var attribute in checkoutAttributes)
            {
                var attributeModel = new ShoppingCartModel.CheckoutAttributeModel
                {
                    Id = attribute.Id,
                    Name = attribute.Name,
                    TextPrompt = attribute.TextPrompt,
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType,
                    DefaultValue = attribute.DefaultValue
                };
                if (!String.IsNullOrEmpty(attribute.ValidationFileAllowedExtensions))
                {
                    attributeModel.AllowedFileExtensions = attribute.ValidationFileAllowedExtensions
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                }

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _checkoutAttributeDomainService.GetCheckoutAttributeValues(attribute.Id);
                    foreach (var attributeValue in attributeValues)
                    {
                        var attributeValueModel = new ShoppingCartModel.CheckoutAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = attributeValue.Name,
                            ColorSquaresRgb = attributeValue.ColorSquaresRgb,
                            IsPreSelected = attributeValue.IsPreSelected,
                        };
                        attributeModel.Values.Add(attributeValueModel);

                        //display price if allowed
                        //if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
                        {

                            decimal priceAdjustmentBase = _taxDomainService.GetCheckoutAttributePrice(attributeValue, CurrentUser);
                            decimal priceAdjustment = _currencyDomainService.ConvertFromPrimaryStoreCurrency(priceAdjustmentBase, WorkingCurrency);
                            if (priceAdjustmentBase > decimal.Zero)
                                attributeValueModel.PriceAdjustment = "+" + _priceFormatter.FormatPrice(priceAdjustment, true, WorkingCurrency);
                            else if (priceAdjustmentBase < decimal.Zero)
                                attributeValueModel.PriceAdjustment = "-" + _priceFormatter.FormatPrice(-priceAdjustment, true, WorkingCurrency);
                        }
                    }
                }



                //set already selected attributes

                var selectedCheckoutAttributes = CurrentUser.GetAttribute<string>(SystemCustomerAttributeNames.CheckoutAttributes, _genericAttributeDomianService, _storeContext.CurrentStore.Id);
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                    case AttributeControlType.Checkboxes:
                    case AttributeControlType.ColorSquares:
                        {
                            if (!String.IsNullOrEmpty(selectedCheckoutAttributes))
                            {
                                //clear default selection
                                foreach (var item in attributeModel.Values)
                                    item.IsPreSelected = false;

                                //select new values
                                var selectedValues = _checkoutAttributeParser.ParseCheckoutAttributeValues(selectedCheckoutAttributes);
                                foreach (var attributeValue in selectedValues)
                                    foreach (var item in attributeModel.Values)
                                        if (attributeValue.Id == item.Id)
                                            item.IsPreSelected = true;
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //do nothing
                            //values are already pre-set
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            if (!String.IsNullOrEmpty(selectedCheckoutAttributes))
                            {
                                var enteredText = _checkoutAttributeParser.ParseValues(selectedCheckoutAttributes, attribute.Id);
                                if (enteredText.Count > 0)
                                    attributeModel.DefaultValue = enteredText[0];
                            }
                        }
                        break;
                    case AttributeControlType.Datepicker:
                        {
                            //keep in mind my that the code below works only in the current culture
                            var selectedDateStr = _checkoutAttributeParser.ParseValues(selectedCheckoutAttributes, attribute.Id);
                            if (selectedDateStr.Count > 0)
                            {
                                DateTime selectedDate;
                                if (DateTime.TryParseExact(selectedDateStr[0], "D", CultureInfo.CurrentCulture,
                                                       DateTimeStyles.None, out selectedDate))
                                {
                                    //successfully parsed
                                    attributeModel.SelectedDay = selectedDate.Day;
                                    attributeModel.SelectedMonth = selectedDate.Month;
                                    attributeModel.SelectedYear = selectedDate.Year;
                                }
                            }

                        }
                        break;
                    case AttributeControlType.FileUpload:
                        {
                            //if (!String.IsNullOrEmpty(selectedCheckoutAttributes))
                            //{
                            //    var downloadGuidStr = _checkoutAttributeParser.ParseValues(selectedCheckoutAttributes, attribute.Id).FirstOrDefault();
                            //    Guid downloadGuid;
                            //    Guid.TryParse(downloadGuidStr, out downloadGuid);
                            //    var download = _downloadService.GetDownloadByGuid(downloadGuid);
                            //    if (download != null)
                            //        attributeModel.DefaultValue = download.DownloadGuid.ToString();
                            //}
                        }
                        break;
                    default:
                        break;
                }

                model.CheckoutAttributes.Add(attributeModel);
            }

            #endregion


            #region Estimate shipping


            if (prepareEstimateShippingIfEnabled)
            {
                model.EstimateShipping.Enabled = cart.Count > 0 && cart.RequiresShipping() && ShippingSettings.EstimateShippingEnabled;
                if (model.EstimateShipping.Enabled)
                {
                    //countries
                    int? defaultEstimateCountryId = (setEstimateShippingDefaultAddress && CurrentUser.ShippingAddress != null) ? CurrentUser.ShippingAddress.CountryId : model.EstimateShipping.CountryId;
                    model.EstimateShipping.AvailableCountries.Add(new SelectListItemDto { Text = InfoMsg.Address_SelectCountry, Value = "0" });
                    foreach (var c in _countryDomainService.GetAllCountriesForShipping())
                        model.EstimateShipping.AvailableCountries.Add(new SelectListItemDto
                        {
                            Text = c.Name,
                            Value = c.Id.ToString(),
                            Selected = c.Id == defaultEstimateCountryId
                        });
                    //states

                    int? defaultEstimateStateId = (setEstimateShippingDefaultAddress && CurrentUser.ShippingAddress != null) ? CurrentUser.ShippingAddress.StateProvinceId : model.EstimateShipping.StateProvinceId;
                    var states = defaultEstimateCountryId.HasValue ? _stateProvinceDomainService.GetStateProvincesByCountryId(defaultEstimateCountryId.Value).ToList() : new List<StateProvince>();
                    if (states.Count > 0)
                        foreach (var s in states)
                            model.EstimateShipping.AvailableStates.Add(new SelectListItemDto
                            {
                                Text = s.Name,
                                Value = s.Id.ToString(),
                                Selected = s.Id == defaultEstimateStateId
                            });
                    else
                        model.EstimateShipping.AvailableStates.Add(new SelectListItemDto { Text = InfoMsg.Address_OtherNonUS, Value = "0" });

                    if (setEstimateShippingDefaultAddress && CurrentUser.ShippingAddress != null)
                        model.EstimateShipping.ZipPostalCode = CurrentUser.ShippingAddress.ZipPostalCode;
                }
            }

            #endregion

            #region Cart items

            foreach (var sci in cart)
            {
                var cartItemModel = new ShoppingCartModel.ShoppingCartItemModel
                {
                    Id = sci.Id,
                    Sku = sci.Product.FormatSku(sci.AttributesXml, _productAttributeParser),
                    ProductId = sci.Product.Id,
                    ProductName = sci.Product.Name,
                    //ProductSeName = sci.Product.GetSeName(),
                    Quantity = sci.Quantity,
                    AttributeInfo = _productAttributeFormatter.FormatAttributes(sci.Product, sci.AttributesXml),
                };

                //allow editing?
                //1. setting enabled?
                //2. simple product?
                //3. has attribute or gift card?
                //4. visible individually?
                cartItemModel.AllowItemEditing = ShoppingCartSettings.AllowCartItemEditing &&
                    sci.Product.ProductType == ProductType.SimpleProduct &&
                    (!String.IsNullOrEmpty(cartItemModel.AttributeInfo) || sci.Product.IsGiftCard) &&
                    sci.Product.VisibleIndividually;

                //allowed quantities
                var allowedQuantities = sci.Product.ParseAllowedQuantities();
                foreach (var qty in allowedQuantities)
                {
                    cartItemModel.AllowedQuantities.Add(qty);
                }

                //recurring info
                if (sci.Product.IsRecurring)
                    cartItemModel.RecurringInfo = string.Format(InfoMsg.ShoppingCart_RecurringPeriod, sci.Product.RecurringCycleLength, sci.Product.RecurringCyclePeriod);

                //rental info
                if (sci.Product.IsRental)
                {
                    var rentalStartDate = sci.RentalStartDateUtc.HasValue ? sci.Product.FormatRentalDate(sci.RentalStartDateUtc.Value) : "";
                    var rentalEndDate = sci.RentalEndDateUtc.HasValue ? sci.Product.FormatRentalDate(sci.RentalEndDateUtc.Value) : "";
                    cartItemModel.RentalInfo = string.Format(InfoMsg.ShoppingCart_Rental_FormattedDate,
                        rentalStartDate, rentalEndDate);
                }

                //unit prices
                if (sci.Product.CallForPrice)
                {
                    cartItemModel.UnitPrice = InfoMsg.Products_CallForPrice;
                }
                else
                {

                    decimal taxRate;
                    decimal shoppingCartUnitPriceWithDiscountBase = _taxDomainService.GetProductPrice(sci.Product, _priceCalculationDomainService.GetUnitPrice(sci), CurrentUser, out taxRate);
                    decimal shoppingCartUnitPriceWithDiscount = _currencyDomainService.ConvertFromPrimaryStoreCurrency(shoppingCartUnitPriceWithDiscountBase, WorkingCurrency);
                    cartItemModel.UnitPrice = _priceFormatter.FormatPrice(shoppingCartUnitPriceWithDiscount, true, WorkingCurrency);
                }
                //subtotal, discount
                if (sci.Product.CallForPrice)
                {
                    cartItemModel.SubTotal = InfoMsg.Products_CallForPrice;
                }
                else
                {
                    //sub total
                    Discount scDiscount;
                    decimal shoppingCartItemDiscountBase;
                    decimal taxRate;
                    decimal shoppingCartItemSubTotalWithDiscountBase = _taxDomainService.GetProductPrice(sci.Product, _priceCalculationDomainService.GetSubTotal(sci, true, out shoppingCartItemDiscountBase, out scDiscount), CurrentUser, out taxRate);
                    decimal shoppingCartItemSubTotalWithDiscount = _currencyDomainService.ConvertFromPrimaryStoreCurrency(shoppingCartItemSubTotalWithDiscountBase, WorkingCurrency);
                    cartItemModel.SubTotal = _priceFormatter.FormatPrice(shoppingCartItemSubTotalWithDiscount, true, WorkingCurrency);

                    //display an applied discount amount
                    if (scDiscount != null)
                    {
                        shoppingCartItemDiscountBase = _taxDomainService.GetProductPrice(sci.Product, shoppingCartItemDiscountBase, CurrentUser, out taxRate);
                        if (shoppingCartItemDiscountBase > decimal.Zero)
                        {
                            decimal shoppingCartItemDiscount = _currencyDomainService.ConvertFromPrimaryStoreCurrency(shoppingCartItemDiscountBase, WorkingCurrency);
                            cartItemModel.Discount = _priceFormatter.FormatPrice(shoppingCartItemDiscount, true, WorkingCurrency);
                        }
                    }
                }

                //picture
                if (ShoppingCartSettings.ShowProductImagesOnShoppingCart)
                {
                    cartItemModel.Picture = PrepareCartItemPictureDto(sci,
                        MediaSettings.CartThumbPictureSize, true, cartItemModel.ProductName);
                }

                //item warnings

                var itemWarnings = _shoppingCartDomainService.GetShoppingCartItemWarnings(
                   CurrentUser,
                    sci.ShoppingCartType,
                    sci.Product,
                    sci.StoreId,
                    sci.AttributesXml,
                    sci.CustomerEnteredPrice,
                    sci.RentalStartDateUtc,
                    sci.RentalEndDateUtc,
                    sci.Quantity,
                    false);
                foreach (var warning in itemWarnings)
                    cartItemModel.Warnings.Add(warning);

                model.Items.Add(cartItemModel);
            }

            #endregion

            #region Button payment methods

            //var paymentMethods = _paymentService
            //    .LoadActivePaymentMethods(CurrentUser.Id, _storeContext.CurrentStore.Id)
            //    .Where(pm => pm.PaymentMethodType == PaymentMethodType.Button)
            //    .Where(pm => !pm.HidePaymentMethod(cart))
            //    .ToList();
            //foreach (var pm in paymentMethods)
            //{
            //    if (cart.IsRecurring() && pm.RecurringPaymentType == RecurringPaymentType.NotSupported)
            //        continue;

            //    string actionName;
            //    string controllerName;
            //    //RouteValueDictionary routeValues;
            //    pm.GetPaymentInfoRoute(out actionName, out controllerName, out routeValues);

            //    model.ButtonPaymentMethodActionNames.Add(actionName);
            //    model.ButtonPaymentMethodControllerNames.Add(controllerName);
            //    //model.ButtonPaymentMethodRouteValues.Add(routeValues);
            //}

            #endregion

            #region Order review data

            if (prepareAndDisplayOrderReviewData)
            {
                model.OrderReviewData.Display = true;

                //billing info
                var billingAddress = CurrentUser.BillingAddress;
                if (billingAddress != null)
                    model.OrderReviewData.BillingAddress.PrepareModel(
                        address: billingAddress,
                        excludeProperties: false,
                        addressSettings: AddressSettings,
                        genericAttributeDomianService: _genericAttributeDomianService,
                        addressAttributeFormatter: _addressAttributeFormatter);

                //shipping info
                if (cart.RequiresShipping())
                {
                    model.OrderReviewData.IsShippable = true;

                    if (ShippingSettings.AllowPickUpInStore)
                    {

                        model.OrderReviewData.SelectedPickUpInStore = CurrentUser.GetAttribute<bool>(SystemCustomerAttributeNames.SelectedPickUpInStore, _genericAttributeDomianService, _storeContext.CurrentStore.Id);
                    }

                    if (!model.OrderReviewData.SelectedPickUpInStore)
                    {
                        var shippingAddress = CurrentUser.ShippingAddress;
                        if (shippingAddress != null)
                        {
                            model.OrderReviewData.ShippingAddress.PrepareModel(
                                address: shippingAddress,
                                excludeProperties: false,
                                addressSettings: AddressSettings,
                                genericAttributeDomianService: _genericAttributeDomianService,
                                addressAttributeFormatter: _addressAttributeFormatter);
                        }
                    }


                    //selected shipping method
                    var shippingOption = CurrentUser.GetAttribute<ShippingOption>(SystemCustomerAttributeNames.SelectedShippingOption, _genericAttributeDomianService, _storeContext.CurrentStore.Id);
                    if (shippingOption != null)
                        model.OrderReviewData.ShippingMethod = shippingOption.Name;
                }

                #region payment info
                //var selectedPaymentMethodSystemName = CurrentUser.GetAttribute<string>(
                //    SystemCustomerAttributeNames.SelectedPaymentMethod, _genericAttributeDomianService, _storeContext.CurrentStore.Id);
                //var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(selectedPaymentMethodSystemName);
                //model.OrderReviewData.PaymentMethod = paymentMethod != null ? paymentMethod.GetLocalizedFriendlyName(_localizationService, _workContext.WorkingLanguage.Id) : "";
                #endregion

                #region  custom values
                //var processPaymentRequest = _httpContext.Session["OrderPaymentInfo"] as ProcessPaymentRequest;
                //if (processPaymentRequest != null)
                //{
                //    model.OrderReviewData.CustomValues = processPaymentRequest.CustomValues;
                //}
                #endregion
            }
            #endregion
        }


        protected virtual PictureDto PrepareCartItemPictureDto(ShoppingCartItem sci,
    int pictureSize, bool showDefaultPicture, string productName)
        {

            //var pictureCacheKey = string.Format(ModelCacheEventConsumer.CART_PICTURE_MODEL_KEY, sci.Id, pictureSize, true, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
            //var model = _cacheManager.Get(pictureCacheKey,
            //    //as we cache per user (shopping cart item identifier)
            //    //let's cache just for 3 minutes
            //    3, () =>
            //    {
            //shopping cart item picture
            var sciPicture = sci.Product.GetProductPicture(sci.AttributesXml, _pictureDomainService, _productAttributeParser);
            var model = new PictureDto
            {
                ImageUrl = _pictureDomainService.GetPictureUrl(sciPicture, pictureSize, showDefaultPicture),
                Title = string.Format(InfoMsg.Media_Product_ImageLinkTitleFormat, productName),
                AlternateText = string.Format(InfoMsg.Media_Product_ImageAlternateTextFormat, productName),
            };
            //});
            return model;
        }
        #endregion


    }
}