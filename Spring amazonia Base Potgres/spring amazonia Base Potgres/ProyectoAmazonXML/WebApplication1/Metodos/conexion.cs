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
namespace WebApplication1.Metodos
{
    public class conexion : Iconexion
    {
        public static string cadena = "Data Source=JAV13R5;Persist Security Info= true;User ID=basexmlamazon;Password=123456";
        
        //static string cadenasql = "Data Source=JAV13R5;Initial Catalog=BasePython;Persist Security Info=True;User ID=sa;Password=123456.";
        static string cadenasql = @"Data Source=JAV13R5\JAVIER;Initial Catalog=BasePython;Integrated Security=True";
        
        // POSTGRE
        static string strconexionconfig = System.Configuration.ConfigurationManager.ConnectionStrings["AmazonConex"].ConnectionString;
        static string connstring = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=baseAmazon;";
        public NpgsqlConnection Postgrecon = new NpgsqlConnection(strconexionconfig);
        public string getCadenaPostgre()
        {
            return strconexionconfig;
        }        
        //conexion POSTGRE***********************
        public NpgsqlConnection conectarPOSTGRE()
        {
            if (Postgrecon.State == ConnectionState.Closed) { Postgrecon.Open(); }
            return Postgrecon;
        }
        public void desconectarPOSTGRE()
        {
            if (Postgrecon.State == ConnectionState.Open) { Postgrecon.Close(); }
        }                
        public void MensajeError(Exception Ex, Label lbl)
        {
            lbl.Text = "Error: " + Ex.Message;            
        }
        public void MensajeNormal(string msj, Label lbl)
        {
            lbl.Text ="Mensaje: " + msj;
        }
    }
}