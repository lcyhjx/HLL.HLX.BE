using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.MobilityH5.Catalog.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public partial class MiniShoppingCartDto : EntityDto
    {
        public MiniShoppingCartDto()
        {
            Items = new List<ShoppingCartItemModel>();
        }

        public IList<ShoppingCartItemModel> Items { get; set; }
        public int TotalProducts { get; set; }
        public string SubTotal { get; set; }
        public bool DisplayShoppingCartButton { get; set; }
        public bool DisplayCheckoutButton { get; set; }
        public bool CurrentCustomerIsGuest { get; set; }
        public bool AnonymousCheckoutAllowed { get; set; }
        public bool ShowProductImages { get; set; }


        #region Nested Classes

        public partial class ShoppingCartItemModel : EntityDto
        {
            public ShoppingCartItemModel()
            {
                Picture = new PictureDto();
            }

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public string ProductSeName { get; set; }

            public int Quantity { get; set; }

            public string UnitPrice { get; set; }

            public string AttributeInfo { get; set; }

            public PictureDto Picture { get; set; }
        }

        #endregion
    }
}