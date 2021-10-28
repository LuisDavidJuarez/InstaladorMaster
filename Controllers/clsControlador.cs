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
        string strCadena, strRespuesta, strScript;
        string strIp = "";
        //private string strDirectorioScripts = @"C:\Users\ljuarez\Documents\GitHub\InstaladorMaster\InstaladorMaster\scripts\";
        private string strDirectorioScripts = @"\\192.168.13.30\c$\Instalador\Scripts\";
        DataTable dt, dtSucursal, dtTablas;

        public clsControlador(DataTable dtSeleccionada)
        {
            dtSucursal = dtSeleccionada;

            Data = new clsData();
        }

        private void vGeneraInstalador()
        {
            foreach (DataRow dr in dtSucursal.Rows)
            {
                strIp = dr["IP"].ToString();
                Instalador = new clsInstalador(dr["Base"].ToString(), strIp, dr["Usuario"].ToString(), dr["Password"].ToString());

                //strIp = "192.168.13.158";
                //Instalador = new clsInstalador("COTL-23-TEST", strIp, "sa", "123");
            }
        }

        public void vCrearDataBase()
        {
            string strDataBase = "";
            foreach (DataRow dr in dtSucursal.Rows)
            {
                strIp = dr["IP"].ToString();
                strDataBase = dr["Base"].ToString();
                Instalador = new clsInstalador("master", strIp, dr["Usuario"].ToString(), dr["Password"].ToString());
            }

            //strIp = "192.168.13.158";
            //strDataBase = "COTL-23";
            //Instalador = new clsInstalador("master", strIp, "sa", "123");

            strCadena = "IF db_id('" + strDataBase + "') IS NULL  " +
                "CREATE DATABASE[" + strDataBase + "];";

            Instalador.vEjecutar(strCadena);
            vGeneraInstalador();
        }

        public void vEjecutarScripts()
        {
            vEjecutarScrips();
            //if (iScript == 1)
            //    vEjecutarScriptTablas();
            //else
            //{
            //}
        }

        private void vEjecutarScriptTablas()
        {
            Instalador.vEjecutar(strObtenerScript("tablas.txt"));
        }

        private void vEjecutarScrips()
        {
            DirectoryInfo d = new DirectoryInfo(strDirectorioScripts);
            foreach (var file in d.GetFiles("*.txt"))
            {
                Instalador.vEjecutar(strObtenerScript(file.FullName));
            }
        }

        public void vInsertarTablas()
        {
            strCadena = "Select * from [Opesys].[REPLICAS].[ConfiguracionTablas] " +
                "WHERE Instalacion = 1";
            dtTablas = Data.dtConsulta(strCadena);

            string[] strCampos;
            string strTablaAntes, strTablaDespues;
            try
            {
                foreach (DataRow drow in dtTablas.Rows)
                {
                    if(drow["Tabla"].ToString() != "SincronizacionFechaTabla")
                    {
                        strTablaAntes = drow["Esquema"].ToString() + "." + drow["Tabla"].ToString();
                        strTablaDespues = "dbo." + drow["Tabla"].ToString();
                        if (bInsertar(strTablaDespues))
                        {
                            strCadena = "Select * from " + strTablaAntes + ";";
                            dt = dtLimpia(Data.dtConsulta(strCadena));

                            strCadena = "Select UltimaSincronizacion FROM [Opesys].[REPLICAS].[SincronizacionFechaTabla] where Tabla = '" +
                            strLimpiarBD(strTablaDespues) + "';";
                            strRespuesta = Data.strConsulta(strCadena);

                            strCampos = strObtenerColumnas(dt, strTablaDespues);
                            Instalador.vInsertarBulk(strCampos, dt);

                            strCadena = "Insert into [dbo].[SincronizacionFechaTabla] (Tabla, UltimaSincronizacion)" +
                                "values ('" + strLimpiarBD(strTablaDespues) + "', '" + strRespuesta + "')";
                            Instalador.vEjecutar(strCadena);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string strerror = ex.Message;
            }
        }

        public void vGeneraTablas()
        {
            strCadena = "Select * from [Opesys].[REPLICAS].[ConfiguracionTablas] " +
                "WHERE Instalacion = 1";
            dtTablas = Data.dtConsulta(strCadena);
            string strTablaDespues;

            foreach (DataRow drow in dtTablas.Rows)
            {
                strTablaDespues = "dbo." + drow["Tabla"].ToString();
                Instalador.vEjecutar(vGeneraScriptTabla(strLimpiarBD(strTablaDespues)));
            }
        }

        public string vGeneraScriptTabla(string strTabla)
        {
            int iContador = 1;
            strScript = "IF OBJECT_ID('" + strTabla + "', 'U') IS NOT NULL \n";
            strScript += "DROP TABLE " + strTabla + " \n\n";

            strCadena = "EXEC REPLICAS.xpDatosTabla 'REPLICAS', '" + strTabla + "'";
            dt = Data.dtConsulta(strCadena);

            strScript += "CREATE TABLE " + strTabla + "(\n";
           
            foreach(DataRow dr in dt.Rows)
            {
                strScript += dr["CAMPO"].ToString();
                if(iContador != dt.Rows.Count)
                    strScript += ",";
                strScript += "\n";
                iContador++;
            }


            strScript += ");";

            return strScript;
        }

        private DataTable dtLimpia(DataTable dt)
        {
            if (dt.Columns.Contains("FechaAlta"))
                dt.Columns.Remove("FechaAlta");

            if (dt.Columns.Contains("FechaUltMod"))
                dt.Columns.Remove("FechaUltMod");

            return dt;
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
            return strLeerArchivo(strArchivo);
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
            string strPath = @"\\"+ strIp + @"\c$\CotizadorWeb";
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