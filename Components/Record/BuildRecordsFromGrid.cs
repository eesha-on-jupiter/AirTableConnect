using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Assembles a Field Names list and a Values tree (one branch per record) into a Record JSON
    // list — handles one record (a single branch) or many with the same logic
    public class BuildRecordsFromGridComponent : GH_Component
    {
        public BuildRecordsFromGridComponent()
          : base("Build Records From Table", "RecordGrid",
              "Assembles field names and a per-record values tree into a Record JSON list.",
              PluginUtilities.TabName, PluginUtilities.CategoryAERecord)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Field Names", "FN", "Field names, in the order values are supplied per record.", GH_ParamAccess.list);
            pManager.AddTextParameter("Values", "V", "Per-record field values — one branch per record.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Record IDs", "RID", "Existing record IDs, positional — omit for new records.", GH_ParamAccess.list);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Record JSON", "RJ", "Assembled record JSON, one entry per record.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var fieldNames = new List<string>();
            var recordIds = new List<string>();

            DA.GetDataList(0, fieldNames);
            GH_Structure<GH_String> tree;
            DA.GetDataTree(1, out tree);
            DA.GetDataList(2, recordIds);

            var recordValues = new List<IList<string>>();
            foreach (var branch in tree.Branches)
            {
                var values = new List<string>();
                foreach (var item in branch)
                    values.Add(item?.Value ?? "");
                recordValues.Add(values);
            }

            var result = RecordBuilders.AssembleRecords(fieldNames, recordValues, recordIds);
            var payloads = result.Item1;
            var warnings = result.Item2;

            for (int i = 0; i < warnings.Count; i++)
                if (!string.IsNullOrEmpty(warnings[i]))
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, warnings[i]);

            DA.SetDataList(0, payloads);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_BuildRecordsFromGrid;
        public override Guid ComponentGuid => new Guid("493ed103-2292-4be5-bb20-b1b0eab7c2d6");
    }
}
