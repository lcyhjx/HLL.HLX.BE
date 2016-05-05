using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using HLL.HLX.BE.Application.Mobility.Videos.Dto;
using HLL.HLX.BE.Core.Business.Videos;

namespace HLL.HLX.BE.Application.Mobility.Videos
{
    public interface IVideoAppService : IApplicationService
    {
        /// <summary>
        ///  获取预告的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetUnstartedVideoOutput<VideoExDto, VideoEx> GetUnstartedVideo(GetUnstartedVideoInput input);

        /// <summary>
        ///  获取正在直播或预告中的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetLiveVideoOutput<VideoExDto, VideoEx> GetLiveVideo(GetLiveVideoInput input);

        /// <summary>
        /// 获取录播的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GetEndedVideoOutput<VideoExDto, VideoEx> GetEndedVideo(GetEndedVideoInput input);


        /// <summary>
        /// 发布视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PublishVideoOutput PublishVideo(PublishVideoInput input);

        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DeleteVideoOutput DeleteVideo(DeleteVideoInput input);

        /// <summary>
        /// 开始视频直播
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>        
        StartVideoOutput StartVideo(StartVideoInput input);

        /// <summary>
        /// 结束视频直播
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>        
        EndVideoOutput EndVideo(EndVideoInput input);


        /// <summary>
        /// 用户进入直播间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>        
        EnterRoomOutput EnterRoom(EnterRoomInput input);


        /// <summary>
        /// 用户退出直播间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>        
        LeaveRoomOutput LeaveRoom(LeaveRoomInput input);

    }
}
