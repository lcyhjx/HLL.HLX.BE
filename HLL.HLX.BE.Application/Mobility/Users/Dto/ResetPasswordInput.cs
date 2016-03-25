using System.ComponentModel.DataAnnotations;
using HLL.HLX.BE.Application.Common.Dto;

namespace HLL.HLX.BE.Application.Mobility.Users.Dto
{
    public class ResetPasswordInput : BaseInput
    {
        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     短信验证码
        /// </summary>
        //[Required]
        public string SmsVerificationCode { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}