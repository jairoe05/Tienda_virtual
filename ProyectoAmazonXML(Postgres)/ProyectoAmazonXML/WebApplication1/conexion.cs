using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Npgsql;
using Mono.Security;
namespace WebApplication1
{
    public class conexion
    {
        public static string cadena = "Data Source=JAV13R5;Persist Security Info= true;User ID=basexmlamazon;Password=123456";
        public OracleConnection conex = new OracleConnection(cadena);
        
        //static string cadenasql = "Data Source=JAV13R5;Initial Catalog=BasePython;Persist Security Info=True;User ID=sa;Password=123456.";
        static string cadenasql = @"Data Source=JAV13R5\JAVIER;Initial Catalog=BasePython;Integrated Security=True";
        public SqlConnection consql = new SqlConnection(cadenasql);
        //public string path = @"C:\Users\Administrador\Documents\AmazoniaXml\";


        // POSTGRE
        static string connstring = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=baseAmazon;";
        public NpgsqlConnection Postgrecon = new NpgsqlConnection(connstring);

        public void conectar()
        {
            if (conex.State == ConnectionState.Closed) { conex.Open(); } 
        }
        public void desconectar()
        {
            if (conex.State == ConnectionState.Open) { conex.Close(); }
        }
        //conexion POSTGRE***********************
        public void conectarPOSTGRE()
        {
            if (Postgrecon.State == ConnectionState.Closed) { Postgrecon.Open(); }
        }
        public void desconectarPOSTGRE()
        {
            if (Postgrecon.State == ConnectionState.Open) { Postgrecon.Close(); }
        }

        public void conectarSqlS()
        {
            if (consql.State == ConnectionState.Closed) { consql.Open(); } 
        }
        public void desconectarSqlS()
        {
            if (consql.State == ConnectionState.Open) { consql.Close(); }
        }
        public void MensajeError(Exception Ex, Label lbl)
        {
            //Response.Write("<script>alert('"+Ex+"');</script>");
            lbl.Text = "Error: " + Ex.Message;
            
        }
        public void EjecutarQuery(string query)
        {
            conectarPOSTGRE();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, Postgrecon);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                //MensajeError(Ex);
            }
            finally { desconectarPOSTGRE(); }
        }
        public void MensajeNormal(string msj, Label lbl)
        {
            lbl.Text ="Mensaje: " + msj;
        }

    }
}