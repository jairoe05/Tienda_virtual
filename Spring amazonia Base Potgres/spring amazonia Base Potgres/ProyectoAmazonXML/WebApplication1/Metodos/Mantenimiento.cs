using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI.WebControls;
using Npgsql;
using System.Data;
using System.Xml;
using NpgsqlTypes;

namespace WebApplication1.Metodos
{
    public class Mantenimiento : IMantenimiento
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        public void cargaInventario(GridView gridUsar, string tablausar, Label Label1)
        {            
            try
            {
                string qry = "select detalle from " + tablausar;
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
                    {                  
                        row[nombrecolumna[contadorcolumnas]] = nodo1.InnerText;
                        contadorcolumnas++;
                    }
                    contadorcolumnas = 0;
                    dt.Rows.Add(row);
                }
                gridUsar.DataSource = dt;
                gridUsar.DataBind();
                readsql.Close();
            }
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public string ManteAmazonDistri(int pCodigo, string pNombre, string pCategoria, int pPrecio, int pStock, Byte[] pImagen, string tabla,string accion)
        {
            try
            {                
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (tabla.Equals("amazon"))
                {
                    if (accion.Equals("insertar")) { cmd.CommandText = "insertaamazonxml"; }
                    if (accion.Equals("actualizar")) { cmd.CommandText = "updateamazonxml"; }
                    if (accion.Equals("eliminar")) { cmd.CommandText = "eliminaamazonxml"; }
                }
                else if (tabla.Equals("distribuidor")) {
                    if (accion.Equals("insertar")) { cmd.CommandText = "insertaDistribuidorXML"; }
                    if (accion.Equals("actualizar")) { cmd.CommandText = "updatedistribuidorxml"; }
                    if (accion.Equals("eliminar")) { cmd.CommandText = "eliminadistribuidorxml"; }
                }
                cmd.Connection = objconexion.conectarPOSTGRE();
                NpgsqlParameter paramCod = new NpgsqlParameter();
                paramCod.NpgsqlDbType = NpgsqlDbType.Integer;
                paramCod.ParameterName = "pcodigo";
                paramCod.Value = pCodigo;
                cmd.Parameters.Add(paramCod);
                if (!accion.Equals("eliminar"))
                {
                    NpgsqlParameter paramDet = new NpgsqlParameter();
                    paramDet.NpgsqlDbType = NpgsqlDbType.Xml;
                    paramDet.ParameterName = "pdetalle";

                    XmlDocument doc = new XmlDocument();
                    XmlElement RaizAmazon = doc.CreateElement("Producto");
                    doc.AppendChild(RaizAmazon);
                    XmlElement Codigo = doc.CreateElement("Codigo");
                    Codigo.AppendChild(doc.CreateTextNode("" + pCodigo));
                    RaizAmazon.AppendChild(Codigo);
                    XmlElement Nombre = doc.CreateElement("Nombre");
                    Nombre.AppendChild(doc.CreateTextNode(pNombre));
                    RaizAmazon.AppendChild(Nombre);
                    XmlElement Categoria = doc.CreateElement("Categoria");
                    Categoria.AppendChild(doc.CreateTextNode(pCategoria));
                    RaizAmazon.AppendChild(Categoria);
                    XmlElement Precio = doc.CreateElement("Precio");
                    Precio.AppendChild(doc.CreateTextNode("" + pPrecio));
                    RaizAmazon.AppendChild(Precio);
                    XmlElement Cantidad = doc.CreateElement("Cantidad");
                    Cantidad.AppendChild(doc.CreateTextNode("" + pStock));
                    RaizAmazon.AppendChild(Cantidad);
                    paramDet.Value = doc.OuterXml.ToString();
                    cmd.Parameters.Add(paramDet);
                    NpgsqlParameter paramImg = new NpgsqlParameter();
                    paramImg.NpgsqlDbType = NpgsqlDbType.Bytea;
                    paramImg.ParameterName = "pimagen";
                    paramImg.Value = pImagen;
                    cmd.Parameters.Add(paramImg);
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
            finally { objconexion.desconectarPOSTGRE(); }
            return "Mantenimiento exitoso";
        }
        public string ManteUsuaVende(string CodCed, string Nombre, string Apellidos, string telefono, string contra, string correo, string tabla, string accion)
        {
            string query = "";
            if(tabla.Equals("usuario")){
                if (accion.Equals("insertar")) { query = "insert into usuario values(" + CodCed + ", '" + Nombre + "', '" + Apellidos + "', " + telefono + ", '" + contra + "', '" + correo + "');"; }
                if (accion.Equals("actualizar")) { query = "update usuario set nombre='"+Nombre+"', apellidos='"+Apellidos+"', telefono="+telefono+" ,pass='"+contra+"',correo='"+correo+"' where cedula="+CodCed+";"; }
                if (accion.Equals("eliminar")) { query = "delete from usuario where cedula = "+CodCed+";"; }
            }else if(tabla.Equals("vendedor")){
                if (accion.Equals("insertar")) { query = "insert into vendedor values(" + CodCed + ", '" + Nombre + "', '" + Apellidos + "', " + telefono + ", '" + contra + "', '" + correo + "');"; }
                if (accion.Equals("actualizar")) { query = "update vendedor set nombre='" + Nombre + "', apellidos='" + Apellidos + "', telefono=" + telefono + " ,pass='" + contra + "',correo='" + correo + "' where codigo=" + CodCed + ";"; }
                if (accion.Equals("eliminar")) { query = "delete from vendedor where codigo = " + CodCed + ";"; }
            }
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
            finally { objconexion.desconectarPOSTGRE(); }
            return "Mantenimiento Exitoso!";
        }
        public void cargarUsuaVende(GridView GridView2, Label LabelMensaje, string tabla)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select * from " + tabla + "", objconexion.conectarPOSTGRE());
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable();
                adap.Fill(tb);
                GridView2.DataSource = tb;
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                objconexion.MensajeError(ex, LabelMensaje);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYcargaImagen(string codigoProducto, int num, Image Image1, Label Label1)
        {
            try
            {
                string query;
                if (num == 1)
                    query = "select Imagen from amazonxml where Codigo = " + codigoProducto + "";
                else
                    query = "select Imagen from distribuidorxml where Codigo = " + codigoProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
    }
}