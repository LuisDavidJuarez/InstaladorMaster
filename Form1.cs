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
            Ctrl = new clsControlador(dt);
            Ctrl.vInstalar();

            MessageBox.Show("Creacion Completa...", "Listo..");
        }

        private void cbSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbSucursales.SelectedIndex > 0)
            {
                vConsulta(cbSucursales.SelectedItem.ToString());
                btnInstalar.Enabled = true;
            }
            else
            {
                btnInstalar.Enabled = false;
            }
        }
    }
}
