using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirTableConnect
{
    public sealed class AirTableConnectClient
    {
        #region Constructor and setup

        private static readonly HttpMethod Patch = new HttpMethod("PATCH");

        private readonly HttpClient _http;
        private readonly HttpClient _contentHttp;
        private readonly string _token;

        public AirTableConnectClient(string token)
        {
            _token = (token ?? "").Trim();
            _http = new HttpClient { BaseAddress = new Uri("https://api.airtable.com") };
            // uploadAttachment is served from a different host than every other endpoint
            _contentHttp = new HttpClient { BaseAddress = new Uri("https://content.airtable.com") };
        }

        private HttpRequestMessage NewRequest(HttpMethod method, string relativeUrl, bool useContentHost = false)
        {
            var req = new HttpRequestMessage(method, relativeUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            return req;
        }

        private async Task<Tuple<bool, string, string>> SendAsync(HttpRequestMessage req, bool useContentHost = false)
        {
            var http = useContentHost ? _contentHttp : _http;
            var res = await http.SendAsync(req).ConfigureAwait(false);
            string body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!res.IsSuccessStatusCode)
                return Fail("HTTP " + (int)res.StatusCode + " " + res.ReasonPhrase + "\n" + body);

            return Ok(body);
        }

        #endregion

        #region Auth methods

        public async Task<Tuple<bool, string, string>> WhoAmiAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_token)) return Fail("Token is empty.");

                var req = NewRequest(HttpMethod.Get, "/v0/meta/whoami");
                return await SendAsync(req).ConfigureAwait(false);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Base methods

        public async Task<Tuple<bool, string, string>> GetBasesAsync()
        {
            try
            {
                var req = NewRequest(HttpMethod.Get, "/v0/meta/bases");
                return await SendAsync(req).ConfigureAwait(false);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        public async Task<Tuple<bool, string, string>> GetBaseSchemaAsync(string baseId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");

                var req = NewRequest(HttpMethod.Get, $"/v0/meta/bases/{baseId}/tables");
                return await SendAsync(req).ConfigureAwait(false);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Table methods

        public async Task<Tuple<bool, string, string>> CreateTableAsync(string baseId, string tableJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");
                if (string.IsNullOrWhiteSpace(tableJson)) return Fail("Table JSON is empty.");

                var req = NewRequest(HttpMethod.Post, $"/v0/meta/bases/{baseId}/tables");
                req.Content = new StringContent(tableJson, Encoding.UTF8, "application/json");
                return await SendAsync(req).ConfigureAwait(false);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Field methods

        public async Task<Tuple<bool, string, string>> CreateFieldAsync(string baseId, string tableId, string fieldJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");
                if (string.IsNullOrWhiteSpace(tableId)) return Fail("Table ID is empty.");
                if (string.IsNullOrWhiteSpace(fieldJson)) return Fail("Field JSON is empty.");

                var req = NewRequest(HttpMethod.Post, $"/v0/meta/bases/{baseId}/tables/{tableId}/fields");
                req.Content = new StringContent(fieldJson, Encoding.UTF8, "application/json");
                return await SendAsync(req).ConfigureAwait(false);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        public async Task<Tuple<bool, string, string>> UpdateFieldAsync(string baseId, string tableId, string fieldId, string fieldJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");
                if (string.IsNullOrWhiteSpace(tableId)) return Fail("Table ID is empty.");
                if (string.IsNullOrWhiteSpace(fieldId)) return Fail("Field ID is empty.");
                if (string.IsNullOrWhiteSpace(fieldJson)) return Fail("Field JSON is empty.");

                var req = NewRequest(Patch, $"/v0/meta/bases/{baseId}/tables/{tableId}/fields/{fieldId}");
                req.Content = new StringContent(fieldJson, Encoding.UTF8, "application/json");
                return await SendAsync(req).ConfigureAwait(false);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Record methods

        // Auto-loops the offset pagination cursor until exhausted or maxRecords is reached (Batch Processor)
        public async Task<Tuple<bool, string, string>> ListRecordsAsync(string baseId, string table, string filterFormula, string view, int maxRecords)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");
                if (string.IsNullOrWhiteSpace(table)) return Fail("Table is empty.");

                var allRecords = new JArray();
                string offset = null;

                do
                {
                    var query = new List<string>();
                    if (!string.IsNullOrWhiteSpace(filterFormula)) query.Add("filterByFormula=" + Uri.EscapeDataString(filterFormula));
                    if (!string.IsNullOrWhiteSpace(view)) query.Add("view=" + Uri.EscapeDataString(view));
                    query.Add("pageSize=100");
                    if (!string.IsNullOrWhiteSpace(offset)) query.Add("offset=" + Uri.EscapeDataString(offset));

                    string url = $"/v0/{baseId}/{Uri.EscapeDataString(table)}?" + string.Join("&", query);
                    var req = NewRequest(HttpMethod.Get, url);
                    var result = await SendAsync(req).ConfigureAwait(false);
                    if (!result.Item1) return result;

                    var page = JObject.Parse(result.Item2);
                    foreach (var rec in (JArray)page["records"])
                    {
                        allRecords.Add(rec);
                        if (maxRecords > 0 && allRecords.Count >= maxRecords) break;
                    }

                    offset = (string)page["offset"];
                    if (maxRecords > 0 && allRecords.Count >= maxRecords) break;
                    if (offset != null) await Task.Delay(350).ConfigureAwait(false);
                }
                while (offset != null);

                var combined = new JObject { ["records"] = allRecords };
                return Ok(combined.ToString(Formatting.None));
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // Auto-chunks the incoming record list into groups of 10 — Airtable's per-call cap (Batch Processor)
        public async Task<Tuple<bool, string, string>> CreateRecordsAsync(string baseId, string table, List<string> recordJsonList, bool typecast)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");
                if (string.IsNullOrWhiteSpace(table)) return Fail("Table is empty.");
                if (recordJsonList == null || recordJsonList.Count == 0) return Fail("Record JSON is empty.");

                var allRecords = new JArray();

                for (int i = 0; i < recordJsonList.Count; i += 10)
                {
                    var chunk = recordJsonList.Skip(i).Take(10);
                    var records = new JArray();
                    foreach (var json in chunk) records.Add(JObject.Parse(json));

                    var body = new JObject { ["records"] = records, ["typecast"] = typecast };
                    var req = NewRequest(HttpMethod.Post, $"/v0/{baseId}/{Uri.EscapeDataString(table)}");
                    req.Content = new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json");

                    var result = await SendAsync(req).ConfigureAwait(false);
                    if (!result.Item1) return result;

                    var parsed = JObject.Parse(result.Item2);
                    foreach (var rec in (JArray)parsed["records"]) allRecords.Add(rec);

                    if (i + 10 < recordJsonList.Count) await Task.Delay(350).ConfigureAwait(false);
                }

                var combined = new JObject { ["records"] = allRecords };
                return Ok(combined.ToString(Formatting.None));
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        // Auto-chunks the incoming record list into groups of 10 — Airtable's per-call cap (Batch Processor)
        public async Task<Tuple<bool, string, string>> UpdateRecordsAsync(string baseId, string table, List<string> recordJsonList, bool upsert, List<string> fieldsToMergeOn, bool typecast)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");
                if (string.IsNullOrWhiteSpace(table)) return Fail("Table is empty.");
                if (recordJsonList == null || recordJsonList.Count == 0) return Fail("Record JSON is empty.");
                if (upsert && (fieldsToMergeOn == null || fieldsToMergeOn.Count == 0)) return Fail("Fields To Merge On is required when Upsert is true.");

                var allRecords = new JArray();
                var createdRecords = new JArray();
                var updatedRecords = new JArray();

                for (int i = 0; i < recordJsonList.Count; i += 10)
                {
                    var chunk = recordJsonList.Skip(i).Take(10);
                    var records = new JArray();
                    foreach (var json in chunk) records.Add(JObject.Parse(json));

                    var body = new JObject { ["records"] = records, ["typecast"] = typecast };
                    if (upsert)
                        body["performUpsert"] = new JObject { ["fieldsToMergeOn"] = new JArray(fieldsToMergeOn) };

                    var req = NewRequest(Patch, $"/v0/{baseId}/{Uri.EscapeDataString(table)}");
                    req.Content = new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json");

                    var result = await SendAsync(req).ConfigureAwait(false);
                    if (!result.Item1) return result;

                    var parsed = JObject.Parse(result.Item2);
                    foreach (var rec in (JArray)parsed["records"]) allRecords.Add(rec);
                    if (parsed["createdRecords"] != null) foreach (var id in (JArray)parsed["createdRecords"]) createdRecords.Add(id);
                    if (parsed["updatedRecords"] != null) foreach (var id in (JArray)parsed["updatedRecords"]) updatedRecords.Add(id);

                    if (i + 10 < recordJsonList.Count) await Task.Delay(350).ConfigureAwait(false);
                }

                var combined = new JObject { ["records"] = allRecords, ["createdRecords"] = createdRecords, ["updatedRecords"] = updatedRecords };
                return Ok(combined.ToString(Formatting.None));
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Attachment methods

        public async Task<Tuple<bool, string, string>> UploadAttachmentAsync(string baseId, string recordId, string field, string attachmentUploadJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseId)) return Fail("Base ID is empty.");
                if (string.IsNullOrWhiteSpace(recordId)) return Fail("Record ID is empty.");
                if (string.IsNullOrWhiteSpace(field)) return Fail("Field is empty.");
                if (string.IsNullOrWhiteSpace(attachmentUploadJson)) return Fail("Attachment Upload JSON is empty.");

                var req = NewRequest(HttpMethod.Post, $"/v0/{baseId}/{recordId}/{Uri.EscapeDataString(field)}/uploadAttachment", useContentHost: true);
                req.Content = new StringContent(attachmentUploadJson, Encoding.UTF8, "application/json");
                return await SendAsync(req, useContentHost: true).ConfigureAwait(false);
            }
            catch (Exception ex) { return Fail(ex.ToString()); }
        }

        #endregion

        #region Private helpers

        private static Tuple<bool, string, string> Ok(string body)
            => new Tuple<bool, string, string>(true, body, "");

        private static Tuple<bool, string, string> Fail(string error)
            => new Tuple<bool, string, string>(false, "", error);

        #endregion
    }
}
