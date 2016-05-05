﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Abp.IO;
using Abp.UI;
using Castle.DynamicProxy.Generators.Emitters;
using HLL.HLX.BE.Common.Util;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.Users;
using Microsoft.AspNet.Identity;

namespace HLL.HLX.BE.Core.Business.Users
{
    /// <summary>
    /// 用户相关业务类
    /// </summary>
    public class UserDomainService : DomainService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager _userManager;
        private readonly IUserAvatarRepository _userAvatarRepository;

        public UserDomainService(IUserRepository userRepository,
           UserManager userManager,
           IUserAvatarRepository userAvatarRepository)
        {
            _userRepository = userRepository;
            _userAvatarRepository = userAvatarRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        public async Task<IdentityResult> UserRegister(User user)
        {           

            User existUser = _userRepository.GetAll().FirstOrDefault(x => x.UserName == user.UserName || x.PhoneNumber == user.PhoneNumber);
            if (existUser != null)
            {
                throw new UserFriendlyException("用户" + user.UserName + "已注册");
            }

            ////验证短信验证码 - by Eleven
            //if (!_smsLogRepository.ValidVCode(user.PhoneNumber, smsVerificationCode, SmsLogType.Register))
            //{
            //    throw new UserFriendlyException("验证码错误");
            //}

            //保存到数据库 
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                CurrentUnitOfWork.SaveChanges();
            }

            return result;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="smsVerificationCode"></param>
        /// <param name="password"></param>
        public void ResetPassword(string phoneNumber, string smsVerificationCode, string password)
        {
            //验证短信验证码 - by Eleven
            //if (!_smsLogRepository.ValidVCode(phoneNumber, smsVerificationCode, SmsLogType.ResetPassword))
            //{
            //    throw new UserFriendlyException("验证码错误");
            //}

            //更改用户密码 - Lakin
            User user = _userRepository.GetAll().FirstOrDefault(x => x.UserName == phoneNumber);
            if (user == null)
            {
                var errorMsg = "用户" + phoneNumber + "不存在";
                throw new UserFriendlyException(errorMsg);
            }

            user.Password = new PasswordHasher().HashPassword(password);

            _userRepository.Update(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="editUser"></param>
        /// <returns></returns>
        
        public void UpdateUser(User editUser)
        {
            User user = _userRepository.Get(editUser.Id);
            user.Name = editUser.Name;
            user.NickName = editUser.NickName;
            user.Gender = editUser.Gender;
            user.Company = editUser.Company;
            user.Title = editUser.Title;
            user.PhoneNumber = editUser.PhoneNumber;
            user.Signature = editUser.Signature;

            _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="userId"></param>        
        /// <param name="imageBase64">用户头像</param>
        public void UpdateUserAvatar(long userId, string imageBase64)
        {
            #region save iamge
            
            string fileName = "UserAvatar_" + userId;            
            string relativePath = HlxBeConsts.USER_AVATAR_DIR + "\\" + fileName;
            string fullNameNoExtension = AppDomain.CurrentDomain.BaseDirectory + relativePath;

            
            ImageUtil.Base64StringToImage(imageBase64, fullNameNoExtension);

            var imageFilePath = relativePath;
            #endregion

            var item =_userAvatarRepository.FirstOrDefault(x => x.UserId == userId);
            if (item == null)
            {
                item = new UserAvatar()
                {
                    UserId = userId,
                    ImageFilePath = imageFilePath                    
                };
                _userAvatarRepository.Insert(item);
            }
            else
            {
                item.ImageFilePath = imageFilePath;
                _userAvatarRepository.Update(item);
            }

            //_userRepository.Update(userId, x =>
            //{
            //    x.AvatarFilePath = imageFilePath;
            //});
        }
    }
}
