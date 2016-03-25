namespace HLL.HLX.BE.EntityFramework.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class _20150325_Add_User : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HlxUserAvatar",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        ImageFilePath = c.String(maxLength: 200),
                        Name = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserAvatar_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            AddColumn("dbo.AbpUsers", "NickName", c => c.String(maxLength: 50));
            AddColumn("dbo.AbpUsers", "Gender", c => c.String(maxLength: 10));
            AddColumn("dbo.AbpUsers", "Company", c => c.String(maxLength: 200));
            AddColumn("dbo.AbpUsers", "Title", c => c.String(maxLength: 100));
            AddColumn("dbo.AbpUsers", "PhoneNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.AbpUsers", "Signature", c => c.String());
            AddColumn("dbo.AbpUsers", "AvatarFilePath", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HlxUserAvatar", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.HlxUserAvatar", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.HlxUserAvatar", "CreatorUserId", "dbo.AbpUsers");
            DropIndex("dbo.HlxUserAvatar", new[] { "CreatorUserId" });
            DropIndex("dbo.HlxUserAvatar", new[] { "LastModifierUserId" });
            DropIndex("dbo.HlxUserAvatar", new[] { "DeleterUserId" });
            DropColumn("dbo.AbpUsers", "AvatarFilePath");
            DropColumn("dbo.AbpUsers", "Signature");
            DropColumn("dbo.AbpUsers", "PhoneNumber");
            DropColumn("dbo.AbpUsers", "Title");
            DropColumn("dbo.AbpUsers", "Company");
            DropColumn("dbo.AbpUsers", "Gender");
            DropColumn("dbo.AbpUsers", "NickName");
            DropTable("dbo.HlxUserAvatar",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserAvatar_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
