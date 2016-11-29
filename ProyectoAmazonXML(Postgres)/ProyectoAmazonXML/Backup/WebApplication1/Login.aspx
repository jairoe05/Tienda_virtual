<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        <br />
        <br />
        <br />
        <table style="width: 100%">
            <tr>
                <td style="width: 295px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    Núnmero de Indentifiación</td>
                <td style="width: 301px">
                    <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
                </td>
                <td style="width: 375px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 295px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    Contraseña de acceso&nbsp;</td>
                <td style="width: 301px">
                    <asp:TextBox ID="txtContraseña" runat="server"></asp:TextBox>
                </td>
                <td style="width: 375px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 295px">
                    &nbsp;</td>
                <td style="width: 301px">
                    <asp:Button ID="btnIngresar" runat="server" onclick="btnIngresar_Click" 
                        Text="Ingresar" />
                </td>
                <td style="width: 375px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 295px">
                    &nbsp;</td>
                <td style="width: 301px">
                    &nbsp;</td>
                <td style="width: 375px">
                    &nbsp;</td>
            </tr>
        </table>
    </p>
</asp:Content>
