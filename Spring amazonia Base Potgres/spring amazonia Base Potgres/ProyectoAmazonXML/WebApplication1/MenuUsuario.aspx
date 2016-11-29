<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true"
    CodeBehind="MenuUsuario.aspx.cs" Inherits="WebApplication1.MenuUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="title text-center">
        Menu Usuario</h2>
    <div>
        <p>
            <br />
            <br />
            <table style="width: 100%">
                <tr>
                    <td style="width: 218px">
                    </td>
                    <td style="width: 324px">
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Style="font-weight: 700; text-decoration: underline;
                            color: #0000FF" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 218px">
                        <asp:Label ID="lblIniciarCompra" runat="server">.</asp:Label><br />
                        Usuario:
                        <asp:Label ID="lblCedula" runat="server" Text="."></asp:Label>
                    </td>
                    <td style="width: 324px">
                        <div class="login-card">
                            <asp:Button ID="btnVerProductos" runat="server" Height="24px" OnClick="btnVerProductos_Click"
                                Style="margin-left: 0px" Text="Ver Productos" Width="185px" CssClass="login login-submit" />
                            <asp:Button ID="btnVerMisPedidos" runat="server" OnClick="btnVerMisPedidos_Click"
                                Style="margin-bottom: 0px" Text="Ver mis Pedidos" Width="185px" CssClass="login login-submit" />
                            <asp:Button ID="btnverFacturas" runat="server" OnClick="btnverFacturas_Click" Text="Ver Mis Facturas"
                                Width="185px" CssClass="login login-submit" />
                        </div>
                    </td>
                    <td>
                        <div class="login-card">
                            <br />
                            <asp:Button ID="btnIniciar" runat="server" CssClass="login login-submit" OnClick="btnIniciar_Click"
                                Text="Inicar el pedido" Width="185px" />
                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" Width="185px" Height="24px"
                                OnClick="btnAgregar_Click" CssClass="login login-submit" />
                            <asp:Button ID="btnDevolver" runat="server" Text="Devolver" Width="185px" OnClick="btnDevolver_Click"
                                CssClass="login login-submit" />
                            <asp:Button ID="btnConcluir" runat="server" Text="Concluir el pedido" Width="185px"
                                OnClick="btnConcluir_Click" Height="24px" CssClass="login login-submit" />
                            <asp:Button ID="btnCerrarsesion" runat="server" Text="Cerrar Sesiòn" Width="185px"
                                OnClick="btnCerrarsesion_Click" CssClass="login login-submit" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 218px">
                        <span style="font-size: large">Cantidad&nbsp;&nbsp; </span>&nbsp;<asp:TextBox ID="txtCantidad"
                            runat="server" Height="24px" TextMode="Number" Width="35px"></asp:TextBox>
                        &nbsp;
                    </td>
                    <td style="width: 324px">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="font-size: large; width: 218px; height: 30px;">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblgrid1" runat="server" Text="Productos"></asp:Label>
                    </td>
                    <td style="width: 324px; height: 30px;">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="height: 30px">
                        &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblgrid2" runat="server" Style="font-weight: 700; font-size: large"
                            Text="Carrito"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 333px">
                        <asp:GridView ID="GridView1" runat="server" BackColor="LightGoldenrodYellow" BorderColor="Tan"
                            BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                            <Columns>
                                <asp:ButtonField CommandName="Select" HeaderText="Elegir" ShowHeader="True" Text="Click" />
                            </Columns>
                            <FooterStyle BackColor="Tan" />
                            <HeaderStyle BackColor="Tan" Font-Bold="True" />
                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                            <SortedAscendingCellStyle BackColor="#FAFAE7" />
                            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                            <SortedDescendingCellStyle BackColor="#E1DB9C" />
                            <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                        </asp:GridView>
                    </td>
                    <td style="width: 102px">
                        <asp:Image ID="Image1" runat="server" Height="157px" Width="161px" Style="margin-left: 0px" />
                    </td>
                    <td style="width: 429px">
                        <asp:GridView ID="GridView2" runat="server" BackColor="LightGoldenrodYellow" BorderColor="Tan"
                            BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" Style="margin-left: 0px"
                            OnSelectedIndexChanged="GridView2_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                            <Columns>
                                <asp:ButtonField CommandName="Select" HeaderText="Seleccionar" ShowHeader="True"
                                    Text="Click" />
                            </Columns>
                            <FooterStyle BackColor="Tan" />
                            <HeaderStyle BackColor="Tan" Font-Bold="True" />
                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
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
    </div>
</asp:Content>
