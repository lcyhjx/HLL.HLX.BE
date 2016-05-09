using Abp.Domain.Repositories;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Core.Model.Orders
{
    public interface IGiftCardUsageHistoryRepository : IRepository<GiftCardUsageHistory, int>
    {
    }
}
