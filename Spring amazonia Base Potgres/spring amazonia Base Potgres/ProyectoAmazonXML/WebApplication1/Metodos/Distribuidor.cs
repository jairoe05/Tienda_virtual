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
using NpgsqlTypes;

namespace WebApplication1.Metodos
{
    public class Distribuidor : IDistribuidor
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        public void leeYcargaGridProductos(GridView GridView1, Label Label2)
        {            
            try
            {
                string qry = "select detalle from distribuidorxml";
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
                GridView1.DataSource = dt;
                GridView1.DataBind();
                readsql.Close();
            }
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void MostrarOrdenes(GridView GridView1, Label Label2)
        {            
            try
            {
                string qry = "select numero_orden as \"Orden #\", Fecha, Codigo_Distribuidor, Estado from Orden_Compra";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
                DataTable tb = new DataTable("Ordenes");
                adap.Fill(tb);
                GridView1.DataSource = tb;
                GridView1.DataBind();
            }
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public bool ComprobarCantidadesSuministrar(string NumOrden, Label Label2)  //pendiente
        {//**            
            int contadorFalsos = 0;
            try
            {
                string qryOra = "select detalle from orden_compra where numero_orden = " + NumOrden + "";
                NpgsqlCommand cmdOra = new NpgsqlCommand(qryOra, objconexion.conectarPOSTGRE());
                NpgsqlDataReader leeOra = cmdOra.ExecuteReader();
                string strxml = "";
                if (leeOra.Read())
                {
                    strxml = "" + leeOra["detalle"];
                    try { leeOra.Close(); }
                    catch (Exception exx) { }
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
                                if (!comprobarExistenciaSQL(codComprobar, nodo2.InnerText, nombreProd,Label2))//no cerrar x comprobar exis
                                { contadorFalsos++; }
                            }
                        }
                    }
                }
                try { leeOra.Close(); }
                catch (Exception exx) { }
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label2);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            if (contadorFalsos > 0)
            {
                return false;
            }
            else { return true; }

        }
        private bool comprobarExistenciaSQL(string CodProducto, string Cantidad, string nombre, Label Label2)
        {            
            int StockDeDistribuidor = 0;
            bool condi = false;
            try
            {
                string qry = "select detalle from distribuidorxml where codigo = " + CodProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataReader readsql = cmd.ExecuteReader();
                string strxml = "";
                if (readsql.Read())
                {
                    strxml = "" + readsql["detalle"];
                    try { readsql.Close(); }
                    catch (Exception exx) { }
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strxml);
                    XmlNode nodo = doc.DocumentElement;
                    foreach (XmlNode nodo1 in nodo.ChildNodes)
                    {
                        if (nodo1.Name == "Cantidad")
                        {
                            StockDeDistribuidor = Convert.ToInt32(nodo1.InnerText);
                            if (Convert.ToInt32(Cantidad) > StockDeDistribuidor)
                            {
                                condi = false;
                                objconexion.MensajeNormal("No hay sificiente cantidad de: " + nombre, Label2);
                            }
                            else { condi = true; }
                        }
                    }
                }
                try { readsql.Close(); }
                catch (Exception exx) { }                
            }
            catch (Exception Ex)
            {
                condi = false;
                StockDeDistribuidor = 0;
            }
            finally { objconexion.desconectarPOSTGRE();} //********************************************para q no deje cerrada al llamador
            return condi;
        }//cerrardata readers
        public void ActualizarStocksDistri(string numOrdeng1_1, Label Label2)
        {            
            try
            {
                string qry = "select detalle from orden_compra where numero_orden = " + numOrdeng1_1 + " and Estado = 'Pendiente'";
                NpgsqlCommand cmd2 = new NpgsqlCommand(qry, objconexion.conectarPOSTGRE());
                NpgsqlDataReader readoracle = cmd2.ExecuteReader();
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
                                actualizaStock(codComprobar, nodo2.InnerText, "-",Label2);
                            }
                        }
                    }
                    updateOrdenARealizado(numOrdeng1_1, Label2);
                    objconexion.MensajeNormal("Se ha reliazo correctamente el suministro para Amazon", Label2);
                }
                else { objconexion.MensajeNormal("Esta orden no está pendiente!!", Label2); }
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label2);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        private void actualizaStock(string codigo, string CantidadActualizar, string signo, Label Label2)
        {//modificado para sqlserver terminado            
            try
            {
                string qry = "select detalle from distribuidorxml where Codigo = " + codigo + "";
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
                    string qryr2 = "ActualizaDetDistribuidor";
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
                }
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label2);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        private void updateOrdenARealizado(string NumOrden, Label Label2)
        {            
            try
            {
                string query = "update orden_compra set Estado = 'Suministrado' where numero_orden = " + NumOrden + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                objconexion.MensajeError(Ex, Label2);
            }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYcargaImagen(string codigoProducto, Image Image1,Label Label2)
        {            
            try
            {
                string query = "select Imagen from distribuidorxml where Codigo = " + codigoProducto + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                Byte[] bytes = (Byte[])cmd.ExecuteScalar();
                Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                Image1.Visible = true;
                cmd.Dispose();
            }
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public void leeYCargaDetalleOrden(string numeroOrden, GridView GridView2,Label Label2)
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
            catch (Exception Ex) { objconexion.MensajeError(Ex, Label2); }
            finally { objconexion.desconectarPOSTGRE(); }
        }
        public bool Entrar(string correo, string codDistri, Label Label1)
        {            
            bool resp = false;
            try
            {
                string query = "select Nombre, Ubicacion from Distribuidor where Correo = '" + correo + "' and Cod_distribuidor = " + codDistri + "";
                NpgsqlCommand cmd = new NpgsqlCommand(query, objconexion.conectarPOSTGRE());
                NpgsqlDataReader lee = cmd.ExecuteReader();
                string nomb = "";
                string ubi = "";
                if (lee.Read())
                {
                    nomb = "" + lee["Nombre"];
                    ubi = "" + lee["Ubicacion"];
                    objconexion.MensajeNormal("Bienvenido:  " + nomb + " de " + ubi, Label1);
                    lee.Close();
                    resp = true;
                }
                else { resp = false; }
            }
            catch (Exception EX)
            {
                resp = false;
                objconexion.MensajeError(EX, Label1);
            }
            finally { objconexion.desconectarPOSTGRE(); }
            return resp;
        }
    }
}