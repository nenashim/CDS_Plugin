using Autodesk.Navisworks.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App = Autodesk.Navisworks.Api.Application;

using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi;

namespace CDS_Plugin
{
    public partial class FormSetSettings : Form
    {
        public FormSetSettings()
        {
            InitializeComponent();
        }

        private void bt_OK_MouseUp(object sender, MouseEventArgs e)
        {
            string folder = tb_folder.Text;
            string category = tb_cat.Text;
            string property = tb_prop.Text;

            try
            {
                Visible = false;
                Document activeDoc = App.ActiveDocument;
                var curSelect = activeDoc.CurrentSelection;
                var selectionSets = activeDoc.SelectionSets;


                int setsValues = selectionSets.Value.IndexOfDisplayName(folder);

                //If folder doesn't exist add folder
                if (setsValues != -1)
                {
                    selectionSets.RemoveAt(selectionSets.RootItem, setsValues);
                }

                selectionSets.AddCopy(new FolderItem() { DisplayName = folder });


                List<string> values = AllCustomAddin.getDataForProp(category, property);


                foreach (string value in values)

                {
                    curSelect.SelectAll();

                    var search = new Search();
                    search.Locations = SearchLocations.DescendantsAndSelf;
                    search.Selection.SelectAll();

                    //If number of group  equal property - add to search
                    SearchCondition searchCondition = SearchCondition.HasPropertyByDisplayName(category, property);

                    search.SearchConditions.Add(searchCondition.EqualValue(VariantData.FromDisplayString(value)));
                    //search.SearchConditions.Add(searchCondition.DisplayStringContains(value));

                    var set = new SelectionSet(search) { DisplayName = value };
                    selectionSets.AddCopy(set);
                    var newSet = selectionSets.Value[selectionSets.Value.IndexOfDisplayName(value)] as SavedItem;


                    var setFolder = selectionSets.Value[selectionSets.Value.IndexOfDisplayName(folder)] as FolderItem;

                    selectionSets.Move(newSet.Parent, selectionSets.Value.IndexOfDisplayName(value), setFolder, 0);

                }

                //Check exist name of folder 
            }
            catch
            {
                MessageBox.Show("Не удалось создать поисковые наборы");
            }

        }

        private void bt_Close_MouseUp(object sender, MouseEventArgs e)
        {
            Visible = false;
        }


        private void lb_prop_Click(object sender, EventArgs e)
        {

        }

        private void tb_cat_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_prop_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_OK_Click(object sender, EventArgs e)
        {

        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
           
        }

        private void tb_folder_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
