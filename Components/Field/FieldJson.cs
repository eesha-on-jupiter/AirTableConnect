using Grasshopper.Kernel;
using System;

namespace AirTableConnect
{
    // Assembles a single Airtable field definition into Field JSON
    public class FieldJsonComponent : GH_Component
    {
        public FieldJsonComponent()
          : base("Build Field JSON", "FieldJSON",
              "Assembles a field name, type, description, and options into Field JSON.",
              PluginUtilities.TabName, PluginUtilities.CategoryADField)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Field Name", "FN", "Name of the field.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Field Type", "FT", "Airtable field type.", GH_ParamAccess.item, "singleLineText");
            pManager.AddTextParameter("Description", "D", "Optional field description.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Options JSON", "OJ", "Optional per-type options JSON (e.g. select choices).", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Field JSON", "FJ", "Assembled field JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string fieldName = "";
            string fieldType = "";
            string description = "";
            string optionsJson = "";

            DA.GetData(0, ref fieldName);
            DA.GetData(1, ref fieldType);
            DA.GetData(2, ref description);
            DA.GetData(3, ref optionsJson);

            string fieldJson = FieldBuilders.BuildFieldJson(fieldName, fieldType, description, optionsJson);

            DA.SetData(0, fieldJson);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_FieldJson;
        public override Guid ComponentGuid => new Guid("3d6f6751-355e-4f00-ae1b-e5b94eebbb7b");
    }
}
