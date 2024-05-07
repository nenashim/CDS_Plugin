namespace CDS_Plugin
{
    partial class FormAddPropEll
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
            this.lb_Name = new System.Windows.Forms.Label();
            this.tb_TabName = new System.Windows.Forms.TextBox();
            this.lbFile = new System.Windows.Forms.Label();
            this.bt_FileEll = new System.Windows.Forms.Button();
            this.bt_Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_Name
            // 
            this.lb_Name.AutoSize = true;
            this.lb_Name.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb_Name.Location = new System.Drawing.Point(12, 9);
            this.lb_Name.Name = "lb_Name";
            this.lb_Name.Size = new System.Drawing.Size(172, 16);
            this.lb_Name.TabIndex = 1;
            this.lb_Name.Text = "Введите название вкладки:";
            // 
            // tb_TabName
            // 
            this.tb_TabName.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_TabName.Location = new System.Drawing.Point(15, 38);
            this.tb_TabName.Name = "tb_TabName";
            this.tb_TabName.Size = new System.Drawing.Size(269, 22);
            this.tb_TabName.TabIndex = 2;
            this.tb_TabName.TextChanged += new System.EventHandler(this.tb_TabName_TextChanged);
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbFile.Location = new System.Drawing.Point(12, 80);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(241, 16);
            this.lbFile.TabIndex = 7;
            this.lbFile.Text = "Выберите файл для создания вкладки:";
            // 
            // bt_FileEll
            // 
            this.bt_FileEll.BackColor = System.Drawing.Color.MediumAquamarine;
            this.bt_FileEll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_FileEll.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_FileEll.Location = new System.Drawing.Point(15, 102);
            this.bt_FileEll.Name = "bt_FileEll";
            this.bt_FileEll.Size = new System.Drawing.Size(105, 37);
            this.bt_FileEll.TabIndex = 8;
            this.bt_FileEll.Text = "Выбрать файл";
            this.bt_FileEll.UseVisualStyleBackColor = false;
            this.bt_FileEll.Click += new System.EventHandler(this.bt_File_Click);
            this.bt_FileEll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_File_MouseUp);
            // 
            // bt_Close
            // 
            this.bt_Close.BackColor = System.Drawing.Color.Salmon;
            this.bt_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Close.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_Close.Location = new System.Drawing.Point(175, 102);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(109, 37);
            this.bt_Close.TabIndex = 9;
            this.bt_Close.Text = "Отмена";
            this.bt_Close.UseVisualStyleBackColor = false;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            this.bt_Close.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bt_Close_MouseUp);
            // 
            // FormAddPropEll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 153);
            this.Controls.Add(this.bt_Close);
            this.Controls.Add(this.bt_FileEll);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.tb_TabName);
            this.Controls.Add(this.lb_Name);
            this.Name = "FormAddPropEll";
            this.Text = "FormAddProp";
            this.Load += new System.EventHandler(this.FormAddProp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_Name;
        private System.Windows.Forms.TextBox tb_TabName;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.Button bt_FileEll;
        private System.Windows.Forms.Button bt_Close;
    }
}