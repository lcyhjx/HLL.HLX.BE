using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.UI;
using AutoMapper;
using HLL.HLX.BE.Application.MobilityH5.Products.Dto;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Core.Business.Catalog;
using HLL.HLX.BE.Core.Business.Configuration;
using HLL.HLX.BE.Core.Business.Directory;
using HLL.HLX.BE.Core.Business.Media;
using HLL.HLX.BE.Core.Business.Shipping;
using HLL.HLX.BE.Core.Business.Stores;
using HLL.HLX.BE.Core.Business.Tax;
using HLL.HLX.BE.Core.Business.Vendors;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Directory;
using HLL.HLX.BE.Core.Model.Media;
using HLL.HLX.BE.Core.Model.Orders;
using HLL.HLX.BE.Core.Model.Seo;

namespace HLL.HLX.BE.Application.MobilityH5.Products
{
    public class ProductAppService : HlxBeAppServiceBase, IProductAppService
    {
        #region Fields
        private readonly ProductDomainService _productDomainService;
        private readonly SettingDomainService _settingDomainService;
        private readonly ShippingDomainServie _shippingDomainServie;        
        private readonly VendorDomainService _vendorDomainService;
        private readonly ProductTemplateDomainService _productTemplateService;
        private readonly PictureDomainService _pictureDomainService;
        private readonly CurrencyDomainService _currencyDomainService;
        private readonly TaxDomainService _taxDomainService;

        private readonly IVendorTest _vendorTest;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;
        #endregion


        #region Properties
        private CatalogSettings _catalogSettings1;
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


        private SeoSettings _seoSettings1;
        public SeoSettings SeoSettings
        {
            get
            {
                if (_seoSettings1 == null)
                {
                    _seoSettings1 = _settingDomainService.LoadSetting<SeoSettings>(_storeContext.CurrentStore.Id);
                }
                return _seoSettings1;
            }
        }

        private MediaSettings _mediaSettings1;
        public MediaSettings MediaSettings
        {
            get
            {
                if (_mediaSettings1 == null)
                {
                    _mediaSettings1 = _settingDomainService.LoadSetting<MediaSettings>(_storeContext.CurrentStore.Id);
                }
                return _mediaSettings1;
            }
        }


        private Currency _cachedCurrency;
        /// <summary>
        /// Get or set current user working currency
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





        public ProductAppService(ProductDomainService productDomainService
            , ShippingDomainServie shippingDomainServie
            , VendorDomainService vendorDomainService            
            , SettingDomainService settingDomainService
            ,ProductTemplateDomainService productTemplateService
            , PictureDomainService pictureDomainService
            ,CurrencyDomainService currencyDomainService
            , TaxDomainService taxDomainService

            , IVendorTest vendorTest
            , IStoreContext storeContext
            , ICacheManager cacheManager)
        {
            _productDomainService = productDomainService;
            _shippingDomainServie = shippingDomainServie;
            _vendorDomainService = vendorDomainService;
            _settingDomainService = settingDomainService;
            _productTemplateService = productTemplateService;
            _pictureDomainService = pictureDomainService;
            _currencyDomainService = currencyDomainService;
            _taxDomainService = taxDomainService;

            _vendorTest = vendorTest;
            _storeContext = storeContext;
            this._cacheManager = cacheManager;

            var a = 5;
        }

        #region Product details page

