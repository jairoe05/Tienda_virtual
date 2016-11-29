using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Npgsql;
using Mono.Security;
using NpgsqlTypes;
using System.Data;
namespace WebApplication1
{
    public partial class LoginVendedor : System.Web.UI.Page
    {
        conexion con = new conexion();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (ComprobarDatos())
            {
                Response.Redirect("~/MenuVendedor.aspx");
            }
        }
        private bool ComprobarDatos()
        {
            con.conectarPOSTGRE();
            bool entrar = false;
            try
            {
                string query = "select nombre +' ' + Apellidos as Vendedor from Vendedor where Codigo =" + txtCodigo.Text + " and Pass = '" + txtPass.Text + "'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                NpgsqlDataReader lee = cmd.ExecuteReader();
                string vende = "";
                if (lee.Read())
                {
                    vende = "" + lee["Vendedor"];
                    entrar = true;
                    con.MensajeNormal("Bienvenido: " + vende, Label1);
                }
                else
                {
                    con.MensajeNormal("Datos incorrectos! \nIntentelo de nuevo", Label1);
                    entrar = false;
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
            return entrar;
        }
    }
}