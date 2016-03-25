﻿using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using HLL.HLX.BE.Core.Model.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace HLL.HLX.BE.Core.Model.Users
{
    public class User : AbpUser<Tenant, User>
    {
        public const string DefaultPassword = "123qwe";

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
        //[Index(IsUnique = true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  个性签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 头像图片存储路径
        /// </summary>
        public string AvatarFilePath { get; set; }



        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}