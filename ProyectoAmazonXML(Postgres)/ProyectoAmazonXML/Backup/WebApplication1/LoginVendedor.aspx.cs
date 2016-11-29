using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

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
            con.conectarSqlS();
            bool entrar = false;
            try
            {
                string query = "select nombre +' ' + Apellidos as Vendedor from Vendedor where Codigo =" + txtCodigo.Text + " and Pass = '" + txtPass.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con.consql);
                SqlDataReader lee = cmd.ExecuteReader();
                string vende = "";
                if (lee.Read())
                {
                    vende = "" + lee["Vendedor"];
                    entrar = true;
                    con.MensajeNormal("Bienvenido: "+vende);
                }
                else
                {                    
                    con.MensajeNormal("Datos incorrectos! \nIntentelo de nuevo");
                    entrar = false;
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectarSqlS(); }
            return entrar;
        }
    }
}