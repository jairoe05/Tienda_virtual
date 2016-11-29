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
    public partial class LoginDistribuidor : System.Web.UI.Page
    {
        conexion con = new conexion();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (Entrar())
            {
                Response.Redirect("~/MenuDistribuidor.aspx");
            }
            else { con.MensajeNormal("No se pudo acceder, Datos incorrectos", Label1); }
        }
        private bool Entrar()
        {
            con.conectarPOSTGRE();
            bool resp = false;
            try
            {
                string query = "select Nombre, Ubicacion from Distribuidor where Correo = '" + TextBox1.Text + "' and Cod_distribuidor = " + TextBox2.Text + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                NpgsqlDataReader lee = cmd.ExecuteReader();
                string nomb = "";
                string ubi = "";
                if (lee.Read())
                {
                    nomb = "" + lee["Nombre"];
                    ubi = "" + lee["Ubicacion"];
                    con.MensajeNormal("Bienvenido:  " + nomb + " de " + ubi, Label1);
                    lee.Close();
                    resp = true;
                }
                else { resp = false; }
            }
            catch (Exception EX)
            {
                resp = false;
                con.MensajeError(EX, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
            return resp;
        }
    }
}