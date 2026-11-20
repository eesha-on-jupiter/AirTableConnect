using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Assembles select-field choices and their colors into Options JSON
    public class SelectFieldOptionsComponent : GH_Component
    {
        public SelectFieldOptionsComponent()
          : base("Build Select Choices", "Choices",
              "Assembles choice labels and colors into Options JSON for a select field.",
              PluginUtilities.TabName, PluginUtilities.CategoryADField)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Choice Labels", "CL", "Select choice labels.", GH_ParamAccess.list);
            pManager.AddTextParameter("Colors", "C", "Airtable color name per choice, positional.", GH_ParamAccess.list);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Options JSON", "OJ", "Assembled select-field options JSON.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var choiceLabels = new List<string>();
            var colors = new List<string>();

            DA.GetDataList(0, choiceLabels);
            DA.GetDataList(1, colors);

            string optionsJson = FieldBuilders.BuildSelectOptionsJson(choiceLabels, colors);

            DA.SetData(0, optionsJson);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_SelectFieldOptions;
        public override Guid ComponentGuid => new Guid("efd5de95-e962-4763-a7c9-88c13d59cf4c");
    }
}
