<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true"
    CodeBehind="MenuDistribuidor.aspx.cs" Inherits="WebApplication1.MenuDistribuidor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="title text-center">
        Menu Distribuior</h2>
    <div>
        <p>
        </p>
        
        <table style="width: 117%">
            <tr>
                <td style="width: 265px">
                    &nbsp;
                </td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Style="font-weight: 700; text-decoration: underline;
                        color: #0000FF" Text="-"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 265px">
                 <div class="login-card">
                 <asp:Button ID="btnVerInventario" runat="server" OnClick="btnVerInventario_Click"
                        Text="Ver Inventario" Width="184px" />
                    <asp:Button ID="btnVerOrdenes" runat="server" OnClick="btnVerOrdenes_Click" Text="Ver Ordenes de Compra"
                        Width="184px" />
                    <asp:Button ID="btnSuministrar" runat="server" OnClick="btnSuministrar_Click" Text="Suministrar Orden" Width="184px" />
                 </div>
                    
                </td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnSalir" runat="server" OnClick="btnSalir_Click" Text="Salir" Width="103px" />
                </td>
            </tr>
            <tr>
                <td style="width: 265px">
                    &nbsp;
                </td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 265px">
                    &nbsp;</td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 265px">
                    &nbsp;
                </td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 265px">
                    &nbsp;</td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 265px">
                    &nbsp;
                </td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 265px">
                    <asp:Label ID="lblTituloGrid1" runat="server"></asp:Label>
                </td>
                <td style="width: 315px">
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblTituloGrid2" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <p>
            <table style="width: 99%">
                <tr>
                    <td style="width: 208px">
                        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                            OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:ButtonField CommandName="Select" HeaderText="Seleccionar" ShowHeader="True"
                                    Text="Click" />
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </td>
                    <td style="width: 209px">
                        <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#999999"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                            OnSelectedIndexChanged="GridView2_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <FooterStyle BackColor="#CCCCCC" />
                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#808080" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#383838" />
                        </asp:GridView>
                    </td>
                    <td>
                        <asp:Image ID="Image1" runat="server" Height="163px" Width="166px" />
                    </td>
                </tr>
            </table>
            <br />
        </p>
        <br />
        <br />
    </div>
</asp:Content>
