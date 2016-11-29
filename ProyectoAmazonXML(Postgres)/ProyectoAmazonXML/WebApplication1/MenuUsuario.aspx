<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MenuUsuario.aspx.cs" Inherits="WebApplication1.MenuUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
    <br />
    <br />
    <table style="width: 100%">
        <tr>
            <td style="width: 218px">
                <asp:Button ID="btnIniciar" runat="server" Text="Inicar el pedido" 
                    onclick="btnIniciar_Click" Width="131px" />
            </td>
            <td style="width: 324px">
                <asp:Label ID="lblIniciarCompra" runat="server">.</asp:Label>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" 
                    style="font-weight: 700; text-decoration: underline; color: #0000FF" 
                    Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 218px">
                Usuario:
                <asp:Label ID="lblCedula" runat="server" Text="."></asp:Label>
            </td>
            <td style="width: 324px">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnVerProductos" runat="server" Height="24px" 
                    onclick="btnVerProductos_Click" style="margin-left: 0px" Text="Ver Productos" 
                    Width="146px" />
            </td>
            <td>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCerrarsesion" runat="server" Text="Cerrar Sesiòn" 
                    onclick="btnCerrarsesion_Click" />
            </td>
        </tr>
        <tr>
            <td style="width: 218px">
                <span style="font-size: large">Cantidad&nbsp;&nbsp; </span>&nbsp;<asp:TextBox 
                    ID="txtCantidad" runat="server" Height="24px" TextMode="Number" 
                    Width="35px"></asp:TextBox>
&nbsp;<asp:Button ID="btnAgregar" runat="server" Text="Agregar" Width="61px" Height="24px" 
                    onclick="btnAgregar_Click" />
            </td>
            <td style="width: 324px">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnVerMisPedidos" runat="server" 
                    onclick="btnVerMisPedidos_Click" style="margin-bottom: 0px" 
                    Text="Ver mis Pedidos" />
            </td>
            <td>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnConcluir" runat="server" Text="Concluir el pedido" 
                    onclick="btnConcluir_Click" Height="24px" />
            </td>
        </tr>
        <tr>
            <td style="font-size: large; width: 218px; height: 30px;">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblgrid1" runat="server" Text="Productos"></asp:Label>
            </td>
            <td style="width: 324px; height: 30px;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnverFacturas" runat="server" onclick="btnverFacturas_Click" 
                    Text="Ver Mis Facturas" />
            </td>
            <td style="height: 30px">
&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblgrid2" runat="server" 
                    style="font-weight: 700; font-size: large" Text="Carrito"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnDevolver" runat="server" Text="Devolver" 
                    onclick="btnDevolver_Click" />
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="width: 333px">
                <asp:GridView ID="GridView1" runat="server" BackColor="LightGoldenrodYellow" 
                    BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
                    GridLines="None" onselectedindexchanged="GridView1_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                    <Columns>
                        <asp:ButtonField CommandName="Select" HeaderText="Elegir" 
                            ShowHeader="True" Text="Click" />
                    </Columns>
                    <FooterStyle BackColor="Tan" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                        HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                </asp:GridView>
            </td>
            <td style="width: 102px">
                <asp:Image ID="Image1" runat="server" Height="157px" Width="161px" 
                    style="margin-left: 0px" />
            </td>
            <td style="width: 429px">
                <asp:GridView ID="GridView2" runat="server" BackColor="LightGoldenrodYellow" 
                    BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" 
                    GridLines="None" style="margin-left: 0px" 
                    onselectedindexchanged="GridView2_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                    <Columns>
                        <asp:ButtonField CommandName="Select" HeaderText="Seleccionar" 
                            ShowHeader="True" Text="Click" />
                    </Columns>
                    <FooterStyle BackColor="Tan" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                        HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                </asp:GridView>
            </td>
        </tr>
    </table>
</p>
</asp:Content>
