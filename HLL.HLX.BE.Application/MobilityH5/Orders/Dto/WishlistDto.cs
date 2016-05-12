using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.MobilityH5.Catalog.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public partial class WishlistModel : EntityDto
    {
        public WishlistModel()
        {
            Items = new List<ShoppingCartItemModel>();
            Warnings = new List<string>();
        }

        public Guid CustomerGuid { get; set; }
        public string CustomerFullname { get; set; }

        public bool EmailWishlistEnabled { get; set; }

        public bool ShowSku { get; set; }

        public bool ShowProductImages { get; set; }

        public bool IsEditable { get; set; }

        public bool DisplayAddToCart { get; set; }

        public bool DisplayTaxShippingInfo { get; set; }

        public IList<ShoppingCartItemModel> Items { get; set; }

        public IList<string> Warnings { get; set; }
        
		#region Nested Classes

        public partial class ShoppingCartItemModel : EntityDto
        {
            public ShoppingCartItemModel()
            {
                Picture = new PictureDto();
                //AllowedQuantities = new List<SelectListItem>();
                Warnings = new List<string>();
            }
            public string Sku { get; set; }

            public PictureDto Picture {get;set;}

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public string ProductSeName { get; set; }

            public string UnitPrice { get; set; }

            public string SubTotal { get; set; }

            public string Discount { get; set; }

            public int Quantity { get; set; }
            //public List<SelectListItem> AllowedQuantities { get; set; }
            
            public string AttributeInfo { get; set; }

            public string RecurringInfo { get; set; }

            public string RentalInfo { get; set; }

            public IList<string> Warnings { get; set; }

        }

		#endregion
    }
}