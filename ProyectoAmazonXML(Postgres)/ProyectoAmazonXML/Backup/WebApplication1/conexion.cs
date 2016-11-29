using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1
{
    public class conexion
    {
        public static string cadena = "Data Source=JAV13R5;Persist Security Info= true;User ID=basexmlamazon;Password=123456;";
        public OracleConnection conex = new OracleConnection(cadena);
        static string cadenasql = "Data Source=JAV13R5\\JAVIER;Initial Catalog=BasePython;Integrated Security=true";
        public SqlConnection consql = new SqlConnection(cadenasql);
        public string path = @"C:\Users\Javier\Documents\PROGRAMACION CUC\BASE DE DATOS ADECIO\AmazonXML\";
        public void conectar()
        {
            if (conex.State == ConnectionState.Closed) { conex.Open(); } 
        }
        public void desconectar()
        {
            if (conex.State == ConnectionState.Open) { conex.Close(); }
        }
        public void conectarSqlS()
        {
            if (consql.State == ConnectionState.Closed) { consql.Open(); } 
        }
        public void desconectarSqlS()
        {
            if (consql.State == ConnectionState.Open) { consql.Close(); }
        }
        public void MensajeError(Exception Ex)
        {
            System.Windows.Forms.MessageBox.Show("Error: "+Ex.Message,"   Error   ");
        }
        public void EjecutarQuery(string query)
        {
            conectar();
            try
            {
                OracleCommand cmd = new OracleCommand(query, conex);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                MensajeError(Ex);
            }
            finally { desconectar(); }
        }
        public void MensajeNormal(string msj)
        {
            System.Windows.Forms.MessageBox.Show("Mensaje: " + msj, "   Mensaje   ");
        }

    }
}