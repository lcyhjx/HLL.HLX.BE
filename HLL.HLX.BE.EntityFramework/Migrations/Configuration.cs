using System.Data.Entity.Migrations;
using HLL.HLX.BE.EntityFramework.EF;
using HLL.HLX.BE.Migrations.SeedData;

namespace HLL.HLX.BE.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<HlxBeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HlxBe";
        }

        protected override void Seed(HlxBeDbContext context)
        {
            new InitialDataBuilder(context).Build();
        }
    }
}