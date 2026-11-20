using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;
using System;

namespace AirTableConnect
{
    // Creates a field on an existing table
    public class CreateFieldComponent : ButtonComponent
    {
        public CreateFieldComponent()
            : base("Create Field", "CreateField",
                "Creates a field on an existing table from Field JSON.",
                PluginUtilities.TabName, PluginUtilities.CategoryADField)
        { }

        public override string ButtonLabel => "Create Field";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Base ID", "BI", "Base containing the table.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Table ID", "TI", "Table to add the field to.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Field JSON", "FJ", "Assembled field JSON.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Field ID", "FID", "ID of the created field.", GH_ParamAccess.item);
            pManager.AddTextParameter("Status", "S", "\"OK\" if the call succeeded, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = "";
            string baseId = "";
            string tableId = "";
            string fieldJson = "";

            DA.GetData(0, ref token);
            DA.GetData(1, ref baseId);
            DA.GetData(2, ref tableId);
            DA.GetData(3, ref fieldJson);

            string fieldId = "";
            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.CreateFieldAsync(baseId, tableId, fieldJson).GetAwaiter().GetResult();

                if (result.Item1)
                {
                    response = result.Item2;
                    fieldId = (string)JObject.Parse(response)["id"];
                    status = "OK";
                }
                else
                {
                    status = result.Item3;
                }
            }

            DA.SetData(0, fieldId);
            DA.SetData(1, status);
            DA.SetData(2, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_CreateField;
        public override Guid ComponentGuid => new Guid("3f115ada-1fd1-4f38-9517-cbcb01362178");
    }
}
