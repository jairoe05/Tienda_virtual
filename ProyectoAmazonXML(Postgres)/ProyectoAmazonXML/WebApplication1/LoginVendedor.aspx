﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="LoginVendedor.aspx.cs" Inherits="WebApplication1.LoginVendedor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <br />
        <table style="width: 99%">
            <tr>
                <td style="width: 311px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    Código de vendedor&nbsp;</td>
                <td style="width: 305px">
                    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 311px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    Password&nbsp;</td>
                <td style="width: 305px">
                    <asp:TextBox ID="txtPass" runat="server"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 311px; height: 30px;">
                    </td>
                <td style="width: 305px; height: 30px;">
                    <asp:Button ID="btnEntrar" runat="server" onclick="btnEntrar_Click" 
                        Text="Entrar" Width="92px" />
                </td>
                <td style="height: 30px">
                    </td>
            </tr>
            <tr>
                <td style="width: 311px">
                    &nbsp;</td>
                <td style="width: 305px">
                    <asp:Label ID="Label1" runat="server" 
                        style="font-weight: 700; text-decoration: underline; color: #0000FF" 
                        Text="Label"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </p>
    <p>
        <br />
        
        &nbsp;</p>
</asp:Content>
