namespace HLL.HLX.BE.Application.MobilityH5.Products.Dto
{
    public partial class ProductSpecificationDto 
    {
        public int SpecificationAttributeId { get; set; }

        public string SpecificationAttributeName { get; set; }

        //this value is already HTML encoded
        public string ValueRaw { get; set; }
    }
}