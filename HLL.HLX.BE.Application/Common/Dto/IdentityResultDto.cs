using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.AspNet.Identity;

namespace HLL.HLX.BE.Application.Common.Dto
{
    [AutoMapFrom(typeof (IdentityResult))]
    public class IdentityResultDto : EntityDto
    {
        // Summary:
        //     List of errors
        public IEnumerable<string> Errors { get; set; }
        //
        // Summary:
        //     True if the operation was successful
        public bool Succeeded { get; set; }
    }
}