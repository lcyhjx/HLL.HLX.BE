using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HLL.HLX.BE.Application.Common.Dto;
using HLL.HLX.BE.Common.Util;
using HLL.HLX.BE.Core.Model.Users;
using Microsoft.AspNet.Identity;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    public class UserRegisterInput : BaseInput
    {
        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     短信验证码
        /// </summary>
        /// <summary>
        ///     密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        ///     用户呢称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        ///     自定义验证input
        /// </summary>
        /// <param name="results"></param>
        public override void AddValidationErrors(List<ValidationResult> results)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                results.Add(new ValidationResult("手机号不能为空", new[] {"PhoneNumber"}));
            }
            //SmsVerificationCode = SmsVerificationCode.Trim();
            //if (string.IsNullOrEmpty(SmsVerificationCode))
            //{
            //    results.Add(new ValidationResult("验证码不能为空", new[] { "SmsVerificationCode" }));
            //}
            Password = Password.Trim();
            if (string.IsNullOrEmpty(Password))
            {
                results.Add(new ValidationResult("密码不能为空", new[] {"Password"}));
            }

            if (!CommonUtil.CheckIsValidPassword(Password))
            {
                results.Add(new ValidationResult("密码必须由英文字母，英文符号和数字组成", new[] {"Password"}));
            }

            if (string.IsNullOrEmpty(NickName))
            {
                results.Add(new ValidationResult("用户呢称不能为空", new[] {"NickName"}));
            }
        }

        //public void Normalize()
        //{
        //    if (!string.IsNullOrEmpty(ReferrerPhoneOrCode))
        //    {
        //        ReferrerPhoneOrCode = ReferrerPhoneOrCode.ToUpper();
        //    }
        //}


        /// <summary>
        ///     根据input创建User对象
        /// </summary>
        /// <returns></returns>
        public User Map2User(int? tenantId)
        {
            //创建新用户
            var user = new User
            {
                TenantId = tenantId,
                Name = !string.IsNullOrEmpty(NickName) ? NickName : PhoneNumber,
                Surname = PhoneNumber,
                EmailAddress = PhoneNumber + "@51hlx.com",
                IsEmailConfirmed = true,
                UserName = PhoneNumber,
                Password = Password,
                PhoneNumber = PhoneNumber,
                NickName = NickName
            };
            //加密密码
            user.Password = new PasswordHasher().HashPassword(user.Password);
            return user;
        }
    }
}