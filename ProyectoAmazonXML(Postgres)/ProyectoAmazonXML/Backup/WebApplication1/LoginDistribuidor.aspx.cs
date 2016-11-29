using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
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
            else { con.MensajeNormal("No se pudo acceder, Datos incorrectos"); }
        }
        private bool Entrar()
        {
            con.conectarSqlS();
            bool resp = false;
            try
            {
                string query = "select Nombre, Ubicacion from Distribuidor where Correo = '" + TextBox1.Text + "' and Cod_distribuidor = " + TextBox2.Text + "";
                SqlCommand cmd = new SqlCommand(query, con.consql);
                SqlDataReader lee = cmd.ExecuteReader();
                string nomb = "";
                string ubi = "";
                if (lee.Read())
                {
                    nomb = "" + lee["Nombre"];
                    ubi = "" + lee["Ubicacion"];
                    con.MensajeNormal("Bienvenido:  "+nomb+" de "+ubi);
                    lee.Close();
                    resp = true;
                }
                else { resp = false; }
            }
            catch (Exception EX)
            {
                resp = false;
                con.MensajeError(EX);
            }
            finally { con.desconectarSqlS(); }
            return resp;
        }
    }
}