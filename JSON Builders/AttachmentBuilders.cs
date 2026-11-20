using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirTableConnect
{
    public static class AttachmentBuilders
    {
        #region Attachment

        // attachments (embedded, url-sourced)
        public static string BuildAttachmentsJson(string url, string filename)
        {
            var attachment = new JObject { ["url"] = url ?? "" };
            if (!string.IsNullOrWhiteSpace(filename))
                attachment["filename"] = filename;

            var array = new JArray { attachment };
            return array.ToString(Formatting.None);
        }

        // attachment upload (base64, existing record)
        public static string BuildAttachmentUploadJson(string base64, string contentType, string filename)
        {
            var upload = new JObject
            {
                ["contentType"] = contentType ?? "",
                ["file"] = base64 ?? "",
                ["filename"] = filename ?? ""
            };
            return upload.ToString(Formatting.None);
        }

        #endregion

        #region Parse helpers

        public static JObject Parse(string json)
            => string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);

        public static JArray ParseArray(string json)
            => string.IsNullOrWhiteSpace(json) ? null : JArray.Parse(json);

        #endregion
    }
}
