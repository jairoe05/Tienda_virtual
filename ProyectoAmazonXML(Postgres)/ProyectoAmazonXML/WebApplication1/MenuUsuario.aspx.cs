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
using System.Data;

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
                    else { con.MensajeNormal("Debe Elegir ver Prodcutos!!", Label1); }                  
                }
                else { con.MensajeNormal("Debe Seleccionar un producto", Label1); }                
            }
            else { con.MensajeNormal("Debe Iniciar La Compra Antes", Label1); }            
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
                    else { con.MensajeNormal("Debe Debe Elegir ver productos", Label1); }
                }
                else { con.MensajeNormal("Debe Seleccionar un producto antes", Label1); }                  
            }
            else { con.MensajeNormal("Debe Iniciar La Compra Antes", Label1); }   
        }
        private void cargarUsuario()
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "select CEDULA, NOMBRE from login where numero = (select max(numero) from login)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                NpgsqlDataReader read = cmd.ExecuteReader();
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
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
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
                else { con.MensajeNormal("El pedido no puede estar vacio!!", Label1); }
            }
            else { con.MensajeNormal("Debe Iniciar La Compra Antes", Label1); }
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            leeYcargaImagen(deGrid2(1));
        }
        protected void btnCerrarsesion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
        }
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
            catch (Exception Ex) { con.MensajeError(Ex, Label1); }
            finally { con.desconectarPOSTGRE(); }
        }
        private void leeYcargaGridCarrito() //une varias culumnas xml en una
        {
            con.conectar();
            try
            {
                string qry = "select detalle from carrito";
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
                while (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
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
                GridView2.DataSource = dt;
                GridView2.DataBind();
                readoracle.Close();
            }
            catch (Exception Ex) { con.MensajeError(Ex, Label1); }
            finally { con.desconectarPOSTGRE(); }
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
            catch (Exception Ex) { con.MensajeError(Ex, Label1); }
            finally { con.desconectarPOSTGRE(); }
        }
        private void agregaAcarrito(string pcodigo, string pnombre, string pcategoria, string pprecio, string cantidad)
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "InsertarCarrito";
                NpgsqlParameter cmdxm = new NpgsqlParameter(query, con.Postgrecon);
                cmdxm.CommandType = CommandType.StoredProcedure;
                NpgsqlParameter prmNum = new NpgsqlParameter();
                prmNum.ParameterName = "num";
                prmNum.NpgsqlDbType = NpgsqlDbType.Integer;
                prmNum.Value = Convert.ToInt32(pcodigo);
                cmdxm.Parameters.Add(prmNum);
                NpgsqlParameter prmDetalle = new NpgsqlParameter();
                prmDetalle.ParameterName = "deta";
                prmDetalle.NpgsqlDbType = NpgsqlDbType.Xml;

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
                con.MensajeNormal("Producto Añadido al carrito", Label1);
            }
            catch (Exception Ex)
            {
                con.MensajeNormal("Ya ha agregado el producto indicado\nPuede agregar otro", Label1);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        private void DevolverProducto()
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "delete from carrito where numero_carrito = " + deGrid2(1) + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                cmd.ExecuteNonQuery();
                con.MensajeNormal("El producto ha sido devuelto", Label1);
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        private void truncateCarrito()
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "truncate table carrito";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        private void agregarPedido()
        {
            int NumPedido = NumMaxPedi();
            con.conectarPOSTGRE();
            try
            {
                string query = "InsertarPedido";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                cmd.CommandType = CommandType.StoredProcedure;
                NpgsqlParameter prmNumero = new NpgsqlParameter();
                prmNumero.ParameterName = "num";
                prmNumero.NpgsqlDbType = NpgsqlDbType.Integer;
                prmNumero.Value = NumPedido;
                cmd.Parameters.Add(prmNumero);
                NpgsqlParameter prmCedula = new NpgsqlParameter();
                prmCedula.ParameterName = "ced";
                prmCedula.NpgsqlDbType = NpgsqlDbType.Integer;
                prmCedula.Value = Convert.ToInt32(lblCedula.Text);
                cmd.Parameters.Add(prmCedula);
                NpgsqlParameter prmFecha = new NpgsqlParameter();
                prmFecha.ParameterName = "fech";
                DateTime fecha = DateTime.Now;
                prmFecha.NpgsqlDbType = NpgsqlDbType.Date;
                prmFecha.Value = fecha;
                cmd.Parameters.Add(prmFecha);
                NpgsqlParameter prmDeta = new NpgsqlParameter();
                prmDeta.ParameterName = "deta";
                prmDeta.NpgsqlDbType = NpgsqlDbType.Xml;
                //*********************************************************
                XmlDocument doc = new XmlDocument();
                XmlElement pedido = doc.CreateElement("Pedido");
                doc.AppendChild(pedido);

                string qry = "select detalle from carrito";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                string[] nombrecolumna = { "Codigo", "Nombre", "Categoria", "Precio", "Cantidad" };
                while (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.LoadXml(strxml);
                    XmlNode nodo = doccarri.DocumentElement;
                    XmlElement Producto = doc.CreateElement(nodo.Name);
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

                NpgsqlParameter prmEstad = new NpgsqlParameter();
                prmEstad.ParameterName = "estad";
                prmEstad.NpgsqlDbType = NpgsqlDbType.Char;
                prmEstad.Size = 30;
                prmEstad.Value = "Pendiente";
                cmd.Parameters.Add(prmEstad);
                cmd.ExecuteNonQuery();
                con.MensajeNormal("El pedido se ha enviado correctamente",Label1);
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex,Label1);
            }
            finally { con.desconectarPOSTGRE(); }
        }
        private int NumMaxPedi()
        {
            con.conectarPOSTGRE();
            string strnum = "";
            int numero = 1;
            try
            {
                string query ="select max(NUMERO_PEDIDO) max from pedido";
                NpgsqlCommand cmd = new NpgsqlCommand(query,con.Postgrecon);
                NpgsqlDataReader lee = cmd.ExecuteReader();
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
            finally { con.desconectarPOSTGRE(); }
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
                    con.MensajeNormal("La Cantidad Debe ser positiva", Label1);
                }
                else
                {
                    con.conectarPOSTGRE();
                    int StockDeAmazon = 0;
                    try
                    {
                        string qry = "select detalle from AmazonXML where codigo = "+deGrid1(1)+"";
                        NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                        NpgsqlDataReader readsql= cmd.ExecuteReader();
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
                    finally { con.desconectarPOSTGRE(); }
                    if (numero > StockDeAmazon)
                    {
                        con.MensajeNormal("No Hay Suficiente Cantidad del producto", Label1);
                        respuesta = false;
                    }
                    else { respuesta = true; }
                }
                
            }
            catch (Exception Ex)
            {
                respuesta = false;
                con.MensajeNormal("La Cantidad debe ser  un número Entero", Label1);
            }
            return respuesta;
        }
        private void actualizarStockAmazon(string codigo,string CantidadActualizar, string signo)
        {//modificado para sqlserver terminado
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from AmazonXML where Codigo = " + codigo + "";
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
                    string qryr2 = "ActualizaDetAmazon";
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
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
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
            else { con.MensajeNormal("Debe Iniciar el proceso para consultar", Label1); }
        }
        private void VerMisPedidos()
        {
            con.conectarPOSTGRE();
            try
            {
                string query = "select numero_pedido, cedula_cliente, fecha, Estado from pedido where cedula_cliente = "+lblCedula.Text+"";
                NpgsqlCommand cmd = new NpgsqlCommand(query,con.Postgrecon);
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);

                DataTable tb = new DataTable("MisPedidos");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblgrid1.Text = "Pedidos";
                lblgrid2.Text = "Detalle de Pedido";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
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
            con.conectarPOSTGRE();
            try
            {
                string qry = "select detalle from pedido where numero_pedido = " + numeropedido + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
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
            catch (Exception Ex) { con.MensajeError(Ex, Label1); }
            finally { con.desconectarPOSTGRE(); }
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
            else { con.MensajeNormal("Debe Iniciar el proceso para consultar", Label1); }
        }
        private void VerMisFacturas()
        {
            con.conectar();
            try
            {
                string query = "select numero_factura, cedula_cliente, fecha, Subtotal from factura where cedula_cliente = " + lblCedula.Text + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con.Postgrecon);
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("MisFacturas");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
                lblgrid1.Text = "Facturas";
                lblgrid2.Text = "Detalle de Factura";
            }
            catch (Exception Ex)
            {
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
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
            catch (Exception Ex) { con.MensajeError(Ex, Label1); }
            finally { con.desconectarPOSTGRE(); }
        }
        private int CuentaCarrito()
        {
            con.conectarPOSTGRE();
            int Cantidad = 0;
            try
            {
                string qry = "select count(numero_carrito) as Contador from carrito";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, con.Postgrecon);
                NpgsqlDataReader lee = cmd.ExecuteReader();
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
                con.MensajeError(Ex, Label1);
            }
            finally { con.desconectarPOSTGRE(); }
            return Cantidad;
        }
    }
}