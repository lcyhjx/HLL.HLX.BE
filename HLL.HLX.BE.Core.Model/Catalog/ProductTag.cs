using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Core.Model.Users;

namespace HLL.HLX.BE.Core.Model.Catalog
{
    /// <summary>
    ///     Represents a product tag
    /// </summary>
    public class ProductTag : FullAuditedEntity<int, User>
    {
        private ICollection<Product> _products;

        /// <summary>
        ///     Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the products
        /// </summary>
        public virtual ICollection<Product> Products
        {
            get { return _products ?? (_products = new List<Product>()); }
            protected set { _products = value; }
        }
    }
}