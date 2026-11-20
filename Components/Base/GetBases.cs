using Grasshopper.Kernel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AirTableConnect
{
    // Lists the bases the connected token can access
    public class GetBasesComponent : GH_Component
    {
        public GetBasesComponent()
          : base("List Bases", "Bases",
              "Lists the Airtable bases the connected token can access.",
              PluginUtilities.TabName, PluginUtilities.CategoryABBase)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Base Names", "BN", "Accessible base names.", GH_ParamAccess.list);
            pManager.AddTextParameter("Base IDs", "BI", "Accessible base IDs.", GH_ParamAccess.list);
            pManager.AddTextParameter("Status", "S", "\"OK\" if the call succeeded, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = "";
            DA.GetData(0, ref token);

            var names = new List<string>();
            var ids = new List<string>();
            string status;
            string response = "";

            if (string.IsNullOrWhiteSpace(token))
            {
                status = "Token is empty.";
            }
            else
            {
                var client = new AirTableConnectClient(token);
                var result = client.GetBasesAsync().GetAwaiter().GetResult();

                if (result.Item1)
                {
                    response = result.Item2;
                    var bases = (JArray)JObject.Parse(response)["bases"];
                    foreach (var b in bases)
                    {
                        names.Add((string)b["name"]);
                        ids.Add((string)b["id"]);
                    }
                    status = "OK";
                }
                else
                {
                    status = result.Item3;
                }
            }

            DA.SetDataList(0, names);
            DA.SetDataList(1, ids);
            DA.SetData(2, status);
            DA.SetData(3, response);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_GetBases;
        public override Guid ComponentGuid => new Guid("b351cb3a-4787-4534-920e-6628c83f0e41");
    }
}
