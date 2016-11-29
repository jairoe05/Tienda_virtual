using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Mono.Security;
using System.Web.UI.WebControls;
namespace WebApplication1.Metodos
{
    interface Iconexion
    {
        NpgsqlConnection conectarPOSTGRE();
        string getCadenaPostgre();
        void MensajeNormal(string msj, Label lbl);
        void MensajeError(Exception Ex, Label lbl);
        void desconectarPOSTGRE();
    }
}
