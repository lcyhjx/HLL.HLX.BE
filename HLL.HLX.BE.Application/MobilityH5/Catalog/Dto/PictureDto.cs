using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class PictureDto : EntityDto<int>
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}
