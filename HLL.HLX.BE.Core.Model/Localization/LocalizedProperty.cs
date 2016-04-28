using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Localization
{
    /// <summary>
    /// Represents a localized property
    /// </summary>
    public partial class LocalizedProperty : FullAuditedEntity<int, User>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the locale key group
        /// </summary>
        public string LocaleKeyGroup { get; set; }

        /// <summary>
        /// Gets or sets the locale key
        /// </summary>
        public string LocaleKey { get; set; }

        /// <summary>
        /// Gets or sets the locale value
        /// </summary>
        public string LocaleValue { get; set; }
        
        /// <summary>
        /// Gets the language
        /// </summary>
        public virtual Language Language { get; set; }
    }
}