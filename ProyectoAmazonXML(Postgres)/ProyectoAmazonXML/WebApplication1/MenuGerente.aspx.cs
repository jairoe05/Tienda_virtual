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
namespace WebApplication1
{
    public partial class MenuGerente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        conexion con = new conexion();
        protected void btnVerInventario_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "Bienvenido")
            {
                lblGrid.Text = "Inventario";
                leeYcargaGridProductos();
                GridView2.DataSource = null;
                GridView2.DataBind();
                Image1.Visible = false;
            }
            else { con.MensajeNormal("Debe Loggearse!", Label2); }
        }

        protected void btnVerOrdenes_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "Bienvenido")
            {
                MostrarOrdenes();
                GridView2.DataSource = null;
                GridView2.DataBind();
                Image1.Visible = false;
            }
            else { con.MensajeNormal("Debe Loggearse!", Label2); }
        }

        protected void btnVerVentas_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "Bienvenido")
            {
                if (txtFecha.Text == "" || txtFecha2.Text == "")
                {
                    con.MensajeNormal("Debe indicar los 2 parametros de fecha!", Label2);
                }
                else
                {
                    try
                    {
                        VerMisFacturas(txtFecha.Text, txtFecha2.Text);
                        Image1.Visible = false;
                        GridView2.DataSource = null;
                        GridView2.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        con.MensajeError(Ex, Label2);
                    }
                }
            }
            else { con.MensajeNormal("Debe Loggearse!", Label2); }
        }

        //METODOS***********************************
        private void leeYcargaGridProductos()
        {
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from AmazonXML";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader readsql = cmd.ExecuteReader();
                string strxml = "";
                DataTable dt = new DataTable();
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Categoria");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Stock");
                string[] nombrecolumna = { "Codigo", "Nombre", "Categoria", "Precio", "Stock" };
                DataRow row;
                int contadorcolumnas = 0;
                while (readsql.Read())
                {
                    strxml = "" + readsql["detalle"];
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;
                    row = dt.NewRow();
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);                        
                        row[nombrecolumna[contadorcolumnas]] = nodo1.InnerText;
                        contadorcolumnas++;
                    }
                    contadorcolumnas = 0;
                    dt.Rows.Add(row);
                }
                GridView1.DataSource = dt;
                GridView1.DataBind();
                readsql.Close();
            }
            catch (Exception Ex) { con.MensajeError(Ex, Label2); }
            finally { con.desconectarPOSTGRE(); }
        }
        private void MostrarOrdenes()
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "select numero_orden as \"Orden # \", fecha, codigo_distribuidor as Distribuidor, estado from orden_compra";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("Ordenes");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblGrid.Text = "Ordenes de Compra";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label2);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        private void VerMisFacturas(string f1, string f2)
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "select numero_factura, cedula_cliente, fecha, Subtotal from factura where fecha > TO_DATE('" + f1 + "','YYYY,mm,dd') and fecha < TO_DATE('"+f2+"','YYYY,mm,dd')";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("Ventas");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblGrid.Text = "Ventas";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label2);
            }
            finally { con.desconectarPOSTGRE(); }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblGrid.Text == "Ventas")
            {
                leeYCargaDetalleFactura(GridView1.SelectedRow.Cells[1].Text);
            }
            if (lblGrid.Text == "Inventario")
            {
                Image1.Visible = true;
                leeYcargaImagen(GridView1.SelectedRow.Cells[1].Text);
            }
            if (lblGrid.Text == "Ordenes de Compra")
            {
                leeYCargaDetalleOrden(GridView1.SelectedRow.Cells[1].Text);
            }
        }
        private void leeYCargaDetalleFactura(string numeroFactura)
        {
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from factura where numero_factura = " + numeroFactura + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader readoracle = cmd.ExecuteReader();
                string strxml = "";
                DataTable dt = new DataTable();
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Categoria");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Cantidad");
                string[] nombrecolumna = { "Codigo", "Nombre", "Categoria", "Precio", "Cantidad" };
                DataRow row;
                int contadorcolumnas = 0;
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;

                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);     
                        row = dt.NewRow();
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {//con.MensajeNormal(nodo1.InnerText);                        
                            row[nombrecolumna[contadorcolumnas]] = nodo2.InnerText;
                            contadorcolumnas++;
                        }
                        contadorcolumnas = 0;
                        dt.Rows.Add(row);
                    }
                }
                GridView2.DataSource = dt;
                GridView2.DataBind();
                readoracle.Close();
            }
            catch (Exception Ex) { con.MensajeError(Ex, Label2); }
            finally { con.desconectar(); }
        }
        private void leeYcargaImagen(string codigoProducto)
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "select Imagen from AmazonXML where Codigo = " + codigoProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { con.MensajeError(Ex, Label2); }
            finally { con.desconectarPOSTGRE(); }
        }
        private void leeYCargaDetalleOrden(string numeroOrden)
        {
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + numeroOrden + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader readoracle = cmd.ExecuteReader();
                string strxml = "";
                DataTable dt = new DataTable();
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Categoria");
                dt.Columns.Add("Precio");
                dt.Columns.Add("Cantidad");
                string[] nombrecolumna = { "Codigo", "Nombre", "Categoria", "Precio", "Cantidad" };
                DataRow row;
                int contadorcolumnas = 0;
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);     
                        row = dt.NewRow();
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {//con.MensajeNormal(nodo1.InnerText);                        
                            row[nombrecolumna[contadorcolumnas]] = nodo2.InnerText;
                            contadorcolumnas++;
                        }
                        contadorcolumnas = 0;
                        dt.Rows.Add(row);
                    }
                }
                GridView2.DataSource = dt;
                GridView2.DataBind();
                readoracle.Close();
            }
            catch (Exception Ex) { con.MensajeError(Ex, Label2); }
            finally { con.desconectarPOSTGRE(); }
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text =="1111" && TextBox2.Text =="123")
            {
                con.MensajeNormal("Bienvenido Gerente!", Label2);
                Label1.Text = "Bienvenido";
            }
        }
    }
}