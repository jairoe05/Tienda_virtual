using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Npgsql;
using Mono.Security;
using NpgsqlTypes;
using System.Data;
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;
namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {

        static IApplicationContext context = ContextRegistry.GetContext();
        IUsuario user = (IUsuario)context.GetObject("Usuario");   
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnIngresar_Click(object sender, EventArgs e)
        {           
            if (user.ComprobarDatos(txtCedula.Text, txtContraseña.Text, Label1))
            {
                Response.Redirect("~/MenuUsuario.aspx?Cedula=" + txtCedula.Text);
            }
        }
    }
}
