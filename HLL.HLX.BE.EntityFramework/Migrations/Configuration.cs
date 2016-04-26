using System.Data.Entity.Migrations;
using HLL.HLX.BE.EntityFramework.Migrations.SeedData;

namespace HLL.HLX.BE.EntityFramework.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<HlxBeDbContext>
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
