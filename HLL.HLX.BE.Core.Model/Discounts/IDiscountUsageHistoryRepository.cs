using Abp.Domain.Repositories;
using HLL.HLX.BE.Core.Model.Directory;

namespace HLL.HLX.BE.Core.Model.Discounts
{
    public interface IDiscountUsageHistoryRepository : IRepository<DiscountUsageHistory, int>
    {
    }
}
