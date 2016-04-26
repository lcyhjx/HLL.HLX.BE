using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    public partial class ProductTagDto : EntityDto<int>
    {
        public string Name { get; set; }

        public string SeName { get; set; }

        public int ProductCount { get; set; }
    }
}