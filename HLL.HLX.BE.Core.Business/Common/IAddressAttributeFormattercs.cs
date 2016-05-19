using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL.HLX.BE.Core.Business.Common
{
    /// <summary>
    /// Checkout attribute helper
    /// </summary>
    public partial interface IAddressAttributeFormatter
    {
        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="serapator">Serapator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <returns>Attributes</returns>
        string FormatAttributes(string attributesXml,
            string serapator = "<br />",
            bool htmlEncode = true);
    }
}
