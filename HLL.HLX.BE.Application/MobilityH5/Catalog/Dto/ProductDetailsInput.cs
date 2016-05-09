using System.ComponentModel.DataAnnotations;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public class ProductDetailsInput : BaseInput
    {
        [Required]
        public int? ProductId { get; set; }

        public int Updatecartitemid { get; set; }
    }
}
