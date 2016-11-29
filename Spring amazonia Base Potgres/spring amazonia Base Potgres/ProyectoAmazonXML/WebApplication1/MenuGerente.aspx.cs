using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using Oracle.DataAccess.Client;
using System.IO;
using Npgsql;
using Mono.Security;
using NpgsqlTypes;
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;
namespace WebApplication1
{
    public partial class MenuGerente : System.Web.UI.Page
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        IAdministrador admin = (IAdministrador)context.GetObject("Administrador");
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnVerInventario_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "Bienvenido")
            {
                lblGrid.Text = "Inventario";
                admin.leeYcargaGridProductos(GridView1,Label1);
                GridView2.DataSource = null;
                GridView2.DataBind();
                Image1.Visible = false;
            }
            else { objconexion.MensajeNormal("Debe Loggearse!", Label2); }
        }
        protected void btnVerOrdenes_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "Bienvenido")
            {
                admin.MostrarOrdenes(GridView1,lblGrid,Label2);
                GridView2.DataSource = null;
                GridView2.DataBind();
                Image1.Visible = false;
            }
            else { objconexion.MensajeNormal("Debe Loggearse!", Label2); }
        }
        protected void btnVerVentas_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "Bienvenido")
            {
                if (txtFecha.Text == "" || txtFecha2.Text == "")
                {
                    objconexion.MensajeNormal("Debe indicar los 2 parametros de fecha!", Label2);
                }
                else
                {
                    try
                    {
                        admin.VerMisFacturas(txtFecha.Text, txtFecha2.Text,GridView1,lblGrid,Label2);
                        Image1.Visible = false;
                        GridView2.DataSource = null;
                        GridView2.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        objconexion.MensajeError(Ex, Label2);
                    }
                }
            }
            else { objconexion.MensajeNormal("Debe Loggearse!", Label2); }
        }     
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblGrid.Text == "Ventas")
            {
                admin.leeYCargaDetalleFactura(GridView1.SelectedRow.Cells[1].Text,GridView2,Label2);
            }
            if (lblGrid.Text == "Inventario")
            {
                Image1.Visible = true;
                admin.leeYcargaImagen(GridView1.SelectedRow.Cells[1].Text,Image1,Label2);
            }
            if (lblGrid.Text == "Ordenes de Compra")
            {
                admin.leeYCargaDetalleOrden(GridView1.SelectedRow.Cells[1].Text,GridView2,Label2);
            }
        }
        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text =="1111" && TextBox2.Text =="123")
            {
                objconexion.MensajeNormal("Bienvenido Gerente!", Label2);
                Label1.Text = "Bienvenido";
            }
        }

        protected void btnMantenimientos_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "Bienvenido")
            {
                Response.Redirect("~/Mantenimientos.aspx");
            }
            else { objconexion.MensajeNormal("Debe Loggearse!", Label2); }
        }
    }
}