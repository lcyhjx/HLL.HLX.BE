using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.Core.Business.Stores
{
    /// <summary>
    /// Store context
    /// </summary>
    public interface IStoreContext
    {
        /// <summary>
        /// Gets or sets the current store
        /// </summary>
        Store CurrentStore { get; }
    }
}
