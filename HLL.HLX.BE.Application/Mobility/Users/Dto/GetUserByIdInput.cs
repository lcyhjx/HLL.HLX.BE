using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    public class GetUserByIdInput : BaseInput
    {
         [Required]
        public long? UserId { get; set; }

       
    }

}
