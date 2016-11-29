<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true"
    CodeBehind="MenuVendedor.aspx.cs" Inherits="WebApplication1.MenuVendedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="title text-center">
        Menu Vendedor</h2>
    <div>
        <table style="width: 103%; height: 150px">
            <tr>
                <td style="width: 429px; height: 25px; text-align: right;">
                </td>
                <td style="font-size: large; height: 25px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Style="font-weight: 700; text-decoration: underline;
                        color: #0000FF; font-size: medium" Text="-"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 429px; height: 22px">
                    <asp:Label ID="lblCodVendedor" runat="server" Text="Label"></asp:Label>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
                <td style="font-size: large; height: 22px">
                    &nbsp;<asp:Button ID="btnNuevaOrden" runat="server" Height="22px" OnClick="btnNuevaOrden_Click"
                        Text="Iniciar una orden de Compra" Width="215px" CssClass="login login-submit" />
                    &nbsp;
                    <asp:Label ID="lblIniciaCompra" runat="server" Style="font-size: medium" Text="."></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSalir" runat="server" Height="24px" OnClick="btnSalir_Click" Text="Salir"
                        Width="45px" CssClass="login login-submit" />
                </td>
            </tr>
            <tr>
                <td style="width: 429px">
                    &nbsp;&nbsp;
                    <div class="login-card"> 
                    <asp:Button ID="btnVerInventario" runat="server" Height="29px" OnClick="btnVerInventario_Click"
                        Text="Ver Inventario" Width="138px" CssClass="login login-submit" />
                    <asp:Button ID="btnVerPedidos" runat="server" Height="27px" OnClick="btnVerPedidos_Click"
                        Text="Ver Pedidos" Width="138px" CssClass="login login-submit" />
                    <asp:Button ID="btnFacturar" runat="server" Height="27px" OnClick="btnFacturar_Click" Width="138px" 
                        Text="Facturar Pedido" CssClass="login login-submit" />
                    <asp:Button ID="btnDVerOrdenes" runat="server" OnClick="btnDVerOrdenes_Click" Text="Ver Ordenes"
                        Width="138px" CssClass="login login-submit" />
                    <asp:Button ID="btnAlmacenarOrden" runat="server" OnClick="btnAlmacenarOrden_Click"     Text="Almacenar Orden" Width="138px" CssClass="login login-submit" />
                    </div>
                   
                </td>
                <td>
                    <div class="login-card">
                        Nombre:&nbsp;&nbsp;
                        <asp:TextBox ID="txtNombre" runat="server" ReadOnly="True" Width="161px"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp; Precio:&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtPrecio" runat="server" ReadOnly="True" Width="161px"></asp:TextBox><br />
                        Categoria:<asp:TextBox ID="txtCategoria" runat="server" ReadOnly="True" Width="161px"></asp:TextBox>
                        &nbsp;&nbsp; Cantidad:
                        <asp:TextBox ID="txtCantidad" runat="server" Height="22px" TextMode="Number" Width="161px"></asp:TextBox><br />
                        Código:&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtCodigo" runat="server" Height="22px" ReadOnly="True" Width="161px"></asp:TextBox><br /><br />
                        <center>
                            <asp:Button ID="btnAgregarOrden" runat="server" Height="25px" OnClick="btnAgregarOrden_Click"
                                Text="Agregar a orden de compra" Width="215px" CssClass="login login-submit" />
                        </center>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 429px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 429px">
                    &nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 429px; height: 25px">
                    &nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                <td>
                    Código de distribuidor:&nbsp;
                    <asp:TextBox ID="txtCodDistribuidor" runat="server" Width="62px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnEnviarOrden" runat="server" OnClick="btnEnviarOrden_Click" Text="Enviar Orden"
                        CssClass="login login-submit" />
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
                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                        Height="133px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:ButtonField CommandName="Select" HeaderText="Elegir" ShowHeader="True" Text="Click" />
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
                    <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                        Style="margin-left: 0px">
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
    </div>
</asp:Content>
