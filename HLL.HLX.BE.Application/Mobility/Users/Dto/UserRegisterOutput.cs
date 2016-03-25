using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    public class UserRegisterOutput : IOutputDto
    {
        public IdentityResultDto Result { get; set; }

        public long UserId { get; set; }
    }
}
