<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="editarPerfil.aspx.cs" Inherits="projectoLocais.utilizador.editarPerfil"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <h2 class="title">Editar Perfil</h2>
    <div class="editarPerfil">
        <table class="table-editarperfil">
            <tr>
                <!--Nome-->
                <td>Nome</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="textName"
                        Class="form-control me-2"
                        runat="server"
                        Style="width: 450px; display: inline">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        CssClass="ErrorMessage"
                        ID="RequiredFieldNome"
                        runat="server"
                        ErrorMessage="Campo Obrigatório"
                        ControlToValidate="textName"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <!--Email-->
                <td>Email</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox
                        ID="textEmail"
                        TextMode="Email"
                        Class="form-control me-2"
                        runat="server"
                        Style="width: 450px; display: inline">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        CssClass="ErrorMessage"
                        ID="RequiredFieldEmail"
                        runat="server"
                        ErrorMessage="Obrigatório"
                        ControlToValidate="textEmail"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator
                        CssClass="ErrorMessage"
                        ID="RegularExpressionEmail"
                        runat="server"
                        ErrorMessage="Email inválido."
                        ControlToValidate="textEmail"
                        Display="Dynamic"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <!--Nome de utilizador-->
                <td>Nome de utilizador</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox
                        ID="textUsername"
                        Class="form-control me-2"
                        runat="server"
                        Style="width: 300px; display: inline">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        CssClass="ErrorMessage"
                        ID="RequiredFieldNUtilizador"
                        runat="server"
                        ErrorMessage="Campo Obrigatório"
                        ControlToValidate="textUsername"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <asp:Button CssClass="buttonEditarPerfil" ID="ButtonEditarPerfil" runat="server" Text="Editar Perfil" OnClick="ButtonEditarPerfil_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
</asp:Content>
