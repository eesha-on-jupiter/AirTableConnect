using Grasshopper.Kernel;
using System;

namespace AirTableConnect
{
    // Verifies a Personal Access Token against Airtable's whoami endpoint
    public class AirTableConnectAuthComponent : GH_Component
    {
        public AirTableConnectAuthComponent()
          : base("Connect to Airtable", "Connect",
              "Verifies a Personal Access Token against Airtable and returns the connected user's ID.",
              PluginUtilities.TabName, PluginUtilities.CategoryAAAuth)
        { }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Token", "T", "Airtable Personal Access Token.", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Status", "S", "\"OK\" if the token is valid, otherwise the error.", GH_ParamAccess.item);
            pManager.AddTextParameter("Response", "R", "Raw JSON response from Airtable.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string token = "";

            DA.GetData(0, ref token);

            if (string.IsNullOrWhiteSpace(token))
            {
                DA.SetData(0, "Token is empty.");
                DA.SetData(1, "");
                return;
            }

            var client = new AirTableConnectClient(token);
            var result = client.WhoAmiAsync().GetAwaiter().GetResult();

            DA.SetData(0, result.Item1 ? "OK" : result.Item3);
            DA.SetData(1, result.Item1 ? result.Item2 : "");
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ATC_AirTableConnectAuth;
        public override Guid ComponentGuid => new Guid("c648cb11-4582-4a6f-a8f3-e9bdd053dfe6");
    }
}
