
namespace InstaladorMaster
{
    partial class frmInstalador
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbSucursal = new System.Windows.Forms.Label();
            this.cbSucursales = new System.Windows.Forms.ComboBox();
            this.btnInstalar = new System.Windows.Forms.Button();
            this.barraProgreso = new System.Windows.Forms.ProgressBar();
            this.bwEjecutar = new System.ComponentModel.BackgroundWorker();
            this.lbMensaje = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbSucursal
            // 
            this.lbSucursal.AutoSize = true;
            this.lbSucursal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSucursal.Location = new System.Drawing.Point(60, 68);
            this.lbSucursal.Name = "lbSucursal";
            this.lbSucursal.Size = new System.Drawing.Size(197, 25);
            this.lbSucursal.TabIndex = 0;
            this.lbSucursal.Text = "Seleccione Sucursal:";
            // 
            // cbSucursales
            // 
            this.cbSucursales.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSucursales.FormattingEnabled = true;
            this.cbSucursales.Items.AddRange(new object[] {
            "Seleccione"});
            this.cbSucursales.Location = new System.Drawing.Point(293, 72);
            this.cbSucursales.Name = "cbSucursales";
            this.cbSucursales.Size = new System.Drawing.Size(278, 24);
            this.cbSucursales.TabIndex = 1;
            this.cbSucursales.SelectedIndexChanged += new System.EventHandler(this.cbSucursales_SelectedIndexChanged);
            // 
            // btnInstalar
            // 
            this.btnInstalar.Enabled = false;
            this.btnInstalar.Location = new System.Drawing.Point(175, 198);
            this.btnInstalar.Name = "btnInstalar";
            this.btnInstalar.Size = new System.Drawing.Size(260, 32);
            this.btnInstalar.TabIndex = 2;
            this.btnInstalar.Text = "Instalar";
            this.btnInstalar.UseVisualStyleBackColor = true;
            this.btnInstalar.Click += new System.EventHandler(this.btnInstalar_Click);
            // 
            // barraProgreso
            // 
            this.barraProgreso.Location = new System.Drawing.Point(65, 141);
            this.barraProgreso.Name = "barraProgreso";
            this.barraProgreso.Size = new System.Drawing.Size(506, 31);
            this.barraProgreso.TabIndex = 3;
            this.barraProgreso.Visible = false;
            // 
            // bwEjecutar
            // 
            this.bwEjecutar.WorkerReportsProgress = true;
            this.bwEjecutar.WorkerSupportsCancellation = true;
            this.bwEjecutar.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwEjecutar_DoWork);
            this.bwEjecutar.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwEjecutar_ProgressChanged);
            this.bwEjecutar.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwEjecutar_RunWorkerCompleted);
            // 
            // lbMensaje
            // 
            this.lbMensaje.AutoSize = true;
            this.lbMensaje.Location = new System.Drawing.Point(62, 121);
            this.lbMensaje.Name = "lbMensaje";
            this.lbMensaje.Size = new System.Drawing.Size(147, 17);
            this.lbMensaje.TabIndex = 4;
            this.lbMensaje.Text = "Espere un momento...";
            this.lbMensaje.Visible = false;
            // 
            // frmInstalador
            // 
            this.AcceptButton = this.btnInstalar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 268);
            this.Controls.Add(this.lbMensaje);
            this.Controls.Add(this.barraProgreso);
            this.Controls.Add(this.btnInstalar);
            this.Controls.Add(this.cbSucursales);
            this.Controls.Add(this.lbSucursal);
            this.Name = "frmInstalador";
            this.Text = "instalador Master";
            this.Load += new System.EventHandler(this.frmInstalador_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSucursal;
        private System.Windows.Forms.ComboBox cbSucursales;
        private System.Windows.Forms.Button btnInstalar;
        private System.Windows.Forms.ProgressBar barraProgreso;
        private System.ComponentModel.BackgroundWorker bwEjecutar;
        private System.Windows.Forms.Label lbMensaje;
    }
}

