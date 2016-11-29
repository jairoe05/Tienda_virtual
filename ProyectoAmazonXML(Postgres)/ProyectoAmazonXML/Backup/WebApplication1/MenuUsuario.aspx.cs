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

namespace WebApplication1
{
    public partial class MenuUsuario : System.Web.UI.Page
    {
        conexion con = new conexion();

        protected void Page_Load(object sender, EventArgs e)
        {
            //leeYcargaGridProductos();
        }
        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            cargarUsuario();
            truncateCarrito();
            leeYcargaGridProductos();
        }
        
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (lblCedula.Text != ".")
            {
                if (deGrid1(1) != ".")
                {
                    if (lblgrid1.Text == "Productos")
                    {
                        if (ValidarCantidad())
                        {
                            agregaAcarrito(deGrid1(1), deGrid1(2), deGrid1(3), deGrid1(4), txtCantidad.Text);
                            actualizarStockAmazon(deGrid1(1), txtCantidad.Text, "-");
                            leeYcargaGridCarrito();
                            leeYcargaGridProductos();
                        }
                    }
                    else { con.MensajeNormal("Debe Elegir ver Prodcutos!!"); }                  
                }
                else { con.MensajeNormal("Debe Seleccionar un producto"); }                
            }
            else { con.MensajeNormal("Debe Iniciar La Compra Antes");}            
        }        
        
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblgrid1.Text =="Productos")
            {
                leeYcargaImagen(deGrid1(1));
            }
            
            if (lblgrid1.Text =="Pedidos")
            {
                leeYCargaDetallePedido(deGrid1(1));
            }
            if (lblgrid1.Text == "Facturas")
            {
                leeYCargaDetalleFactura(deGrid1(1));
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
                        DevolverProducto();
                        actualizarStockAmazon(deGrid2(1), deGrid2(5), "+");
                        leeYcargaGridCarrito();
                        leeYcargaGridProductos();
                    }
                    else { con.MensajeNormal("Debe Debe Elegir ver productos"); }
                }
                else { con.MensajeNormal("Debe Seleccionar un producto antes"); }                  
            }
            else { con.MensajeNormal("Debe Iniciar La Compra Antes"); }   
        }
        private void cargarUsuario()
        {
            con.conectar();
            try
            {
                string query = "select CEDULA, NOMBRE from login where numero = (select max(numero) from login)";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                OracleDataReader read = cmd.ExecuteReader();
                if (read.Read())
                {
                    lblCedula.Text = "" + read["Cedula"];
                    lblIniciarCompra.Text = "Cliente: " + read["Nombre"];
                }
                read.Close();
            }
            catch (Exception Ex)
            {
                lblCedula.Text = "";
                lblIniciarCompra.Text = "";
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }          
        protected void btnConcluir_Click(object sender, EventArgs e)
        {            
            if (lblCedula.Text != ".")
            {
                if (CuentaCarrito() > 0)
                {
                    agregarPedido();
                    truncateCarrito();
                    GridView2.DataSource = null;
                    GridView2.DataBind();
                    lblgrid1.Text = ".";
                    lblgrid2.Text = ".";
                    Image1.Visible = false;
                }
                else { con.MensajeNormal("El pedido no puede estar vacio!!"); }
            }
            else { con.MensajeNormal("Debe Iniciar La Compra Antes"); }
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            leeYcargaImagen(deGrid2(1));
        }
        protected void btnCerrarsesion_Click(object sender, EventArgs e)
        {
            //con.MensajeNormal("Cantidd de rows: "+GridView2.Rows.Count);
            Response.Redirect("~/login.aspx");
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
                    StreamWriter file = new StreamWriter(con.path+"ProductoIndividual.xml");
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
        private void leeYcargaGridCarrito() //une varias culumnas xml en una
        {
            con.conectar();
            try
            {
                string qry = "select detalle from carrito";
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
                    StreamWriter file = new StreamWriter(con.path + "CarritoIndividual.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "CarritoIndividual.xml");
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
        private void leeYcargaImagen(string codigoProducto)
        {
            con.conectarSqlS();
            try
            {
                string query = "select Imagen from AmazonXML where Codigo = " + codigoProducto + "";
                SqlCommand cmd = new SqlCommand(query, con.consql);
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { con.MensajeError(Ex); }
            finally { con.desconectarSqlS(); }
        }
        private void agregaAcarrito(string pcodigo, string pnombre, string pcategoria, string pprecio, string cantidad)
        {
            con.conectar();
            try
            {
                string query = "InsertarCarrito";
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
                Cantidad.AppendChild(doc.CreateTextNode(cantidad));
                Carrito.AppendChild(Cantidad);
                
                prmDetalle.Value = doc.OuterXml.ToString();
                cmdxm.Parameters.Add(prmDetalle);
                cmdxm.ExecuteNonQuery();
                con.MensajeNormal("Producto Añadido al carrito");
            }
            catch (Exception Ex)
            {
                con.MensajeNormal("Ya ha agregado el producto indicado\nPuede agregar otro");
            }
            finally { con.desconectar(); }
        }
        private void DevolverProducto()
        {
            con.conectar();
            try
            {
                string query = "delete from carrito where numero_carrito = " + deGrid2(1) + "";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.ExecuteNonQuery();
                con.MensajeNormal("El producto ha sido devuelto");
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void truncateCarrito()
        {
            con.conectar();
            try
            {
                string query = "truncate table carrito";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void agregarPedido()
        {
            int NumPedido =NumMaxPedi();
            con.conectar();
            try
            {
                string query = "InsertarPedido";
                OracleCommand cmd = new OracleCommand(query,con.conex);
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter prmNumero = new OracleParameter();
                prmNumero.ParameterName = "num";
                prmNumero.OracleDbType = OracleDbType.Int32;
                prmNumero.Value = NumPedido;
                cmd.Parameters.Add(prmNumero);
                OracleParameter prmCedula = new OracleParameter();
                prmCedula.ParameterName = "ced";
                prmCedula.OracleDbType = OracleDbType.Int32;
                prmCedula.Value = Convert.ToInt32(lblCedula.Text) ;
                cmd.Parameters.Add(prmCedula);
                OracleParameter prmFecha = new OracleParameter();
                prmFecha.ParameterName = "fech";
                DateTime fecha = DateTime.Now;
                prmFecha.OracleDbType = OracleDbType.Date;
                prmFecha.Value = fecha;
                cmd.Parameters.Add(prmFecha);
                OracleParameter prmDeta = new OracleParameter();
                prmDeta.ParameterName = "deta";
                prmDeta.OracleDbType = OracleDbType.XmlType;
                //*********************************************************
                XmlDocument doc = new XmlDocument();
                XmlElement pedido = doc.CreateElement("Pedido");
                doc.AppendChild(pedido);

                string qry = "select detalle from carrito";
                OracleCommand cmd2 = new OracleCommand(qry, con.conex);
                OracleDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                string[] nombrecolumna = { "Codigo", "Nombre", "Categoria", "Precio", "Cantidad" };
                while (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    StreamWriter file = new StreamWriter(con.path + "CarritoAPedido.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.Load(con.path + "CarritoAPedido.xml");
                    XmlNode nodo = doccarri.DocumentElement;

                    XmlElement Producto = doc.CreateElement(nodo.Name);
                    //Producto.AppendChild(doc.CreateTextNode(nodo.InnerText));
                    pedido.AppendChild(Producto);
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
                con.MensajeNormal("El pedido se ha enviado correctamente\nRecibirá respuesta lo antes posible");
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private int NumMaxPedi()
        {
            con.conectar();
            string strnum = "";
            int numero = 1;
            try
            {
                string query ="select max(NUMERO_PEDIDO) max from pedido";
                OracleCommand cmd = new OracleCommand(query,con.conex);
                OracleDataReader lee = cmd.ExecuteReader();
                if (lee.Read())
                {
                    strnum = ""+lee["max"];
                    numero =Convert.ToInt32(strnum);
                    numero++;
                    
                }
                lee.Close();
            }
            catch (Exception)
            {
                numero = 1;
            }
            finally { con.desconectar(); }
            return numero;
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
        public string deGrid2(int num)
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
        private bool ValidarCantidad()
        {
            bool respuesta = false;
            try
            {
                int numero = Convert.ToInt32(txtCantidad.Text);
                if (numero<1)
                {
                    respuesta = false;
                    con.MensajeNormal("La Cantidad Debe ser positiva");
                }
                else
                {
                    con.conectarSqlS();
                    int StockDeAmazon = 0;
                    try
                    {
                        string qry = "select detalle from AmazonXML where codigo = "+deGrid1(1)+"";
                        SqlCommand cmd = new SqlCommand(qry, con.consql);
                        SqlDataReader readsql= cmd.ExecuteReader();
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
                                }                                
                            }
                        }
                        readsql.Close();
                    }
                    catch (Exception Ex)
                    {
                        StockDeAmazon = 0;
                    }
                    finally { con.desconectarSqlS(); }
                    if (numero > StockDeAmazon)
                    {
                        con.MensajeNormal("No Hay Suficiente Cantidad del producto");
                        respuesta = false;
                    }
                    else { respuesta = true; }
                }
                
            }
            catch (Exception Ex)
            {
                respuesta = false;
                con.MensajeNormal("La Cantidad debe ser  un número Entero");
            }
            return respuesta;
        }
        private void actualizarStockAmazon(string codigo,string CantidadActualizar, string signo)
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

        protected void btnVerMisPedidos_Click(object sender, EventArgs e)
        {
            if (lblCedula.Text != ".")
            {
                VerMisPedidos();
                Image1.Visible = false;
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
            else { con.MensajeNormal("Debe Iniciar el proceso para consultar"); }
        }
        private void VerMisPedidos()
        {
            con.conectar();
            try
            {
                string query = "select numero_pedido, cedula_cliente, fecha, Estado from pedido where cedula_cliente = "+lblCedula.Text+"";
                OracleCommand cmd = new OracleCommand(query,con.conex);
                OracleDataAdapter adap = new OracleDataAdapter(cmd);

                DataTable tb = new DataTable("MisPedidos");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblgrid1.Text = "Pedidos";
                lblgrid2.Text = "Detalle de Pedido";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }

        protected void btnVerProductos_Click(object sender, EventArgs e)
        {
            leeYcargaGridProductos();
            lblgrid1.Text = "Productos";
            lblgrid2.Text = "Carrito";
            Image1.Visible = true;
            GridView2.DataSource = null;
            GridView2.DataBind();
        }
        private void leeYCargaDetallePedido(string numeropedido)
        {
            con.conectar();
            try
            {
                string qry = "select detalle from pedido where numero_pedido = " + numeropedido + "";
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

        protected void btnverFacturas_Click(object sender, EventArgs e)
        {
            if (lblCedula.Text != ".")
            {
                VerMisFacturas();
                Image1.Visible = false;
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
            else { con.MensajeNormal("Debe Iniciar el proceso para consultar"); }
        }
        private void VerMisFacturas()
        {
            con.conectar();
            try
            {
                string query = "select numero_factura, cedula_cliente, fecha, Subtotal from factura where cedula_cliente = " + lblCedula.Text + "";
                OracleCommand cmd = new OracleCommand(query, con.conex);
                OracleDataAdapter adap = new OracleDataAdapter(cmd);

                DataTable tb = new DataTable("MisFacturas");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblgrid1.Text = "Facturas";
                lblgrid2.Text = "Detalle de Factura";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
        }
        private void leeYCargaDetalleFactura(string numeroFactura)
        {
            con.conectar();
            try
            {
                string qry = "select detalle from factura where numero_factura = " + numeroFactura + "";
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
                    StreamWriter file = new StreamWriter(con.path + "FacturaIndividual.xml");
                    file.WriteLine(strxml);
                    file.Close();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(con.path + "FacturaIndividual.xml");
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
        private int CuentaCarrito()
        {
            con.conectar();
            int Cantidad = 0;
            try
            {
                string qry = "select count(numero_carrito) as Contador from carrito";
                OracleCommand cmd = new OracleCommand(qry, con.conex);
                OracleDataReader lee = cmd.ExecuteReader();
                string numstr = "";
                if (lee.Read())
                {
                    numstr = "" + lee["Contador"];
                    lee.Close();
                    Cantidad = Convert.ToInt32(numstr);
                }
            }
            catch (Exception Ex)
            {
                Cantidad = 0;
                con.MensajeError(Ex);
            }
            finally { con.desconectar(); }
            return Cantidad;
        }
    }
}