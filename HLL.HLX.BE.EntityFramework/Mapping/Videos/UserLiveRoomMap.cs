using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Core.Model.Videos;

namespace HLL.HLX.BE.EntityFramework.Mapping.Videos
{
    public class UserLiveRoomMap : HlxEntityTypeConfiguration<UserLiveRoom>
    {
        public UserLiveRoomMap()
        {
            ToTable("UserLiveRoom");

            Property(x => x.UserId).IsRequired();
            Property(x => x.LiveRoomId).IsRequired();
            Property(x => x.Status).IsRequired();           

        }
    }
}
