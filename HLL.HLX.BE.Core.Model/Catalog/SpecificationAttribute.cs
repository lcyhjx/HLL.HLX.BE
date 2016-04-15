using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a specification attribute
    /// </summary>
    public class SpecificationAttribute : FullAuditedEntity<long, User>
    {
        private ICollection<SpecificationAttributeOption> _specificationAttributeOptions;

        /// <summary>
        ///     Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///     Gets or sets the specification attribute options
        /// </summary>
        public virtual ICollection<SpecificationAttributeOption> SpecificationAttributeOptions
        {
            get
            {
                return _specificationAttributeOptions ??
                       (_specificationAttributeOptions = new List<SpecificationAttributeOption>());
            }
            protected set { _specificationAttributeOptions = value; }
        }
    }
}