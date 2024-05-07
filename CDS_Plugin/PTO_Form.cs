using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDS_Plugin
{

    public partial class PTO_Form : Form
    {
        public IEnumerable<string> modelscoll { get; private set; }

        public PTO_Form()
        {
            InitializeComponent();

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            ModelItemEnumerableCollection modelItemCollection = doc.Models.RootItemDescendants;
            //ModelItemEnumerableCollection newCollection = new ModelItemEnumerableCollection();
            //ModelItemEnumerableCollection invertCollection = new ModelItemEnumerableCollection();
            //Копируем все выбранные элементы в инвертируемую коллекцию
            //invertCollection.CopyFrom(selectionItems);

            //if (modelItemCollection.Count > 0)
            //{
            //    MessageBox.Show("да");

            //}
            //else
            //{
            //    MessageBox.Show("нет");
            //}


            List<string> modelscoll = new List<string>((IEnumerable<string>)modelItemCollection);
            //List<string> modelscoll = new List<string> () { "red", "green", "blue" };

            foreach (string model in modelscoll)
            {
                comboBox1.Items.Add(model);
            }

            //if (modelscoll.Count > 0 )
            //{
            //    MessageBox.Show("да") ;

            //}
            //else {
            //    MessageBox.Show("нет");
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          

           
          

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
