using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Windows.Markup;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Interop.ComApi;
using System.Drawing;
using System.Windows.Shapes;
using System.Linq;

namespace CDS_Plugin
{
    public partial class FormAddRole : Form
    {
        public dynamic ReadRoleName { get; set; }
        public dynamic ReadPersonName { get; set; }

        public FormAddRole()
        {
            InitializeComponent();
        }

        private void bt_OK_MouseUp(object sender, MouseEventArgs e)
        {
            string RoleName = null;
            string PersonName = textBoxPersonName.Text;

            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;


            if (radioButton1.Checked)
            {
                RoleName = "Начальник участка (ИТР)";
            }
            else if (radioButton2.Checked)
            {
                RoleName = "Инженер УСК";
            }
            else
            {
                MessageBox.Show("Выберите роль!");
            }
            

            if (PersonName.Trim() != "")
            {

                string Rolefile = RoleName + "\n" + PersonName;
               
                try
                {
                   File.WriteAllText(@"C:\Users\Public\RolePC.txt", Rolefile);
                  

                    Visible = false;
                }   
                catch {
                    MessageBox.Show("Ошибка!");

                }
                
            }
        
                //@"C:\Program Files\Autodesk\Navisworks Manage 2022\Plugins\CDS_Plugin\

            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
            }
           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lb_folder_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
           
        }

        private class data
        {

            public string RoleName { get; set; }
            public string PersonName { get; set; }
        }

        private void textBoxPersonName_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormAddRole_Load(object sender, EventArgs e)
        {

        }
    }
}
