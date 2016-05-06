using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Application.MobilityH5.Products.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Products
{
    public interface IProductAppService : IApplicationService
    {
        #region Product details page
        ProductDetailsOutput ProductDetails(ProductDetailsInput input);
        #endregion
    }
}
