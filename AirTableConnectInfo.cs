using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace AirTableConnect
{
    public class AirTableConnectInfo : GH_AssemblyInfo
    {
        public override string Name => "AirTableConnect";
        public override Bitmap Icon => null;
        public override string Description => "Grasshopper plugin for the Airtable REST API.";
        public override Guid Id => new Guid("11d365ae-8a92-4948-937f-31226bbc4f2f");
        public override string AuthorName => "Eesha Jain";
        public override string AuthorContact => "toeesha@gmail.com";
        public override string AssemblyVersion => "1.0.0";
    }

    public class AirTableConnectCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon("AirTableConnect", Properties.Resources.ATC_AirTableConnectLogo);
            Instances.ComponentServer.AddCategorySymbolName("AirTableConnect", 'A');
            return GH_LoadingInstruction.Proceed;
        }
    }
}
