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

namespace WebApplication1
{
    public partial class MenuVendedor : System.Web.UI.Page
    {
        conexion con = new conexion();
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        int contadorFalsos = 0;
        //obtener inventario
        protected void btnVerInventario_Click(object sender, EventArgs e)
        {
            lblTituloGrid.Text = "Inventario";
            lblTituloGrid2.Text = "Imagen";
            leeYcargaGridProductos();
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
        }

        protected void btnVerPedidos_Click(object sender, EventArgs e)
        {
            lblTituloGrid.Text = "Pedidos";
            MostrarPedidos();
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
        }

        protected void btnNuevaOrden_Click(object sender, EventArgs e)
        {
            lblIniciaCompra.Text ="Orden iniciada";
            truncateCarritoOrden();
            Image1.Visible = false;
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {    
            if (lblTituloGrid.Text =="Pedidos")
            {
                leeYCargaDetallePedido(deGrid1(1));
                lblTituloGrid2.Text = "Detalle del pedido";
            }
            if (lblTituloGrid.Text =="Inventario")
            {                
                txtCodigo.Text = deGrid1(1);
                txtNombre.Text = deGrid1(2);
                txtCategoria.Text = deGrid1(3);
                txtPrecio.Text = deGrid1(4);
                Image1.Visible = true;
                leeYcargaImagen(deGrid1(1));
            }
            if (lblTituloGrid.Text == "Ordenes de Compra")
            {
                leeYCargaDetalleOrden(deGrid1(1));
                lblTituloGrid2.Text = "Detalle de la Orden";
            }
        }
        protected void btnAgregarOrden_Click(object sender, EventArgs e)
        {
            if (lblIniciaCompra.Text != ".")
            {
                if (ValidarCantidad())
                {
                    agregarDetalleOrden(txtCodigo.Text, txtNombre.Text, txtCategoria.Text, txtPrecio.Text, txtCantidad.Text);
                    leeYcargaGridCarritoOrden();
                    lblTituloGrid2.Text = "Productos para la orden de compra";
                }                
            }
            else { con.MensajeNormal("Debe Iniciar antes una orden de compra"); }            
        }
        protected void btnEnviarOrden_Click(object sender, EventArgs e)
        {
            if (lblIniciaCompra.Text != ".")
            {
                if (ExisteDistribuidor(txtCodDistribuidor.Text))
                {
                    enviarOrden();
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    lblIniciaCompra.Text = ".";
                    lblTituloGrid2.Text = "";
                    Image1.Visible = false;
                }
                else { con.MensajeNormal("El codigo de distribuidor no es válido!"); }
            }
            else { con.MensajeNormal("Debe Iniciar antes una orden de compra"); }   
        }
        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            if (lblTituloGrid.Text == "Pedidos")
            {
                if (ComprobarCantidadesFacturar(deGrid1(1)))
                {
                    string subtotal = "" + sacarSubTotal();
                    Facturar("" + subtotal);
                    Image1.Visible = false;
                }
            }
            else { con.MensajeNormal("Debe seleccionar un pedido"); }
        }
       
