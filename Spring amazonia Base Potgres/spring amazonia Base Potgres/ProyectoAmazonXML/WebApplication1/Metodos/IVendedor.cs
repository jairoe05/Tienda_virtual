using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication1.Metodos
{
    interface IVendedor
    {
        bool ComprobarDatos(string codvendedor, string pass, Label Label1);
        void leeYcargaGridProductos(GridView GridView1, Label Label1);
        void MostrarPedidos(GridView GridView1, Label Label1);
        void leeYCargaDetallePedido(string numeropedido, GridView GridView2, Label Label1);
        void agregarDetalleOrden(string pcodigo, string pnombre, string pcategoria, string pprecio, string pcantidad, Label Label1);
        void enviarOrden(string coddistribuidor, Label Label1, Label lblIniciaCompra);
        void truncateCarritoOrden(Label Label1);
        void leeYcargaGridCarritoOrden(GridView GridView2, Label Label1);
        int sacarSubTotal(string numpedido, Label Label1);
        void leeYCargaDetalleOrden(string numeroOrden, GridView GridView2, Label Label1);
        void AlmacenarProductosDeOrden(string numOrdenl, GridView GridView1, Label lblTituloGrid, Label lblTituloGrid2, Label Label1);
        void updateOrdenARealizado(string NumPedido, Label Label1);
        void MostrarOrdenes(GridView GridView1, Label Label1, Label lblTituloGrid, Label lblTituloGrid2);
        void actualizarStockAmazon(string codigo, string CantidadActualizar, string signo, Label Label1);
        void updatePedidoARealizado(string NumPedido, Label Label1);
        void leeYcargaImagen(string codigoProducto, Image Image1, Label Label1);
        bool ExisteDistribuidor(string codDistribuidor, Label Label1);
        bool ComprobarCantidadesFacturar(string NumPedido, Label Label1);
        bool comprobarExistenciaSQL(string CodProducto, string Cantidad, string nombre, Label Label1);
        void Facturar(string cedulaClientedGrid1_2, string numpediDgrid1_1, string subtotal, Label Label1, GridView GridView1);
    }
}