using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Catalog.Dto
{
    public partial class TopMenuDto 
    {
        public TopMenuDto()
        {
            Categories = new List<CategorySimpleDto>();
            Topics = new List<TopMenuTopicModel>();
        }

        public IList<CategorySimpleDto> Categories { get; set; }
        public IList<TopMenuTopicModel> Topics { get; set; }

        public bool BlogEnabled { get; set; }
        public bool NewProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }

        #region Nested classes

        public class TopMenuTopicModel : EntityDto<int>
        {
            public string Name { get; set; }
            public string SeName { get; set; }
        }

        #endregion
    }
}