        public ProductDetailsOutput ProductDetails(ProductDetailsInput input)
        {

           
            var b = _vendorTest.GetTest();

            var product = _productDomainService.GetProductById(input.ProductId.GetValueOrDefault());
            if (product == null || product.Deleted)
            {
                throw new UserFriendlyException(string.Format("商品[ID:{0}]不存在", input.ProductId.GetValueOrDefault()));
            }



            int aa = 5;

            //published?
            //if (!_catalogSettings.AllowViewUnpublishedProductPage)
            //{
            //    //Check whether the current user has a "Manage catalog" permission
            //    //It allows him to preview a product before publishing
            //    if (!product.Published && !_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //        return InvokeHttp404();
            //}

            ////ACL (access control list)
            //if (!_aclService.Authorize(product))
            //    return InvokeHttp404();

            ////Store mapping
            //if (!_storeMappingService.Authorize(product))
            //    return InvokeHttp404();

            ////availability dates
            //if (!product.IsAvailable())
            //    return InvokeHttp404();

            //visible individually?
            //if (!product.VisibleIndividually)
            //{
            //    //is this one an associated products?
            //    var parentGroupedProduct = _productService.GetProductById(product.ParentGroupedProductId);
            //    if (parentGroupedProduct == null)
            //        return RedirectToRoute("HomePage");

            //    return RedirectToRoute("Product", new { SeName = parentGroupedProduct.GetSeName() });
            //}

            //update existing shopping cart item?
            //ShoppingCartItem updatecartitem = null;
            //if (_shoppingCartSettings.AllowCartItemEditing && updatecartitemid > 0)
            //{
            //    var cart = _workContext.CurrentCustomer.ShoppingCartItems
            //        .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
            //        .LimitPerStore(_storeContext.CurrentStore.Id)
            //        .ToList();
            //    updatecartitem = cart.FirstOrDefault(x => x.Id == updatecartitemid);
            //    //not found?
            //    if (updatecartitem == null)
            //    {
            //        return RedirectToRoute("Product", new { SeName = product.GetSeName() });
            //    }
            //    //is it this product?
            //    if (product.Id != updatecartitem.ProductId)
            //    {
            //        return RedirectToRoute("Product", new { SeName = product.GetSeName() });
            //    }
            //}

            ////prepare the model
            //var model = PrepareProductDetailsPageModel(product, updatecartitem, false);

            ////save as recently viewed
            //_recentlyViewedProductsService.AddProductToRecentlyViewedList(product.Id);

            ////activity log
            //_customerActivityService.InsertActivity("PublicStore.ViewProduct", _localizationService.GetResource("ActivityLog.PublicStore.ViewProduct"), product.Name);

            //return View(model.ProductTemplateViewPath, model);

            return new ProductDetailsOutput();
        }

        #endregion

        #region Utilities

        protected virtual ProductDetailsDto PrepareProductDetailsPageDto(Product product,
            ShoppingCartItem updatecartitem = null, bool isAssociatedProduct = false)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            #region Standard properties

            //var model = new ProductDetailsDto
            //{
            //    Id = product.Id,
            //    Name = product.GetLocalized(x => x.Name),
            //    ShortDescription = product.GetLocalized(x => x.ShortDescription),
            //    FullDescription = product.GetLocalized(x => x.FullDescription),
            //    MetaKeywords = product.GetLocalized(x => x.MetaKeywords),
            //    MetaDescription = product.GetLocalized(x => x.MetaDescription),
            //    MetaTitle = product.GetLocalized(x => x.MetaTitle),
            //    SeName = product.GetSeName(),
            //    ShowSku = _catalogSettings.ShowProductSku,
            //    Sku = product.Sku,
            //    ShowManufacturerPartNumber = _catalogSettings.ShowManufacturerPartNumber,
            //    FreeShippingNotificationEnabled = _catalogSettings.ShowFreeShippingNotification,
            //    ManufacturerPartNumber = product.ManufacturerPartNumber,
            //    ShowGtin = _catalogSettings.ShowGtin,
            //    Gtin = product.Gtin,
            //    StockAvailability = product.FormatStockMessage("", _localizationService, _productAttributeParser),
            //    HasSampleDownload = product.IsDownload && product.HasSampleDownload,
            //    DisplayDiscontinuedMessage = !product.Published && _catalogSettings.DisplayDiscontinuedMessageForUnpublishedProducts
            //};

            var model = Mapper.Map<ProductDetailsDto>(product);
            model.HasSampleDownload = product.IsDownload && product.HasSampleDownload;
            model.ShowSku = CatalogSettings.ShowProductSku;
            model.ShowManufacturerPartNumber = CatalogSettings.ShowManufacturerPartNumber;
            model.FreeShippingNotificationEnabled = CatalogSettings.ShowFreeShippingNotification;
            model.ShowGtin = CatalogSettings.ShowGtin;
            model.DisplayDiscontinuedMessage = !product.Published &&
                                               CatalogSettings.DisplayDiscontinuedMessageForUnpublishedProducts;

            //automatically generate product description?
            if (SeoSettings.GenerateProductMetaDescription && String.IsNullOrEmpty(model.MetaDescription))
            {
                //based on short description
                model.MetaDescription = model.ShortDescription;
            }

            //shipping info
            model.IsShipEnabled = product.IsShipEnabled;
            if (product.IsShipEnabled)
            {
                model.IsFreeShipping = product.IsFreeShipping;
                //delivery date
                var deliveryDate = _shippingDomainServie.GetDeliveryDateById(product.DeliveryDateId);
                if (deliveryDate != null)
                {
                    //model.DeliveryDate = deliveryDate.GetLocalized(dd => dd.Name);
                    model.DeliveryDate = deliveryDate.Name;
                }
            }

