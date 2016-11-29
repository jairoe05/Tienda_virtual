<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MenuVendedor.aspx.cs" Inherits="WebApplication1.MenuVendedor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 103%; height: 150px">
        <tr>
            <td style="width: 429px; height: 25px; text-align: right;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="font-size: large">Menú Administrador</span></strong>&nbsp;</td>
            <td style="font-size: large; height: 25px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 429px; height: 25px">
                &nbsp;</td>
            <td style="font-size: large; height: 25px">
&nbsp;<asp:Button ID="btnNuevaOrden" runat="server" Height="22px" onclick="btnNuevaOrden_Click" 
                    Text="Iniciar una orden de Compra" Width="178px" />
&nbsp;
                <asp:Label ID="lblIniciaCompra" runat="server" style="font-size: medium" 
                    Text="."></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSalir" runat="server" Height="24px" onclick="btnSalir_Click" 
                    Text="Salir" Width="45px" />
            </td>
        </tr>
        <tr>
            <td style="width: 429px">
                &nbsp;&nbsp;
                <asp:Button ID="btnVerInventario" runat="server" Height="29px" 
                    onclick="btnVerInventario_Click" Text="Ver Inventario" Width="125px" />
            </td>
            <td>
                Nombre:&nbsp;&nbsp;
                <asp:TextBox ID="txtNombre" runat="server" ReadOnly="True" Width="161px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp; Precio:&nbsp;&nbsp;
                <asp:TextBox ID="txtPrecio" runat="server" ReadOnly="True" Width="71px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 429px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                Categoria:<asp:TextBox ID="txtCategoria" runat="server" ReadOnly="True" 
                    Width="161px"></asp:TextBox>
&nbsp;&nbsp; Cantidad:
                <asp:TextBox ID="txtCantidad" runat="server" Height="22px" TextMode="Number" 
                    Width="70px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 429px">
&nbsp;&nbsp;&nbsp; <asp:Button ID="btnVerPedidos" runat="server" Height="27px" onclick="btnVerPedidos_Click" 
                    Text="Ver Pedidos" Width="159px" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnFacturar" runat="server" Height="27px" 
                    onclick="btnFacturar_Click" Text="Facturar Pedido" />
            </td>
            <td>
                Código:&nbsp;&nbsp;<asp:TextBox ID="txtCodigo" runat="server" Height="22px" ReadOnly="True" 
                    Width="31px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnAgregarOrden" runat="server" Height="25px" 
                    onclick="btnAgregarOrden_Click" Text="Agregar a orden de compra" 
                    Width="175px" />
            </td>
        </tr>
        <tr>
            <td style="width: 429px; height: 25px">                
&nbsp;&nbsp; <asp:Button ID="btnDVerOrdenes" runat="server" 
                    onclick="btnDVerOrdenes_Click" Text="Ver Ordenes" Width="138px"/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnAlmacenarOrden" runat="server" 
                    onclick="btnAlmacenarOrden_Click" Text="Almacenar Orden" Width="138px" />
            </td>
            <td>
                Código de distribuidor:&nbsp;
                <asp:TextBox ID="txtCodDistribuidor" runat="server" Width="62px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnEnviarOrden" runat="server" onclick="btnEnviarOrden_Click" 
                    Text="Enviar Orden" />
            </td>
        </tr>
    </table>
&nbsp;&nbsp;
    <table style="width: 103%">
        <tr>
            <td style="width: 560px">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTituloGrid" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTituloGrid2" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 103%; height: 153px">
        <tr>
            <td style="width: 215px">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" Height="133px" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:ButtonField CommandName="Select" HeaderText="Elegir" ShowHeader="True" 
                            Text="Click" />
                    </Columns>
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
            </td>
            <td style="width: 80px">
                <asp:Image ID="Image1" runat="server" Height="102px" Width="112px" />
            </td>
            <td style="width: 356px">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" style="margin-left: 0px">
                    <AlternatingRowStyle BackColor="White" />
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
            </td>
        </tr>
    </table>
&nbsp;
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
