using Grasshopper.Kernel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Lists records from a table, auto-looping pagination internally until Max Records is reached
    public class ListRecordsComponent : GH_Component
    {
        public ListRecordsComponent()
          : base("List Records", "Records",
              "Lists records from a table, optionally filtered, auto-paginating past 100 rows.",
              PluginUtilities.TabName, PluginUtilities.CategoryAERecord)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Base ID", "BI", "Base containing the table.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Table", "TB", "Table name or ID.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Filter Formula", "FF", "Optional Airtable formula filter.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("View", "V", "Optional view name or ID.", GH_ParamAccess.item, "");
            pManager.AddIntegerParameter("Max Records", "MR", "Maximum records to return — 0 for no limit.", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Record IDs", "RID", "IDs of the returned records.", GH_ParamAccess.list);
            pManager.AddTextParameter("Record JSON", "RJ", "Full JSON per returned record.", GH_ParamAccess.list);
            pManager.AddTextParameter("Status", "S", "\"OK\" if the call succeeded, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw combined JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = "";
            string baseId = "";
            string table = "";
            string filterFormula = "";
            string view = "";
            int maxRecords = 0;

            DA.GetData(0, ref token);
            DA.GetData(1, ref baseId);
            DA.GetData(2, ref table);
            DA.GetData(3, ref filterFormula);
            DA.GetData(4, ref view);
            DA.GetData(5, ref maxRecords);

            var recordIds = new List<string>();
            var recordJson = new List<string>();
            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.ListRecordsAsync(baseId, table, filterFormula, view, maxRecords).GetAwaiter().GetResult();

                if (result.Item1)
                {
                    response = result.Item2;
                    var records = (JArray)JObject.Parse(response)["records"];
                    foreach (var rec in records)
                    {
                        recordIds.Add((string)rec["id"]);
                        recordJson.Add(rec.ToString(Formatting.None));
                    }
                    status = "OK";
                }
                else
                {
                    status = result.Item3;
                }
            }

            DA.SetDataList(0, recordIds);
            DA.SetDataList(1, recordJson);
            DA.SetData(2, status);
            DA.SetData(3, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_ListRecords;
        public override Guid ComponentGuid => new Guid("fd8d5815-3997-4204-af0c-1744cc1d4a46");
    }
}
