using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InstaladorMaster.Models
{
    public class clsData
    {
        string strConex;

        public clsData()
        {
            strConex = @"Data Source=192.168.13.16;Initial Catalog=Opesys;Persist Security Info=True;User ID=sa;Password=S1ST3M45AD";
        }

        private SqlConnection GetConnection()
        {
            SqlConnection sqlConex = new SqlConnection(strConex);
            return sqlConex;
        }

        public DataTable dtConsulta(string strConsulta)
        {
            DataSet datos = new DataSet();
            using (SqlConnection sqlConex = GetConnection())
            {
                try
                {
                    if (sqlConex.State != ConnectionState.Open)
                    { sqlConex.Open(); }

                    SqlDataAdapter dbConsulta = new SqlDataAdapter(strConsulta, sqlConex);
                    dbConsulta.Fill(datos, "S");
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;
                }
            }
            return datos.Tables["S"];
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
                    string strError = ex.Message;
                }
            }
            return strRespuesta;
        }
    }
}