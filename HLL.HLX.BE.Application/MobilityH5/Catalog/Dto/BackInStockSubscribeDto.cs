﻿namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class BackInStockSubscribeDto 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSeName { get; set; }

        public bool IsCurrentCustomerRegistered { get; set; }
        public bool SubscriptionAllowed { get; set; }
        public bool AlreadySubscribed { get; set; }

        public int MaximumBackInStockSubscriptions { get; set; }
        public int CurrentNumberOfBackInStockSubscriptions { get; set; }
    }
}