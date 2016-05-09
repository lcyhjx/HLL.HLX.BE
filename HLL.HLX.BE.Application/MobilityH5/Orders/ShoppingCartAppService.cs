//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Policy;
//using System.Text;
//using System.Threading.Tasks;
//using Abp.UI;
//using HLL.HLX.BE.Core.Business.Catalog;
//using HLL.HLX.BE.Core.Business.Orders;
//using HLL.HLX.BE.Core.Model.Catalog;
//using HLL.HLX.BE.Core.Model.Orders;

//namespace HLL.HLX.BE.Application.MobilityH5.Orders
//{
//    public  class ShoppingCartAppService : HlxBeAppServiceBase, IShoppingCartAppService
//    {
//        #region Fields

//        private readonly ProductDomainService _productDomainService;
//        #endregion

//        #region Constructors
//        public ShoppingCartAppService(ProductDomainService productDomainService)
//        {
//            _productDomainService = productDomainService;
//        }
//        #endregion


//        #region Shopping cart

//        public void AddProductToCartForDetails(int productId, int shoppingCartTypeId, FormCollection form)
//        {
//            var product = _productDomainService.GetProductById(productId);
//            if (product == null)
//            {
//               throw new UserFriendlyException(string.Format("商品(Id:{0})不存在",productId));
//            }

//            //we can add only simple products
//            if (product.ProductType != ProductType.SimpleProduct)
//            {
//                throw new UserFriendlyException("Only simple products could be added to the cart");
//            }

//            #region Update existing shopping cart item?
//            int updatecartitemid = 0;
//            //foreach (string formKey in form.AllKeys)
//            //    if (formKey.Equals(string.Format("addtocart_{0}.UpdatedShoppingCartItemId", productId), StringComparison.InvariantCultureIgnoreCase))
//            //    {
//            //        int.TryParse(form[formKey], out updatecartitemid);
//            //        break;
//            //    }
//            ShoppingCartItem updatecartitem = null;
//            if (_shoppingCartSettings.AllowCartItemEditing && updatecartitemid > 0)
//            {
                
//                var cart = CurrentUser.ShoppingCartItems
//                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
//                    .LimitPerStore(_storeContext.CurrentStore.Id)
//                    .ToList();
//                updatecartitem = cart.FirstOrDefault(x => x.Id == updatecartitemid);
//                //not found?
//                if (updatecartitem == null)
//                {
//                    return Json(new
//                    {
//                        success = false,
//                        message = "No shopping cart item found to update"
//                    });
//                }
//                //is it this product?
//                if (product.Id != updatecartitem.ProductId)
//                {
//                    return Json(new
//                    {
//                        success = false,
//                        message = "This product does not match a passed shopping cart item identifier"
//                    });
//                }
//            }
//            #endregion

//            #region Customer entered price
//            decimal customerEnteredPriceConverted = decimal.Zero;
//            if (product.CustomerEntersPrice)
//            {
//                foreach (string formKey in form.AllKeys)
//                {
//                    if (formKey.Equals(string.Format("addtocart_{0}.CustomerEnteredPrice", productId), StringComparison.InvariantCultureIgnoreCase))
//                    {
//                        decimal customerEnteredPrice;
//                        if (decimal.TryParse(form[formKey], out customerEnteredPrice))
//                            customerEnteredPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(customerEnteredPrice, _workContext.WorkingCurrency);
//                        break;
//                    }
//                }
//            }
//            #endregion

//            #region Quantity

//            int quantity = 1;
//            foreach (string formKey in form.AllKeys)
//                if (formKey.Equals(string.Format("addtocart_{0}.EnteredQuantity", productId), StringComparison.InvariantCultureIgnoreCase))
//                {
//                    int.TryParse(form[formKey], out quantity);
//                    break;
//                }

//            #endregion

//            //product and gift card attributes
//            string attributes = ParseProductAttributes(product, form);

//            //rental attributes
//            DateTime? rentalStartDate = null;
//            DateTime? rentalEndDate = null;
//            if (product.IsRental)
//            {
//                ParseRentalDates(product, form, out rentalStartDate, out rentalEndDate);
//            }

