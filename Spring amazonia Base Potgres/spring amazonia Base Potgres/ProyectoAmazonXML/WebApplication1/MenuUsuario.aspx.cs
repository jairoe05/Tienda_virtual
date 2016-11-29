using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using Oracle.DataAccess;
using System.Data;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;

namespace WebApplication1
{
    public partial class MenuUsuario : System.Web.UI.Page
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        IUsuario user = (IUsuario)context.GetObject("Usuario");   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Cedula"] != null)
            {
                lblCedula.Text = Request.QueryString["Cedula"];
                lblIniciarCompra.Text = user.cargarUsuario(lblCedula.Text,Label1);
            }
        }
        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            user.truncateCarrito(Label1);
            user.leeYcargaGridProductos(GridView1,Label1);
        }        
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (lblCedula.Text != ".")
            {
                if (deGrid1(1) != ".")
                {
                    if (lblgrid1.Text == "Productos")
                    {
                        if (user.ValidarCantidad(deGrid1(1),txtCantidad.Text,Label1))
                        {
                            user.agregaAcarrito(deGrid1(1), deGrid1(2), deGrid1(3), deGrid1(4), txtCantidad.Text,Label1);
                            user.actualizarStockAmazon(deGrid1(1), txtCantidad.Text, "-",Label1);
                            user.leeYcargaGridCarrito(GridView2,Label1);
                            user.leeYcargaGridProductos(GridView1, Label1);
                        }
                    }
                    else { objconexion.MensajeNormal("Debe Elegir ver Prodcutos!!", Label1); }                  
                }
                else { objconexion.MensajeNormal("Debe Seleccionar un producto", Label1); }                
            }
            else { objconexion.MensajeNormal("Debe Iniciar La Compra Antes", Label1); }            
        }                
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblgrid1.Text =="Productos")
            {
                user.leeYcargaImagen(deGrid1(1),Image1,Label1);
            }
            
            if (lblgrid1.Text =="Pedidos")
            {
                user.leeYCargaDetallePedido(deGrid1(1),GridView2,Label1);
            }
            if (lblgrid1.Text == "Facturas")
            {
                user.leeYCargaDetalleFactura(deGrid1(1),GridView2,Label1);
            }
        }
        protected void btnDevolver_Click(object sender, EventArgs e)
        {
            if (lblCedula.Text != ".")
            {
                if (deGrid2(1)!=".")
                {
                    if (lblgrid1.Text == "Productos")
                    {
                        user.DevolverProducto(deGrid2(1),Label1);
                        user.actualizarStockAmazon(deGrid2(1), deGrid2(5), "+",Label1);
                        user.leeYcargaGridCarrito(GridView2,Label1);
                        user.leeYcargaGridProductos(GridView1, Label1);
                    }
                    else { objconexion.MensajeNormal("Debe Debe Elegir ver productos", Label1); }
                }
                else { objconexion.MensajeNormal("Debe Seleccionar un producto antes", Label1); }                  
            }
            else { objconexion.MensajeNormal("Debe Iniciar La Compra Antes", Label1); }   
        }
        protected void btnConcluir_Click(object sender, EventArgs e)
        {            
            if (lblCedula.Text != ".")
            {
                if (user.CuentaCarrito(Label1) > 0)
                {
                    user.agregarPedido(lblCedula.Text,Label1);
                    user.truncateCarrito(Label1);
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    lblgrid1.Text = ".";
                    lblgrid2.Text = ".";
                    Image1.Visible = false;
                }
                else { objconexion.MensajeNormal("El pedido no puede estar vacio!!", Label1); }
            }
            else { objconexion.MensajeNormal("Debe Iniciar La Compra Antes", Label1); }
        }
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.leeYcargaImagen(deGrid2(1),Image1,Label1);
        }
        protected void btnCerrarsesion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
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
        private string deGrid2(int num)
        {
            try
            {
                return GridView2.SelectedRow.Cells[num].Text;
            }
            catch (Exception)
            {
                return ".";
            }
        }               
        protected void btnVerMisPedidos_Click(object sender, EventArgs e)
        {
            if (lblCedula.Text != ".")
            {
                user.VerMisPedidos(lblCedula.Text,GridView1,Label1,lblgrid1,lblgrid2);
                Image1.Visible = false;
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
            else { objconexion.MensajeNormal("Debe Iniciar el proceso para consultar", Label1); }
        } 
        protected void btnVerProductos_Click(object sender, EventArgs e)
        {
            user.leeYcargaGridProductos(GridView1, Label1);
            lblgrid1.Text = "Productos";
            lblgrid2.Text = "Carrito";
            Image1.Visible = true;
            GridView2.DataSource = null;
            GridView2.DataBind();
        }        
        protected void btnverFacturas_Click(object sender, EventArgs e)
        {
            if (lblCedula.Text != ".")
            {
                user.VerMisFacturas(lblCedula.Text,GridView1,Label1,lblgrid1,lblgrid2);
                Image1.Visible = false;
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
            else { objconexion.MensajeNormal("Debe Iniciar el proceso para consultar", Label1); }
        }        
    }
}