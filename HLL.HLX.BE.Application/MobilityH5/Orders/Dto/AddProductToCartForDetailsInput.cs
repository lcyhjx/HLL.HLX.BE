using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HLL.HLX.BE.Application.Common.Dto;
using HLL.HLX.BE.Core.Model.Orders;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public class AddProductToCartForDetailsInput : BaseInput
    {
        /// <summary>
        ///     产品id
        /// </summary>
        [Required]
        public int? ProductId { get; set; }

        /// <summary>
        ///     产品id
        /// </summary>
        [Required]
        public ShoppingCartType ShoppingCartTypeId { get; set; }



        /// <summary>
        ///     产品的属性信息
        /// </summary>
        public List<CartItemAttributeDto> CartItemAttributes { get; set; }

        /// <summary>
        ///     如果产品是GitCard, GiftCard的相关属性
        /// </summary>
        public CartItemGiftCardDto CartItemGiftCard { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int? Quantity { get; set; }


        public DateTime? RentalStartDate { get; set; }
        public DateTime? RentalEndDate { get; set; }


        public decimal? CustomerEnteredPrice { get; set; }
       
    }
}