            //email a friend
            model.EmailAFriendEnabled = CatalogSettings.EmailAFriendEnabled;
            //compare products
            model.CompareProductsEnabled = CatalogSettings.CompareProductsEnabled;

            #endregion

            #region Vendor details

            //vendor
            //if (_vendorSettings.ShowVendorOnProductDetailsPage)
            {
                var vendor = _vendorDomainService.GetVendorById(product.VendorId);
                if (vendor != null && !vendor.Deleted && vendor.Active)
                {
                    model.ShowVendor = true;
                    model.VendorDto = Mapper.Map<VendorBriefInfoDto>(vendor);
                    //model.VendorModel = new VendorBriefInfoDto
                    //{
                    //    Id = vendor.Id,
                    //    Name = vendor.GetLocalized(x => x.Name),
                    //    SeName = vendor.GetSeName(),
                    //};
                }
            }

            #endregion

            #region Page sharing

            //if (_catalogSettings.ShowShareButton && !String.IsNullOrEmpty(_catalogSettings.PageShareCode))
            //{
            //    var shareCode = _catalogSettings.PageShareCode;
            //    if (_webHelper.IsCurrentConnectionSecured())
            //    {
            //        //need to change the addthis link to be https linked when the page is, so that the page doesnt ask about mixed mode when viewed in https...
            //        shareCode = shareCode.Replace("http://", "https://");
            //    }
            //    model.PageShareCode = shareCode;
            //}

            #endregion

            #region Back in stock subscriptions

            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                product.GetTotalStockQuantity() <= 0)
            {
                //out of stock
                model.DisplayBackInStockSubscription = true;
            }

            #endregion

            #region Breadcrumb

            //do not prepare this model for the associated products. anyway it's not used
            //if (_catalogSettings.CategoryBreadcrumbEnabled && !isAssociatedProduct)
            //{
            //    var breadcrumbCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_BREADCRUMB_MODEL_KEY,
            //        product.Id,
            //        _workContext.WorkingLanguage.Id,
            //        string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()),
            //        _storeContext.CurrentStore.Id);
            //    model.Breadcrumb = _cacheManager.Get(breadcrumbCacheKey, () =>
            //    {
            //        var breadcrumbModel = new ProductDetailsModel.ProductBreadcrumbModel
            //        {
            //            Enabled = _catalogSettings.CategoryBreadcrumbEnabled,
            //            ProductId = product.Id,
            //            ProductName = product.GetLocalized(x => x.Name),
            //            ProductSeName = product.GetSeName()
            //        };
            //        var productCategories = _categoryService.GetProductCategoriesByProductId(product.Id);
            //        if (productCategories.Count > 0)
            //        {
            //            var category = productCategories[0].Category;
            //            if (category != null)
            //            {
            //                foreach (
            //                    var catBr in
            //                        category.GetCategoryBreadCrumb(_categoryService, _aclService, _storeMappingService))
            //                {
            //                    breadcrumbModel.CategoryBreadcrumb.Add(new CategorySimpleModel
            //                    {
            //                        Id = catBr.Id,
            //                        Name = catBr.GetLocalized(x => x.Name),
            //                        SeName = catBr.GetSeName(),
            //                        IncludeInTopMenu = catBr.IncludeInTopMenu
            //                    });
            //                }
            //            }
            //        }
            //        return breadcrumbModel;
            //    });
            //}

            #endregion

            #region Product tags

            //do not prepare this model for the associated products. anyway it's not used
            //if (!isAssociatedProduct)
            //{
            //    var productTagsCacheKey = string.Format(ModelCacheEventConsumer.PRODUCTTAG_BY_PRODUCT_MODEL_KEY, product.Id, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            //    model.ProductTags = _cacheManager.Get(productTagsCacheKey, () =>
            //        product.ProductTags
            //        //filter by store
            //        .Where(x => _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id) > 0)
            //        .Select(x => new ProductTagDto
            //        {
            //            Id = x.Id,
            //            Name = x.GetLocalized(y => y.Name),
            //            SeName = x.GetSeName(),
            //            ProductCount = _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id)
            //        })
            //        .ToList());
            //}

            #endregion

            #region Templates

            //var templateCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_TEMPLATE_MODEL_KEY, product.ProductTemplateId);
            //model.ProductTemplateViewPath = _cacheManager.Get(templateCacheKey, () =>
            //{
                var template = _productTemplateService.GetProductTemplateById(product.ProductTemplateId);
                if (template == null)
                    template = _productTemplateService.GetAllProductTemplates().FirstOrDefault();
                if (template == null)
                    throw new Exception("No default template could be loaded");
                //return template.ViewPath;
            model.ProductTemplateViewPath = template.ViewPath;
            //});

