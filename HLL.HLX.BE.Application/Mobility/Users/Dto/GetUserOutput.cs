using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    public class GetUserOutput : IOutputDto
    {
        public UserDto User { get; set; }
    }
}