using Autodesk.Navisworks.Api.Data;
using Autodesk.Navisworks.Api.Interop;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.Takeoff;
using CDS_Plugin.Properties;
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
                        NavisworksParameter p = cmd.CreateParameter();

                        p = cmd.CreateParameter();
                        p.ParameterName = "@itemID";
                        p.Value = itemId;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@name";
                        p.Value = name;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@description";
                        p.Value = description;
                        cmd.Parameters.Add(p);

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

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

                        cmd.CommandText = "Select rowId FROM TK_ITEM WHERE name = @name";

                        NavisworksParameter p = cmd.CreateParameter();
                        p.ParameterName = "@resourceId";
                        p.Value = resourceId;// последнее цифра в индексе wbs
                        cmd.Parameters.Add(p);

                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                return 1;
            }

            return 1;

        }

    }
}
