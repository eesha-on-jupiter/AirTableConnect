using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirTableConnect
{
    public static class TableBuilders
    {
        #region Table

        // table
        public static string BuildTableJson(string name, string description, List<string> fieldJsonList)
        {
            var fields = new JArray();
            if (fieldJsonList != null)
                foreach (var json in fieldJsonList)
                    if (!string.IsNullOrWhiteSpace(json))
                        fields.Add(JObject.Parse(json));

            var table = new JObject
            {
                ["name"] = name ?? "",
                ["fields"] = fields
            };
            if (!string.IsNullOrWhiteSpace(description))
                table["description"] = description;

            return table.ToString(Formatting.None);
        }

        #endregion

        #region Parse helpers

        public static JObject Parse(string json)
            => string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);

        #endregion
    }
}
