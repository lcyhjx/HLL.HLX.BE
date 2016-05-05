using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Videos
{
    public  class UserLiveRoom : FullAuditedEntity<int, User>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 直播间Id
        /// </summary>

        public string LiveRoomId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public UserLiveRommStatus Status { get; set; }

        /// <summary>
        /// 进入直播间的时间
        /// </summary>
        public DateTime? EnterTime { get; set; }

        /// <summary>
        /// 退出直播间的时间
        /// </summary>
        public DateTime? LeaveTime { get; set; }
    }
}
