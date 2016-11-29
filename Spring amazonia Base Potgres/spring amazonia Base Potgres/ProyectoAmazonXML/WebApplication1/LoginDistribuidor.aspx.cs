using System;
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;
namespace WebApplication1
{
    public partial class LoginDistribuidor : System.Web.UI.Page
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        IDistribuidor distri = (IDistribuidor)context.GetObject("Distribuidor");  
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (distri.Entrar(TextBox1.Text,TextBox2.Text,Label1))
            {
                Response.Redirect("~/MenuDistribuidor.aspx");
            }
            else { objconexion.MensajeNormal("No se pudo acceder, Datos incorrectos", Label1); }
        }        
    }
}