<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="LoginDistribuidor.aspx.cs" Inherits="WebApplication1.LoginDistribuidor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 99%">
        <tr>
            <td style="width: 397px">
                &nbsp;</td>
            <td style="width: 297px">
                LOGIN</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 397px">
                &nbsp;</td>
            <td style="width: 297px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 397px; text-align: right">
                <strong>Correo Distribuidor</strong></td>
            <td style="width: 297px">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 397px; text-align: right">
                Contrasseña</td>
            <td style="width: 297px">
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 397px">
                &nbsp;</td>
            <td style="width: 297px">
                <asp:Button ID="btnEntrar" runat="server" onclick="btnEntrar_Click" 
                    Text="Entrar" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