            #endregion

            #region Pictures

            model.DefaultPictureZoomEnabled = MediaSettings.DefaultPictureZoomEnabled;
            //default picture
            var defaultPictureSize = isAssociatedProduct ?
                MediaSettings.AssociatedProductPictureSize :
                MediaSettings.ProductDetailsPictureSize;
            //prepare picture models
            //var productPicturesCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DETAILS_PICTURES_MODEL_KEY, product.Id, defaultPictureSize, isAssociatedProduct, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
            //var cachedPictures = _cacheManager.Get(productPicturesCacheKey, () =>
            //{
                var pictures = _pictureDomainService.GetPicturesByProductId(product.Id);
                var defaultPicture = pictures.FirstOrDefault();
                var defaultPictureModel = new PictureDto
                {
                    ImageUrl = _pictureDomainService.GetPictureUrl(defaultPicture, defaultPictureSize, !isAssociatedProduct),
                    FullSizeImageUrl = _pictureDomainService.GetPictureUrl(defaultPicture, 0, !isAssociatedProduct),
                    Title = string.Format(InfoMsg.Media_Product_ImageLinkTitleFormat_Details, model.Name),
                    AlternateText = string.Format(InfoMsg.Media_Product_ImageAlternateTextFormat_Details, model.Name),
                };
                //"title" attribute
                defaultPictureModel.Title = (defaultPicture != null && !string.IsNullOrEmpty(defaultPicture.TitleAttribute)) ?
                    defaultPicture.TitleAttribute :
                    string.Format(InfoMsg.Media_Product_ImageLinkTitleFormat_Details, model.Name);
                //"alt" attribute
                defaultPictureModel.AlternateText = (defaultPicture != null && !string.IsNullOrEmpty(defaultPicture.AltAttribute)) ?
                    defaultPicture.AltAttribute :
                    string.Format(InfoMsg.Media_Product_ImageAlternateTextFormat_Details, model.Name);

                //all pictures
                var pictureModels = new List<PictureDto>();
                foreach (var picture in pictures)
                {
                    var pictureModel = new PictureDto
                    {
                        ImageUrl = _pictureDomainService.GetPictureUrl(picture, MediaSettings.ProductThumbPictureSizeOnProductDetailsPage),
                        FullSizeImageUrl = _pictureDomainService.GetPictureUrl(picture),
                        Title = string.Format(InfoMsg.Media_Product_ImageLinkTitleFormat_Details, model.Name),
                        AlternateText = string.Format(InfoMsg.Media_Product_ImageAlternateTextFormat_Details, model.Name),
                    };
                    //"title" attribute
                    pictureModel.Title = !string.IsNullOrEmpty(picture.TitleAttribute) ?
                        picture.TitleAttribute :
                        string.Format(InfoMsg.Media_Product_ImageLinkTitleFormat_Details, model.Name);
                    //"alt" attribute
                    pictureModel.AlternateText = !string.IsNullOrEmpty(picture.AltAttribute) ?
                        picture.AltAttribute :
                        string.Format(InfoMsg.Media_Product_ImageAlternateTextFormat_Details, model.Name);

                    pictureModels.Add(pictureModel);
                }

                //return new { DefaultPictureModel = defaultPictureModel, PictureModels = pictureModels };

                var cachedPictures = new { DefaultPictureModel = defaultPictureModel, PictureModels = pictureModels };
            //});
            model.DefaultPictureModel = cachedPictures.DefaultPictureModel;
            model.PictureModels = cachedPictures.PictureModels;

            #endregion

            #region Product price

