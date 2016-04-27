using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Web.Models.Account
{
    class UserModel
    {
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 公司职务
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 腾讯登录服务的用户签名
        /// </summary>
        public string TlsSig { get; set; }


        public UserModel()
        {

        }

        public UserModel(User user)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            this.Name = user.Name;
            this.NickName = user.NickName;
            this.Gender = user.Gender;
            this.Company = user.Company;
            this.Title = user.Title;
            this.PhoneNumber = user.PhoneNumber;
        }
    }
}
