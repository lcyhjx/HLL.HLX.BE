
using HLL.HLX.BE.Common.Configuration;

namespace HLL.HLX.BE.Core.Model.Directory
{
    public class MeasureSettings : ISettings
    {
        public int BaseDimensionId { get; set; }
        public int BaseWeightId { get; set; }
    }
}