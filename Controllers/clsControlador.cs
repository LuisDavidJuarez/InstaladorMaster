using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstaladorMaster.Models;
using Newtonsoft.Json;

namespace InstaladorMaster.Controllers
{
    class clsControlador
    {
        clsInstalador Instalador;
        clsData Data;
        string strCadena;
        private string strLigaScripts = @"C:\Users\ljuarez\Documents\GitHub\InstaladorMaster\InstaladorMaster\scripts\";
        DataTable dt, dtSucursal;

        string[] strTablasAntes = { "[Opesys].[REPLICAS].[Art]",
                                        "[Opesys].[dbo].[ConfiguracionGateway]",
                                        "[Opesys].[REPLICAS].[CB]",
                                        "[Opesys].[REPLICAS].[Politica]",
                                        "[Opesys].[REPLICAS].[Precio]",
                                        "[Opesys].[REPLICAS].[PrecioD]",
                                        "[Opesys].[REPLICAS].[Sucursal]",
                                        "[Opesys].[REPLICAS].[ZonaImp]" };


        string[] strTablasDespues = { "[dbo].[Art]",
                                        "[dbo].[ConfiguracionGateway]",
                                        "[dbo].[CB]",
                                        "[dbo].[Politica]",
                                        "[dbo].[Precio]",
                                        "[dbo].[PrecioD]",
                                        "[dbo].[Sucursal]",
                                        "[dbo].[ZonaImp]" };

        public clsControlador(DataTable dtSeleccionada)
        {
            dtSucursal = dtSeleccionada;

            Data = new clsData();
        }

        private void vGeneraInstalador()
        {
            foreach (DataRow dr in dtSucursal.Rows)
            {
                Instalador = new clsInstalador(dr["Base"].ToString() + "-TEST", dr["IP"].ToString(), dr["Usuario"].ToString(), dr["Password"].ToString());
            }
        }

        public void vCrearDataBase()
        {
            string strDataBase = "";
            foreach (DataRow dr in dtSucursal.Rows)
            {
                strDataBase = dr["Base"].ToString() + "-TEST";
                Instalador = new clsInstalador("master", dr["IP"].ToString(), dr["Usuario"].ToString(), dr["Password"].ToString());
            }
            strCadena = "IF db_id('" + strDataBase + "') IS NULL  " +
                "CREATE DATABASE[" + strDataBase + "];";

            Instalador.vEjecutar(strCadena);
        }

        public void vEjecutarScripts(int iScript)
        {
            if (iScript == 1)
                vEjecutarScriptTablas();
            else
            {
                vEjecutarScrips();
            }
        }

        private void vEjecutarScriptTablas()
        {
            vGeneraInstalador();
            Instalador.vEjecutar(strObtenerScript("tablas.txt"));
        }

        private void vEjecutarScrips()
        {
            string[] strArchivos = { "fnListaDescuentos.txt",
                                     "xpBusquedaSugeridos.txt",
                                     "xpBusquedaArticulo.txt",
                                     "sp_Sincronizacion.txt",
                                     "sp_CreateTempTable.txt",
                                     "xpBusquedaAutocompletar.txt",
                                     "sp_GetSucursal.txt",
                                     "xpConsultaMostradorArticulo.txt"};

            foreach (string strArchivo in strArchivos)
            {
                Instalador.vEjecutar(strObtenerScript(strArchivo));
            }
        }

        public void vInsertarTablas()
        {
            string[] strCampos;
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    if(bInsertar(strTablasDespues[i]))
                    {
                        strCadena = "Select * from " + strTablasAntes[i] + ";";
                        dt = Data.dtConsulta(strCadena);

                        strCampos = strObtenerColumnas(dt, strTablasDespues[i]);
                        Instalador.vInsertarBulk(strCampos, dt);

                        strCadena = "Insert into [dbo].[SincronizacionFechaTabla] (Tabla, UltimaSincronizacion)" +
                            "values ('" + strLimpiarBD(strTablasDespues[i]) + "', CURRENT_TIMESTAMP)";
                        Instalador.vEjecutar(strCadena);
                    }
                }
            }
            catch (Exception ex)
            {
                string strerror = ex.Message;
            }
        }

        private string strLimpiarBD(string strBD)
        {
            strBD = strBD.Replace("dbo", "");
            strBD = strBD.Replace("[", "");
            strBD = strBD.Replace("]", "");
            strBD = strBD.Replace(".", "");

            return strBD;
        }

        private string strObtenerScript(string strArchivo)
        {
            return strLeerArchivo(strLigaScripts + strArchivo);
        }

        private bool bInsertar(string strTablaNueva)
        {
            bool bResultado = true;

            if (iContador(strTablaNueva) > 0)
                bResultado = false;

            return bResultado;
        }

        private int iContador(string strTabla)
        {
            int iresp = 0;
            strCadena = "Select Count(*) from " + strTabla + ";";
            try
            {
                iresp = Int32.Parse(Instalador.strConsulta(strCadena));
            }
            catch (Exception ex)
            {
                string strerror = ex.Message;
            }

            return iresp;
        }

        private string strLeerArchivo(string strPath)
        {
            return System.IO.File.ReadAllText(strPath); ;
        }

        private string[] strObtenerColumnas(DataTable dt, string strTabla)
        {
            List<string> lstResultado = new List<string>();

            lstResultado.Add(strTabla);
            foreach (DataColumn dc in dt.Columns)
            {
                lstResultado.Add(dc.ColumnName);
            }

            return lstResultado.ToArray();
        }

        public string DataTableToJsonWithJSONNet()
        {
            //string JSONString = string.Empty;
            string JSONString = JsonConvert.SerializeObject(dtSucursal);
            return JSONString;
        }

        public void vGuardarLlaves()
        {
            string strPath = @"C:\CotizadorWeb";
            try
            {
                Directory.CreateDirectory(strPath);
                strPath += @"\Llaves.json";
                //File.Create(strPath);

                using (StreamWriter write = new StreamWriter(strPath, true, Encoding.UTF8))
                {
                    write.WriteLine(DataTableToJsonWithJSONNet());
                    write.Close();
                }

                //using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text files|*.txt" })
                //{
                //    if (sfd.ShowDialog() == DialogResult.OK)
                //    {
                //        using (StreamWriter write = new StreamWriter(sfd.FileName, true, Encoding.UTF8))
                //        {
                //            write.WriteLine(textBox1.Text);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
            }
        }
    }
}