using System.Linq;
using Abp.Application.Editions;
using HLL.HLX.BE.Core.Model.Editions;
using HLL.HLX.BE.EntityFramework;
using HLL.HLX.BE.EntityFramework.EF;

namespace HLL.HLX.BE.Migrations.SeedData
{
    public class DefaultEditionsBuilder
    {
        private readonly HlxBeDbContext _context;

        public DefaultEditionsBuilder(HlxBeDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new Edition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

                //TODO: Add desired features to the standard edition, if wanted!
            }   
        }
    }
}