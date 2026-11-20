using Grasshopper.Kernel;
using System;

namespace AirTableConnect
{
    // Updates an existing field's name, description, or options
    public class UpdateFieldComponent : ButtonComponent
    {
        public UpdateFieldComponent()
            : base("Update Field", "UpdateField",
                "Updates an existing field's name, description, or options from Field JSON.",
                PluginUtilities.TabName, PluginUtilities.CategoryADField)
        { }

        public override string ButtonLabel => "Update Field";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Base ID", "BI", "Base containing the table.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Table ID", "TI", "Table containing the field.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Field ID", "FID", "Field to update.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Field JSON", "FJ", "Assembled field JSON.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "S", "\"OK\" if the call succeeded, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsTriggered) return;

            string token = "";
            string baseId = "";
            string tableId = "";
            string fieldId = "";
            string fieldJson = "";

            DA.GetData(0, ref token);
            DA.GetData(1, ref baseId);
            DA.GetData(2, ref tableId);
            DA.GetData(3, ref fieldId);
            DA.GetData(4, ref fieldJson);

            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.UpdateFieldAsync(baseId, tableId, fieldId, fieldJson).GetAwaiter().GetResult();

                status = result.Item1 ? "OK" : result.Item3;
                response = result.Item1 ? result.Item2 : "";
            }

            DA.SetData(0, status);
            DA.SetData(1, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_UpdateField;
        public override Guid ComponentGuid => new Guid("753a603e-ffb1-4b28-9b09-91c82ed6b2b1");
    }
}
