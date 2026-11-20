using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AirTableConnect
{
    // Base64-encodes a file or an in-memory bitmap into Attachment Upload JSON — Bitmap wins if both are given
    public class AttachmentFromFileOrBitmapComponent : GH_Component
    {
        public AttachmentFromFileOrBitmapComponent()
          : base("Attachment From File Or Bitmap", "FromFileOrBitmap",
              "Base64-encodes a file path or bitmap into Attachment Upload JSON for an existing record.",
              PluginUtilities.TabName, PluginUtilities.CategoryAFAttachment)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "FP", "Path to the file on disk.", GH_ParamAccess.item, "");
            pManager.AddGenericParameter("Bitmap", "B", "In-memory bitmap — wins over File Path if both are given.", GH_ParamAccess.item);
            pManager.AddTextParameter("Filename", "FN", "Optional filename.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Content Type", "CT", "Optional MIME type.", GH_ParamAccess.item, "");
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Attachment Upload JSON", "AU", "Assembled attachment upload JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = "";
            Bitmap bitmap = null;
            string filename = "";
            string contentType = "";

            DA.GetData(0, ref filePath);
            DA.GetData(1, ref bitmap);
            DA.GetData(2, ref filename);
            DA.GetData(3, ref contentType);

            if (bitmap == null && string.IsNullOrWhiteSpace(filePath))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Provide either File Path or Bitmap.");
                return;
            }

            byte[] bytes;
            string resolvedContentType = contentType;
            string resolvedFilename = filename;

            if (bitmap != null)
            {
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    bytes = ms.ToArray();
                }
                if (string.IsNullOrWhiteSpace(resolvedContentType)) resolvedContentType = "image/png";
                if (string.IsNullOrWhiteSpace(resolvedFilename)) resolvedFilename = "image.png";
            }
            else
            {
                bytes = File.ReadAllBytes(filePath);
                if (string.IsNullOrWhiteSpace(resolvedContentType)) resolvedContentType = "application/octet-stream";
                if (string.IsNullOrWhiteSpace(resolvedFilename)) resolvedFilename = Path.GetFileName(filePath);
            }

            string base64 = Convert.ToBase64String(bytes);
            string attachmentUploadJson = AttachmentBuilders.BuildAttachmentUploadJson(base64, resolvedContentType, resolvedFilename);

            DA.SetData(0, attachmentUploadJson);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_AttachmentFromFileOrBitmap;
        public override Guid ComponentGuid => new Guid("5217b5e1-91ab-4599-ac3b-d4c3bd615fce");
    }
}
