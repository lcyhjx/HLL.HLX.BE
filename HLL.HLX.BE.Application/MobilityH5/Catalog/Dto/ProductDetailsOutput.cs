using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public class ProductDetailsOutput : IOutputDto
    {
        public ProductDetailsDto ProductDetail { get; set; }
    }
}
