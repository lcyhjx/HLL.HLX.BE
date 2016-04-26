using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
