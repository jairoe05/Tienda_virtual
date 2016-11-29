using System;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using Spring.Context;
using Spring.Context.Support;
using System.Web.UI.WebControls;
using System.Xml;
namespace WebApplication1.Metodos
{
    public class Usuario : IUsuario
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        public bool ComprobarDatos(string cedula, string pass, Label Label1)
        {
            
            NpgsqlConnection con = objconexion.conectarPOSTGRE();
            bool entrar = false;
            string rol = "";
            string nomb = "";
            try
            {
                string query = "select Cedula, nombre as Cliente from Usuario where cedula =" + cedula + " and Pass = '" + pass + "'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataReader lee = cmd.ExecuteReader();

                if (lee.Read())
                {
                    rol = "" + lee["Cedula"];
                    nomb = "" + lee["Cliente"];
                    entrar = true;
                   // InsertarLogin(nomb);
                }
                else
                {
                    entrar = false;
                    objconexion.MensajeNormal("Datos incorrectos! \nIntentelo de nuevo", Label1);
                }
                lee.Close();
            }
            catch (Exception Ex)
            {
                entrar = false;
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return entrar;
        }
        public void agregarPedido(string pCedula, Label Label1)
        {
            int NumPedido = NumMaxPedi();
            NpgsqlConnection con = objconexion.conectarPOSTGRE();
            try
            {
                string query = "InsertarPedido";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                NpgsqlParameter prmNumero = new NpgsqlParameter();
                prmNumero.ParameterName = "num";
                prmNumero.NpgsqlDbType = NpgsqlDbType.Integer;
                prmNumero.Value = NumPedido;
                cmd.Parameters.Add(prmNumero);
                NpgsqlParameter prmCedula = new NpgsqlParameter();
                prmCedula.ParameterName = "ced";
                prmCedula.NpgsqlDbType = NpgsqlDbType.Integer;
                prmCedula.Value = Convert.ToInt32(pCedula);
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
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, con);
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
                objconexion.MensajeNormal("El pedido se ha enviado correctamente", Label1);
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        private int NumMaxPedi()
        {
            NpgsqlConnection con = objconexion.conectarPOSTGRE();
            string strnum = "";
            int numero = 1;
            try
            {
                string query = "select max(NUMERO_PEDIDO) max from pedido";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataReader lee = cmd.ExecuteReader();
                if (lee.Read())
                {
                    strnum = "" + lee["max"];
                    numero = Convert.ToInt32(strnum);
                    numero++;
                }
                lee.Close();
            }
            catch (Exception)
            {
                numero = 1;
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return numero;
        }
        public void truncateCarrito(Label Label1)
        {
            
            try
            {
                string query = "truncate table carrito";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYcargaGridProductos(GridView GridView1, Label Label1)
        {           
            try
            {
                string qry = "select detalle from amazonxml order by codigo";
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
        public bool ValidarCantidad(string pcodigo,string pcantidad, Label Label1)
        {
            bool respuesta = false;
            try
            {
                int numero = Convert.ToInt32(pcantidad);
                if (numero < 1)
                {
                    respuesta = false;
                    objconexion.MensajeNormal("La Cantidad Debe ser positiva", Label1);
                }
                else
                {                    
                    int StockDeAmazon = 0;
                    try
                    {
                        string qry = "select detalle from amazonxml where codigo = " + pcodigo + "";
                        NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                        NpgsqlDataReader readsql = cmd.ExecuteReader();
                        string strxml = "";
                        if (readsql.Read())
                        {
                            strxml = "" + readsql["detalle"];
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(strxml);
                            XmlNode nodo = doc.DocumentElement;
                            foreach (XmlNode nodo1 in nodo.ChildNodes)
                            {
                                if (nodo1.Name == "Cantidad")
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
                    finally { objconexion.desconectarPOSTGRE(); }
                    if (numero > StockDeAmazon)
                    {
                        objconexion.MensajeNormal("No Hay Suficiente Cantidad del producto", Label1);
                        respuesta = false;
                    }
                    else { respuesta = true; }
                }

            }
            catch (Exception Ex)
            {
                respuesta = false;
                objconexion.MensajeNormal("La Cantidad debe ser  un número Entero", Label1);
            }
            return respuesta;
        }
        public void agregaAcarrito(string pcodigo, string pnombre, string pcategoria, string pprecio, string cantidad, Label Label1)
        {            
            try
            {
                string query = "InsertarCarrito";
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
                Cantidad.AppendChild(doc.CreateTextNode(cantidad));
                Carrito.AppendChild(Cantidad);

                prmDetalle.Value = doc.OuterXml.ToString();
                cmdxm.Parameters.Add(prmDetalle);
                cmdxm.ExecuteNonQuery();
                objconexion.MensajeNormal("Producto Añadido al carrito", Label1);
            }
            catch (Exception Ex)
            {
                objconexion.MensajeNormal("Ya ha agregado el producto indicado\nPuede agregar otro", Label1);
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
        public void leeYcargaGridCarrito(GridView GridView2, Label Label1) //une varias culumnas xml en una
        {            
            try
            {
                string qry = "select detalle from carrito";
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
                    {                       
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
        public void leeYCargaDetallePedido(string numeropedido, GridView GridView2, Label Label1)
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
        public void leeYCargaDetalleFactura(string numeroFactura, GridView GridView2, Label Label1)
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label1); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void VerMisPedidos(string cedula, GridView GridView1, Label Label1, Label lblgrid1, Label lblgrid2)
        {
            
            try
            {
                string query = "select numero_pedido, cedula_cliente, fecha, Estado from pedido where cedula_cliente = " + cedula + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
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
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void VerMisFacturas(string cedula, GridView GridView1, Label Label1, Label lblgrid1, Label lblgrid2)
        {            
            try
            {
                string query = "select numero_factura, cedula_cliente, fecha, Subtotal from factura where cedula_cliente = " + cedula + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
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
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void DevolverProducto(string codigo2, Label Label1)
        {            
            try
            {
                string query = "delete from carrito where numero_carrito = " + codigo2 + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.ExecuteNonQuery();
                objconexion.MensajeNormal("El producto ha sido devuelto", Label1);
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public int CuentaCarrito(Label Label1)
        {            
            int Cantidad = 0;
            try
            {
                string qry = "select count(numero_carrito) as Contador from carrito";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
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
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return Cantidad;
        }
        public string cargarUsuario(string cedula,Label Label1)
        {         
            string nombre="",apellidos="";
            try
            {
                string query = "select nombre, apellidos from usuario where cedula ="+cedula;
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataReader read = cmd.ExecuteReader();
                
                if (read.Read())
                {
                    nombre = "" + read["nombre"];
                    apellidos = "" + read["apellidos"];
                }
                read.Close();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return "Cliente: " + nombre + " " + apellidos;
        } 
    }
}