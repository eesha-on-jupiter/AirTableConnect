using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirTableConnect
{
    public static class FieldBuilders
    {
        #region Field

        // field
        public static string BuildFieldJson(string name, string type, string description, string optionsJson)
        {
            var field = new JObject
            {
                ["name"] = name ?? "",
                ["type"] = type ?? ""
            };
            if (!string.IsNullOrWhiteSpace(description))
                field["description"] = description;
            if (!string.IsNullOrWhiteSpace(optionsJson))
                field["options"] = JObject.Parse(optionsJson);

            return field.ToString(Formatting.None);
        }

        // select options
        public static string BuildSelectOptionsJson(List<string> choiceLabels, List<string> colors)
        {
            var choices = new JArray();
            if (choiceLabels != null)
            {
                for (int i = 0; i < choiceLabels.Count; i++)
                {
                    var choice = new JObject { ["name"] = choiceLabels[i] ?? "" };
                    if (colors != null && i < colors.Count && !string.IsNullOrWhiteSpace(colors[i]))
                        choice["color"] = colors[i];
                    choices.Add(choice);
                }
            }

            var options = new JObject { ["choices"] = choices };
            return options.ToString(Formatting.None);
        }

        #endregion

        #region Parse helpers

        public static JObject Parse(string json)
            => string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);

        #endregion
    }
}
