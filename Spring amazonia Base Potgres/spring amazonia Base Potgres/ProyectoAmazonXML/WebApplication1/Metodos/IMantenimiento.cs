using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace WebApplication1.Metodos
{
    interface IMantenimiento
    {
        void cargaInventario(GridView gridUsar, string tablausar, Label Label1);
        string ManteAmazonDistri(int pCodigo, string pNombre, string pCategoria, int pPrecio, int pStock, Byte[] pImagen, string tabla, string accion);
        string ManteUsuaVende(string CodCed, string Nombre, string Apellidos, string telefono, string contra, string correo, string tabla, string accion);
        void cargarUsuaVende(GridView GridView2, Label LabelMensaje, string tabla);
        void leeYcargaImagen(string codigoProducto, int num, Image Image1, Label Label1);
    }
}
