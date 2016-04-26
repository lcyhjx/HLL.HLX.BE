using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    public partial class CustomerBackInStockSubscriptionsDto
    {
        public CustomerBackInStockSubscriptionsDto()
        {
            this.Subscriptions = new List<BackInStockSubscriptionModel>();
        }

        public IList<BackInStockSubscriptionModel> Subscriptions { get; set; }
        //public PagerModel PagerModel { get; set; }

        #region Nested classes

        public partial class BackInStockSubscriptionModel : EntityDto<int>
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string SeName { get; set; }
        }

        #endregion
    }
}