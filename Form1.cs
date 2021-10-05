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
            vEjecutar();
        }

        private void vEjecutar()
        {
            btnInstalar.Enabled = false;
            progBar1.Visible = true;
            progBar1.Value = 10;

            Ctrl = new clsControlador(dt);

            Ctrl.vCrearDataBase();
            progBar1.Value = 20;

            Ctrl.vEjecutarScripts(1);
            progBar1.Value = 45;

            Ctrl.vInsertarTablas();
            progBar1.Value = 70;

            Ctrl.vEjecutarScripts(2);
            progBar1.Value = 90;

            Ctrl.vGuardarLlaves();
            progBar1.Value = 100;

            cbSucursales.Text = "Sucursal";
            MessageBox.Show("Creacion Completa...", "Listo..");
        }

        private void cbSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbSucursales.SelectedIndex > 0)
            {
                btnInstalar.Enabled = true;

                vConsulta(cbSucursales.SelectedItem.ToString());
            }
            else
            {
                btnInstalar.Enabled = false;
            }
        }

        private void tmrConteo_Tick(object sender, EventArgs e)
        {
            if (progBar1.Value < 100)
            {
                progBar1.Value++;
            }
            else
            {
                progBar1.Value = 0;
            }
        }
    }
}
