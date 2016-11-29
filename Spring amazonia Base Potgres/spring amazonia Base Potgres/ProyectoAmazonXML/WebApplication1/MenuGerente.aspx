<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true"
    CodeBehind="MenuGerente.aspx.cs" Inherits="WebApplication1.MenuGerente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <div>
        <h2 class="title text-center">
            Administrador</h2>
        <link href="Temas/MasterPage/css/Style.css" rel="stylesheet" type="text/css" />
        <table style="width: 100%">
            <tr>
                <td style="height: 91px; width: 178px">
                </td>
                <td style="height: 91px; width: 67px">
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </td>
                <td style="height: 91px; width: 284px">
                    <asp:Label ID="Label2" runat="server" Style="font-weight: 700; text-decoration: underline;
                        color: #0000FF" Text="-"></asp:Label>
                </td>
                <td style="width: 195px; height: 91px">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="height: 20px; width: 67px">
                    <div class="login-card">
                        <center>
                            <strong>Identificación:</strong>
                            <br />
                            <asp:TextBox ID="TextBox1" runat="server">1111</asp:TextBox>
                            <br />
                            <strong>Contraseña:</strong>
                            <br />
                            <asp:TextBox ID="TextBox2" runat="server">123</asp:TextBox>
                            <br />
                            <br />
                            <asp:Button ID="btnEntrar" runat="server" Text="Acceder" OnClick="btnEntrar_Click" CssClass="login login-submit"/>
                        </center>
                    </div>
                </td>
                <td style="height: 20px; width: 284px; text-align: left">
                </td>
                <td style="width: 195px; height: 20px">
                    <div class="login-card">
                        <center>
                            <strong>Fecha Entre:</strong></center>
                        <br />
                        <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" Width="108px" Height="22px"></asp:TextBox>
                        &nbsp; Y&nbsp;
                        <asp:TextBox ID="txtFecha2" runat="server" TextMode="Date" Width="115px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 178px; text-align: right; height: 30px;">
                    &nbsp;
                </td>
                <td style="width: 67px; height: 30px;">
                </td>
                <td style="width: 284px; height: 30px;">
                </td>
                <td style="width: 195px; height: 30px;">
                    
                </td>
            </tr>
            <tr>
                <td style="width: 178px; text-align: right; height: 30px;">
                </td>
                <td style="width: 67px; height: 30px;">
                    <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999"
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                        Style="margin-left: 0px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
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
                    <asp:Image ID="Image1" runat="server" Height="102px" Width="117px" />
                    <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#999999"
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                        Width="227px">
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
                <td style="width: 284px; height: 30px;">
                    &nbsp;&nbsp;
                </td>
                <td style="width: 195px; height: 30px;">
                    <div class="login-card">
                        <center>
                            <asp:Button ID="btnMantenimientos" runat="server" OnClick="btnMantenimientos_Click" Text="Mantenimientos" CssClass="login login-submit" Width="185px" />
                            <br /><br />
                            <asp:Button ID="btnVerInventario" runat="server" Text="Ver Inventario" 
                                OnClick="btnVerInventario_Click" CssClass="login login-submit" Width="185px" />
                            <br /><br />
                            <asp:Button ID="btnVerOrdenes" runat="server" Text="Ver ventas Amazon" Width="185px"
                                 OnClick="btnVerOrdenes_Click" CssClass="login login-submit"  />
                            <br /><br />
                            <asp:Button ID="btnVerVentas" runat="server" Text="Ver ventas" Width="185px" OnClick="btnVerVentas_Click" CssClass="login login-submit" />
                            
                        </center>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 178px; height: 42px;">
                    
                </td>
                <td style="width: 67px; height: 42px;">
                    
                </td>
                <td style="width: 284px; height: 42px;">
                </td>
                <td style="width: 195px; height: 42px;">
                    
                </td>
            </tr>
        </table>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblGrid" runat="server" Text="."></asp:Label>
        <br />
        <table style="width: 100%">
            <tr>
                <td style="width: 98px">
                    &nbsp;
                </td>
                <td style="width: 218px">
                    &nbsp;</td>
                <td style="width: 198px">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        <br />
        <br />
    </div>
</asp:Content>
