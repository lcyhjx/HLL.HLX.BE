using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HLL.HLX.BE.Core.Business.Users;
using HLL.HLX.BE.Core.Model.Users;
using HLL.HLX.BE.Users;

namespace HLL.HLX.BE.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
    }
}