        protected void btnAlmacenarOrden_Click(object sender, EventArgs e)
        {
            if (lblTituloGrid.Text == "Ordenes de Compra")
            {
                AlmacenarProductosDeOrden(deGrid1(1));
                Image1.Visible = false;
            }
            else { con.MensajeNormal("Debe Seleccionar una orden de compra"); }
        }
        private void leeYcargaGridProductos()
        {
            con.conectarSqlS();
            try
            {
                string qry = "select detalle from AmazonXML";
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
                    StreamWriter file = new StreamWriter(con.path + "ProductoIndividual.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "ProductoIndividual.xml");
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
        private void MostrarPedidos()
        {
            con.conectar();
            try
            {
                string qry = "select numero_pedido as \"Pedido #\", cedula_cliente as Cedula, Fecha, Estado from pedido";
                OracleCommand cmd = new OracleCommand(qry, con.conex);
                OracleDataAdapter adap = new OracleDataAdapter(cmd);
                DataTable tb = new DataTable("Pedidos");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
            }
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectar(); }
        }        
        public string deGrid1(int num)
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
        private void leeYCargaDetallePedido(string numeropedido)
        {
            con.conectar();
            try
            {
                string qry = "select detalle from pedido where numero_pedido = "+numeropedido+"";
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
                    StreamWriter file = new StreamWriter(con.path + "PedidoIndividual.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "PedidoIndividual.xml");
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
        private void agregarDetalleOrden(string pcodigo,string pnombre, string pcategoria,string pprecio, string pcantidad)
        {
            con.conectar();
            try
            {
                string query = "InsertarDetalleOrden";
                OracleCommand cmdxm = new OracleCommand(query, con.conex);
                cmdxm.CommandType = CommandType.StoredProcedure;
                OracleParameter prmNum = new OracleParameter();
                prmNum.ParameterName = "num";
                prmNum.OracleDbType = OracleDbType.Int32;
                prmNum.Value = Convert.ToInt32(pcodigo);
                cmdxm.Parameters.Add(prmNum);
                OracleParameter prmDetalle = new OracleParameter();
                prmDetalle.ParameterName = "deta";
                prmDetalle.OracleDbType = OracleDbType.XmlType;

                XmlDocument doc = new XmlDocument();
                XmlElement Carrito = doc.CreateElement("Producto");
                doc.AppendChild(Carrito);

                XmlElement Codigo = doc.CreateElement("Codigo");
                Codigo.AppendChild(doc.CreateTextNode(pcodigo));
                Carrito.AppendChild(Codigo);

                XmlElement Nombre = doc.CreateElement("Nombre");
                Nombre.AppendChild(doc.CreateTextNode(pnombre));
                Carrito.AppendChild(Nombre);

                XmlElement Categoria = doc.CreateElement("Categoria");
                Categoria.AppendChild(doc.CreateTextNode(pcategoria));
                Carrito.AppendChild(Categoria);

                XmlElement Precio = doc.CreateElement("Precio");
                Precio.AppendChild(doc.CreateTextNode(pprecio));
                Carrito.AppendChild(Precio);

                XmlElement Cantidad = doc.CreateElement("Cantidad");
                Cantidad.AppendChild(doc.CreateTextNode(pcantidad));
                Carrito.AppendChild(Cantidad);

                prmDetalle.Value = doc.OuterXml.ToString();
                cmdxm.Parameters.Add(prmDetalle);
                cmdxm.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeNormal("Ya ha pedido el producto indicado\nPuede pedir otro");
            }
            finally { con.desconectar(); }
        }
        private int NumMaxOrden()
        {
            con.conectar();
            string strnum = "";
            int numero = 1;
            try
            {
                string query = "select max(numero_orden) max from orden_Compra";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                OracleDataReader lee = cmd.ExecuteReader();
                if (lee.Read())
                {
                    strnum = "" + lee["max"];
                    numero = Convert.ToInt32(strnum);
                    numero++;
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
                numero = 1;
            }
            finally { con.desconectar(); }
            return numero;
        }
        
        private void enviarOrden()
        {
            int NumOrden = NumMaxOrden();
            con.conectar();
            try
            {
                string query = "InsertarOrdenDeCompra";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter prmNumero = new OracleParameter();
                prmNumero.ParameterName = "num";
                prmNumero.OracleDbType = OracleDbType.Int32;
                prmNumero.Value = NumOrden;
                cmd.Parameters.Add(prmNumero);
                
                OracleParameter prmFecha = new OracleParameter();
                prmFecha.ParameterName = "fech";
                DateTime fecha = DateTime.Now;//fecha del momento
                prmFecha.OracleDbType = OracleDbType.Date;
                prmFecha.Value = fecha;
                cmd.Parameters.Add(prmFecha);
                OracleParameter prmCodDistri = new OracleParameter();
                prmCodDistri.ParameterName = "cod_distri";
                prmCodDistri.OracleDbType = OracleDbType.Int32;
                prmCodDistri.Value = Convert.ToInt32(txtCodDistribuidor.Text);
                cmd.Parameters.Add(prmCodDistri);
                OracleParameter prmDeta = new OracleParameter();
                prmDeta.ParameterName = "deta";
                prmDeta.OracleDbType = OracleDbType.XmlType;
                //*********************************************************
                XmlDocument doc = new XmlDocument();
                XmlElement Orden = doc.CreateElement("Orden");
                doc.AppendChild(Orden);

                string qry = "select detalle from CarritoOrdenCompra";
                OracleCommand cmd2 = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                string[] nombrecolumna = { "Codigo", "Nombre", "Categoria", "Precio", "Cantidad" };
                while (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    StreamWriter file = new StreamWriter(con.path + "CarritoAOrden.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.Load(con.path + "CarritoAOrden.xml");
                    XmlNode nodo = doccarri.DocumentElement;

                    XmlElement Producto = doc.CreateElement(nodo.Name);
                    Orden.AppendChild(Producto);
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText); 
                        XmlElement lll = doc.CreateElement(nodo1.Name);
                        lll.AppendChild(doc.CreateTextNode(nodo1.InnerText));
                        Producto.AppendChild(lll);
                    }
                }
                readoracle.Close();
                prmDeta.Value = doc.OuterXml.ToString();
                cmd.Parameters.Add(prmDeta);

                OracleParameter prmEstad = new OracleParameter();
                prmEstad.ParameterName = "estad";
                prmEstad.OracleDbType = OracleDbType.Varchar2;
                prmEstad.Value = "Pendiente";
                cmd.Parameters.Add(prmEstad);

                cmd.ExecuteNonQuery();
                con.MensajeNormal("La Orden de Compra Se ha Enviado Correctamente\nDebe Esperar la respuesta Del Distribuidor");
                truncateCarritoOrden();
                lblIniciaCompra.Text = ".";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void truncateCarritoOrden()
        {
            con.conectar();
            try
            {
                string query = "truncate table carritoordencompra";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void leeYcargaGridCarritoOrden()
        {
            con.conectar();
            try
            {
                string qry = "select detalle from carritoordencompra";
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
                while (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    StreamWriter file = new StreamWriter(con.path + "CarritoOrdenIndividual.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "CarritoOrdenIndividual.xml");
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
                GridView2.DataSource = dt;
                GridView2.DataBind();
                readoracle.Close();
            }
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectar(); }
        }
        private int sacarSubTotal()
        {
            con.conectar();
            int Subtotal = 0;
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement Factura = doc.CreateElement("Factura");
                doc.AppendChild(Factura);

                string qry = "select detalle from pedido where numero_pedido = "+deGrid1(1)+"";
                OracleCommand cmd2 = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                
                int AcumuladoXProducto = 0;
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    StreamWriter file = new StreamWriter(con.path + "PedidoAFactura.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.Load(con.path + "PedidoAFactura.xml");
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
        private bool ValidarCantidad()
        {
            bool respuesta = false;
            try
            {
                int numero = Convert.ToInt32(txtCantidad.Text);
                if (numero < 1)
                {
                    respuesta = false;
                    con.MensajeNormal("La Cantidad Debe ser positiva");
                }
                else
                {
                    respuesta = true; 
                }
            }
            catch (Exception Ex)
            {
                respuesta = false;
                con.MensajeNormal("La Cantidad debe ser  un número Entero");
            }
            return respuesta;
        }
        private bool ExisteDistribuidor(string codDistribuidor)
        {
            con.conectarSqlS();
            bool condi = false;
            try
            {
                string query = "select Nombre + ' de '+ Ubicacion as Distribuidor from Distribuidor where Cod_distribuidor = "+codDistribuidor+"";
                SqlCommand cmd = new SqlCommand(query,con.consql);
                SqlDataReader lee = cmd.ExecuteReader();
                string Distri = "";
                if (lee.Read())
                {
                    Distri = "" + lee["Distribuidor"];
                    condi = true;
                    con.MensajeNormal("Orden enviada al distribuidor  " + Distri);
                    lee.Close();
                }
                else { condi = false; }
                
            }
            catch (Exception Ex)
            {
                condi = false;
                con.MensajeError(Ex);
            }
            finally { con.desconectarSqlS(); }
            return condi;
        }
        private bool ComprobarCantidadesFacturar(string NumPedido)   //pendiente
        {//**
            con.conectar();
            contadorFalsos = 0;
            try
            {
                string qryOra = "select detalle from pedido where numero_pedido = " + NumPedido + "";
                OracleCommand cmdOra = new OracleCommand(qryOra,con.conex);
                OracleDataReader leeOra = cmdOra.ExecuteReader();
                string strxml = "";
                if (leeOra.Read())
                {
                    strxml = "" + leeOra["detalle"];
                    StreamWriter file = new StreamWriter(con.path + "PedidoAFactura.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "PedidoAFactura.xml");
                    XmlNode nodo = doc.DocumentElement;
                    string codComprobar = "";
                    string nombreProd = "";
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {
                        foreach (XmlNode nodo2 in nodo1.ChildNodes)
                        {
                            if (nodo2.Name == "Codigo" )
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
            finally {con.desconectar(); }
            if (contadorFalsos > 0)
            {
                return false;
            }
            else { return true; }
            
        }
        private bool comprobarExistenciaSQL(string CodProducto, string Cantidad, string nombre)
        {
            con.conectarSqlS();
            int StockDeAmazon = 0;
            bool condi = false;
            try
            {
                string qry = "select detalle from AmazonXML where codigo = " + CodProducto + "";
                SqlCommand cmd = new SqlCommand(qry, con.consql);
                SqlDataReader readsql = cmd.ExecuteReader();
                string strxml = "";
                if (readsql.Read())
                {
                    strxml = "" + readsql["detalle"];
                    StreamWriter file = new StreamWriter(con.path + "ComprobarCantidad.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "ComprobarCantidad.xml");
                    XmlNode nodo = doc.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);
                        if (nodo1.Name == "Stock")
                        {
                            StockDeAmazon = Convert.ToInt32(nodo1.InnerText);
                            if (Convert.ToInt32(Cantidad) > StockDeAmazon)
                            {
                                condi = false;
                                con.MensajeNormal("No hay sificiente cantidad de: " + nombre);
                            }else{condi = true;}
                        }
                    }
                }
                readsql.Close();
            }
            catch (Exception Ex)
            {
                condi = false;
                StockDeAmazon = 0;
            }
            finally { con.desconectarSqlS(); }
            return condi;
        }
        private void Facturar(string subtotal)
        {
            int numfactura = NumMaxFactura();
            con.conectar();
            try
            {
                string query = "InsertarFactura";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter prmNumero = new OracleParameter();
                prmNumero.ParameterName = "num";
                prmNumero.OracleDbType = OracleDbType.Int32;
                prmNumero.Value = numfactura;
                cmd.Parameters.Add(prmNumero);

                OracleParameter prmCed = new OracleParameter();
                prmCed.ParameterName = "ced";
                prmCed.OracleDbType = OracleDbType.Int32;
                prmCed.Value = Convert.ToInt32(deGrid1(2));
                cmd.Parameters.Add(prmCed);

                OracleParameter prmFecha = new OracleParameter();
                prmFecha.ParameterName = "fech";
                DateTime fecha = DateTime.Now;//fecha del momento
                prmFecha.OracleDbType = OracleDbType.Date;
                prmFecha.Value = fecha;
                cmd.Parameters.Add(prmFecha);
                
                OracleParameter prmDeta = new OracleParameter();
                prmDeta.ParameterName = "deta";
                prmDeta.OracleDbType = OracleDbType.XmlType;
                //********************************************************* ;
                string qry = "select detalle from pedido where numero_pedido = "+deGrid1(1)+" and Estado = 'Pendiente'";
                OracleCommand cmd2 = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];

                    readoracle.Close();
                    prmDeta.Value = strxml;
                    cmd.Parameters.Add(prmDeta);
                    OracleParameter prmSubto = new OracleParameter();
                    prmSubto.ParameterName = "subto";
                    prmSubto.OracleDbType = OracleDbType.Int32;
                    prmSubto.Value = Convert.ToInt32(subtotal);
                    cmd.Parameters.Add(prmSubto);

                    cmd.ExecuteNonQuery();
                    con.MensajeNormal("Se ha Facturado Correctamente");
                    //actualizar stocks de todos los productos
                    StreamWriter file = new StreamWriter(con.path + "UpdtStocs.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument docStocks = new XmlDocument();
                    docStocks.Load(con.path + "UpdtStocs.xml");
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
                                actualizarStockAmazon(codComprobar, nodo2.InnerText, "-");
                            }
                        }
                    }
                    updatePedidoARealizado(deGrid1(1));
                    MostrarPedidos();
                    con.MensajeNormal("El Subtotal de esta factura es: " + subtotal);
                }
                else { con.MensajeNormal("La Factura ya está realizada!!"); }
                
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private int NumMaxFactura()
        {
            con.conectar();
            string strnum = "";
            int numero = 1;
            try
            {
                string query = "select max(numero_factura) max from factura";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                OracleDataReader lee = cmd.ExecuteReader();
                if (lee.Read())
                {
                    strnum = "" + lee["max"];
                    numero = Convert.ToInt32(strnum);
                    numero++;
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
            return numero;
        }
        private void actualizarStockAmazon(string codigo, string CantidadActualizar, string signo)
        {//modificado para sqlserver terminado
            con.conectarSqlS();
            try
            {
                string qry = "select detalle from AmazonXML where Codigo = " + codigo + "";
                SqlCommand cmd = new SqlCommand(qry, con.consql);
                SqlDataReader readosql = cmd.ExecuteReader();
                string strxml = "";
                if (readosql.Read())
                {
                    strxml = "" + readosql["detalle"];
                    readosql.Close();
                    StreamWriter file = new StreamWriter(con.path + "ActualizarStock.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "ActualizarStock.xml");
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
                    string qryr2 = "ActualizaDetAmazon";
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
        private void updatePedidoARealizado(string NumPedido)
        {
            con.conectar();
            try
            {
                string query = "update pedido set Estado = 'Realizado' where Numero_Pedido = "+NumPedido+"";
                OracleCommand cmd = new OracleCommand(query,con.conex);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void leeYcargaImagen(string codigoProducto)
        {
            con.conectarSqlS();
            try
            {
                string query = "select Imagen from AmazonXML where Codigo = " + codigoProducto + "";
                SqlCommand cmd = new SqlCommand(query, con.consql);
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                //con.MensajeNormal("Largo: "+bytes.LongLength);
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectarSqlS(); }
        }

        protected void btnDVerOrdenes_Click(object sender, EventArgs e)
        {
            MostrarOrdenes();
            GridView2.DataSource = null;
            GridView2.DataBind();
            Image1.Visible = false;
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
                    StreamWriter file = new StreamWriter(con.path + "detalleOrden.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "detalleOrden.xml");
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
        private void AlmacenarProductosDeOrden(string numOrdenl)  //*********************** estado suministrado
        {
            con.conectar();
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + deGrid1(1) + " and Estado = 'Suministrado'";
                OracleCommand cmd2 = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];

                    readoracle.Close();
                    //actualizar stocks de todos los productos
                    StreamWriter file = new StreamWriter(con.path + "UpdtStocsOrden.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument docStocks = new XmlDocument();
                    docStocks.Load(con.path + "UpdtStocsOrden.xml");
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
                                actualizarStockAmazon(codComprobar, nodo2.InnerText, "+");
                            }
                        }
                    }
                    updateOrdenARealizado(deGrid1(1)); 
                    MostrarOrdenes();
                    con.MensajeNormal("Se han almacenado los productos recibidos del distribuidor");
                }
                else { con.MensajeNormal("La Orden no está para almacenar!"); }

            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void updateOrdenARealizado(string NumPedido)
        {
            con.conectar();
            try
            {
                string query = "update orden_compra set Estado = 'Realizado' where numero_orden = " + NumPedido + "";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void MostrarOrdenes()
        {
            con.conectar();
            try
            {
                string query = "select numero_orden as \"Orden # \", fecha, codigo_distribuidor as Distribuidor, estado from orden_compra";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                OracleDataAdapter adap = new OracleDataAdapter(cmd);
                DataTable tb = new DataTable("Ordenes");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblTituloGrid.Text = "Ordenes de Compra";
                lblTituloGrid2.Text = "Detalle de Orden";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LoginVendedor.aspx");
        }
        
    }
}