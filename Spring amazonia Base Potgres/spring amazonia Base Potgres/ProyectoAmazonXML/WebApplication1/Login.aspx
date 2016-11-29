<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title>Login</title>
    <link rel='stylesheet prefetch' href='http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/themes/smoothness/jquery-ui.css'>
    <link href="Temas/Login/css/style.css" rel="stylesheet" type="text/css" />
    <link href="Temas/Login/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Temas/Login/css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <script src="Temas/Login/js/bootstrap.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="login-card">
        <h1>
            Amazonia</h1>
        <br>
        <div>
            <strong>Id:</strong>
            <asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
            <strong>Clave:</strong>
            <asp:TextBox ID="txtContraseña" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnIngresar" runat="server" OnClick="btnIngresar_Click" Text="Ingresar"
                CssClass="login login-submit" />
        </div>
        <div class="login-help">
            <asp:Label ID="Label1" runat="server" Style="font-weight: 700; text-decoration: underline;
                color: #0000FF" Text="-"></asp:Label>
        </div>
    </div>
    <!-- <div id="error"><img src="https://dl.dropboxusercontent.com/u/23299152/Delete-icon.png" /> Your caps-lock is on.</div> -->
    <script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
    <script src='http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js'></script>
    </form>
</body>
</html>
