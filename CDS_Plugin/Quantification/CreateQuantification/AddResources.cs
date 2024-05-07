using Autodesk.Navisworks.Api.Data;
using Autodesk.Navisworks.Api.Interop;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.Takeoff;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CDS_Plugin.Quantification.CreateQuantification
{
    internal class AddResources : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {

            Int64 InsertintoTK_Step(Int64 itemId , string name ,string description)
            {
                DocumentTakeoff docTakeoff = Autodesk.Navisworks.Api.Application.MainDocument.GetTakeoff();
                string sql = "INSERT INTO TK_Step(itemID, name, description) VALUES(@itemID, @name, @description)";
                using (NavisworksTransaction trans = docTakeoff.Database.BeginTransaction(DatabaseChangedAction.Edited))
                {

                    using (NavisworksCommand cmd = docTakeoff.Database.Value.CreateCommand())
                    {
                       // StepResourceTable resourceTable = docTakeoff.Resources;
                        NavisworksParameter p = cmd.CreateParameter();
                        p.ParameterName = "@itemID";

                    }
                    trans.Commit();
                }
                return 1;
            }

            Int64 InsertintoTK_StepResource(Int64 resourceId, Int64 itemID, string name, string description)
            {
                DocumentTakeoff docTakeoff = Autodesk.Navisworks.Api.Application.MainDocument.GetTakeoff();
                string sql = "INSERT INTO TK_StepResource(resourceId, itemID, name, description) VALUES(@resourceId, @itemID, @name, @description)";
                using (NavisworksTransaction trans = docTakeoff.Database.BeginTransaction(DatabaseChangedAction.Edited))
                {
                    using (NavisworksCommand cmd = docTakeoff.Database.Value.CreateCommand())
                    {
                        ResourceTable newResource = docTakeoff.Resources;
                        NavisworksParameter p = cmd.CreateParameter();
                        p.ParameterName = "@resourceId";
                        int a = 0;

                    }
                    trans.Commit();
                }
                return 1;
            }

           
               

                List<ResourceGroupTable> resources = new List<ResourceGroupTable>();
              //  MessageBox.Show(resource.ToString());

            return 1;

        }

    }
}
