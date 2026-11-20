using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AirTableConnect.Presets
{
    // Airtable's fixed set of select-field choice colors
    public class AirtableColorPresetComponent : GH_ValueList
    {
        public AirtableColorPresetComponent()
        {
            Category = PluginUtilities.TabName;
            SubCategory = PluginUtilities.CategoryZYPresets;
            Name = "Airtable Color";
            NickName = "Color";
            Description = "Airtable's fixed set of select-field choice colors.";

            ListItems.Clear();
            foreach (var kvp in Items)
                ListItems.Add(new GH_ValueListItem(kvp.Key, $"\"{kvp.Value}\""));
        }

        private static readonly Dictionary<string, string> Items = new Dictionary<string, string>
        {
            { "blueLight2",   "blueLight2" },
            { "cyanLight2",   "cyanLight2" },
            { "tealLight2",   "tealLight2" },
            { "greenLight2",  "greenLight2" },
            { "yellowLight2", "yellowLight2" },
            { "orangeLight2", "orangeLight2" },
            { "redLight2",    "redLight2" },
            { "pinkLight2",   "pinkLight2" },
            { "purpleLight2", "purpleLight2" },
            { "grayLight2",   "grayLight2" },
            { "blueLight1",   "blueLight1" },
            { "cyanLight1",   "cyanLight1" },
            { "tealLight1",   "tealLight1" },
            { "greenLight1",  "greenLight1" },
            { "yellowLight1", "yellowLight1" },
            { "orangeLight1", "orangeLight1" },
            { "redLight1",    "redLight1" },
            { "pinkLight1",   "pinkLight1" },
            { "purpleLight1", "purpleLight1" },
            { "grayLight1",   "grayLight1" },
            { "blueBright",   "blueBright" },
            { "cyanBright",   "cyanBright" },
            { "tealBright",   "tealBright" },
            { "greenBright",  "greenBright" },
            { "yellowBright", "yellowBright" },
            { "orangeBright", "orangeBright" },
            { "redBright",    "redBright" },
            { "pinkBright",   "pinkBright" },
            { "purpleBright", "purpleBright" },
            { "grayBright",   "grayBright" },
            { "blueDark1",    "blueDark1" },
            { "cyanDark1",    "cyanDark1" },
            { "tealDark1",    "tealDark1" },
            { "greenDark1",   "greenDark1" },
            { "yellowDark1",  "yellowDark1" },
            { "orangeDark1",  "orangeDark1" },
            { "redDark1",     "redDark1" },
            { "pinkDark1",    "pinkDark1" },
            { "purpleDark1",  "purpleDark1" },
            { "grayDark1",    "grayDark1" },
        };

        protected override Bitmap Icon => Properties.Resources.ATC_AirtableColorPreset;
        public override Guid ComponentGuid => new Guid("7eef265c-a773-41e4-962b-c91b57d85f54");
    }
}
