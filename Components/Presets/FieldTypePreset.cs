using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AirTableConnect.Presets
{
    // Curated subset of Airtable's writable field types (formula/rollup/lookup/linkedRecord excluded for v1)
    public class FieldTypePresetComponent : GH_ValueList
    {
        public FieldTypePresetComponent()
        {
            Category = PluginUtilities.TabName;
            SubCategory = PluginUtilities.CategoryZYPresets;
            Name = "Field Type";
            NickName = "Type";
            Description = "Curated set of Airtable's writable field types.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "Single line text", "singleLineText" },
            { "Long text",        "multilineText" },
            { "Number",           "number" },
            { "Checkbox",         "checkbox" },
            { "Single select",    "singleSelect" },
            { "Multiple select",  "multipleSelects" },
            { "Date",             "date" },
            { "Attachment",       "attachment" },
            { "URL",              "url" },
            { "Email",            "email" },
            { "Phone number",     "phoneNumber" },
            { "Currency",         "currency" },
            { "Percent",          "percent" },
            { "Rating",           "rating" },
        };

        protected override Bitmap Icon => Properties.Resources.ATC_FieldTypePreset;
        public override Guid ComponentGuid => new Guid("3f295136-c4a0-45f3-bb4c-5a408f8b6989");
    }
}