            model.ProductPrice.ProductId = product.Id;
            //if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            {
                model.ProductPrice.HidePrices = false;
                if (product.CustomerEntersPrice)
                {
                    model.ProductPrice.CustomerEntersPrice = true;
                }
                else
                {
                    if (product.CallForPrice)
                    {
                        model.ProductPrice.CallForPrice = true;
                    }
                    else
                    {
                        
                        decimal taxRate;
                        decimal oldPriceBase = _taxDomainService.GetProductPrice(product, product.OldPrice, CurrentUser,out taxRate);
                        //decimal finalPriceWithoutDiscountBase = _taxDomainService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, CurrentUser, includeDiscounts: false), out taxRate);
                        //decimal finalPriceWithDiscountBase = _taxDomainService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, CurrentUser, includeDiscounts: true), out taxRate);

                        //decimal oldPrice = _currencyDomainService.ConvertFromPrimaryStoreCurrency(oldPriceBase, WorkingCurrency);
                        //decimal finalPriceWithoutDiscount = _currencyDomainService.ConvertFromPrimaryStoreCurrency(finalPriceWithoutDiscountBase, WorkingCurrency);
                        //decimal finalPriceWithDiscount = _currencyDomainService.ConvertFromPrimaryStoreCurrency(finalPriceWithDiscountBase, WorkingCurrency);

                        //if (finalPriceWithoutDiscountBase != oldPriceBase && oldPriceBase > decimal.Zero)
                        //    model.ProductPrice.OldPrice = _priceFormatter.FormatPrice(oldPrice);

                        //model.ProductPrice.Price = _priceFormatter.FormatPrice(finalPriceWithoutDiscount);

                        //if (finalPriceWithoutDiscountBase != finalPriceWithDiscountBase)
                        //    model.ProductPrice.PriceWithDiscount = _priceFormatter.FormatPrice(finalPriceWithDiscount);

                        //model.ProductPrice.PriceValue = finalPriceWithDiscount;

                        ////property for German market
                        ////we display tax/shipping info only with "shipping enabled" for this product
                        ////we also ensure this it's not free shipping
                        //model.ProductPrice.DisplayTaxShippingInfo = CatalogSettings.DisplayTaxShippingInfoProductDetailsPage
                        //    && product.IsShipEnabled &&
                        //    !product.IsFreeShipping;

                        ////PAngV baseprice (used in Germany)
                        //model.ProductPrice.BasePricePAngV = product.FormatBasePrice(finalPriceWithDiscountBase,
                        //    _localizationService, _measureService, _currencyService, _workContext, _priceFormatter);

                        ////currency code
                        //model.ProductPrice.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;

                        ////rental
                        //if (product.IsRental)
                        //{
                        //    model.ProductPrice.IsRental = true;
                        //    var priceStr = _priceFormatter.FormatPrice(finalPriceWithDiscount);
                        //    model.ProductPrice.RentalPrice = _priceFormatter.FormatRentalProductPeriod(product, priceStr);
                        //}
                    }
                }
            }
            //else
            //{
            //    model.ProductPrice.HidePrices = true;
            //    model.ProductPrice.OldPrice = null;
            //    model.ProductPrice.Price = null;
            //}
            #endregion

            //#region 'Add to cart' model

            //model.AddToCart.ProductId = product.Id;
            //model.AddToCart.UpdatedShoppingCartItemId = updatecartitem != null ? updatecartitem.Id : 0;

            ////quantity
            //model.AddToCart.EnteredQuantity = updatecartitem != null ? updatecartitem.Quantity : product.OrderMinimumQuantity;
            ////allowed quantities
            //var allowedQuantities = product.ParseAllowedQuantities();
            //foreach (var qty in allowedQuantities)
            //{
            //    model.AddToCart.AllowedQuantities.Add(new SelectListItem
            //    {
            //        Text = qty.ToString(),
            //        Value = qty.ToString(),
            //        Selected = updatecartitem != null && updatecartitem.Quantity == qty
            //    });
            //}
            ////minimum quantity notification
            //if (product.OrderMinimumQuantity > 1)
            //{
            //    model.AddToCart.MinimumQuantityNotification = string.Format(_localizationService.GetResource("Products.MinimumQuantityNotification"), product.OrderMinimumQuantity);
            //}

            ////'add to cart', 'add to wishlist' buttons
            //model.AddToCart.DisableBuyButton = product.DisableBuyButton || !_permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart);
            //model.AddToCart.DisableWishlistButton = product.DisableWishlistButton || !_permissionService.Authorize(StandardPermissionProvider.EnableWishlist);
            //if (!_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            //{
            //    model.AddToCart.DisableBuyButton = true;
            //    model.AddToCart.DisableWishlistButton = true;
            //}
            ////pre-order
            //if (product.AvailableForPreOrder)
            //{
            //    model.AddToCart.AvailableForPreOrder = !product.PreOrderAvailabilityStartDateTimeUtc.HasValue ||
            //        product.PreOrderAvailabilityStartDateTimeUtc.Value >= DateTime.UtcNow;
            //    model.AddToCart.PreOrderAvailabilityStartDateTimeUtc = product.PreOrderAvailabilityStartDateTimeUtc;
            //}
            ////rental
            //model.AddToCart.IsRental = product.IsRental;

            ////customer entered price
            //model.AddToCart.CustomerEntersPrice = product.CustomerEntersPrice;
            //if (model.AddToCart.CustomerEntersPrice)
            //{
            //    decimal minimumCustomerEnteredPrice = _currencyService.ConvertFromPrimaryStoreCurrency(product.MinimumCustomerEnteredPrice, _workContext.WorkingCurrency);
            //    decimal maximumCustomerEnteredPrice = _currencyService.ConvertFromPrimaryStoreCurrency(product.MaximumCustomerEnteredPrice, _workContext.WorkingCurrency);

            //    model.AddToCart.CustomerEnteredPrice = updatecartitem != null ? updatecartitem.CustomerEnteredPrice : minimumCustomerEnteredPrice;
            //    model.AddToCart.CustomerEnteredPriceRange = string.Format(_localizationService.GetResource("Products.EnterProductPrice.Range"),
            //        _priceFormatter.FormatPrice(minimumCustomerEnteredPrice, false, false),
            //        _priceFormatter.FormatPrice(maximumCustomerEnteredPrice, false, false));
            //}

            //#endregion

            //#region Gift card

            //model.GiftCard.IsGiftCard = product.IsGiftCard;
            //if (model.GiftCard.IsGiftCard)
            //{
            //    model.GiftCard.GiftCardType = product.GiftCardType;

            //    if (updatecartitem == null)
            //    {
            //        model.GiftCard.SenderName = _workContext.CurrentCustomer.GetFullName();
            //        model.GiftCard.SenderEmail = _workContext.CurrentCustomer.Email;
            //    }
            //    else
            //    {
            //        string giftCardRecipientName, giftCardRecipientEmail, giftCardSenderName, giftCardSenderEmail, giftCardMessage;
            //        _productAttributeParser.GetGiftCardAttribute(updatecartitem.AttributesXml,
            //            out giftCardRecipientName, out giftCardRecipientEmail,
            //            out giftCardSenderName, out giftCardSenderEmail, out giftCardMessage);

            //        model.GiftCard.RecipientName = giftCardRecipientName;
            //        model.GiftCard.RecipientEmail = giftCardRecipientEmail;
            //        model.GiftCard.SenderName = giftCardSenderName;
            //        model.GiftCard.SenderEmail = giftCardSenderEmail;
            //        model.GiftCard.Message = giftCardMessage;
            //    }
            //}

            //#endregion

            //#region Product attributes

            ////performance optimization
            ////We cache a value indicating whether a product has attributes
            //IList<ProductAttributeMapping> productAttributeMapping = null;
            //string cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_HAS_PRODUCT_ATTRIBUTES_KEY, product.Id);
            //var hasProductAttributesCache = _cacheManager.Get<bool?>(cacheKey);
            //if (!hasProductAttributesCache.HasValue)
            //{
            //    //no value in the cache yet
            //    //let's load attributes and cache the result (true/false)
            //    productAttributeMapping = _productAttributeService.GetProductAttributeMappingsByProductId(product.Id);
            //    hasProductAttributesCache = productAttributeMapping.Count > 0;
            //    _cacheManager.Set(cacheKey, hasProductAttributesCache, 60);
            //}
            //if (hasProductAttributesCache.Value && productAttributeMapping == null)
            //{
            //    //cache indicates that the product has attributes
            //    //let's load them
            //    productAttributeMapping = _productAttributeService.GetProductAttributeMappingsByProductId(product.Id);
            //}
            //if (productAttributeMapping == null)
            //{
            //    productAttributeMapping = new List<ProductAttributeMapping>();
            //}
            //foreach (var attribute in productAttributeMapping)
            //{
            //    var attributeModel = new ProductDetailsModel.ProductAttributeModel
            //    {
            //        Id = attribute.Id,
            //        ProductId = product.Id,
            //        ProductAttributeId = attribute.ProductAttributeId,
            //        Name = attribute.ProductAttribute.GetLocalized(x => x.Name),
            //        Description = attribute.ProductAttribute.GetLocalized(x => x.Description),
            //        TextPrompt = attribute.TextPrompt,
            //        IsRequired = attribute.IsRequired,
            //        AttributeControlType = attribute.AttributeControlType,
            //        DefaultValue = updatecartitem != null ? null : attribute.DefaultValue,
            //        HasCondition = !String.IsNullOrEmpty(attribute.ConditionAttributeXml)
            //    };
            //    if (!String.IsNullOrEmpty(attribute.ValidationFileAllowedExtensions))
            //    {
            //        attributeModel.AllowedFileExtensions = attribute.ValidationFileAllowedExtensions
            //            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //            .ToList();
            //    }

            //    if (attribute.ShouldHaveValues())
            //    {
            //        //values
            //        var attributeValues = _productAttributeService.GetProductAttributeValues(attribute.Id);
            //        foreach (var attributeValue in attributeValues)
            //        {
            //            var valueModel = new ProductDetailsModel.ProductAttributeValueModel
            //            {
            //                Id = attributeValue.Id,
            //                Name = attributeValue.GetLocalized(x => x.Name),
            //                ColorSquaresRgb = attributeValue.ColorSquaresRgb, //used with "Color squares" attribute type
            //                IsPreSelected = attributeValue.IsPreSelected
            //            };
            //            attributeModel.Values.Add(valueModel);

            //            //display price if allowed
            //            if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            //            {
            //                decimal taxRate;
            //                decimal attributeValuePriceAdjustment = _priceCalculationService.GetProductAttributeValuePriceAdjustment(attributeValue);
            //                decimal priceAdjustmentBase = _taxService.GetProductPrice(product, attributeValuePriceAdjustment, out taxRate);
            //                decimal priceAdjustment = _currencyService.ConvertFromPrimaryStoreCurrency(priceAdjustmentBase, _workContext.WorkingCurrency);
            //                if (priceAdjustmentBase > decimal.Zero)
            //                    valueModel.PriceAdjustment = "+" + _priceFormatter.FormatPrice(priceAdjustment, false, false);
            //                else if (priceAdjustmentBase < decimal.Zero)
            //                    valueModel.PriceAdjustment = "-" + _priceFormatter.FormatPrice(-priceAdjustment, false, false);

            //                valueModel.PriceAdjustmentValue = priceAdjustment;
            //            }

            //            //picture
            //            if (attributeValue.PictureId > 0)
            //            {
            //                var productAttributePictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCTATTRIBUTE_PICTURE_MODEL_KEY,
            //                    attributeValue.PictureId,
            //                    _webHelper.IsCurrentConnectionSecured(),
            //                    _storeContext.CurrentStore.Id);
            //                valueModel.PictureModel = _cacheManager.Get(productAttributePictureCacheKey, () =>
            //                {
            //                    var valuePicture = _pictureService.GetPictureById(attributeValue.PictureId);
            //                    if (valuePicture != null)
            //                    {
            //                        return new PictureModel
            //                        {
            //                            FullSizeImageUrl = _pictureService.GetPictureUrl(valuePicture),
            //                            ImageUrl = _pictureService.GetPictureUrl(valuePicture, defaultPictureSize)
            //                        };
            //                    }
            //                    return new PictureModel();
            //                });
            //            }
            //        }
            //    }

            //    //set already selected attributes (if we're going to update the existing shopping cart item)
            //    if (updatecartitem != null)
            //    {
            //        switch (attribute.AttributeControlType)
            //        {
            //            case AttributeControlType.DropdownList:
            //            case AttributeControlType.RadioList:
            //            case AttributeControlType.Checkboxes:
            //            case AttributeControlType.ColorSquares:
            //                {
            //                    if (!String.IsNullOrEmpty(updatecartitem.AttributesXml))
            //                    {
            //                        //clear default selection
            //                        foreach (var item in attributeModel.Values)
            //                            item.IsPreSelected = false;

            //                        //select new values
            //                        var selectedValues = _productAttributeParser.ParseProductAttributeValues(updatecartitem.AttributesXml);
            //                        foreach (var attributeValue in selectedValues)
            //                            foreach (var item in attributeModel.Values)
            //                                if (attributeValue.Id == item.Id)
            //                                    item.IsPreSelected = true;
            //                    }
            //                }
            //                break;
            //            case AttributeControlType.ReadonlyCheckboxes:
            //                {
            //                    //do nothing
            //                    //values are already pre-set
            //                }
            //                break;
            //            case AttributeControlType.TextBox:
            //            case AttributeControlType.MultilineTextbox:
            //                {
            //                    if (!String.IsNullOrEmpty(updatecartitem.AttributesXml))
            //                    {
            //                        var enteredText = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
            //                        if (enteredText.Count > 0)
            //                            attributeModel.DefaultValue = enteredText[0];
            //                    }
            //                }
            //                break;
            //            case AttributeControlType.Datepicker:
            //                {
            //                    //keep in mind my that the code below works only in the current culture
            //                    var selectedDateStr = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
            //                    if (selectedDateStr.Count > 0)
            //                    {
            //                        DateTime selectedDate;
            //                        if (DateTime.TryParseExact(selectedDateStr[0], "D", CultureInfo.CurrentCulture,
            //                                               DateTimeStyles.None, out selectedDate))
            //                        {
            //                            //successfully parsed
            //                            attributeModel.SelectedDay = selectedDate.Day;
            //                            attributeModel.SelectedMonth = selectedDate.Month;
            //                            attributeModel.SelectedYear = selectedDate.Year;
            //                        }
            //                    }

            //                }
            //                break;
            //            case AttributeControlType.FileUpload:
            //                {
            //                    if (!String.IsNullOrEmpty(updatecartitem.AttributesXml))
            //                    {
            //                        var downloadGuidStr = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id).FirstOrDefault();
            //                        Guid downloadGuid;
            //                        Guid.TryParse(downloadGuidStr, out downloadGuid);
            //                        var download = _downloadService.GetDownloadByGuid(downloadGuid);
            //                        if (download != null)
            //                            attributeModel.DefaultValue = download.DownloadGuid.ToString();
            //                    }
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //    model.ProductAttributes.Add(attributeModel);
            //}

            //#endregion 

            //#region Product specifications

            ////do not prepare this model for the associated products. any it's not used
            //if (!isAssociatedProduct)
            //{
            //    model.ProductSpecifications = this.PrepareProductSpecificationModel(_workContext,
            //        _specificationAttributeService,
            //        _cacheManager,
            //        product);
            //}

            //#endregion

            //#region Product review overview

            //model.ProductReviewOverview = new ProductReviewOverviewModel
            //{
            //    ProductId = product.Id,
            //    RatingSum = product.ApprovedRatingSum,
            //    TotalReviews = product.ApprovedTotalReviews,
            //    AllowCustomerReviews = product.AllowCustomerReviews
            //};

            //#endregion

            //#region Tier prices

            //if (product.HasTierPrices && _permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            //{
            //    model.TierPrices = product.TierPrices
            //        .OrderBy(x => x.Quantity)
            //        .ToList()
            //        .FilterByStore(_storeContext.CurrentStore.Id)
            //        .FilterForCustomer(_workContext.CurrentCustomer)
            //        .RemoveDuplicatedQuantities()
            //        .Select(tierPrice =>
            //        {
            //            var m = new ProductDetailsModel.TierPriceModel
            //            {
            //                Quantity = tierPrice.Quantity,
            //            };
            //            decimal taxRate;
            //            decimal priceBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, _workContext.CurrentCustomer, decimal.Zero, _catalogSettings.DisplayTierPricesWithDiscounts, tierPrice.Quantity), out taxRate);
            //            decimal price = _currencyService.ConvertFromPrimaryStoreCurrency(priceBase, _workContext.WorkingCurrency);
            //            m.Price = _priceFormatter.FormatPrice(price, false, false);
            //            return m;
            //        })
            //        .ToList();
            //}

            //#endregion

            //#region Manufacturers

            ////do not prepare this model for the associated products. any it's not used
            //if (!isAssociatedProduct)
            //{
            //    string manufacturersCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_MANUFACTURERS_MODEL_KEY,
            //        product.Id,
            //        _workContext.WorkingLanguage.Id,
            //        string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()),
            //        _storeContext.CurrentStore.Id);
            //    model.ProductManufacturers = _cacheManager.Get(manufacturersCacheKey, () =>
            //        _manufacturerService.GetProductManufacturersByProductId(product.Id)
            //        .Select(x => x.Manufacturer.ToModel())
            //        .ToList()
            //        );
            //}
            //#endregion

            //#region Rental products

            //if (product.IsRental)
            //{
            //    model.IsRental = true;
            //    //set already entered dates attributes (if we're going to update the existing shopping cart item)
            //    if (updatecartitem != null)
            //    {
            //        model.RentalStartDate = updatecartitem.RentalStartDateUtc;
            //        model.RentalEndDate = updatecartitem.RentalEndDateUtc;
            //    }
            //}

            //#endregion

            //#region Associated products

            //if (product.ProductType == ProductType.GroupedProduct)
            //{
            //    //ensure no circular references
            //    if (!isAssociatedProduct)
            //    {
            //        var associatedProducts = _productService.GetAssociatedProducts(product.Id, _storeContext.CurrentStore.Id);
            //        foreach (var associatedProduct in associatedProducts)
            //            model.AssociatedProducts.Add(PrepareProductDetailsPageModel(associatedProduct, null, true));
            //    }
            //}

            //#endregion

            return model;
        }

        #endregion

    }

}