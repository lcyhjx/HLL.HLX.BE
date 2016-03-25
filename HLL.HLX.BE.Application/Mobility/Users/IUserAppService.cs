using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Application.Mobility.Users.Dto;

namespace HLL.HLX.BE.Application.Mobility.Users
{
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        ///     用户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserRegisterOutput> UserRegister(UserRegisterInput input);

        /// <summary>
        ///     重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ResetPasswordOutput ResetPassword(ResetPasswordInput input);

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetUserOutput GetUser(GetUserInput input);

        /// <summary>
        ///     获取用户信息 by id ---- add by eleven
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetUserByIdOutput GetUserById(GetUserByIdInput input);

        /// <summary>
        ///     更新用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        UpdateUserOutput UpdateUser(UpdateUserInput input);

        /// <summary>
        ///     获取用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetUserAvatarOutput GetUserAvatar(GetUserAvatarInput input);

        /// <summary>
        ///     更新用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        UpdateUserAvatarOutput UpdateUserAvatar(UpdateUserAvatarInput input);
    }
}