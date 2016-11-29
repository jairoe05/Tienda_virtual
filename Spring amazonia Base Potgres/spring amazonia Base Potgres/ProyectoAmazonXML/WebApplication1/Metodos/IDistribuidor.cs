using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication1.Metodos
{
    interface IDistribuidor
    {
        void leeYcargaGridProductos(GridView GridView1, Label Label2);
        void MostrarOrdenes(GridView GridView1, Label Label2);
        bool ComprobarCantidadesSuministrar(string NumOrden, Label Label2);
        void ActualizarStocksDistri(string numOrdeng1_1, Label Label2);
        void leeYcargaImagen(string codigoProducto, Image Image1, Label Label2);
        void leeYCargaDetalleOrden(string numeroOrden, GridView GridView2, Label Label2);
        bool Entrar(string correo, string codDistri, Label Label1);
    }
}