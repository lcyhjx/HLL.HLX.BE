namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    
    public partial class ProductEmailAFriendDto 
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSeName { get; set; }

        public string FriendEmail { get; set; }

        public string YourEmailAddress { get; set; }

        public string PersonalMessage { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}