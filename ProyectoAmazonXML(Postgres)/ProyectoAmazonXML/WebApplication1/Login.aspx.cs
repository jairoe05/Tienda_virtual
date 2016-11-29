using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using Npgsql;
using Mono.Security;
using NpgsqlTypes;
using System.Data;
namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {
        conexion con = new conexion();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (ComprobarDatos())
            {
                Response.Redirect("~/MenuUsuario.aspx");
            }
        }
        private bool ComprobarDatos()
        {
            con.conectarPOSTGRE();
            bool entrar = false;
            string rol = "";
            string nomb = "";
            try
            {
                string query = "select Cedula, nombre +' ' + Apellidos as Cliente from Usuario where cedula =" + txtCedula.Text + " and Pass = '" + txtContraseña.Text + "'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                NpgsqlDataReader lee = cmd.ExecuteReader();
                
                if (lee.Read())
                {
                    rol = "" + lee["Cedula"];
                    nomb = "" + lee["Cliente"];
                    entrar = true;
                    InsertarLogin(nomb);
                }
                else
                {
                    entrar = false;
                    con.MensajeNormal("Datos incorrectos! \nIntentelo de nuevo",Label1);
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                entrar = false;
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
            return entrar;
        }
        private void InsertarLogin(string nombre)
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "insert into login (cedula, nombre) values(" + txtCedula.Text + ", '" + nombre + "')";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        //private void InsertarLogin(string nombre)
        //{
        //    con.conectarPOSTGRE();
        //    try
        //    {
        //        string query = "insert into login (cedula, nombre) values(" + txtCedula.Text + ", '" + nombre + "')";
        //        OracleCommand cmd = new OracleCommand(query, con.conex);
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception Ex)
        //    {
        //        con.MensajeError(Ex, Label1);
        //    }
        //    finally { con.desconectarPOSTGRE(); }
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "procesoPrueba";
                NpgsqlCommand cmd4 = new NpgsqlCommand();
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.CommandText = "procesoPrueba";
                cmd4.Connection = con.Postgrecon;
                NpgsqlParameter param1 = new NpgsqlParameter();
                NpgsqlParameter param2 = new NpgsqlParameter();
                NpgsqlParameter param3 = new NpgsqlParameter();
                param1.NpgsqlDbType = NpgsqlDbType.Char;
                param1.Size = 10;
                param1.ParameterName = "num";
                param1.Value = "9999col";
                cmd4.Parameters.Add(param1);

                param2.NpgsqlDbType = NpgsqlDbType.Integer;                
                param2.Value = 45;
                param1.ParameterName = "ced";
                cmd4.Parameters.Add(param2);

                param3.NpgsqlDbType = NpgsqlDbType.Char;
                param3.Size = 50;
                param1.ParameterName = "det";
                param3.Value = "sisisisisisggg";
                cmd4.Parameters.Add(param3);


                cmd4.ExecuteNonQuery();
                con.MensajeNormal("se inserto correctamente",Label2);
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
        }
    }
}