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
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;
namespace WebApplication1
{
    public partial class LoginVendedor : System.Web.UI.Page
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        IVendedor vendedor = (IVendedor)context.GetObject("Vendedor");  
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (vendedor.ComprobarDatos(txtCodigo.Text,txtPass.Text,Label1))
            {
                Response.Redirect("~/MenuVendedor.aspx?Codigo=" + txtCodigo.Text);
            }
        }        
    }
}