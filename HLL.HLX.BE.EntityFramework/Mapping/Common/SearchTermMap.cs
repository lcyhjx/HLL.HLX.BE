using HLL.HLX.BE.Core.Model.Common;

namespace HLL.HLX.BE.EntityFramework.Mapping.Common
{
    public partial class SearchTermMap : HlxEntityTypeConfiguration<SearchTerm>
    {
        public SearchTermMap()
        {
            this.ToTable("SearchTerm");
            this.HasKey(st => st.Id);
        }
    }
}
