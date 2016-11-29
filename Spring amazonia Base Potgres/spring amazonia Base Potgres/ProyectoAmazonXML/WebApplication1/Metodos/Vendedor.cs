using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
using Npgsql;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using NpgsqlTypes;

namespace WebApplication1.Metodos
{
    public class Vendedor : IVendedor
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        public bool ComprobarDatos(string codvendedor,string pass,Label Label1)
        {            
            bool entrar = false;
            try
            {
                string query = "select nombre, Apellidos from Vendedor where Codigo =" + codvendedor + " and Pass = '" + pass + "'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataReader lee = cmd.ExecuteReader();
                string nomb = "",ape="";
                if (lee.Read())
                {
                    nomb = "" + lee["nombre"];
                    ape = "" + lee["Apellidos"];
                    entrar = true;
                    objconexion.MensajeNormal("Bienvenido: " + nomb+" "+ape, Label1);
                }
                else
                {
                    objconexion.MensajeNormal("Datos incorrectos! \nIntentelo de nuevo", Label1);
                    entrar = false;
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return entrar;
        }
        public void leeYcargaGridProductos(GridView GridView1, Label Label1)
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void MostrarPedidos(GridView GridView1, Label Label1)
        {            
            try
            {
                string qry = "select numero_pedido as \"Pedido #\", cedula_cliente as Cedula, Fecha, Estado from pedido";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("Pedidos");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
            }
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYCargaDetallePedido(string numeropedido, GridView GridView2,Label Label1)
        {           
            try
            {
                string qry = "select detalle from pedido where numero_pedido = " + numeropedido + "";
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void agregarDetalleOrden(string pcodigo, string pnombre, string pcategoria, string pprecio, string pcantidad, Label Label1)
        {            
            try
            {
                string query = "InsertarDetalleOrden";
                NpgsqlCommand cmdxm = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
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
                Cantidad.AppendChild(doc.CreateTextNode(pcantidad));
                Carrito.AppendChild(Cantidad);

                prmDetalle.Value = doc.OuterXml.ToString();
                cmdxm.Parameters.Add(prmDetalle);
                cmdxm.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeNormal("Ya ha pedido el producto indicado\nPuede pedir otro", Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        private int NumMaxOrden(Label Label1)
        {            
            string strnum = "";
            int numero = 1;
            try
            {
                string query = "select max(numero_orden) max from orden_Compra";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataReader lee = cmd.ExecuteReader();
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
                objconexion.MensajeError(Ex, Label1);
                numero = 1;
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return numero;
        }
        public void enviarOrden(string coddistribuidor, Label Label1, Label lblIniciaCompra)
        {
            int NumOrden = NumMaxOrden(Label1);            
            try
            {
                string query = "InsertarOrdenDeCompra";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.CommandType = CommandType.StoredProcedure;
                NpgsqlParameter prmNumero = new NpgsqlParameter();
                prmNumero.ParameterName = "num";
                prmNumero.NpgsqlDbType = NpgsqlDbType.Integer;
                prmNumero.Value = NumOrden;
                cmd.Parameters.Add(prmNumero);

                NpgsqlParameter prmFecha = new NpgsqlParameter();
                prmFecha.ParameterName = "fech";
                DateTime fecha = DateTime.Now;//fecha del momento
                prmFecha.NpgsqlDbType = NpgsqlDbType.Date;
                prmFecha.Value = fecha;
                cmd.Parameters.Add(prmFecha);
                NpgsqlParameter prmCodDistri = new NpgsqlParameter();
                prmCodDistri.ParameterName = "cod_distri";
                prmCodDistri.NpgsqlDbType = NpgsqlDbType.Integer;
                prmCodDistri.Value = Convert.ToInt32(coddistribuidor);
                cmd.Parameters.Add(prmCodDistri);
                NpgsqlParameter prmDeta = new NpgsqlParameter();
                prmDeta.ParameterName = "deta";
                prmDeta.NpgsqlDbType = NpgsqlDbType.Xml;
                //*********************************************************
                XmlDocument doc = new XmlDocument();
                XmlElement Orden = doc.CreateElement("Orden");
                doc.AppendChild(Orden);

                string qry = "select detalle from CarritoOrdenCompra";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
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
                    Orden.AppendChild(Producto);
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    { 
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
                objconexion.MensajeNormal("La Orden de Compra Se ha Enviado Correctamente", Label1);
                truncateCarritoOrden(Label1);
                lblIniciaCompra.Text = ".";
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void truncateCarritoOrden(Label Label1)
        {            
            try
            {
                string query = "truncate table carritoordencompra";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYcargaGridCarritoOrden(GridView GridView2, Label Label1)
        {            
            try
            {
                string qry = "select detalle from carritoordencompra";
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public int sacarSubTotal(string numpedido, Label Label1)
        {
            
            int Subtotal = 0;
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement Factura = doc.CreateElement("Factura");
                doc.AppendChild(Factura);

                string qry = "select detalle from pedido where numero_pedido = " + numpedido + "";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";

                int AcumuladoXProducto = 0;
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.LoadXml(strxml);
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
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return Subtotal;
        }
        public void leeYCargaDetalleOrden(string numeroOrden, GridView GridView2, Label Label1)
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void AlmacenarProductosDeOrden(string numOrdenl, GridView GridView1, Label lblTituloGrid, Label lblTituloGrid2, Label Label1)  //*********************** estado suministrado
        {
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + numOrdenl + " and Estado = 'Suministrado'";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];

                    readoracle.Close();
                    //actualizar stocks de todos los productos
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.LoadXml(strxml);
                    XmlNode nodo = doccarri.DocumentElement;
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
                                actualizarStockAmazon(codComprobar, nodo2.InnerText, "+",Label1);
                            }
                        }
                    }
                    updateOrdenARealizado(numOrdenl,Label1);
                    MostrarOrdenes(GridView1,Label1,lblTituloGrid,lblTituloGrid2);
                    objconexion.MensajeNormal("Se han almacenado los productos recibidos del distribuidor", Label1);
                }
                else { objconexion.MensajeNormal("La Orden no está para almacenar!", Label1); }

            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void updateOrdenARealizado(string NumPedido, Label Label1)
        {            
            try
            {
                string query = "update orden_compra set Estado = 'Realizado' where numero_orden = " + NumPedido + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void MostrarOrdenes(GridView GridView1, Label Label1, Label lblTituloGrid, Label lblTituloGrid2)
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
                lblTituloGrid.Text = "Ordenes de Compra";
                lblTituloGrid2.Text = "Detalle de Orden";
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void actualizarStockAmazon(string codigo, string CantidadActualizar, string signo, Label Label1)
        {//modificado para sqlserver terminado

            try
            {
                string qry = "select detalle from amazonxml where Codigo = " + codigo + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
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
                        if (nodo1.Name == "Cantidad")
                        {
                            int StockNum = Convert.ToInt32(nodo1.InnerText);
                            if (signo == "+") { StockNum = StockNum + Convert.ToInt32(CantidadActualizar); }
                            if (signo == "-") { StockNum = StockNum - Convert.ToInt32(CantidadActualizar); }
                            nodo1.InnerText = Convert.ToString(StockNum);
                        }
                    }
                    //********
                    string qryr2 = "ActualizaDetAmazon";
                    NpgsqlCommand cmdr2 = new NpgsqlCommand(qryr2, objconexion.conectarPOSTGRE());
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
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void updatePedidoARealizado(string NumPedido, Label Label1)
        {
            try
            {
                string query = "update pedido set Estado = 'Realizado' where Numero_Pedido = " + NumPedido + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYcargaImagen(string codigoProducto, Image Image1, Label Label1)
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public bool ExisteDistribuidor(string codDistribuidor, Label Label1)
        {
            bool condi = false;
            try
            {
                string query = "select Nombre as Distribuidor from Distribuidor where Cod_distribuidor = " + codDistribuidor + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataReader lee = cmd.ExecuteReader();
                string Distri = "";
                if (lee.Read())
                {
                    Distri = "" + lee["Distribuidor"];
                    condi = true;
                    objconexion.MensajeNormal("Orden enviada al distribuidor  " + Distri, Label1);
                    lee.Close();
                }
                else { condi = false; }

            }
            catch (Exception Ex)
            {
                condi = false;
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return condi;
        }
        public bool ComprobarCantidadesFacturar(string NumPedido, Label Label1)   //pendiente
        {//**
            
            int contadorFalsos = 0;
            try
            {
                string qryOra = "select detalle from pedido where numero_pedido = " + NumPedido + "";
                NpgsqlCommand cmdOra = new NpgsqlCommand(qryOra, objconexion.conectarPOSTGRE());
                NpgsqlDataReader leeOra = cmdOra.ExecuteReader();
                string strxml = "";
                if (leeOra.Read())
                {
                    strxml = "" + leeOra["detalle"];
                    try { leeOra.Close(); }
                    catch (Exception edf) { }
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
                            {
                                if (!comprobarExistenciaSQL(codComprobar, nodo2.InnerText, nombreProd,Label1))
                                { contadorFalsos++; }
                            }
                        }
                    }
                }
                try { leeOra.Close(); }
                catch (Exception edf) { }
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            if (contadorFalsos > 0)
            {
                return false;
            }
            else { return true; }

        }
        public bool comprobarExistenciaSQL(string CodProducto, string Cantidad, string nombre, Label Label1)
        {            
            int StockDeAmazon = 0;
            bool condi = false;
            try
            {
                string qry = "select detalle from amazonxml where codigo = " + CodProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataReader readsql2 = cmd.ExecuteReader();
                string strxml = "";
                if (readsql2.Read())
                {
                    strxml = "" + readsql2["detalle"];
                    try { readsql2.Close(); }
                    catch (Exception ed) { }
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {//con.MensajeNormal(nodo1.InnerText);
                        if (nodo1.Name == "Cantidad")
                        {
                            StockDeAmazon = Convert.ToInt32(nodo1.InnerText);
                            if (Convert.ToInt32(Cantidad) > StockDeAmazon)
                            {
                                condi = false;
                                objconexion.MensajeNormal("No hay sificiente cantidad de: " + nombre, Label1);
                            }
                            else { condi = true; }
                        }
                    }
                }
                try { readsql2.Close(); } catch (Exception ed) { }
                
            }
            catch (Exception Ex)
            {
                condi = false;
                StockDeAmazon = 0;
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return condi;
        }
        public void Facturar(string cedulaClientedGrid1_2, string numpediDgrid1_1, string subtotal, Label Label1, GridView GridView1)
        {
            int numfactura = NumMaxFactura(Label1);
            try
            {
                string query = "InsertarFactura";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.CommandType = CommandType.StoredProcedure;
                NpgsqlParameter prmNumero = new NpgsqlParameter();
                prmNumero.ParameterName = "num";
                prmNumero.NpgsqlDbType = NpgsqlDbType.Integer;
                prmNumero.Value = numfactura;
                cmd.Parameters.Add(prmNumero);

                NpgsqlParameter prmCed = new NpgsqlParameter();
                prmCed.ParameterName = "ced";
                prmCed.NpgsqlDbType = NpgsqlDbType.Integer;
                prmCed.Value = Convert.ToInt32(cedulaClientedGrid1_2);
                cmd.Parameters.Add(prmCed);

                NpgsqlParameter prmFecha = new NpgsqlParameter();
                prmFecha.ParameterName = "fech";
                DateTime fecha = DateTime.Now;//fecha del momento
                prmFecha.NpgsqlDbType = NpgsqlDbType.Date;
                prmFecha.Value = fecha;
                cmd.Parameters.Add(prmFecha);

                NpgsqlParameter prmDeta = new NpgsqlParameter();
                prmDeta.ParameterName = "deta";
                prmDeta.NpgsqlDbType = NpgsqlDbType.Xml;
                //********************************************************* ;
                string qry = "select detalle from pedido where numero_pedido = " + numpediDgrid1_1 + " and Estado = 'Pendiente'";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataReader readoracle = cmd2.ExecuteReader();
                string strxml = "";
                if (readoracle.Read())
                {
                    strxml = "" + readoracle["detalle"];

                    readoracle.Close();
                    prmDeta.Value = strxml;
                    cmd.Parameters.Add(prmDeta);
                    NpgsqlParameter prmSubto = new NpgsqlParameter();
                    prmSubto.ParameterName = "subto";
                    prmSubto.NpgsqlDbType = NpgsqlDbType.Integer;
                    prmSubto.Value = Convert.ToInt32(subtotal);
                    cmd.Parameters.Add(prmSubto);

                    cmd.ExecuteNonQuery();
                    objconexion.MensajeNormal("Se ha Facturado Correctamente", Label1);
                    //actualizar stocks de todos los productos
                    XmlDocument doccarri = new XmlDocument();
                    doccarri.LoadXml(strxml);
                    XmlNode nodo = doccarri.DocumentElement;
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
                                actualizarStockAmazon(codComprobar, nodo2.InnerText, "-",Label1);
                            }
                        }
                    }
                    updatePedidoARealizado(numpediDgrid1_1,Label1);
                    MostrarPedidos(GridView1,Label1);
                }
                else { objconexion.MensajeNormal("La Factura ya está realizada!!", Label1); }

            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        private int NumMaxFactura(Label Label1)
        {
            string strnum = "";
            int numero = 1;
            try
            {
                string query = "select max(numero_factura) max from factura";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataReader lee = cmd.ExecuteReader();
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
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return numero;
        }
    }
}