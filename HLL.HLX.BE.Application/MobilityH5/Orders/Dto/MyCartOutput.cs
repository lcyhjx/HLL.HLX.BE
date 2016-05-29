using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    public class MyCartOutput :IOutputDto
    {
        public ShoppingCartDto ShoppingCart { get; set; }
    }
}
