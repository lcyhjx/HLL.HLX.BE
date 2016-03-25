using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using AutoMapper;
using HLL.HLX.BE.Application.Common.Dto;
using HLL.HLX.BE.Application.Mobility.Users.Dto;
using HLL.HLX.BE.Core.Business.Users;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Application.Mobility.Users
{
    public class UserAppService : HlxBeAppServiceBase, IUserAppService
    {
        private readonly IUserAvatarRepository _userAvatarRepository;
        private readonly UserDomainService _userDomainService;
        private readonly IUserRepository _userRepository;

        public UserAppService(UserDomainService userDomainService,
            IUserRepository userRepository,
            IUserAvatarRepository userAvatarRepository)
        {
            _userDomainService = userDomainService;
            _userRepository = userRepository;
            _userAvatarRepository = userAvatarRepository;
        }

        /// <summary>
        ///     用户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UserRegisterOutput> UserRegister(UserRegisterInput input)
        {
            //input dto -> business obj
            var user = input.Map2User(AbpSession.GetTenantId());

            //用户注册
            var result = await _userDomainService.UserRegister(user);

            //business obj -> output dto
            var dtoItem = Mapper.Map<IdentityResultDto>(result);
            return new UserRegisterOutput
            {
                Result = dtoItem,
                UserId = user.Id
            };
        }

        /// <summary>
        ///     重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ResetPasswordOutput ResetPassword(ResetPasswordInput input)
        {
            //重置密码 
            _userDomainService.ResetPassword(input.PhoneNumber, input.SmsVerificationCode, input.Password);

            return new ResetPasswordOutput();
        }

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetUserOutput GetUser(GetUserInput input)
        {
            //查询用户
            var user = _userRepository.Get(AbpSession.GetUserId());

            //output
            var dtoItem = Mapper.Map<UserDto>(user);

            return new GetUserOutput
            {
                User = dtoItem
            };
        }

        /// <summary>
        ///     获取用户信息 by id ---- add by eleven
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetUserByIdOutput GetUserById(GetUserByIdInput input)
        {
            //查询用户
            var user = _userRepository.Get(input.UserId.GetValueOrDefault());

            //output
            var dtoItem = Mapper.Map<UserDto>(user);

            return new GetUserByIdOutput
            {
                User = dtoItem
            };
        }

        /// <summary>
        ///     更新用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public UpdateUserOutput UpdateUser(UpdateUserInput input)
        {
            //input -> business obj
            var user = Mapper.Map<User>(input);
            user.Id = AbpSession.GetUserId();

            //更新用户信息
            _userDomainService.UpdateUser(user);

            return new UpdateUserOutput();
        }

        /// <summary>
        ///     获取用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetUserAvatarOutput GetUserAvatar(GetUserAvatarInput input)
        {
            var userAvatars = _userAvatarRepository.GetAll()
                .Where(x => x.UserId == AbpSession.GetUserId()).ToList();

            var dtoItems = Mapper.Map<List<UserAvatarDto>>(userAvatars);
            return new GetUserAvatarOutput
            {
                UserAvatars = dtoItems
            };
        }

        /// <summary>
        ///     更新用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public UpdateUserAvatarOutput UpdateUserAvatar(UpdateUserAvatarInput input)
        {
            var userId = AbpSession.GetUserId();
            _userDomainService.UpdateUserAvatar(userId, input.ImageBytes);

            //input -> business obj
            //UserAvatar userAvatar = Mapper.Map<UserAvatar>(input);
            //userAvatar.UserId = AbpSession.UserId;

            //更新用户头像
            //_userDomainService.UpdateUserAvatar(userAvatar, input.ImageBytes);

            return new UpdateUserAvatarOutput();
        }
    }
}