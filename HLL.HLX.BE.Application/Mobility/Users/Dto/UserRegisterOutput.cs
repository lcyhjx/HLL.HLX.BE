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