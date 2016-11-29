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

namespace WebApplication1
{
    public partial class MenuDistribuidor : System.Web.UI.Page
    {
        conexion con = new conexion();
        static string path = @"C:\Users\Javier\Documents\PROGRAMACION CUC\BASE DE DATOS ADECIO\AmazonXML\";
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
            else { con.MensajeNormal("Debe seleccionar una Orden"); }
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
            con.conectarSqlS();
            try
            {
                string qry = "select detalle from DistribuidorXML";
                SqlCommand cmd = new SqlCommand(qry, con.consql);
                SqlDataReader readsql = cmd.ExecuteReader();
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
                    StreamWriter file = new StreamWriter(path + "ProductoIndividualDistri.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "ProductoIndividualDistri.xml");
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
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectarSqlS(); }
        }
        private void MostrarOrdenes()
        {
            con.conectar();
            try
            {
                string qry = "select numero_orden as \"Orden #\", Fecha, Codigo_Distribuidor, Estado from Orden_Compra";
                OracleCommand cmd = new OracleCommand(qry, con.conex);
                OracleDataAdapter adap = new OracleDataAdapter(cmd);
                DataTable tb = new DataTable("Ordenes");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
            }
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectar(); }
        }
        private void leeYcargaImagen(string codigoProducto)
        {
            con.conectarSqlS();
            try
            {
                string query = "select Imagen from DistribuidorXML where Codigo = " + codigoProducto + "";
                SqlCommand cmd = new SqlCommand(query, con.consql);
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectarSqlS(); }
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
            con.conectar();
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + numeroOrden + "";
                OracleCommand cmd = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd.ExecuteReader();
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
                    StreamWriter file = new StreamWriter(path + "detalleOrdenDistri.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "detalleOrdenDistri.xml");
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
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectar(); }
        }
        private bool ComprobarCantidadesSuministrar(string NumOrden)  //pendiente
        {//**
            con.conectar();
            contadorFalsos = 0;
            try
            {
                string qryOra = "select detalle from orden_compra where numero_orden = " + NumOrden + "";
                OracleCommand cmdOra = new OracleCommand(qryOra, con.conex);
                OracleDataReader leeOra = cmdOra.ExecuteReader();
                string strxml = "";
                if (leeOra.Read())
                {
                    strxml = "" + leeOra["detalle"];
                    StreamWriter file = new StreamWriter(path + "OrdenASuministro.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "OrdenASuministro.xml");
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
                            {
                                if (!comprobarExistenciaSQL(codComprobar, nodo2.InnerText, nombreProd))
                                { contadorFalsos++; }
                            }
                        }
                    }
                }
                leeOra.Close();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
            if (contadorFalsos > 0)
            {
                return false;
            }
            else { return true; }

        }
        private bool comprobarExistenciaSQL(string CodProducto, string Cantidad, string nombre)
        {
            con.conectarSqlS();
            int StockDeDistribuidor = 0;
            bool condi = false;
            try
            {
                string qry = "select detalle from DistribuidorXML where codigo = " + CodProducto + "";
                SqlCommand cmd = new SqlCommand(qry, con.consql);
                SqlDataReader readsql = cmd.ExecuteReader();
                string strxml = "";
                if (readsql.Read())
                {
                    strxml = "" + readsql["detalle"];
                    StreamWriter file = new StreamWriter(path + "ComprobarCantidadDistri.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "ComprobarCantidadDistri.xml");
                    XmlNode nodo = doc.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);
                        if (nodo1.Name == "Stock")
                        {
                            StockDeDistribuidor = Convert.ToInt32(nodo1.InnerText);
                            if (Convert.ToInt32(Cantidad) > StockDeDistribuidor)
                            {
                                condi = false;
                                con.MensajeNormal("No hay sificiente cantidad de: " + nombre);
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
            finally { con.desconectarSqlS(); }
            return condi;
        }
        private void ActualizarStocksDistri()
        {
            con.conectar();
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + deGrid1(1) + " and Estado = 'Pendiente'";
                OracleCommand cmd2 = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    readoracle.Close();
                    //actualizar stocks de todos los productos
                    StreamWriter file = new StreamWriter(path + "UpdtStocsDistri.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument docStocks = new XmlDocument();
                    docStocks.Load(path + "UpdtStocsDistri.xml");
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
                    con.MensajeNormal("Se ha reliazo correctamente el suministro para Amazon");
                }
                else { con.MensajeNormal("Esta orden no está pendiente!!"); }
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void actualizaStock(string codigo, string CantidadActualizar, string signo)
        {//modificado para sqlserver terminado
            con.conectarSqlS();
            try
            {
                string qry = "select detalle from DistribuidorXML where Codigo = " + codigo + "";
                SqlCommand cmd = new SqlCommand(qry, con.consql);
                SqlDataReader readosql = cmd.ExecuteReader();
                string strxml = "";
                if (readosql.Read())
                {
                    strxml = "" + readosql["detalle"];
                    readosql.Close();
                    StreamWriter file = new StreamWriter(path + "ActualizarStock.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "ActualizarStock.xml");
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
                    SqlCommand cmdr2 = new SqlCommand(qryr2, con.consql);
                    cmdr2.CommandType = CommandType.StoredProcedure;
                    SqlParameter prmCod = new SqlParameter();
                    prmCod.ParameterName = "@cod";
                    prmCod.SqlDbType = SqlDbType.Int;
                    prmCod.Value = Convert.ToInt32(codigo);
                    cmdr2.Parameters.Add(prmCod);
                    SqlParameter prmDet = new SqlParameter();
                    prmDet.ParameterName = "@deta";
                    prmDet.SqlDbType = SqlDbType.Xml;
                    prmDet.Value = doc.OuterXml.ToString();
                    cmdr2.Parameters.Add(prmDet);
                    cmdr2.ExecuteNonQuery();
                    //****
                }

            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectarSqlS(); }
        }
        private int sacarSubTotal()
        {
            con.conectar();
            int Subtotal = 0;
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement Orden = doc.CreateElement("Orden");
                doc.AppendChild(Orden);

                string qry = "select detalle from orden_compra where numero_orden = " + deGrid1(1) + "";
                OracleCommand cmd2 = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";

                int AcumuladoXProducto = 0;
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    StreamWriter file = new StreamWriter(path + "OrdenASubtotal.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.Load(path + "OrdenASubtotal.xml");
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
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
            return Subtotal;
        }
        private void updateOrdenARealizado(string NumOrden)
        {
            con.conectar();
            try
            {
                string query = "update orden_compra set Estado = 'Suministrado' where numero_orden = " + NumOrden + "";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LoginDistribuidor.aspx");
        }
    }
}