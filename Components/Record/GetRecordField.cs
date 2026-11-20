using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Extracts one field's value from a list of Record JSON — pure JSON extraction, no HTTP call
    public class GetRecordFieldComponent : GH_Component
    {
        public GetRecordFieldComponent()
          : base("Get Field From Records", "GetField",
              "Extracts one field's value from each record in a Record JSON list.",
              PluginUtilities.TabName, PluginUtilities.CategoryAERecord)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Record JSON", "RJ", "Record JSON list.", GH_ParamAccess.list);
            pManager.AddTextParameter("Field Name", "FN", "Field to extract.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Field Value", "FV", "Extracted field value, positional per record.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var recordJsonList = new List<string>();
            string fieldName = "";

            DA.GetDataList(0, recordJsonList);
            DA.GetData(1, ref fieldName);

            var values = new List<string>();
            foreach (var json in recordJsonList)
            {
                if (string.IsNullOrWhiteSpace(json)) { values.Add(""); continue; }

                var record = JObject.Parse(json);
                var fields = record["fields"] as JObject;
                var token = fields?[fieldName ?? ""];
                values.Add(token == null ? "" : token.Type == JTokenType.String ? token.Value<string>() : token.ToString());
            }

            DA.SetDataList(0, values);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_GetRecordField;
        public override Guid ComponentGuid => new Guid("890eb08a-57e5-4e08-9a66-86c4f4eb6d86");
    }
}
