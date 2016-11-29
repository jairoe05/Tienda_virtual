using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Xml;
using Spring.Context;
using Spring.Context.Support;
using WebApplication1.Metodos;

namespace WebApplication1
{
    public partial class Mantenimientos : System.Web.UI.Page
    {
        static IApplicationContext context = ContextRegistry.GetContext();
        Iconexion objconexion = (Iconexion)context.GetObject("Conexion");
        IMantenimiento manteni = (IMantenimiento)context.GetObject("Mantenimiento");
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox1.Text = deGrid(GridView1, 1);
            TextBox2.Text = deGrid(GridView1, 2);
            TextBox3.Text = deGrid(GridView1, 3);
            TextBox4.Text = deGrid(GridView1, 4);
            TextBox5.Text = deGrid(GridView1, 5);
            Image1.Visible = true;
            manteni.leeYcargaImagen(deGrid(GridView1,1), 1,Image1,Label1);
        }
        
        private string deGrid(GridView gridusar, int index)
        {
            return gridusar.SelectedRow.Cells[index].Text;
        }
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox6.Text = deGrid(GridView2, 1);
            TextBox7.Text = deGrid(GridView2, 2);
            TextBox8.Text = deGrid(GridView2, 3);
            TextBox9.Text = deGrid(GridView2, 4);
            TextBox10.Text = deGrid(GridView2, 5);
            TextBox11.Text = deGrid(GridView2, 6);
            Image1.Visible = false;
        }
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            manteni.cargaInventario(GridView1, "amazonxml", Label1);
            manteni.cargarUsuaVende(GridView2, Label1,"usuario");
            lblProductos.Text = "Productos Amazon";
            lblUsuarios.Text = "Usuarios Registrados";
        }        
        protected void btnInserDistri_Click(object sender, EventArgs e)
        {
            lblProductos.Text = "Productos Distribuidor";
            Byte[] byteimagen = FileUpload1.FileBytes;
            Label1.Text = manteni.ManteAmazonDistri(Convert.ToInt32(TextBox1.Text), TextBox2.Text, TextBox3.Text,
            Convert.ToInt32(TextBox4.Text), Convert.ToInt32(TextBox5.Text), byteimagen, "distribuidor", "insertar");
            manteni.cargaInventario(GridView1, "distribuidorxml", Label1);
        }
        protected void btnUpdaDistri_Click(object sender, EventArgs e)
        {
            lblProductos.Text = "Productos Distribuidor";
            Byte[] byteimagen = FileUpload1.FileBytes;
            Label1.Text = manteni.ManteAmazonDistri(Convert.ToInt32(TextBox1.Text), TextBox2.Text, TextBox3.Text,
                Convert.ToInt32(TextBox4.Text), Convert.ToInt32(TextBox5.Text), byteimagen, "distribuidor", "actualizar");
            manteni.cargaInventario(GridView1, "distribuidorxml", Label1);
        }
        protected void btnElimDistri_Click(object sender, EventArgs e)
        {
            lblProductos.Text = "Productos Distribuidor";
            Byte[] byteimagen = FileUpload1.FileBytes;
            Label1.Text = manteni.ManteAmazonDistri(Convert.ToInt32(TextBox1.Text), TextBox2.Text, TextBox3.Text,
                Convert.ToInt32(TextBox4.Text), Convert.ToInt32(TextBox5.Text), byteimagen, "distribuidor", "eliminar");
            manteni.cargaInventario(GridView1, "distribuidorxml", Label1);
        }
        protected void btnInserAmazon_Click(object sender, EventArgs e)
        {
            lblProductos.Text = "Productos Amazon";
            Byte[] byteimagen = FileUpload1.FileBytes;
            Label1.Text = manteni.ManteAmazonDistri(Convert.ToInt32(TextBox1.Text), TextBox2.Text, TextBox3.Text,
                Convert.ToInt32(TextBox4.Text), Convert.ToInt32(TextBox5.Text), byteimagen, "amazon", "insertar");
            manteni.cargaInventario(GridView1, "amazonxml", Label1);
        }
        protected void btnElimAmazon_Click(object sender, EventArgs e)
        {
            lblProductos.Text = "Productos Amazon";
            Byte[] byteimagen = FileUpload1.FileBytes;
            Label1.Text = manteni.ManteAmazonDistri(Convert.ToInt32(TextBox1.Text), TextBox2.Text, TextBox3.Text,
                Convert.ToInt32(TextBox4.Text), Convert.ToInt32(TextBox5.Text), byteimagen, "amazon", "eliminar");
            manteni.cargaInventario(GridView1, "amazonxml", Label1);
        }
        protected void btnUpdaAmazon_Click(object sender, EventArgs e)
        {
            lblProductos.Text = "Productos Amazon";
            Byte[] byteimagen = FileUpload1.FileBytes;
            Label1.Text = manteni.ManteAmazonDistri(Convert.ToInt32(TextBox1.Text), TextBox2.Text, TextBox3.Text,
                Convert.ToInt32(TextBox4.Text), Convert.ToInt32(TextBox5.Text), byteimagen, "amazon", "actualizar");
            manteni.cargaInventario(GridView1, "amazonxml", Label1);
        }
        protected void btnInserUsua_Click(object sender, EventArgs e)
        {
            lblUsuarios.Text = "Usuarios Registrados";
            Label1.Text = manteni.ManteUsuaVende(TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, "usuario", "insertar");
            manteni.cargarUsuaVende(GridView2, Label1, "usuario");
        }
        protected void btnUpdaUsua_Click(object sender, EventArgs e)
        {
            lblUsuarios.Text = "Usuarios Registrados";
            Label1.Text = manteni.ManteUsuaVende(TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, "usuario", "actualizar");
            manteni.cargarUsuaVende(GridView2, Label1, "usuario");
        }
        protected void btnElimUsua_Click(object sender, EventArgs e)
        {
            lblUsuarios.Text = "Usuarios Registrados";
            Label1.Text = manteni.ManteUsuaVende(TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, "usuario", "eliminar");
            manteni.cargarUsuaVende(GridView2, Label1, "usuario");
        }
        protected void btnInserVende_Click(object sender, EventArgs e)
        {
            lblUsuarios.Text = "Vendedores Registrados";
            Label1.Text = manteni.ManteUsuaVende(TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, "vendedor", "insertar");
            manteni.cargarUsuaVende(GridView2, Label1, "vendedor");
        }
        protected void btnUpdaVende_Click(object sender, EventArgs e)
        {
            lblUsuarios.Text = "Vendedores Registrados";
            Label1.Text = manteni.ManteUsuaVende(TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, "vendedor", "actualizar");
            manteni.cargarUsuaVende(GridView2, Label1, "vendedor");
        }
        protected void btnElimVende_Click(object sender, EventArgs e)
        {
            lblUsuarios.Text = "Vendedores Registrados";
            Label1.Text = manteni.ManteUsuaVende(TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, "vendedor", "eliminar");
            manteni.cargarUsuaVende(GridView2, Label1, "vendedor");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MenuGerente.aspx");
        }
    }
}