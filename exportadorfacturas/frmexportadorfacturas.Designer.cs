namespace exportadorfacturas
{
    partial class frmexportadorfacturas
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmexportadorfacturas));
            this.lblError = new System.Windows.Forms.Label();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCantClientes = new System.Windows.Forms.Label();
            this.lblCantRecargas = new System.Windows.Forms.Label();
            this.lblCantLlamadas = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grilla_informe = new System.Windows.Forms.DataGridView();
            this.btnConfig = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.btnEjecutar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.chkExportarTxt = new System.Windows.Forms.CheckBox();
            this.chkFacturaElectronica = new System.Windows.Forms.CheckBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grilla_informe)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Coral;
            this.lblError.Location = new System.Drawing.Point(24, 127);
            this.lblError.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 48);
            this.lblError.TabIndex = 3;
            // 
            // lblMensajes
            // 
            this.lblMensajes.AutoSize = true;
            this.lblMensajes.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensajes.ForeColor = System.Drawing.SystemColors.Window;
            this.lblMensajes.Location = new System.Drawing.Point(24, 46);
            this.lblMensajes.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(863, 48);
            this.lblMensajes.TabIndex = 2;
            this.lblMensajes.Text = "Inicializando componentes de Exportador...";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCantClientes);
            this.groupBox1.Controls.Add(this.lblCantRecargas);
            this.groupBox1.Controls.Add(this.lblCantLlamadas);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.grilla_informe);
            this.groupBox1.Location = new System.Drawing.Point(1192, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(636, 112);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // lblCantClientes
            // 
            this.lblCantClientes.AutoSize = true;
            this.lblCantClientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantClientes.ForeColor = System.Drawing.SystemColors.Window;
            this.lblCantClientes.Location = new System.Drawing.Point(442, 56);
            this.lblCantClientes.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCantClientes.Name = "lblCantClientes";
            this.lblCantClientes.Size = new System.Drawing.Size(36, 37);
            this.lblCantClientes.TabIndex = 15;
            this.lblCantClientes.Text = "0";
            // 
            // lblCantRecargas
            // 
            this.lblCantRecargas.AutoSize = true;
            this.lblCantRecargas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantRecargas.ForeColor = System.Drawing.SystemColors.Window;
            this.lblCantRecargas.Location = new System.Drawing.Point(260, 56);
            this.lblCantRecargas.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCantRecargas.Name = "lblCantRecargas";
            this.lblCantRecargas.Size = new System.Drawing.Size(36, 37);
            this.lblCantRecargas.TabIndex = 14;
            this.lblCantRecargas.Text = "0";
            // 
            // lblCantLlamadas
            // 
            this.lblCantLlamadas.AutoSize = true;
            this.lblCantLlamadas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantLlamadas.ForeColor = System.Drawing.SystemColors.Window;
            this.lblCantLlamadas.Location = new System.Drawing.Point(62, 56);
            this.lblCantLlamadas.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCantLlamadas.Name = "lblCantLlamadas";
            this.lblCantLlamadas.Size = new System.Drawing.Size(36, 37);
            this.lblCantLlamadas.TabIndex = 13;
            this.lblCantLlamadas.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(442, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 37);
            this.label3.TabIndex = 12;
            this.label3.Text = "Error";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(216, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 37);
            this.label2.TabIndex = 11;
            this.label2.Text = "Enviadas OK";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 37);
            this.label1.TabIndex = 10;
            this.label1.Text = "Procesando";
            // 
            // grilla_informe
            // 
            this.grilla_informe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grilla_informe.Location = new System.Drawing.Point(50, 31);
            this.grilla_informe.Margin = new System.Windows.Forms.Padding(6);
            this.grilla_informe.Name = "grilla_informe";
            this.grilla_informe.Size = new System.Drawing.Size(88, 19);
            this.grilla_informe.TabIndex = 14;
            // 
            // btnConfig
            // 
            this.btnConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnConfig.Image")));
            this.btnConfig.Location = new System.Drawing.Point(6, 2);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(6);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(46, 38);
            this.btnConfig.TabIndex = 11;
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(36, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 188);
            this.panel1.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Location = new System.Drawing.Point(26, 8);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(6);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(952, 165);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.Color.White;
            this.tabPage7.Controls.Add(this.btnEjecutar);
            this.tabPage7.Controls.Add(this.label6);
            this.tabPage7.Controls.Add(this.label5);
            this.tabPage7.Controls.Add(this.txtMin);
            this.tabPage7.Controls.Add(this.chkExportarTxt);
            this.tabPage7.Controls.Add(this.chkFacturaElectronica);
            this.tabPage7.Location = new System.Drawing.Point(8, 39);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage7.Size = new System.Drawing.Size(936, 118);
            this.tabPage7.TabIndex = 7;
            this.tabPage7.Text = "FE";
            // 
            // btnEjecutar
            // 
            this.btnEjecutar.Location = new System.Drawing.Point(654, 65);
            this.btnEjecutar.Margin = new System.Windows.Forms.Padding(6);
            this.btnEjecutar.Name = "btnEjecutar";
            this.btnEjecutar.Size = new System.Drawing.Size(132, 44);
            this.btnEjecutar.TabIndex = 22;
            this.btnEjecutar.Text = "Ejecutar";
            this.btnEjecutar.UseVisualStyleBackColor = true;
            this.btnEjecutar.Click += new System.EventHandler(this.btnEjecutar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(688, 21);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 25);
            this.label6.TabIndex = 20;
            this.label6.Text = "minutos";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(562, 21);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 25);
            this.label5.TabIndex = 19;
            this.label5.Text = "Cada:";
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(626, 15);
            this.txtMin.Margin = new System.Windows.Forms.Padding(6);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(58, 31);
            this.txtMin.TabIndex = 18;
            // 
            // chkExportarTxt
            // 
            this.chkExportarTxt.AutoSize = true;
            this.chkExportarTxt.Location = new System.Drawing.Point(40, 60);
            this.chkExportarTxt.Margin = new System.Windows.Forms.Padding(6);
            this.chkExportarTxt.Name = "chkExportarTxt";
            this.chkExportarTxt.Size = new System.Drawing.Size(302, 29);
            this.chkExportarTxt.TabIndex = 15;
            this.chkExportarTxt.Text = "Exportar a archivo de texto";
            this.chkExportarTxt.UseVisualStyleBackColor = true;
            // 
            // chkFacturaElectronica
            // 
            this.chkFacturaElectronica.AutoSize = true;
            this.chkFacturaElectronica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFacturaElectronica.Location = new System.Drawing.Point(12, 15);
            this.chkFacturaElectronica.Margin = new System.Windows.Forms.Padding(6);
            this.chkFacturaElectronica.Name = "chkFacturaElectronica";
            this.chkFacturaElectronica.Size = new System.Drawing.Size(520, 41);
            this.chkFacturaElectronica.TabIndex = 14;
            this.chkFacturaElectronica.Text = "PROCESO EXPORTACION FE";
            this.chkFacturaElectronica.UseVisualStyleBackColor = true;
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.SelectedPath = "C:\\H2O";
            // 
            // timer3
            // 
            this.timer3.Interval = 30000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // frmexportadorfacturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1840, 206);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.lblError);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmexportadorfacturas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exportador de Comprobantes Uruguay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmexportadorfacturas_FormClosing);
            this.Load += new System.EventHandler(this.frmexportadorfacturas_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grilla_informe)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCantClientes;
        private System.Windows.Forms.Label lblCantRecargas;
        private System.Windows.Forms.Label lblCantLlamadas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.Button btnEjecutar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.CheckBox chkExportarTxt;
        private System.Windows.Forms.CheckBox chkFacturaElectronica;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.DataGridView grilla_informe;
    }
}