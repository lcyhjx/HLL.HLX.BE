using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using HLL.HLX.BE.Common.Html;
using HLL.HLX.BE.Core.Model.Vendors;

namespace HLL.HLX.BE.Core.Business.Vendors
{
    public static class VendorExtensions
    {
        /// <summary>
        /// Formats the vendor note text
        /// </summary>
        /// <param name="vendorNote">Vendor note</param>
        /// <returns>Formatted text</returns>
        public static string FormatVendorNoteText(this VendorNote vendorNote)
        {
            if (vendorNote == null)
                throw new UserFriendlyException("vendorNote不存在");

            string text = vendorNote.Note;

            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, false, false, false);

            return text;
        }
    }
}
