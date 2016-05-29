using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Application.MobilityH5.Orders.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Orders
{
    public interface IShoppingCartAppService : IApplicationService
    {
        /// <summary>
        /// 添加产品到购物车- For Detail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>        
        AddProductToCartForDetailsOutput AddProductToCartForDetails(AddProductToCartForDetailsInput input);


        /// <summary>
        /// 我的购物车
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>        
        MyCartOutput MyCart(MyCartInput input);
    }
}
