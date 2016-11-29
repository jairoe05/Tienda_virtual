using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication1.Metodos
{
    interface IAdministrador
    {
        void leeYcargaGridProductos(GridView GridView1, Label Label2);
        void MostrarOrdenes(GridView GridView1, Label lblGrid, Label Label2);
        void VerMisFacturas(string f1, string f2, GridView GridView1, Label lblGrid, Label Label2);
        void leeYCargaDetalleFactura(string numeroFactura, GridView GridView2, Label Label2);
        void leeYcargaImagen(string codigoProducto, Image Image1, Label Label2);
        void leeYCargaDetalleOrden(string numeroOrden, GridView GridView2, Label Label2);
    }
}