using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;
using System;

namespace AirTableConnect
{
    // Creates a new table from Table JSON
    public class CreateTableComponent : ButtonComponent
    {
        public CreateTableComponent()
            : base("Create Table Layout", "CreateTable",
                "Creates a new table from Table JSON.",
                PluginUtilities.TabName, PluginUtilities.CategoryACTable)
        { }

        public override string ButtonLabel => "Create Table";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Base ID", "BI", "Base to create the table in.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Table JSON", "TJ", "Assembled table JSON.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Table ID", "TI", "ID of the created table.", GH_ParamAccess.item);
            pManager.AddTextParameter("Status", "S", "\"OK\" if the call succeeded, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = "";
            string baseId = "";
            string tableJson = "";

            DA.GetData(0, ref token);
            DA.GetData(1, ref baseId);
            DA.GetData(2, ref tableJson);

            string tableId = "";
            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.CreateTableAsync(baseId, tableJson).GetAwaiter().GetResult();

                if (result.Item1)
                {
                    response = result.Item2;
                    tableId = (string)JObject.Parse(response)["id"];
                    status = "OK";
                }
                else
                {
                    status = result.Item3;
                }
            }

            DA.SetData(0, tableId);
            DA.SetData(1, status);
            DA.SetData(2, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_CreateTable;
        public override Guid ComponentGuid => new Guid("3f6c42bd-085e-4ad0-8422-60306e1e45c9");
    }
}
