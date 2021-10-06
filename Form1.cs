using InstaladorMaster.Models;
using InstaladorMaster.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace InstaladorMaster
{
    public partial class frmInstalador : Form
    {
        clsData Data;
        clsControlador Ctrl;
        DataTable dt;
        string strCadena;

        public frmInstalador()
        {
            InitializeComponent();
            vLLenarComboBox();
        }

        private void vLLenarComboBox()
        {
            Data = new clsData();
            vConsulta("All");

            foreach (DataRow dr in dt.Rows)
            {
                cbSucursales.Items.Add(dr["Sucursal"].ToString());
            }
        }

        private void vConsulta (string strCampo)
        {
            if(strCampo == "All")
            {
                strCadena = "select ID, Sucursal, IP, Base, Usuario, Password from [REPLICAS].[SucursalTransferencia]";
            }
            else
            {
                strCadena = "select ID, Sucursal, IP, Base, Usuario, Password from [REPLICAS].[SucursalTransferencia] where Sucursal=" + strCampo;
            }
            dt = Data.dtConsulta(strCadena);
        }

        private void btnInstalar_Click(object sender, EventArgs e)
        {
            btnInstalar.Enabled = false;
            barraProgreso.Visible = true;
            lbMensaje.Visible = true;
            bwEjecutar.RunWorkerAsync();
        }

        private void vEjecutar()
        {

            Ctrl = new clsControlador(dt);

            Ctrl.vCrearDataBase();

            Ctrl.vEjecutarScripts(1);

            Ctrl.vInsertarTablas();

            Ctrl.vEjecutarScripts(2);

            Ctrl.vGuardarLlaves();

        }

        private void cbSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbSucursales.SelectedIndex > 0)
            {
                btnInstalar.Enabled = true;
                barraProgreso.Visible = false;
                lbMensaje.Visible = false;

                vConsulta(cbSucursales.SelectedItem.ToString());
            }
            else
            {
                btnInstalar.Enabled = false;
            }
        }

        private void bwEjecutar_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Ctrl = new clsControlador(dt);

                Ctrl.vGuardarLlaves();//5%

                int iProgress = 0;
                string mensaje = "Creando Base de Datos...";
                Thread.Sleep(2000);
                for (int i = 1; i <= 5; i++)
                {
                    switch(i)
                    {
                        case 1:
                            Ctrl.vCrearDataBase();//5%
                            mensaje = "Base de Datos Creada, Se crearan Tablas...";
                            iProgress = 5;
                            break;

                        case 2:
                            Ctrl.vEjecutarScripts(1);//10%
                            mensaje = "Tablas Creadas, se Insertaran Datos...";
                            iProgress = 10;
                            break;

                        case 3:
                            Ctrl.vInsertarTablas();//70%
                            mensaje = "Datos Insertados, se Crearan funciones y Procedimientos...";
                            iProgress = 70;
                            break;

                        case 4:
                            Ctrl.vEjecutarScripts(2);//10%
                            mensaje = "Funciones y Procedimientos Creadas, Generando Llaves de Acceso...";
                            iProgress = 10;
                            break;

                        case 5:
                            Ctrl.vEjecutarScripts(2);//10%
                            mensaje = "Funciones y Procedimientos Creadas, Generando Llaves de Acceso...";
                            iProgress = 10;
                            break;
                    }

                    Thread.Sleep(2000);
                    ((BackgroundWorker)sender).ReportProgress((int)(i * 100d/ 5));
                    this.Invoke((MethodInvoker)(() => setMessage(mensaje)));
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("bwEjecutar_DoWork():Error" + exp.Message, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.bwEjecutar.CancelAsync();
            }
        }

        private void setMessage(string strMensaje)
        {
            lbMensaje.Text = strMensaje;
        }

        private void bwEjecutar_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            setMessage("Instalacion Exitosa...");
            cbSucursales.Text = "Sucursal";
            MessageBox.Show("Creacion Completa...", "Listo..");
        }

        private void bwEjecutar_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            barraProgreso.Value = e.ProgressPercentage;
        }

        private void frmInstalador_Load(object sender, EventArgs e)
        {
            barraProgreso.Style = ProgressBarStyle.Continuous;
            //
        }
    }
}
