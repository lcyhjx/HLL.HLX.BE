using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.EntityFramework.Migrations
{
    public class HlxDbMigration : DbMigration
    {
        public override void Up()
        {
            #region 设置数据表中一些字段的默认值 - 比如creationtime, isdeleted
            List<string> tableNames = new List<string>
            {
                                 
                //Catalog
                "BackInStockSubscription",
                "Category",
                "CategoryTemplate",
                "CrossSellProduct",
                "Manufacturer",
                "ManufacturerTemplate",
                "PredefinedProductAttributeValue",
                "ProductAttributeCombination",
                "ProductAttribute",
                "Product_ProductAttribute_Mapping",
                "ProductAttributeValue",
                "Product_Category_Mapping",
                "Product_Manufacturer_Mapping",
                "product",
                "Product_Picture_Mapping",
                "ProductReviewHelpfulness",
                "ProductReview",
                "Product_SpecificationAttribute_Mapping",
                "ProductTag",
                "ProductTemplate",
                "ProductWarehouseInventory",
                "RelatedProduct",
                "SpecificationAttribute",
                "SpecificationAttributeOption",
                "TierPrice",

                //Common
                "AddressAttribute",
                "AddressAttributeValue",
                "Address",
                "GenericAttribute",
                "SearchTerm",

                //Configuration
                "ParaSetting",

                //Directory
                "Country",
                "Currency",
                "MeasureDimension",
                "MeasureWeight",
                "StateProvince",

                //Discounts
                "Discount",
                "DiscountRequirement",
                "DiscountUsageHistory",

                 //Localization
                "Language",
                "LocaleStringResource",
                "LocalizedProperty",

                //Media
                "Download",
                "Picture",

                //Orders
                "CheckoutAttribute",
                "CheckoutAttributeValue",
                "GiftCard",
                "GiftCardUsageHistory",
                "OrderItem",
                "Order",
                "OrderNote",
                "RecurringPaymentHistory",
                "RecurringPayment",
                "ReturnRequestAction",
                "ReturnRequest",
                "ReturnRequestReason",
                "ShoppingCartItem",

                //Seo
                "UrlRecord",

                //Shipping
                "DeliveryDate",
                "ShipmentItem",
                "Shipment",
                "ShippingMethod",
                "Warehouse",

                //Stores
                "Store",
                "StoreMapping",

                //Tax
                "TaxCategory",

                //Vendors
                "Vendor",
                "VendorNote",

            };


            foreach (var tablename in tableNames)
            {
                string sqlAlter = @"Alter table [{0}] add constraint DF_{0}_CreationTime default getdate() for CreationTime
                                Alter table [{0}] add constraint DF_{0}_IsDeleted default 0 for IsDeleted";
                sqlAlter = string.Format(sqlAlter, tablename);
                Sql(sqlAlter);
            }
            //Sql("Alter table product add constraint DF_Product_CreationTime default getdate() for CreationTime");
            //Sql("Alter table product add constraint DF_Product_IsDeleted default 0 for IsDeleted");

            ////Store
            //Sql("Alter table Store add constraint DF_Store_CreationTime default getdate() for CreationTime");
            //Sql("Alter table Store add constraint DF_Store_IsDeleted default 0 for IsDeleted");
            #endregion
        }
    }
}
