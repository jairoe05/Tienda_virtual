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

namespace WebApplication1
{
    public partial class MenuDistribuidor : System.Web.UI.Page
    {
        conexion con = new conexion();
        int contadorFalsos = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVerInventario_Click(object sender, EventArgs e)
        {
            lblTituloGrid1.Text = "Inventario";
            leeYcargaGridProductos();
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
        }

        protected void btnVerOrdenes_Click(object sender, EventArgs e)
        {            
            MostrarOrdenes();
            GridView2.DataSource = null;
            GridView2.DataBind();
            lblTituloGrid1.Text = "Ordenes de Compra";
            Image1.Visible = false;
        }
        protected void btnSuministrar_Click(object sender, EventArgs e)
        {
            if (lblTituloGrid1.Text == "Ordenes de Compra" && deGrid1(1) != ".")
            {
                if (ComprobarCantidadesSuministrar(deGrid1(1)))
                {     
                    ActualizarStocksDistri();                    
                    //string subtotal = "" + sacarSubTotal();
                    //con.MensajeNormal("Subtotal de la orden de compra: " + subtotal);
                    MostrarOrdenes();
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
                leeYcargaImagen(deGrid1(1));
            }
            if (lblTituloGrid1.Text == "Ordenes de Compra")
            {
                leeYCargaDetalleOrden(deGrid1(1));
                lblTituloGrid2.Text = "Detalle de la Orden";
            }
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }  
        private void leeYcargaGridProductos()
        {
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from DistribuidorXML";
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
                string qry = "select numero_orden as \"Orden #\", Fecha, Codigo_Distribuidor, Estado from Orden_Compra";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("Ordenes");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
            }
            catch (Exception Ex) { con.MensajeError(Ex, Label2); }
            finally { con.desconectarPOSTGRE(); }
        }
        private void leeYcargaImagen(string codigoProducto)
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "select Imagen from DistribuidorXML where Codigo = " + codigoProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { con.MensajeError(Ex, Label2); }
            finally { con.desconectarPOSTGRE(); }
        }
        public string deGrid1(int num)
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
        private bool ComprobarCantidadesSuministrar(string NumOrden)  //pendiente
        {//**
            con.conectarPOSTGRE();
            contadorFalsos = 0;
            try
            {
                string qryOra = "select detalle from orden_compra where numero_orden = " + NumOrden + "";
                NpgsqlCommand cmdOra = new NpgsqlCommand(qryOra, con.Postgrecon);
                NpgsqlDataReader leeOra = cmdOra.ExecuteReader();
                string strxml = "";
                if (leeOra.Read())
                {
                    strxml = "" + leeOra["detalle"];
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;
                    string codComprobar = "";
                    string nombreProd = "";
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {
                            if (nodo2.Name == "Codigo")
                            {
                                codComprobar = nodo2.InnerText;
                            }
                            if (nodo2.Name == "Nombre")
                            {
                                nombreProd = nodo2.InnerText;
                            }
                            if (nodo2.Name == "Cantidad")
                            {            //********************************************************************************
                                if (!comprobarExistenciaSQL(codComprobar, nodo2.InnerText, nombreProd))//no cerrar x comprobar exis
                                { contadorFalsos++; }
                            }
                        }
                    }
                }
                leeOra.Close();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label2);
            }
            finally { con.desconectarPOSTGRE(); }
            if (contadorFalsos > 0)
            {
                return false;
            }
            else { return true; }

        }
        private bool comprobarExistenciaSQL(string CodProducto, string Cantidad, string nombre)
        {
            con.conectarPOSTGRE();
            int StockDeDistribuidor = 0;
            bool condi = false;
            try
            {
                string qry = "select detalle from DistribuidorXML where codigo = " + CodProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader readsql = cmd.ExecuteReader();
                string strxml = "";
                if (readsql.Read())
                {
                    strxml = "" + readsql["detalle"];
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);
                        if (nodo1.Name == "Stock")
                        {
                            StockDeDistribuidor = Convert.ToInt32(nodo1.InnerText);
                            if (Convert.ToInt32(Cantidad) > StockDeDistribuidor)
                            {
                                condi = false;
                                con.MensajeNormal("No hay sificiente cantidad de: " + nombre, Label2);
                            }
                            else { condi = true; }
                        }
                    }
                }
                readsql.Close();
            }
            catch (Exception Ex)
            {
                condi = false;
                StockDeDistribuidor = 0;
            }
            finally {/* con.desconectarSqlS();*/ } //********************************************para q no deje cerrada al llamador
            return condi;
        }
        private void ActualizarStocksDistri()
        {
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + deGrid1(1) + " and Estado = 'Pendiente'";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, con.Postgrecon);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    readoracle.Close();
                    //actualizar stocks de todos los productos
                    XmlDocument docStocks = new XmlDocument();
                    docStocks.LoadXml(strxml);
                    XmlNode nodo = docStocks.DocumentElement;
                    string codComprobar = "";
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {
                            if (nodo2.Name == "Codigo")
                            {
                                codComprobar = nodo2.InnerText;
                            }
                            if (nodo2.Name == "Cantidad")
                            {
                                actualizaStock(codComprobar, nodo2.InnerText, "-");
                            }
                        }
                    }
                    updateOrdenARealizado(deGrid1(1));
                    con.MensajeNormal("Se ha reliazo correctamente el suministro para Amazon", Label2);
                }
                else { con.MensajeNormal("Esta orden no está pendiente!!", Label2); }
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label2);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        private void actualizaStock(string codigo, string CantidadActualizar, string signo)
        {//modificado para sqlserver terminado
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from DistribuidorXML where Codigo = " + codigo + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader readosql = cmd.ExecuteReader();
                string strxml = "";
                if (readosql.Read())
                {
                    strxml = "" + readosql["detalle"];
                    readosql.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);
                        if (nodo1.Name == "Stock")
                        {
                            int StockNum = Convert.ToInt32(nodo1.InnerText);
                            if (signo == "+") { StockNum = StockNum + Convert.ToInt32(CantidadActualizar); }
                            if (signo == "-") { StockNum = StockNum - Convert.ToInt32(CantidadActualizar); }
                            nodo1.InnerText = Convert.ToString(StockNum);
                        }
                    }
                    //********
                    string qryr2 = "ActualizaDetDistribuidor";
                    NpgsqlCommand cmdr2 = new NpgsqlCommand(qryr2, con.Postgrecon);
                    cmdr2.CommandType = CommandType.StoredProcedure;
                    NpgsqlParameter prmCod = new NpgsqlParameter();
                    prmCod.ParameterName = "cod";
                    prmCod.NpgsqlDbType = NpgsqlDbType.Integer;
                    prmCod.Value = Convert.ToInt32(codigo);
                    cmdr2.Parameters.Add(prmCod);
                    NpgsqlParameter prmDet = new NpgsqlParameter();
                    prmDet.ParameterName = "deta";
                    prmDet.NpgsqlDbType = NpgsqlDbType.Xml;
                    prmDet.Value = doc.OuterXml.ToString();
                    cmdr2.Parameters.Add(prmDet);
                    cmdr2.ExecuteNonQuery();
                    //****
                }

            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label2);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        private int sacarSubTotal()
        {
            con.conectarPOSTGRE();
            int Subtotal = 0;
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement Orden = doc.CreateElement("Orden");
                doc.AppendChild(Orden);

                string qry = "select detalle from orden_compra where numero_orden = " + deGrid1(1) + "";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                int AcumuladoXProducto = 0;
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    XmlDocument doccarri = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doccarri.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText); 
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {//con.MensajeNormal(nodo1.InnerText); 
                            if (nodo2.Name == "Precio")
                            {
                                AcumuladoXProducto = Convert.ToInt32(nodo2.InnerText);
                            }
                            if (nodo2.Name == "Cantidad")
                            {
                                AcumuladoXProducto = AcumuladoXProducto * Convert.ToInt32(nodo2.InnerText);
                            }
                        }
                        Subtotal += AcumuladoXProducto;
                        AcumuladoXProducto = 0;
                    }
                }
                readoracle.Close();
            }
            catch (Exception Ex)
            {
                Subtotal = 0;
                con.MensajeError(Ex, Label2);
            }
            finally { con.desconectarPOSTGRE(); }
            return Subtotal;
        }
        private void updateOrdenARealizado(string NumOrden)
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "update orden_compra set Estado = 'Suministrado' where numero_orden = " + NumOrden + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label2);
            }
            finally { con.desconectarPOSTGRE(); }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LoginDistribuidor.aspx");
        }
    }
}