namespace CDS_Plugin
{
    partial class FormRemark
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
            this.lb_folder = new System.Windows.Forms.Label();
            this.textRemark = new System.Windows.Forms.TextBox();
            this.bt_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_folder
            // 
            this.lb_folder.AutoSize = true;
            this.lb_folder.Location = new System.Drawing.Point(12, 9);
            this.lb_folder.Name = "lb_folder";
            this.lb_folder.Size = new System.Drawing.Size(141, 13);
            this.lb_folder.TabIndex = 9;
            this.lb_folder.Text = "Введите текст замечания:";
            this.lb_folder.Click += new System.EventHandler(this.lb_folder_Click);
            // 
            // textRemark
            // 
            this.textRemark.Location = new System.Drawing.Point(12, 31);
            this.textRemark.Multiline = true;
            this.textRemark.Name = "textRemark";
            this.textRemark.Size = new System.Drawing.Size(276, 116);
            this.textRemark.TabIndex = 10;
            this.textRemark.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // bt_OK
            // 
            this.bt_OK.BackColor = System.Drawing.Color.MediumAquamarine;
            this.bt_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_OK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bt_OK.Location = new System.Drawing.Point(94, 162);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(109, 27);
            this.bt_OK.TabIndex = 13;
            this.bt_OK.Text = "ОК";
            this.bt_OK.UseVisualStyleBackColor = false;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // FormRemark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.textRemark);
            this.Controls.Add(this.lb_folder);
            this.Name = "FormRemark";
            this.Text = "Комментарий";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_folder;
        public System.Windows.Forms.TextBox textRemark;
        private System.Windows.Forms.Button bt_OK;
    }
}