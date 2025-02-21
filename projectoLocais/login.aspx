<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master"
    AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="projectoLocais.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2 class="titleLogin">Iniciar Sessão</h2>
    <asp:Login
        CssClass="login-form"
        ID="loginUser" runat="server"
        CreateUserText="Criar conta"
        CreateUserUrl="~/criar_conta.aspx"
        DestinationPageUrl="~/paginaInicial.aspx"
        OnLoggedIn="loginUtilizador_LoggedIn">
    </asp:Login>
</asp:Content>



