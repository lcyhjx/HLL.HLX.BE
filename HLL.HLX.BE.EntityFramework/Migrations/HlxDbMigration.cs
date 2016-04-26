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
                 //Configuration
                "ParaSetting",

                 //Localization
                "Language",
                "LocaleStringResource",
                "LocalizedProperty",

                //Catalog
                "PredefinedProductAttributeValue",
                "product",
                "ProductAttribute",
                "ProductAttributeCombination",
                "Product_ProductAttribute_Mapping",
                "ProductAttributeValue",


                //Stores
                "Store",
                "StoreMapping",



            };


            foreach (var tablename in tableNames)
            {
                string sqlAlter = @"Alter table {0} add constraint DF_{0}_CreationTime default getdate() for CreationTime
                                Alter table {0} add constraint DF_{0}_IsDeleted default 0 for IsDeleted";
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
