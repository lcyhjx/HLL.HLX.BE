using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Abp.UI;
using HLL.HLX.BE.Application.MobilityH5.Orders.Dto;
using HLL.HLX.BE.Core.Business.Catalog;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Directory;
using HLL.HLX.BE.Core.Business.Orders;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Directory;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.Application.MobilityH5.Orders
{
    public class ShoppingCartAppService : HlxBeAppServiceBase, IShoppingCartAppService
    {
        #region Constructors

        public ShoppingCartAppService(ProductDomainService productDomainService
            , SettingDomainService settingDomainService
            , CurrencyDomainService currencyDomainService
            , ShoppingCartDomainService shoppingCartDomainService
            , ProductAttributeDomainService productAttributeDomainService
            , IProductAttributeParser productAttributeParser
            , IStoreContext storeContext)
        {
            _productDomainService = productDomainService;
            _settingDomainService = settingDomainService;
            _currencyDomainService = currencyDomainService;
            _shoppingCartDomainService = shoppingCartDomainService;
            _productAttributeDomainService = productAttributeDomainService;
            _productAttributeParser = productAttributeParser;

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
                                var item in ctrlAttributes.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
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

        #endregion

        #region Fields

        private readonly ProductDomainService _productDomainService;
        private readonly SettingDomainService _settingDomainService;
        private readonly CurrencyDomainService _currencyDomainService;
        private readonly ShoppingCartDomainService _shoppingCartDomainService;
        private readonly ProductAttributeDomainService _productAttributeDomainService;
        private readonly IProductAttributeParser _productAttributeParser;

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
    }
}