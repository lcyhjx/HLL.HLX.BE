using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Application.Mobility.Users.Dto;

namespace HLL.HLX.BE.Application.Mobility.Users
{
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserRegisterOutput> UserRegister(UserRegisterInput input);
    }
}