//            //save item
//            var addToCartWarnings = new List<string>();
//            var cartType = (ShoppingCartType)shoppingCartTypeId;
//            if (updatecartitem == null)
//            {
//                //add to the cart
//                addToCartWarnings.AddRange(_shoppingCartService.AddToCart(_workContext.CurrentCustomer,
//                    product, cartType, _storeContext.CurrentStore.Id,
//                    attributes, customerEnteredPriceConverted,
//                    rentalStartDate, rentalEndDate, quantity, true));
//            }
//            else
//            {
//                var cart = _workContext.CurrentCustomer.ShoppingCartItems
//                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
//                    .LimitPerStore(_storeContext.CurrentStore.Id)
//                    .ToList();
//                var otherCartItemWithSameParameters = _shoppingCartService.FindShoppingCartItemInTheCart(
//                    cart, cartType, product, attributes, customerEnteredPriceConverted,
//                    rentalStartDate, rentalEndDate);
//                if (otherCartItemWithSameParameters != null &&
//                    otherCartItemWithSameParameters.Id == updatecartitem.Id)
//                {
//                    //ensure it's other shopping cart cart item
//                    otherCartItemWithSameParameters = null;
//                }
//                //update existing item
//                addToCartWarnings.AddRange(_shoppingCartService.UpdateShoppingCartItem(_workContext.CurrentCustomer,
//                    updatecartitem.Id, attributes, customerEnteredPriceConverted,
//                    rentalStartDate, rentalEndDate, quantity, true));
//                if (otherCartItemWithSameParameters != null && addToCartWarnings.Count == 0)
//                {
//                    //delete the same shopping cart item (the other one)
//                    _shoppingCartService.DeleteShoppingCartItem(otherCartItemWithSameParameters);
//                }
//            }

//            #region Return result

//            if (addToCartWarnings.Count > 0)
//            {
//                //cannot be added to the cart/wishlist
//                //let's display warnings
//                return Json(new
//                {
//                    success = false,
//                    message = addToCartWarnings.ToArray()
//                });
//            }

//            //added to the cart/wishlist
//            switch (cartType)
//            {
//                case ShoppingCartType.Wishlist:
//                    {
//                        //activity log
//                        _customerActivityService.InsertActivity("PublicStore.AddToWishlist", _localizationService.GetResource("ActivityLog.PublicStore.AddToWishlist"), product.Name);

//                        if (_shoppingCartSettings.DisplayWishlistAfterAddingProduct)
//                        {
//                            //redirect to the wishlist page
//                            return Json(new
//                            {
//                                redirect = Url.RouteUrl("Wishlist"),
//                            });
//                        }

//                        //display notification message and update appropriate blocks
//                        var updatetopwishlistsectionhtml = string.Format(_localizationService.GetResource("Wishlist.HeaderQuantity"),
//                        _workContext.CurrentCustomer.ShoppingCartItems
//                        .Where(sci => sci.ShoppingCartType == ShoppingCartType.Wishlist)
//                        .LimitPerStore(_storeContext.CurrentStore.Id)
//                        .ToList()
//                        .GetTotalProducts());

//                        return Json(new
//                        {
//                            success = true,
//                            message = string.Format(_localizationService.GetResource("Products.ProductHasBeenAddedToTheWishlist.Link"), Url.RouteUrl("Wishlist")),
//                            updatetopwishlistsectionhtml = updatetopwishlistsectionhtml,
//                        });
//                    }
//                case ShoppingCartType.ShoppingCart:
//                default:
//                    {
//                        //activity log
//                        _customerActivityService.InsertActivity("PublicStore.AddToShoppingCart", _localizationService.GetResource("ActivityLog.PublicStore.AddToShoppingCart"), product.Name);

//                        if (_shoppingCartSettings.DisplayCartAfterAddingProduct)
//                        {
//                            //redirect to the shopping cart page
//                            return Json(new
//                            {
//                                redirect = Url.RouteUrl("ShoppingCart"),
//                            });
//                        }

//                        //display notification message and update appropriate blocks
//                        var updatetopcartsectionhtml = string.Format(_localizationService.GetResource("ShoppingCart.HeaderQuantity"),
//                        _workContext.CurrentCustomer.ShoppingCartItems
//                        .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
//                        .LimitPerStore(_storeContext.CurrentStore.Id)
//                        .ToList()
//                        .GetTotalProducts());

//                        var updateflyoutcartsectionhtml = _shoppingCartSettings.MiniShoppingCartEnabled
//                            ? this.RenderPartialViewToString("FlyoutShoppingCart", PrepareMiniShoppingCartModel())
//                            : "";

//                        return Json(new
//                        {
//                            success = true,
//                            message = string.Format(_localizationService.GetResource("Products.ProductHasBeenAddedToTheCart.Link"), Url.RouteUrl("ShoppingCart")),
//                            updatetopcartsectionhtml = updatetopcartsectionhtml,
//                            updateflyoutcartsectionhtml = updateflyoutcartsectionhtml
//                        });
//                    }
//            }
//            #endregion


//            #endregion
//        }
//    }
//}
