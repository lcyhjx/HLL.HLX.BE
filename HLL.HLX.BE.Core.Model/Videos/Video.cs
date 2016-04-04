using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Videos
{
    /// <summary>
    /// 视频实体类
    /// </summary>
    public class Video : FullAuditedEntity<long, User>
    {
        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频封面的图片地址
        /// </summary>
        public string CoverPicPath { get; set; }

        /// <summary>
        /// 视频流媒体地址
        /// </summary>
        public string StreamMediaPath { get; set; }

        /// <summary>
        /// 预计直播开始时间
        /// </summary>
        public DateTime? EstimatedStartTime { get; set; }

        /// <summary>
        /// 实际直播开始时间
        /// </summary>
        public DateTime? ActualStartTime { get; set; }

        /// <summary>
        /// 实际直播结束时间
        /// </summary>
        public DateTime? ActualEndTime { get; set; }

        /// <summary>
        /// 视频状态
        /// </summary>
        public VideoStatus Status { get; set; }


        /// <summary>
        /// 视频发布人
        /// </summary>
        public long PublishUserId { get; set; }


        /// <summary>
        /// 关注人数
        /// </summary>
        public long limelightCount { get; set; }

    }
}
