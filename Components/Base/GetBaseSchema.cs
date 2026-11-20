using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Lists the tables in a base
    public class GetBaseSchemaComponent : GH_Component
    {
        public GetBaseSchemaComponent()
          : base("Get Base Schema", "Schema",
              "Lists the tables defined in an Airtable base.",
              PluginUtilities.TabName, PluginUtilities.CategoryABBase)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Base ID", "BI", "Base to inspect.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Table Names", "TN", "Table names in the base.", GH_ParamAccess.list);
            pManager.AddTextParameter("Table IDs", "TI", "Table IDs in the base.", GH_ParamAccess.list);
            pManager.AddTextParameter("Status", "S", "\"OK\" if the call succeeded, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = "";
            string baseId = "";
            DA.GetData(0, ref token);
            DA.GetData(1, ref baseId);

            var names = new List<string>();
            var ids = new List<string>();
            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else if (string.IsNullOrWhiteSpace(baseId))
            {
                status = "Base ID is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.GetBaseSchemaAsync(baseId).GetAwaiter().GetResult();

                if (result.Item1)
                {
                    response = result.Item2;
                    var tables = (JArray)JObject.Parse(response)["tables"];
                    foreach (var t in tables)
                    {
                        names.Add((string)t["name"]);
                        ids.Add((string)t["id"]);
                    }
                    status = "OK";
                }
                else
                {
                    status = result.Item3;
                }
            }

            DA.SetDataList(0, names);
            DA.SetDataList(1, ids);
            DA.SetData(2, status);
            DA.SetData(3, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_GetBaseSchema;
        public override Guid ComponentGuid => new Guid("5d358c61-a2b2-46b8-9dee-1346ea5ebf46");
    }
}
