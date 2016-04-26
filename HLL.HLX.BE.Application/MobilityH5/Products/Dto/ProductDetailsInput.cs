using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    public class ProductDetailsInput : BaseInput
    {
        [Required]
        public int? ProductId { get; set; }

        public int Updatecartitemid { get; set; }
    }
}
