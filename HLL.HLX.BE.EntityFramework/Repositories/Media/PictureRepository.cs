using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using HLL.HLX.BE.Core.Model.Media;

namespace HLL.HLX.BE.EntityFramework.Repositories.Media
{
    public class PictureRepository : HlxBeRepositoryBase<Picture, int>, IPictureRepository
    {
        public PictureRepository(IDbContextProvider<HlxBeDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
