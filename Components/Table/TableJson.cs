using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Assembles a table name, description, and field list into Table JSON
    public class TableJsonComponent : GH_Component
    {
        public TableJsonComponent()
          : base("Build Table JSON", "TableJSON",
              "Assembles a table name, description, and Field JSON list into Table JSON.",
              PluginUtilities.TabName, PluginUtilities.CategoryACTable)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Table Name", "TN", "Name of the table.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Description", "D", "Optional table description.", GH_ParamAccess.item, "");
            pManager.AddTextParameter("Field JSON", "FJ", "Field JSON list — first field becomes the primary field.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Table JSON", "TJ", "Assembled table JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string tableName = "";
            string description = "";
            var fieldJsonList = new List<string>();

            DA.GetData(0, ref tableName);
            DA.GetData(1, ref description);
            DA.GetDataList(2, fieldJsonList);

            string tableJson = TableBuilders.BuildTableJson(tableName, description, fieldJsonList);

            DA.SetData(0, tableJson);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_TableJson;
        public override Guid ComponentGuid => new Guid("c4c61276-d037-4ff6-8b8a-d11f4b0ddaf6");
    }
}
