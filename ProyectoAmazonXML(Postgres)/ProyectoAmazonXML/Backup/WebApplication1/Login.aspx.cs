using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;

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
            con.conectarSqlS();
            bool entrar = false;
            string rol = "";
            string nomb = "";
            try
            {
                string query = "select Cedula, nombre +' ' + Apellidos as Cliente from Usuario where cedula =" + txtCedula.Text + " and Pass = '" + txtContraseña.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con.consql);
                SqlDataReader lee = cmd.ExecuteReader();

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
                    con.MensajeNormal("Datos incorrectos! \nIntentelo de nuevo");
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                entrar = false;
                con.MensajeError(Ex);
            }
            finally { con.desconectarSqlS(); }
            return entrar;
        }
        private void InsertarLogin(string nombre)
        {
            con.conectar();
            try
            {
                string query = "insert into login (cedula, nombre) values(" + txtCedula.Text + ", '" + nombre + "')";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
    }
}