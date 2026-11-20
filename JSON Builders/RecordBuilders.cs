using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirTableConnect
{
    public static class RecordBuilders
    {
        #region Private helpers

        // A value that looks like a JSON array/object (e.g. an embedded Attachments JSON) is
        // embedded as structured JSON rather than as a quoted string
        private static JToken ParseFieldValue(string raw)
        {
            string trimmed = (raw ?? "").TrimStart();
            if (trimmed.StartsWith("[") || trimmed.StartsWith("{"))
            {
                try { return JToken.Parse(raw); }
                catch { return raw; }
            }
            return raw;
        }

        #endregion

        #region Record

        // records (tree -> list of full record objects; "id" included per record when provided)
        public static Tuple<List<string>, List<string>> AssembleRecords(
            IList<string> fieldNames, IList<IList<string>> recordValues, IList<string> recordIds)
        {
            var payloads = new List<string>();
            var warnings = new List<string>();
            int fieldCount = fieldNames?.Count ?? 0;
            int recordCount = recordValues?.Count ?? 0;

            for (int r = 0; r < recordCount; r++)
            {
                var branch = recordValues[r] ?? new List<string>();
                int n = Math.Min(fieldCount, branch.Count);
                warnings.Add(n < fieldCount || n < branch.Count
                    ? $"Record {r}: field count ({fieldCount}) and value count ({branch.Count}) differ, using the first {n}."
                    : "");

                var fields = new JObject();
                for (int i = 0; i < n; i++)
                {
                    if (string.IsNullOrWhiteSpace(branch[i])) continue;
                    fields[fieldNames[i] ?? ""] = ParseFieldValue(branch[i]);
                }

                var record = new JObject { ["fields"] = fields };
                if (recordIds != null && r < recordIds.Count && !string.IsNullOrWhiteSpace(recordIds[r]))
                    record["id"] = recordIds[r];

                payloads.Add(record.ToString(Formatting.None));
            }

            return new Tuple<List<string>, List<string>>(payloads, warnings);
        }

        #endregion

        #region Parse helpers

        public static JObject Parse(string json)
            => string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);

        #endregion
    }
}
