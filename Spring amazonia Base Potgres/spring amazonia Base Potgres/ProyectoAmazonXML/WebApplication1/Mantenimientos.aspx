<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true"
    CodeBehind="Mantenimientos.aspx.cs" Inherits="WebApplication1.Mantenimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="title text-center">
        Mantenimiento</h2>
    <table style="width: 100%">
        <div>
            <tr>
                <td style="width: 396px">
                    &nbsp;
                </td>
                <td style="width: 436px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnVolver" runat="server" OnClick="btnVolver_Click" Text="Volver al Menú" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    <div class="login-card">
                        <center>
                            <strong>Codigo:</strong>
                            <br />
                            <asp:TextBox ID="TextBox1" runat="server" TextMode="Number"></asp:TextBox>
                            <br />
                            <strong>Nombre:</strong>
                            <br />
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            <br />
                            <strong>Categoria:</strong><br />
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                            <br />
                            <strong>Precio:</strong><br />
                            <asp:TextBox ID="TextBox4" runat="server" TextMode="Number"></asp:TextBox>
                            <br />
                            <strong>Stock:</strong><br />
                            <asp:TextBox ID="TextBox5" runat="server" TextMode="Number"></asp:TextBox>
                            <br />
                            <strong>Imagen:</strong><br />
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </center>
                    </div>
                </td>
                <td style="width: 436px">
                    <div class="login-card">
                        Código/Cédula :
                        <asp:TextBox ID="TextBox6" runat="server" TextMode="Number"></asp:TextBox>
                        Nombre:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                        Apellidos:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                        Teléfono:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox9" runat="server" TextMode="Number"></asp:TextBox>
                        Contraseña:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                        Correo:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                </td>
                <td style="width: 436px">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    <div class="login-card">
                        <center>
                            <asp:Button ID="btnCargar" runat="server" OnClick="btnCargar_Click" Text="Cargar Datos"
                                CssClass="login login-submit" /><br />
                            <br />
                            <asp:Button ID="btnUpdaDistri" runat="server" OnClick="btnUpdaDistri_Click" Text="Actualizar"
                                Width="89px" Height="22px" CssClass="login login-submit" />
                            <br />
                            <asp:Button ID="btnInserDistri" runat="server" OnClick="btnInserDistri_Click" Text="Insertar"
                                Width="89px" Height="22px" CssClass="login login-submit" /><br />
                            <asp:Button ID="btnElimDistri" runat="server" OnClick="btnElimDistri_Click" Text="Eliminar"
                                Width="89px" Height="22px" CssClass="login login-submit" />
                        </center>
                        <br />
                        <br />
                        <center>
                            <span style="text-decoration: underline"><strong> Amazon</strong></span><br />
                            <asp:Button ID="btnInserAmazon" runat="server" OnClick="btnInserAmazon_Click" Text="Insertar"
                                Height="24px" Width="89px" /><br />
                            <asp:Button ID="btnUpdaAmazon" runat="server" Height="24px" Text="Actualizar" Width="89px"
                                OnClick="btnUpdaAmazon_Click" /><br />
                            <asp:Button ID="btnElimAmazon" runat="server" Height="24px" Text="Eliminar" Width="89px"
                                OnClick="btnElimAmazon_Click" /><br />
                            <br />
                        </center>
                    </div>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td style="width: 436px">
                    <div class="login-card">
                        <center>
                            &nbsp;Usuario&nbsp;&nbsp;&nbsp;
                            <br />
                            <asp:Button ID="btnInserUsua" runat="server" OnClick="btnInserUsua_Click" Text="Insertar"
                                Height="24px" Width="89px" /><br />
                            &nbsp;<asp:Button ID="btnUpdaUsua" runat="server" Height="24px" Text="Actualizar"
                                Width="89px" OnClick="btnUpdaUsua_Click" /><br />
                            &nbsp;<asp:Button ID="btnElimUsua" runat="server" Height="24px" Text="Eliminar" Width="89px"
                                OnClick="btnElimUsua_Click" /><br />
                            <br />
                            <br />
                            &nbsp;Vendedor<br />
                            <asp:Button ID="btnInserVende" runat="server" OnClick="btnInserVende_Click" Text="Insertar"
                                Height="24px" Width="89px" /><br />
                            &nbsp;<asp:Button ID="btnUpdaVende" runat="server" Height="24px" Text="Actualizar"
                                Width="89px" OnClick="btnUpdaVende_Click" /><br />
                            &nbsp;<asp:Button ID="btnElimVende" runat="server" Height="24px" Text="Eliminar"
                                Width="89px" OnClick="btnElimVende_Click" />
                        </center>
                    </div>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td style="width: 436px">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                </td>
                <td style="width: 436px">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    <asp:Image ID="Image1" runat="server" Height="59px" Width="75px" Style="margin-bottom: 0px" />
                </td>
                <td style="width: 436px">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    &nbsp;
                </td>
                <td style="width: 436px">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="..."></asp:Label>
                </td>
                <td style="width: 436px">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblProductos" runat="server" Text="."></asp:Label>
                </td>
                <td style="width: 436px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblUsuarios" runat="server" Text="."></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                        <Columns>
                            <asp:ButtonField CommandName="Select" HeaderText="Click" ShowHeader="True" Text="Click" />
                        </Columns>
                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FFF1D4" />
                        <SortedAscendingHeaderStyle BackColor="#B95C30" />
                        <SortedDescendingCellStyle BackColor="#F1E5CE" />
                        <SortedDescendingHeaderStyle BackColor="#93451F" />
                    </asp:GridView>
                </td>
                <td style="width: 436px">
                    <asp:GridView ID="GridView2" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" OnSelectedIndexChanged="GridView2_SelectedIndexChanged">
                        <Columns>
                            <asp:ButtonField CommandName="Select" HeaderText="Click" ShowHeader="True" Text="Click" />
                        </Columns>
                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FFF1D4" />
                        <SortedAscendingHeaderStyle BackColor="#B95C30" />
                        <SortedDescendingCellStyle BackColor="#F1E5CE" />
                        <SortedDescendingHeaderStyle BackColor="#93451F" />
                    </asp:GridView>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 396px">
                    &nbsp;
                </td>
                <td style="width: 436px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
    </table>
    </div>
</asp:Content>
