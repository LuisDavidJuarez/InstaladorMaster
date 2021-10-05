
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
            this.progBar1 = new System.Windows.Forms.ProgressBar();
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
            this.cbSucursales.FormattingEnabled = true;
            this.cbSucursales.Location = new System.Drawing.Point(293, 72);
            this.cbSucursales.Name = "cbSucursales";
            this.cbSucursales.Size = new System.Drawing.Size(278, 24);
            this.cbSucursales.TabIndex = 1;
            this.cbSucursales.Text = "Sucursal";
            this.cbSucursales.SelectedIndexChanged += new System.EventHandler(this.cbSucursales_SelectedIndexChanged);
            // 
            // btnInstalar
            // 
            this.btnInstalar.Enabled = false;
            this.btnInstalar.Location = new System.Drawing.Point(175, 181);
            this.btnInstalar.Name = "btnInstalar";
            this.btnInstalar.Size = new System.Drawing.Size(260, 32);
            this.btnInstalar.TabIndex = 2;
            this.btnInstalar.Text = "Instalar";
            this.btnInstalar.UseVisualStyleBackColor = true;
            this.btnInstalar.Click += new System.EventHandler(this.btnInstalar_Click);
            // 
            // progBar1
            // 
            this.progBar1.Location = new System.Drawing.Point(65, 124);
            this.progBar1.Name = "progBar1";
            this.progBar1.Size = new System.Drawing.Size(506, 31);
            this.progBar1.TabIndex = 3;
            this.progBar1.Visible = false;
            // 
            // frmInstalador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 268);
            this.Controls.Add(this.progBar1);
            this.Controls.Add(this.btnInstalar);
            this.Controls.Add(this.cbSucursales);
            this.Controls.Add(this.lbSucursal);
            this.Name = "frmInstalador";
            this.Text = "instalador Master";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSucursal;
        private System.Windows.Forms.ComboBox cbSucursales;
        private System.Windows.Forms.Button btnInstalar;
        private System.Windows.Forms.ProgressBar progBar1;
    }
}

