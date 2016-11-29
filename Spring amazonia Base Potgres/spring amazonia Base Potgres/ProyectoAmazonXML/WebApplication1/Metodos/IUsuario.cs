using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication1.Metodos
{
    interface IUsuario
    {
        bool ComprobarDatos(string cedula, string pass, Label Label1);
        void agregarPedido(string pCedula, Label Label1);
        void truncateCarrito(Label Label1);
        void leeYcargaGridProductos(GridView GridView1, Label Label1);
        bool ValidarCantidad(string pcodigo, string pcantidad, Label Label1);
        void agregaAcarrito(string pcodigo, string pnombre, string pcategoria, string pprecio, string cantidad, Label Label1);
        void actualizarStockAmazon(string codigo, string CantidadActualizar, string signo, Label Label1);
        void leeYcargaGridCarrito(GridView GridView2, Label Label1);
        void leeYcargaImagen(string codigoProducto, Image Image1, Label Label1);
        void leeYCargaDetallePedido(string numeropedido, GridView GridView2, Label Label1);
        void leeYCargaDetalleFactura(string numeroFactura, GridView GridView2, Label Label1);
        void VerMisPedidos(string cedula, GridView GridView1, Label Label1, Label lblgrid1, Label lblgrid2);
        void VerMisFacturas(string cedula, GridView GridView1, Label Label1, Label lblgrid1, Label lblgrid2);
        void DevolverProducto(string codigo2, Label Label1);
        int CuentaCarrito(Label Label1);
        string cargarUsuario(string cedula, Label Label1);
    }
}