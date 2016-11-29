using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;
using System.IO;
using System.Xml;
using Npgsql;
using Mono.Security;
using NpgsqlTypes;
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;

namespace WebApplication1
{
    public partial class MenuDistribuidor : System.Web.UI.Page
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        IDistribuidor distri = (IDistribuidor)context.GetObject("Distribuidor");
        conexion con = new conexion();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnVerInventario_Click(object sender, EventArgs e)
        {
            lblTituloGrid1.Text = "Inventario";
            distri.leeYcargaGridProductos(GridView1,Label2);
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
        }
        protected void btnVerOrdenes_Click(object sender, EventArgs e)
        {            
            distri.MostrarOrdenes(GridView1,Label2);
            GridView2.DataSource = null;
            GridView2.DataBind();
            lblTituloGrid1.Text = "Ordenes de Compra";
            Image1.Visible = false;
        }
        protected void btnSuministrar_Click(object sender, EventArgs e)
        {
            if (lblTituloGrid1.Text == "Ordenes de Compra" && deGrid1(1) != ".")
            {
                if (distri.ComprobarCantidadesSuministrar(deGrid1(1),Label2))
                {     
                    distri.ActualizarStocksDistri(deGrid1(1),Label2);                    
                    distri.MostrarOrdenes(GridView1, Label2);
                    Image1.Visible = false;
                }
            }
            else { con.MensajeNormal("Debe seleccionar una Orden", Label2); }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblTituloGrid1.Text == "Inventario")
            {
                Image1.Visible = true;
                distri.leeYcargaImagen(deGrid1(1),Image1,Label2);
            }
            if (lblTituloGrid1.Text == "Ordenes de Compra")
            {
                distri.leeYCargaDetalleOrden(deGrid1(1),GridView2,Label2);
                lblTituloGrid2.Text = "Detalle de la Orden";
            }
        }
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
        } 
        private string deGrid1(int num)
        {
            try
            {
                return GridView1.SelectedRow.Cells[num].Text;
            }
            catch (Exception)
            {
                return ".";
            }
        }       
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LoginDistribuidor.aspx");
        }
    }
}