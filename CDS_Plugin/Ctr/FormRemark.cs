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

    public partial class FormRemark : Form
    {
        public string Remark;
       
        public FormRemark()
        {

            InitializeComponent();
        }

        private void lb_folder_Click(object sender, EventArgs e)
        {

        }

        public void bt_OK_Click(object sender, EventArgs e)
        {
            Remark = textRemark.Text;

            Visible = false;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
