using Abp.Application.Services.Dto;

namespace HLL.HLX.BE.Application.MobilityH5.Orders.Dto
{
    
    public partial class WishlistEmailAFriendDto : EntityDto
    {
        
        public string FriendEmail { get; set; }

        public string YourEmailAddress { get; set; }

        public string PersonalMessage { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}