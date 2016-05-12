using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Application.MobilityH5.Common.Dto
{
    public partial class AddressAttributeDto : EntityDto
    {
        public AddressAttributeDto()
        {
            Values = new List<AddressAttributeValueDto>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<AddressAttributeValueDto> Values { get; set; }
    }

    public partial class AddressAttributeValueDto : EntityDto
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}