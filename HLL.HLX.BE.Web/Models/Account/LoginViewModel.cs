using System.ComponentModel.DataAnnotations;

namespace HLL.HLX.BE.Web.Models.Account
{
    public class LoginViewModel
    {
        public string TenancyName { get; set; }

        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        /// <summary>
        /// 移动端App版本号
        /// </summary>
        public string MobileAppVersion { get; set; }

        /// <summary>
        /// 移动端操作系统
        /// </summary>
        public string MobileOS { get; set; }


        /// <summary>
        /// 移动端操作系统版本
        /// </summary>
        public string MobileOSVersion { get; set; }
    }
}