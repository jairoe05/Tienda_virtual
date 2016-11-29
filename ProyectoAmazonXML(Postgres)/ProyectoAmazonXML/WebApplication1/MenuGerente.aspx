<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MenuGerente.aspx.cs" Inherits="WebApplication1.MenuGerente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%">
        <tr>
            <td style="width: 178px">
                &nbsp;</td>
            <td style="width: 67px">
                &nbsp;</td>
            <td style="width: 284px">
                Gerente Amazonia</td>
            <td style="width: 195px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="height: 26px; width: 178px">
                </td>
            <td style="height: 26px; width: 67px">
                <asp:Label ID="Label1" runat="server"></asp:Label>
                </td>
            <td style="height: 26px; width: 284px">
            </td>
            <td style="width: 195px; height: 26px">
                <asp:Label ID="Label2" runat="server" 
                    style="font-weight: 700; text-decoration: underline; color: #0000FF" 
                    Text="Label"></asp:Label>
                </td>
        </tr>
        <tr>
            <td style="text-align: right; height: 20px; width: 178px">
                Identificación</td>
            <td style="height: 20px; width: 67px">
                <asp:TextBox ID="TextBox1" runat="server">1111</asp:TextBox>
            </td>
            <td style="height: 20px; width: 284px; text-align: left">
                &nbsp;&nbsp;
                Contraseña&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextBox2" runat="server">123</asp:TextBox>
            </td>
            <td style="width: 195px; height: 20px">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 178px; text-align: right; height: 30px;">
                &nbsp;</td>
            <td style="width: 67px; height: 30px;">
                &nbsp;</td>
            <td style="width: 284px; height: 30px;">
                <asp:Button ID="btnEntrar" runat="server" Text="Acceder" 
                    onclick="btnEntrar_Click" />
            </td>
            <td style="width: 195px; height: 30px;">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 178px; text-align: right; height: 30px;">
                </td>
            <td style="width: 67px; height: 30px;">
                </td>
            <td style="width: 284px; height: 30px;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Fecha&nbsp; 
                Entre</td>
            <td style="width: 195px; height: 30px;">
                </td>
        </tr>
        <tr>
            <td style="width: 178px; height: 42px;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnVerInventario" runat="server" Text="Ver Inventario" 
                    onclick="btnVerInventario_Click" />
            </td>
            <td style="width: 67px; height: 42px;">
                <asp:Button ID="btnVerOrdenes" runat="server" Text="Ver ordenes de compra" 
                    Width="155px" onclick="btnVerOrdenes_Click" />
            </td>
            <td style="width: 284px; height: 42px;">
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" Width="108px"></asp:TextBox>
            &nbsp; Y&nbsp;
                <asp:TextBox ID="txtFecha2" runat="server" TextMode="Date" Width="115px"></asp:TextBox>
            </td>
            <td style="width: 195px; height: 42px;">
                <asp:Button ID="btnVerVentas" runat="server" Text="Ver ventas" Width="77px" 
                    onclick="btnVerVentas_Click" />
            </td>
        </tr>
        </table>
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
      <asp:Label ID="lblGrid" runat="server" Text="."></asp:Label>
        <br />
    <table style="width: 100%">
        <tr>
            <td style="width: 98px">
                &nbsp;</td>
            <td style="width: 218px">
        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999" 
                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                    GridLines="Vertical" style="margin-left: 0px" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:ButtonField CommandName="Select" HeaderText="Seleccionar" 
                    ShowHeader="True" Text="Click" />
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
            <td style="width: 198px">
                <asp:GridView ID="GridView2" runat="server" BackColor="White" 
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    ForeColor="Black" GridLines="Vertical" Width="227px">
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
                <asp:Image ID="Image1" runat="server" Height="102px" Width="117px" />
            </td>
        </tr>
    </table>
        <br />
    <br />

</asp:Content>
