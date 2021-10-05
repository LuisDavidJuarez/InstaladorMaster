using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InstaladorMaster.Models
{
    public class clsInstalador
    {
        public string strConexion;
        public bool bBDCreada = false;

        public clsInstalador(string strDataBase, string strIPAsignada, string strUser, string strPassword)
        {
            //Data Source=192.168.23.130;Initial Catalog=master;User ID=sa;Password=23
            strConexion = @"Data Source=" + strIPAsignada + ";Initial Catalog=" + strDataBase + ";" +
                "Persist Security Info=True;User ID="+ strUser + ";Password=" + strPassword + ";";
        }

        private SqlConnection GetConnection()
        {
            SqlConnection sqlConex = new SqlConnection(strConexion);
            return sqlConex;
        }

        //CREAR BASE DE DATOS Y TABLAS... 
        public void vEjecutar(string strSentencia)
        {
            using (SqlConnection sqlConex = GetConnection())
            {
                using (SqlCommand sqlCmd = new SqlCommand(strSentencia, sqlConex))
                {
                    try
                    {
                        if (sqlConex.State != ConnectionState.Open)
                        { sqlConex.Open(); }
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        sqlConex.Close();
                        string strError = ex.Message;
                    }
                }
            }
        }

        //INSERTAR BULKS
        public void vInsertarBulk(string[] strCampoBD, DataTable dt)
        {
            using (SqlConnection sqlConex = GetConnection())
            {
                using (SqlBulkCopy blk = new SqlBulkCopy(sqlConex))
                {
                    try
                    {
                        blk.DestinationTableName = strCampoBD[0];
                        blk.BulkCopyTimeout = 900;
                        blk.BatchSize = 1000;

                        for (int i = 1; i < strCampoBD.Length; i++)
                            blk.ColumnMappings.Add(strCampoBD[i], strCampoBD[i]);

                        if (sqlConex.State != ConnectionState.Open)
                        { sqlConex.Open(); }


                        blk.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        sqlConex.Close();
                        string strError = ex.Message;
                    }
                }
            }
        }

        public string strConsulta(string strConsulta)
        {
            string strRespuesta = "";
            using (SqlConnection sqlConex = GetConnection())
            {
                try
                {
                    if (sqlConex.State != ConnectionState.Open)
                    { sqlConex.Open(); }

                    using (SqlCommand cmd = new SqlCommand(strConsulta, sqlConex))
                    {
                        strRespuesta = cmd.ExecuteScalar().ToString();
                    }
                }
                catch (Exception ex)
                {
                    sqlConex.Close();
                    string strError = ex.Message;
                }
            }
            return strRespuesta;
        }
    }
}
