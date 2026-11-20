using Grasshopper.Kernel;
using System;

namespace AirTableConnect
{
    // Uploads a base64 attachment to an existing record's attachment field
    public class UploadAttachmentComponent : ButtonComponent
    {
        public UploadAttachmentComponent()
            : base("Upload Picture", "Upload",
                "Uploads a base64-encoded attachment to an existing record.",
                PluginUtilities.TabName, PluginUtilities.CategoryAFAttachment)
        { }

        public override string ButtonLabel => "Upload";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Base ID", "BI", "Base containing the record.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Record ID", "RID", "Record to attach the file to.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Field", "F", "Attachment field name or ID.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Attachment Upload JSON", "AU", "Assembled attachment upload JSON.", GH_ParamAccess.item, "");
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
            string recordId = "";
            string field = "";
            string attachmentUploadJson = "";

            DA.GetData(0, ref token);
            DA.GetData(1, ref baseId);
            DA.GetData(2, ref recordId);
            DA.GetData(3, ref field);
            DA.GetData(4, ref attachmentUploadJson);

            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.UploadAttachmentAsync(baseId, recordId, field, attachmentUploadJson).GetAwaiter().GetResult();

                status = result.Item1 ? "OK" : result.Item3;
                response = result.Item1 ? result.Item2 : "";
            }

            DA.SetData(0, status);
            DA.SetData(1, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_UploadAttachment;
        public override Guid ComponentGuid => new Guid("08347027-c4d2-4ae3-ab7f-829719357449");
    }
}
