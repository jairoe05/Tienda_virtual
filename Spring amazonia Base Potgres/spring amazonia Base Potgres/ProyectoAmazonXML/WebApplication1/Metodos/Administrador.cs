using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
using Npgsql;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;

namespace WebApplication1.Metodos
{
    public class Administrador : IAdministrador
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        public void leeYcargaGridProductos(GridView GridView1, Label Label2)
        {
            
            try
            {
                string qry = "select detalle from amazonxml";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataReader readsql = cmd.ExecuteReader();
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void MostrarOrdenes(GridView GridView1, Label lblGrid, Label Label2)
        {
            
            try
            {
                string query = "select numero_orden as \"Orden # \", fecha, codigo_distribuidor as Distribuidor, estado from orden_compra";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("Ordenes");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblGrid.Text = "Ordenes de Compra";
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label2);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void VerMisFacturas(string f1, string f2, GridView GridView1, Label lblGrid, Label Label2)
        {            
            try
            {
                string query = "select numero_factura, cedula_cliente, fecha, Subtotal from factura where fecha >= TO_DATE('" + f1 + "','YYYY,mm,dd') and fecha <= TO_DATE('" + f2 + "','YYYY,mm,dd')";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("Ventas");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblGrid.Text = "Ventas";
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label2);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYCargaDetalleFactura(string numeroFactura, GridView GridView2, Label Label2)
        {            
            try
            {
                string qry = "select detalle from factura where numero_factura = " + numeroFactura + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
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
                    {  
                        row = dt.NewRow();
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {                      
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYcargaImagen(string codigoProducto, Image Image1, Label Label2)
        {            
            try
            {
                string query = "select Imagen from amazonxml where Codigo = " + codigoProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYCargaDetalleOrden(string numeroOrden, GridView GridView2, Label Label2)
        {            
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + numeroOrden + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
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
                    {  
                        row = dt.NewRow();
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {                     
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
    }
}