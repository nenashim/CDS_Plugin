namespace CDS_Plugin.Quantification.CreateQuantification
{
    partial class CreateQuantification_AR
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
            this.lb_cat = new System.Windows.Forms.Label();
            this.textBox_nameObject = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.tabConsole = new System.Windows.Forms.TabControl();
            this.tab_header = new System.Windows.Forms.TabPage();
            this.Box_countSection = new System.Windows.Forms.NumericUpDown();
            this.tab_param = new System.Windows.Forms.TabPage();
            this.text_param = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.text_cat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabConsole.SuspendLayout();
            this.tab_header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Box_countSection)).BeginInit();
            this.tab_param.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_cat
            // 
            this.lb_cat.AutoSize = true;
            this.lb_cat.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb_cat.ForeColor = System.Drawing.Color.Black;
            this.lb_cat.Location = new System.Drawing.Point(5, 8);
            this.lb_cat.Name = "lb_cat";
            this.lb_cat.Size = new System.Drawing.Size(154, 16);
            this.lb_cat.TabIndex = 1;
            this.lb_cat.Text = "Наименование объекта: ";
            this.lb_cat.Click += new System.EventHandler(this.lb_cat_Click);
            // 
            // textBox_nameObject
            // 
            this.textBox_nameObject.Location = new System.Drawing.Point(7, 31);
            this.textBox_nameObject.Name = "textBox_nameObject";
            this.textBox_nameObject.Size = new System.Drawing.Size(151, 20);
            this.textBox_nameObject.TabIndex = 2;
            this.textBox_nameObject.TextChanged += new System.EventHandler(this.textBox_nameObject_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Количество секций:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(6, 183);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(151, 46);
            this.OK_button.TabIndex = 7;
            this.OK_button.Text = "Создать";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.tab_header);
            this.tabConsole.Controls.Add(this.tab_param);
            this.tabConsole.Location = new System.Drawing.Point(12, 12);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.SelectedIndex = 0;
            this.tabConsole.Size = new System.Drawing.Size(179, 261);
            this.tabConsole.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabConsole.TabIndex = 8;
            // 
            // tab_header
            // 
            this.tab_header.Controls.Add(this.Box_countSection);
            this.tab_header.Controls.Add(this.textBox_nameObject);
            this.tab_header.Controls.Add(this.OK_button);
            this.tab_header.Controls.Add(this.lb_cat);
            this.tab_header.Controls.Add(this.label1);
            this.tab_header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tab_header.Location = new System.Drawing.Point(4, 22);
            this.tab_header.Name = "tab_header";
            this.tab_header.Padding = new System.Windows.Forms.Padding(3);
            this.tab_header.Size = new System.Drawing.Size(171, 235);
            this.tab_header.TabIndex = 0;
            this.tab_header.Text = "Объект";
            this.tab_header.UseVisualStyleBackColor = true;
            this.tab_header.Click += new System.EventHandler(this.tab_header_Click);
            // 
            // Box_countSection
            // 
            this.Box_countSection.Location = new System.Drawing.Point(9, 82);
            this.Box_countSection.Name = "Box_countSection";
            this.Box_countSection.Size = new System.Drawing.Size(47, 20);
            this.Box_countSection.TabIndex = 10;
            this.Box_countSection.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Box_countSection.ValueChanged += new System.EventHandler(this.textBox_countSection_ValueChanged);
            // 
            // tab_param
            // 
            this.tab_param.Controls.Add(this.text_param);
            this.tab_param.Controls.Add(this.label4);
            this.tab_param.Controls.Add(this.text_cat);
            this.tab_param.Controls.Add(this.label3);
            this.tab_param.Location = new System.Drawing.Point(4, 22);
            this.tab_param.Name = "tab_param";
            this.tab_param.Padding = new System.Windows.Forms.Padding(3);
            this.tab_param.Size = new System.Drawing.Size(170, 235);
            this.tab_param.TabIndex = 1;
            this.tab_param.Text = "Параметры";
            this.tab_param.UseVisualStyleBackColor = true;
            // 
            // text_param
            // 
            this.text_param.Location = new System.Drawing.Point(5, 92);
            this.text_param.Name = "text_param";
            this.text_param.Size = new System.Drawing.Size(151, 20);
            this.text_param.TabIndex = 6;
            this.text_param.Text = "ADSK_Этаж";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(3, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Наименование параметра:";
            // 
            // text_cat
            // 
            this.text_cat.Location = new System.Drawing.Point(5, 36);
            this.text_cat.Name = "text_cat";
            this.text_cat.Size = new System.Drawing.Size(151, 20);
            this.text_cat.TabIndex = 4;
            this.text_cat.Text = "Объект";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(3, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Наименование категории:";
            // 
            // CreateQuantification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 286);
            this.Controls.Add(this.tabConsole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CreateQuantification";
            this.Text = "Quanti";
            this.Load += new System.EventHandler(this.CreateQuantification_Load);
            this.tabConsole.ResumeLayout(false);
            this.tab_header.ResumeLayout(false);
            this.tab_header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Box_countSection)).EndInit();
            this.tab_param.ResumeLayout(false);
            this.tab_param.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lb_cat;
        private System.Windows.Forms.TextBox textBox_nameObject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.TabControl tabConsole;
        private System.Windows.Forms.TabPage tab_header;
        private System.Windows.Forms.TabPage tab_param;
        private System.Windows.Forms.TextBox text_param;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox text_cat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Box_countSection;
    }
}