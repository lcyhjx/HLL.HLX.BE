using Abp.Application.Services;
using HLL.HLX.BE.Application.MobilityH5.Catalog.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog
{
    public interface IProductAppService : IApplicationService
    {
        #region Product details page
        /// <summary>
        /// 获取产品明细页面所需要的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ProductDetailsOutput ProductDetails(ProductDetailsInput input);
        #endregion
    }
}
