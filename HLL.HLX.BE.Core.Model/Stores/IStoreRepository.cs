using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace HLL.HLX.BE.Core.Model.Stores
{
    public interface IStoreRepository : IRepository<Store, int>
    {
    }
}
