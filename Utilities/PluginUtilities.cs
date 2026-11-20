using System;
using System.Collections.Generic;
using System.Linq;

namespace AirTableConnect
{
    public static class PluginUtilities
    {
        // Shared enum helper — used by any preset component backed by an enum
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        internal static readonly string TabName = "AirTableConnect";

        // Subcategory constants — one per subcategory, in display order.
        // Each JSON-builder component lives in the same subcategory as the live call it feeds;
        // the icon shape (cut-square vs rounded-square) is what distinguishes them on the canvas.
        internal static readonly string CategoryAAAuth = "1. Auth";
        internal static readonly string CategoryABBase = "2. Base";
        internal static readonly string CategoryACTable = "3. Table";
        internal static readonly string CategoryADField = "4. Field";
        internal static readonly string CategoryAERecord = "5. Record";
        internal static readonly string CategoryAFAttachment = "6. Attachment";
        internal static readonly string CategoryZYPresets = "7. Presets";
    }
}
