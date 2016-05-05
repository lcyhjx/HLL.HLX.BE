using System;
using System.Linq;
using Abp.Domain.Services;
using Abp.UI;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Common.Util;
using HLL.HLX.BE.Core.Model;
using HLL.HLX.BE.Core.Model.Users;
using HLL.HLX.BE.Core.Model.Videos;
using HLL.HLX.BE.EntityFramework.Migrations.SeedData;

namespace HLL.HLX.BE.Core.Business.Videos
{
    /// <summary>
    ///     视频相关的业务类
    /// </summary>
    public class VideoDomainService : DomainService
    {
        private readonly IUserAvatarRepository _userAvatarRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IUserLiveRoomRepository _userLiveRoomRepository;

        public VideoDomainService(IVideoRepository videoRepository
            , IUserRepository userRepository
            , IUserAvatarRepository userAvatarRepository
            , IUserLiveRoomRepository userLiveRoomRepository)
        {
            _videoRepository = videoRepository;
            _userRepository = userRepository;
            _userAvatarRepository = userAvatarRepository;
            _userLiveRoomRepository = userLiveRoomRepository;
        }

        /// <summary>
        ///     根据状态查找视频
        /// </summary>
        /// <returns></returns>
        public IPagedList<VideoEx> GetVideoByStatus(VideoStatus status, int pageIndex = 0, int pageSize = int.MaxValue)
        {            
            var query = from v in _videoRepository.GetAll()
                join u in _userRepository.GetAll() on v.PublishUserId equals u.Id
                join ua in _userAvatarRepository.GetAll() on u.Id equals ua.UserId into uaTemp
                from uaEmpty in uaTemp.DefaultIfEmpty()
                where v.Status == status
                select new VideoEx
                {
                    Id = v.Id,
                    Title = v.Title,
                    StreamMediaPath = v.StreamMediaPath,
                    EstimatedStartTime = v.EstimatedStartTime,
                    ActualStartTime = v.ActualStartTime,
                    ActualEndTime = v.ActualEndTime,
                    Status = v.Status,
                    PublishUserId = v.PublishUserId,
                    limelightCount = v.limelightCount,
                    LiveRoomId = v.LiveRoomId,
                    ChatRoomId = v.ChatRoomId,
                    LivePreviewImagePath = v.LivePreviewImagePath,
                    PublishUserName = u.Name,
                    PublishUserAvatarImagePath = uaEmpty.ImageFilePath
                };
            query = query.OrderBy(x => x.Id);            

            var videos = new PagedList<VideoEx>(query, pageIndex, pageSize);
            return videos;
        }

        /// <summary>
        ///     发布视频
        /// </summary>
        /// <param name="video"></param>        
        /// <param name="livePreviewImageBase64">视频预览图</param>
        /// <returns></returns>
        public long PublishVideo(Video video, string livePreviewImageBase64)
        {
            
            var id = _videoRepository.InsertAndGetId(video);

            #region save iamge

            if (!string.IsNullOrEmpty(livePreviewImageBase64))
            {                
                var fileName = "LivePreviewImage_" + id;                
                var relativePath = HlxBeConsts.LIVE_PREVIEW_IMAGE_DIR + "\\" + fileName;
                var fullNameNoExtension = AppDomain.CurrentDomain.BaseDirectory + relativePath;                

                ImageUtil.Base64StringToImage(livePreviewImageBase64, fullNameNoExtension);

                var imageFilePath = relativePath;
                _videoRepository.Update(id,
                    x => { x.LivePreviewImagePath = imageFilePath; });
            }

            #endregion

            return id;
        }

        /// <summary>
        ///     删除视频
        /// </summary>
        /// <param name="id"></param>
        public void DeleteVideo(int id)
        {
            _videoRepository.Delete(id);
        }


        /// <summary>
        /// 开始视频直播
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="userId"></param>
        public void StartVideo(int videoId,long userId)
        {
            Video video = _videoRepository.FirstOrDefault(x => x.Id == videoId);
            if (video == null)
            {
                throw  new UserFriendlyException(string.Format("视频(Id:{0})不存在",videoId));            
            }
            video.Status = VideoStatus.Started;
            video.ActualStartTime = DateTime.Now;
            video.StartUserId = userId;

            _videoRepository.Update(video);
        }

        /// <summary>
        /// 结束视频直播
        /// </summary>
        /// <param name="liveRoomId"></param>
        /// <param name="userId"></param>
        public void EndVideo(string liveRoomId, long userId)
        {
            var videos =
                _videoRepository.GetAll()
                    .Where(x => x.Status == VideoStatus.Started && x.LiveRoomId == liveRoomId)
                    .ToList();
            if (videos == null || videos.Count == 0)
            {
                throw new UserFriendlyException(string.Format("当前房间({0})没有正在直播的视频", liveRoomId));
            }
            if (videos.Count > 1)
            {
                throw new UserFriendlyException(string.Format("当前房间({0})有多个(超过一个)正在直播的视频,请检查", liveRoomId));
            }


            videos[0].Status = VideoStatus.Ended;
            videos[0].ActualEndTime = DateTime.Now;
            videos[0].EndUserId = userId;

            _videoRepository.Update(videos[0]);

        }

        /// <summary>
        /// 用户进入直播间
        /// </summary>
        /// <param name="liveRoomId"></param>
        /// <param name="userId"></param>
        public void EnterRoom(string liveRoomId, long userId)
        {

            //检查用户是否已经在房间中
            var existItem =
                _userLiveRoomRepository.FirstOrDefault(
                    x => x.LiveRoomId == liveRoomId && x.UserId == userId && x.Status == UserLiveRommStatus.In);
            if (existItem != null)
            {
                throw  new UserFriendlyException(string.Format("用户(Id:{0})已经在房间({1})中",userId,liveRoomId));
            }
            var newItem = new UserLiveRoom()
            {
                UserId = userId,
                LiveRoomId = liveRoomId,
                Status = UserLiveRommStatus.In,
                EnterTime = DateTime.Now
            };

            _userLiveRoomRepository.Insert(newItem);
        }

        /// <summary>
        /// 用户退出直播间
        /// </summary>
        /// <param name="liveRoomId"></param>
        /// <param name="userId"></param>
        public void LeaveRoom(string liveRoomId, long userId)
        {

            //检查用户是否已经在房间中
            var item =
                _userLiveRoomRepository.FirstOrDefault(
                    x => x.LiveRoomId == liveRoomId && x.UserId == userId && x.Status == UserLiveRommStatus.In);
            if (item == null)
            {
                throw new UserFriendlyException(string.Format("用户(Id:{0})不在房间({1})中", userId, liveRoomId));
            }
            item.Status = UserLiveRommStatus.Out;
            item.LeaveTime = DateTime.Now;

            _userLiveRoomRepository.Update(item);
        }
    }
}