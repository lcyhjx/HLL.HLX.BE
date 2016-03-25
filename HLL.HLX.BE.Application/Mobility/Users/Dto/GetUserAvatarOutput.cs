using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    public class GetUserAvatarOutput : IOutputDto
    {

        public List<UserAvatarDto> UserAvatars { get; set; }
    }
}
