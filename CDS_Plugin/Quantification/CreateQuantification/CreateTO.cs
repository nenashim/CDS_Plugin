using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Para evitar ambiguidade entre a API do Navisworks e Windows Forms 
using wf = System.Windows.Forms;
//Navisworks
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.DocumentParts;
//COM
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using COMApi = Autodesk.Navisworks.Api.Interop.ComApi;
using Autodesk.Navisworks.Api.Takeoff;
using Autodesk.Navisworks.Api.Data;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.Xml.Linq;
using System.Windows.Controls;

namespace CDS_Plugin.Quantification.CreateTakeoff
{
 
        [PluginAttribute("QuantifyByProperty", "Winderson", DisplayName = "Quantificar por propriedade", ToolTip = "Quantifica os elementos agrupando a partir de uma propriedade")]
        class QuantifyByProperty : AddInPlugin
        {
            public override int Execute(params string[] parameters)
            {
                //Documento atual
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

                DocumentModels rootItems = doc.Models;
                
                Autodesk.Navisworks.Api.Model root = rootItems[0];

                Units unit = root.Units;

                try
                {
                    /// https://apidocs.co/apps/navisworks/2018/87317537-2911-4c08-b492-6496c82b3ee6.htm
                    DocumentTakeoff docTakeoff = Autodesk.Navisworks.Api.Application.MainDocument.Takeoff as DocumentTakeoff;

                    DocumentTakeoff docTakeoff2 = Autodesk.Navisworks.Api.Application.MainDocument.GetTakeoff();

                    //InsertItem(null, "Item teste", "Item teste criado pela API", "WBS", 1, 0.0);
                    Int64 lastRowId = GetLastInsertRowId();
                    Int64 item1 = InsertItem(lastRowId, "Item test", "Item teste criado pela API", "WBS", 1, 1.1, 1.1, 1.1, 1.1);


            }
                catch (Exception e)
                {
                    wf.MessageBox.Show(e.Message, "Проблемка", wf.MessageBoxButtons.OK, wf.MessageBoxIcon.Information);

                    return 0;
                }

                return 0;
            }
        

            Int64 GetLastInsertRowId()
            {
                DocumentTakeoff docTakeoff = Autodesk.Navisworks.Api.Application.MainDocument.GetTakeoff();
                using (NavisworksCommand cmd = docTakeoff.Database.Value.CreateCommand())
                {
                    //use SELECT ... FROM ... WHERE ... sql for query.
                    //last_insert_rowid() is a stored function used to retrieve the rowid of the last insert row
                    cmd.CommandText = "select last_insert_rowid()";
                    using (NavisWorksDataReader dataReader = cmd.ExecuteReader())
                    {
                        Int64 lastId = -1;
                        if (dataReader.Read())
                        {
                            Int64.TryParse(dataReader[0].ToString(), out lastId);
                        }
                        return lastId;
                    }
                }
            }
       


            Int64 InsertItem(Int64? parent, String name, String description, String wbs, Int32 color, Double transparency, double lineThickness, double CountSymbol, double CountSize)
            {
                Debug.Assert(name != null);
                DocumentTakeoff docTakeoff = Autodesk.Navisworks.Api.Application.MainDocument.GetTakeoff();
                ItemTable table = docTakeoff.Items;
                Debug.Assert(table != null);

                //Directly operate on database
                //Database schema entry: TakeoffTable
                //INSERT INTO TABLE(COL1,COL2,COL3...) VALUES(V1,V2,V3...);
                String sql = "INSERT INTO TK_ITEM(parent, name, description, wbs, color, transparency,LineThickness,CountSymbol,CountSize) VALUES(@parent, @name, @description,@wbs, @color,@transparency,@LineThickness,@CountSymbol,@CountSize)";
                //Modification must be surrounded by NavisworksTransaction
                using (NavisworksTransaction trans = docTakeoff.Database.BeginTransaction(DatabaseChangedAction.Edited))
                {
                    using (NavisworksCommand cmd = docTakeoff.Database.Value.CreateCommand())
                    {
                        NavisworksParameter p = cmd.CreateParameter();

                        p.ParameterName = "@parent";
                        if (parent.HasValue)
                            p.Value = parent.Value;
                        else
                            p.Value = null;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@name";
                        p.Value = name;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@description";
                        p.Value = description;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@wbs";
                        p.Value = wbs;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@color";
                        p.Value = color;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@transparency";
                        p.Value = transparency;
                        cmd.Parameters.Add(p);


                        p = cmd.CreateParameter();
                        p.ParameterName = "@LineThickness";
                        p.Value = lineThickness;
                        cmd.Parameters.Add(p);

                        p = cmd.CreateParameter();
                        p.ParameterName = "@CountSymbol";
                        p.Value = CountSymbol;
                        cmd.Parameters.Add(p);

                        //CountSize
                        p = cmd.CreateParameter();
                        p.ParameterName = "@CountSize";
                        p.Value = CountSize;
                        cmd.Parameters.Add(p);

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }


                    trans.Commit();

                }
                return GetLastInsertRowId();
            }
        }
    }

