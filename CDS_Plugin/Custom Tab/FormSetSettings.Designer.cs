namespace CDS_Plugin
{
    partial class FormSetSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_folder = new System.Windows.Forms.TextBox();
            this.tb_cat = new System.Windows.Forms.TextBox();
            this.tb_prop = new System.Windows.Forms.TextBox();
            this.lb_prop = new System.Windows.Forms.Label();
            this.lb_cat = new System.Windows.Forms.Label();
            this.lb_folder = new System.Windows.Forms.Label();
            this.bt_Close = new System.Windows.Forms.Button();
            this.bt_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_folder
            // 
            this.tb_folder.Location = new System.Drawing.Point(133, 19);
            this.tb_folder.Name = "tb_folder";
            this.tb_folder.Size = new System.Drawing.Size(226, 20);
            this.tb_folder.TabIndex = 2;
            this.tb_folder.TextChanged += new System.EventHandler(this.tb_folder_TextChanged);
            // 
            // tb_cat
            // 
            this.tb_cat.Location = new System.Drawing.Point(133, 55);
            this.tb_cat.Name = "tb_cat";
            this.tb_cat.Size = new System.Drawing.Size(226, 20);
            this.tb_cat.TabIndex = 5;
            this.tb_cat.TextChanged += new System.EventHandler(this.tb_cat_TextChanged);
            // 
            // tb_prop
            // 
            this.tb_prop.Location = new System.Drawing.Point(133, 92);
            this.tb_prop.Name = "tb_prop";
            this.tb_prop.Size = new System.Drawing.Size(226, 20);
            this.tb_prop.TabIndex = 6;
            this.tb_prop.TextChanged += new System.EventHandler(this.tb_prop_TextChanged);
            // 
            // lb_prop
            // 
            this.lb_prop.AutoSize = true;
            this.lb_prop.Location = new System.Drawing.Point(11, 95);
            this.lb_prop.Name = "lb_prop";
            this.lb_prop.Size = new System.Drawing.Size(55, 13);
            this.lb_prop.TabIndex = 9;
            this.lb_prop.Text = "Свойство";
            this.lb_prop.Click += new System.EventHandler(this.lb_prop_Click);
            // 
            // lb_cat
            // 
            this.lb_cat.AutoSize = true;
            this.lb_cat.Location = new System.Drawing.Point(12, 58);
            this.lb_cat.Name = "lb_cat";
            this.lb_cat.Size = new System.Drawing.Size(111, 13);
            this.lb_cat.TabIndex = 8;
            this.lb_cat.Text = "Категория (вкладка)";
            // 
            // lb_folder
            // 
            this.lb_folder.AutoSize = true;
            this.lb_folder.Location = new System.Drawing.Point(11, 22);
            this.lb_folder.Name = "lb_folder";
            this.lb_folder.Size = new System.Drawing.Size(116, 13);
            this.lb_folder.TabIndex = 7;
            this.lb_folder.Text = "Наименование папки";
            // 
            // bt_Close
            // 
            this.bt_Close.BackColor = System.Drawing.Color.Salmon;
            this.bt_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Close.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_Close.Location = new System.Drawing.Point(229, 129);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(130, 27);
            this.bt_Close.TabIndex = 10;
            this.bt_Close.Text = "Отмена";
            this.bt_Close.UseVisualStyleBackColor = false;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            this.bt_Close.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_Close_MouseUp);
            // 
            // bt_OK
            // 
            this.bt_OK.BackColor = System.Drawing.Color.MediumAquamarine;
            this.bt_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_OK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_OK.Location = new System.Drawing.Point(14, 129);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(109, 27);
            this.bt_OK.TabIndex = 11;
            this.bt_OK.Text = "ОК";
            this.bt_OK.UseVisualStyleBackColor = false;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            this.bt_OK.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_OK_MouseUp);
            // 
            // FormSetSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 169);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.lb_prop);
            this.Controls.Add(this.lb_cat);
            this.Controls.Add(this.lb_folder);
            this.Controls.Add(this.tb_prop);
            this.Controls.Add(this.tb_cat);
            this.Controls.Add(this.tb_folder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormSetSettings";
            this.Text = "FormSetSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_folder;
        private System.Windows.Forms.TextBox tb_cat;
        private System.Windows.Forms.TextBox tb_prop;
        private System.Windows.Forms.Label lb_prop;
        private System.Windows.Forms.Label lb_cat;
        private System.Windows.Forms.Label lb_folder;
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Button bt_OK;
    }
}