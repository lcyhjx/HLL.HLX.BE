using HLL.HLX.BE.EntityFramework;
using EntityFramework.DynamicFilters;
using HLL.HLX.BE.EntityFramework.EF;
using HLL.HLX.BE.EntityFramework.Migrations.SeedData;

namespace HLL.HLX.BE.Migrations.SeedData
{
    public class InitialDataBuilder
    {
        private readonly HlxBeDbContext _context;

        public InitialDataBuilder(HlxBeDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            _context.DisableAllFilters();

            new DefaultEditionsBuilder(_context).Build();
            new DefaultTenantRoleAndUserBuilder(_context).Build();
            new DefaultLanguagesBuilder(_context).Build();
        }
    }
}
