using System;
using Autodesk.Navisworks.Api.Plugins;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Linq;
using App = Autodesk.Navisworks.Api.Application;
using Autodesk.Navisworks.Api.DocumentParts;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using AppExcel = Microsoft.Office.Interop.Excel.Application;

using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;

namespace CDS_Plugin
{
    public partial class FormAddPropAll : Form
    {
        public FormAddPropAll()
        {
            InitializeComponent();
        }

        private void bt_File_MouseUp(object sender, EventArgs e)
        {
            if (tb_TabName1.TextLength == 0)
            {
                MessageBox.Show("Введите название вкладки");
            }
            else
            {
                try
                {

                    Visible = false;
                    OpenFileDialog dial = new OpenFileDialog();
                    dial.Filter = "Excel files (*.xls, *.xlsx, *.xlsm)|*.xls;*.xlsx;*.xlsm";
                    if (dial.ShowDialog() == DialogResult.OK)
                    {

                        string path = dial.FileName;

                        List<string> propName = AllCustomAddin.getPropNamesFromExcel(path);
                        ComApi.InwOpState10 state;
                        state = ComApiBridge.ComApiBridge.State;

                        ModelItemCollection curSelect = App.ActiveDocument.CurrentSelection.SelectedItems;

                        //To select individual elements change variable modelItemCollection:
                        //ModelItemCollection modelItemCollection = new ModelItemCollection(curSelect);
                        ModelItemEnumerableCollection modelItemCollection = App.ActiveDocument.Models.RootItemDescendants;
                        List<List<List<string>>> properties = AllCustomAddin.getDataOfAllProperties(1, path);
                        

                        List<string> typeName = AllCustomAddin.getPropTypesFromExcel(path);
                        string tabName = tb_TabName1.Text;

                        foreach (ModelItem oEachSelectedItem in modelItemCollection)
                        {
                                ComApi.InwOaPath oPath = ComApiBridge.ComApiBridge.ToInwOaPath(oEachSelectedItem);
                                ComApi.InwGUIPropertyNode2 propn = (ComApi.InwGUIPropertyNode2)state.GetGUIPropertyNode(oPath, true);
                                ComApi.InwOaPropertyVec newPvec = (ComApi.InwOaPropertyVec)state.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaPropertyVec, null, null);

                                for (var i = 0; i < properties.Count; i++)
                                {
                                    ComApi.InwOaProperty newP = (ComApi.InwOaProperty)state.ObjectFactory(ComApi.nwEObjectType.eObjectType_nwOaProperty, null, null);

                                    for (var j = 0; j < properties[i].Count; j++)
                                    {
                                        for (var k = 0; k < properties[i][j].Count; k++)
                                        {
                                            AllCustomAddin.setValueToProperties(typeName[j], properties[i][j][k], oEachSelectedItem, newP);
                                        }
                                    }

                                    newP.name = propName[i];
                                    newP.UserName = propName[i];

                                    if (newP.value != null)
                                    {
                                        newPvec.Properties().Add(newP);
                                    }
                                }

                                //if custom tab already exist - delete it

                                int index = 1;
                                foreach (ComApi.InwGUIAttribute2 nwAtt in propn.GUIAttributes())
                                {
                                    if (nwAtt.UserDefined)
                                    {
                                        if (nwAtt.ClassUserName == tabName)
                                            break;
                                        index += 1;
                                    }
                                }

                                foreach (ComApi.InwGUIAttribute2 nwAtt in propn.GUIAttributes())
                                {
                                    if (nwAtt.ClassUserName == tabName)
                                    {
                                        propn.RemoveUserDefined(index);
                                    }
                                }

                                //the first argument must be 0 when add new custom tab
                                propn.SetUserDefined(0, tabName, tabName, newPvec);
                        }

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Выберите подходящий файл со свойствами");
                }
            }

        }

        private void bt_Close_MouseUp(object sender, MouseEventArgs e)
        {
            Visible = false;
        }

        private void bt_File_Click(object sender, EventArgs e)
        {

        }

        private void tb_TabName_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_Close_Click(object sender, EventArgs e)
        {

        }

        private void rBt_en_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rBt_ru_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FormAddProp_Load(object sender, EventArgs e)
        {

        }

        private void bt_Close_Click_1(object sender, EventArgs e)
        {

        }
    }
}
