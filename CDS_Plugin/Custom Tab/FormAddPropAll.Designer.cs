namespace CDS_Plugin
{
    partial class FormAddPropAll
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
            this.bt_FileAll = new System.Windows.Forms.Button();
            this.bt_Close = new System.Windows.Forms.Button();
            this.lbFile = new System.Windows.Forms.Label();
            this.tb_TabName1 = new System.Windows.Forms.TextBox();
            this.lb_Name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bt_FileAll
            // 
            this.bt_FileAll.BackColor = System.Drawing.Color.MediumAquamarine;
            this.bt_FileAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_FileAll.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_FileAll.Location = new System.Drawing.Point(24, 95);
            this.bt_FileAll.Name = "bt_FileAll";
            this.bt_FileAll.Size = new System.Drawing.Size(105, 37);
            this.bt_FileAll.TabIndex = 9;
            this.bt_FileAll.Text = "Выбрать файл";
            this.bt_FileAll.UseVisualStyleBackColor = false;
            this.bt_FileAll.Click += new System.EventHandler(this.bt_File_Click);
            this.bt_FileAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_File_MouseUp);
            // 
            // bt_Close
            // 
            this.bt_Close.BackColor = System.Drawing.Color.Salmon;
            this.bt_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Close.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_Close.Location = new System.Drawing.Point(183, 95);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(109, 37);
            this.bt_Close.TabIndex = 10;
            this.bt_Close.Text = "Отмена";
            this.bt_Close.UseVisualStyleBackColor = false;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click_1);
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbFile.Location = new System.Drawing.Point(21, 73);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(241, 16);
            this.lbFile.TabIndex = 11;
            this.lbFile.Text = "Выберите файл для создания вкладки:";
            // 
            // tb_TabName1
            // 
            this.tb_TabName1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_TabName1.Location = new System.Drawing.Point(23, 35);
            this.tb_TabName1.Name = "tb_TabName1";
            this.tb_TabName1.Size = new System.Drawing.Size(269, 22);
            this.tb_TabName1.TabIndex = 12;
            this.tb_TabName1.TextChanged += new System.EventHandler(this.tb_TabName_TextChanged);
            // 
            // lb_Name
            // 
            this.lb_Name.AutoSize = true;
            this.lb_Name.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb_Name.Location = new System.Drawing.Point(21, 9);
            this.lb_Name.Name = "lb_Name";
            this.lb_Name.Size = new System.Drawing.Size(172, 16);
            this.lb_Name.TabIndex = 13;
            this.lb_Name.Text = "Введите название вкладки:";
            // 
            // FormAddPropAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 148);
            this.Controls.Add(this.lb_Name);
            this.Controls.Add(this.tb_TabName1);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_FileAll);
            this.Name = "FormAddPropAll";
            this.Text = "FormAddPropAll";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_FileAll;
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.TextBox tb_TabName1;
        private System.Windows.Forms.Label lb_Name;
    }
}