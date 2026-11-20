using Grasshopper.Kernel;
using System;

namespace AirTableConnect
{
    // Assembles a URL-sourced attachment into Attachments JSON, ready to embed in a record
    public class AttachmentFromUrlComponent : GH_Component
    {
        public AttachmentFromUrlComponent()
          : base("Attachment From URL", "FromURL",
              "Assembles a URL and filename into embeddable Attachments JSON.",
              PluginUtilities.TabName, PluginUtilities.CategoryAFAttachment)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("URL", "U", "Publicly reachable URL of the file.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Filename", "FN", "Optional filename.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Attachments JSON", "AS", "Assembled attachments JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string url = "";
            string filename = "";

            DA.GetData(0, ref url);
            DA.GetData(1, ref filename);

            string attachmentsJson = AttachmentBuilders.BuildAttachmentsJson(url, filename);

            DA.SetData(0, attachmentsJson);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_AttachmentFromUrl;
        public override Guid ComponentGuid => new Guid("470d241c-7af4-4a31-a6c4-c04cb891135b");
    }
}
