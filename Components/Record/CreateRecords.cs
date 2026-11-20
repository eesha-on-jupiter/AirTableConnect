using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Creates records from a Record JSON list, auto-chunking into groups of 10 internally
    public class CreateRecordsComponent : ButtonComponent
    {
        public CreateRecordsComponent()
            : base("Create Records", "CreateRecords",
                "Creates records from a Record JSON list, auto-chunking past 10 rows per call.",
                PluginUtilities.TabName, PluginUtilities.CategoryAERecord)
        { }

        public override string ButtonLabel => "Create Records";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Base ID", "BI", "Base containing the table.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Table", "TB", "Table name or ID.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Record JSON", "RJ", "Record JSON list to create.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Typecast", "TC", "Let Airtable loosely convert mismatched value types.", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Record IDs", "RID", "IDs of the created records.", GH_ParamAccess.list);
            pManager.AddTextParameter("Status", "S", "\"OK\" if the call succeeded, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw combined JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = "";
            string baseId = "";
            string table = "";
            var recordJsonList = new List<string>();
            bool typecast = false;

            DA.GetData(0, ref token);
            DA.GetData(1, ref baseId);
            DA.GetData(2, ref table);
            DA.GetDataList(3, recordJsonList);
            DA.GetData(4, ref typecast);

            var recordIds = new List<string>();
            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.CreateRecordsAsync(baseId, table, recordJsonList, typecast).GetAwaiter().GetResult();

                if (result.Item1)
                {
                    response = result.Item2;
                    foreach (var rec in (JArray)JObject.Parse(response)["records"])
                        recordIds.Add((string)rec["id"]);
                    status = "OK";
                }
                else
                {
                    status = result.Item3;
                }
            }

            DA.SetDataList(0, recordIds);
            DA.SetData(1, status);
            DA.SetData(2, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_CreateRecords;
        public override Guid ComponentGuid => new Guid("0436d7df-780a-4045-a42b-8a016fb7c871");
    }
}
