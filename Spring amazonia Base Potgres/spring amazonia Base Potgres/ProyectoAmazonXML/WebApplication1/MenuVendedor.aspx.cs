using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using Oracle.DataAccess.Client;
using System.IO;
using System.Data.SqlClient;
using Npgsql;
using Mono.Security;
using NpgsqlTypes;
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;

namespace WebApplication1
{
    public partial class MenuVendedor : System.Web.UI.Page
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        IVendedor vendedor = (IVendedor)context.GetObject("Vendedor"); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Codigo"] != null)
            {
                lblCodVendedor.Text = "Código del Vendedor: "+Request.QueryString["Codigo"];
            }
        }        
        protected void btnVerInventario_Click(object sender, EventArgs e)
        {
            lblTituloGrid.Text = "Inventario";
            lblTituloGrid2.Text = "Imagen";
            vendedor.leeYcargaGridProductos(GridView1, Label1);
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
        }
        protected void btnVerPedidos_Click(object sender, EventArgs e)
        {
            lblTituloGrid.Text = "Pedidos";
            vendedor.MostrarPedidos(GridView1, Label1);
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
        }
        protected void btnNuevaOrden_Click(object sender, EventArgs e)
        {
            lblIniciaCompra.Text ="Orden iniciada";
            vendedor.truncateCarritoOrden(Label1);
            Image1.Visible = false;
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {    
            if (lblTituloGrid.Text =="Pedidos")
            {
                vendedor.leeYCargaDetallePedido(deGrid1(1),GridView2,Label1);
                lblTituloGrid2.Text = "Detalle del pedido";
            }
            if (lblTituloGrid.Text =="Inventario")
            {                
                txtCodigo.Text = deGrid1(1);
                txtNombre.Text = deGrid1(2);
                txtCategoria.Text = deGrid1(3);
                txtPrecio.Text = deGrid1(4);
                Image1.Visible = true;
                vendedor.leeYcargaImagen(deGrid1(1), Image1, Label1);
            }
            if (lblTituloGrid.Text == "Ordenes de Compra")
            {
                vendedor.leeYCargaDetalleOrden(deGrid1(1),GridView2,Label1);
                lblTituloGrid2.Text = "Detalle de la Orden";
            }
        }
        protected void btnAgregarOrden_Click(object sender, EventArgs e)
        {
            if (lblIniciaCompra.Text != ".")
            {
                if (ValidarCantidad())
                {
                    vendedor.agregarDetalleOrden(txtCodigo.Text, txtNombre.Text, txtCategoria.Text, txtPrecio.Text, txtCantidad.Text,Label1);
                    vendedor.leeYcargaGridCarritoOrden(GridView2,Label1);
                    lblTituloGrid2.Text = "Productos para la orden de compra";
                }                
            }
            else { objconexion.MensajeNormal("Debe Iniciar antes una orden de compra", Label1); }            
        }
        protected void btnEnviarOrden_Click(object sender, EventArgs e)
        {
            if (lblIniciaCompra.Text != ".")
            {
                if (vendedor.ExisteDistribuidor(txtCodDistribuidor.Text,Label1))
                {
                    vendedor.enviarOrden(txtCodDistribuidor.Text,Label1,lblIniciaCompra);
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    lblIniciaCompra.Text = ".";
                    lblTituloGrid2.Text = "";
                    Image1.Visible = false;
                }
                else { objconexion.MensajeNormal("El codigo de distribuidor no es válido!", Label1); }
            }
            else { objconexion.MensajeNormal("Debe Iniciar antes una orden de compra", Label1); }   
        }
        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            if (lblTituloGrid.Text == "Pedidos")
            {
                
                if (vendedor.ComprobarCantidadesFacturar(deGrid1(1),Label1))
                {
                    Label2.Text = "entra condi if";
                    string subtotal = "" + vendedor.sacarSubTotal(deGrid1(1),Label1);
                    vendedor.Facturar(deGrid1(2), deGrid1(1), subtotal, Label1, GridView1);
                    Image1.Visible = false;
                }
            }
            else { objconexion.MensajeNormal("Debe seleccionar un pedido", Label1); }
        }       
        protected void btnAlmacenarOrden_Click(object sender, EventArgs e)
        {
            if (lblTituloGrid.Text == "Ordenes de Compra")
            {
                vendedor.AlmacenarProductosDeOrden(deGrid1(1),GridView1,lblTituloGrid,lblTituloGrid2,Label1);
                Image1.Visible = false;
            }
            else { objconexion.MensajeNormal("Debe Seleccionar una orden de compra", Label1); }
        }        
        private string deGrid1(int num)
        {
            try
            {
                return GridView1.SelectedRow.Cells[num].Text;
            }
            catch (Exception)
            {
                return null;
            }

        }        
        private bool ValidarCantidad()
        {
            bool respuesta = false;
            try
            {
                int numero = Convert.ToInt32(txtCantidad.Text);
                if (numero < 1)
                {
                    respuesta = false;
                    objconexion.MensajeNormal("La Cantidad Debe ser positiva", Label1);
                }
                else
                {
                    respuesta = true; 
                }
            }
            catch (Exception Ex)
            {
                respuesta = false;
                objconexion.MensajeNormal("La Cantidad debe ser  un número Entero", Label1);
            }
            return respuesta;
        }
        protected void btnDVerOrdenes_Click(object sender, EventArgs e)
        {
            vendedor.MostrarOrdenes(GridView1,Label1,lblTituloGrid,lblTituloGrid2);
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LoginVendedor.aspx");
        }        
    }
}