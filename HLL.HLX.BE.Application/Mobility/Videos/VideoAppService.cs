using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using AutoMapper;
using HLL.HLX.BE.Application.Mobility.Videos.Dto;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Core.Business.Videos;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.Application.Mobility.Videos
{
    public class VideoAppService : HlxBeAppServiceBase, IVideoAppService
    {
        private readonly VideoDomainService _videoDomainService;

        public VideoAppService(VideoDomainService videoDomainService)
        {
            _videoDomainService = videoDomainService;
        }


        /// <summary>
        ///  获取预告的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetUnstartedVideoOutput<VideoExDto, VideoEx> GetUnstartedVideo(GetUnstartedVideoInput input)
        {
            var videos = _videoDomainService.GetVideoByStatus(VideoStatus.Unstarted
                , input.PageIndex.GetValueOrDefault()
                , input.PageSize.GetValueOrDefault());

            //IPagedList<VideoExDto> videoDtos = videos.ConvertTo<VideoExDto>();

            return new GetUnstartedVideoOutput<VideoExDto,VideoEx>(videos)
            {
                //Videos = videoDtos,
            };
        }


        /// <summary>
        ///  获取正在直播的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetLiveVideoOutput<VideoExDto, VideoEx> GetLiveVideo(GetLiveVideoInput input)
        {
            var videos = _videoDomainService.GetVideoByStatus(VideoStatus.Started
                ,input.PageIndex.GetValueOrDefault()
                ,input.PageSize.GetValueOrDefault());        
            
            return new GetLiveVideoOutput<VideoExDto, VideoEx>(videos)
            {
                //Videos = videoDtos,
            };
        }

        

        /// <summary>
        /// 获取录播的视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public GetEndedVideoOutput<VideoExDto, VideoEx> GetEndedVideo(GetEndedVideoInput input)
        {
            var videos = _videoDomainService.GetVideoByStatus(VideoStatus.Ended
                , input.PageIndex.GetValueOrDefault()
                , input.PageSize.GetValueOrDefault());
            

            return new GetEndedVideoOutput<VideoExDto, VideoEx>(videos)
            {
                //Videos = videoDtos,
            };              
        }

        /// <summary>
        /// 发布视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public PublishVideoOutput PublishVideo(PublishVideoInput input)
        {

            var video = Mapper.Map<Video>(input.Video);            

            video.Status = VideoStatus.Unstarted;
            video.PublishUserId = AbpSession.GetUserId();
            video.limelightCount = 0;
            video.PublishTime = DateTime.Now;

           long id =  _videoDomainService.PublishVideo(video,input.Video.LivePreviewImageBase64);

            return new PublishVideoOutput
            {
                Id =  id
            };

        }


        /// <summary>
        /// 开始视频直播
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public StartVideoOutput StartVideo(StartVideoInput input)
        {
            _videoDomainService.StartVideo(input.VideoId.GetValueOrDefault(), AbpSession.GetUserId());
            return new StartVideoOutput();
        }

        /// <summary>
        /// 结束视频直播
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public EndVideoOutput EndVideo(EndVideoInput input)
        {

            _videoDomainService.EndVideo(input.LiveRoomId,AbpSession.GetUserId());

            return new EndVideoOutput()
            {

            };
        }

        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public DeleteVideoOutput DeleteVideo(DeleteVideoInput input)
        {
            _videoDomainService.DeleteVideo(input.Id.GetValueOrDefault());
            return new DeleteVideoOutput();
        }


        /// <summary>
        /// 用户进入直播间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public EnterRoomOutput EnterRoom(EnterRoomInput input)
        {
            
            _videoDomainService.EnterRoom(input.LiveRoomId,AbpSession.GetUserId());
            return new EnterRoomOutput();

        }

        /// <summary>
        /// 用户退出直播间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public LeaveRoomOutput LeaveRoom(LeaveRoomInput input)
        {
            _videoDomainService.LeaveRoom(input.LiveRoomId,AbpSession.GetUserId());
            return new LeaveRoomOutput();
        }
    }
}
