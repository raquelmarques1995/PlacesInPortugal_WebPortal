<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="editarPassword.aspx.cs" Inherits="projectoLocais.utilizador.editarPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <h2 class="titlePassword">Alterar Palavra-Passe</h2>
    <div class="form-alterar">
        <asp:Label runat="server" AssociatedControlID="txtCurrentPassword" Text="Palavra-passe Atual:" CssClass="label"></asp:Label>
        <br />
        <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="text-input" Style="width: 280px; margin-block: 10px"></asp:TextBox>
        <asp:RequiredFieldValidator
            CssClass="ErrorMessage"
            ID="RequiredFieldPassword"
            runat="server"
            ErrorMessage="Campo Obrigatório"
            ControlToValidate="txtCurrentPassword"
            Display="Dynamic" />
        <br />
        <asp:Label runat="server" AssociatedControlID="txtNewPassword" Text="Nova Palavra-passe:" CssClass="label"></asp:Label>
        <br />
        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="text-input" Style="width: 280px; margin-block: 10px"></asp:TextBox>
        <asp:RequiredFieldValidator
            CssClass="ErrorMessage"
            ID="RequiredFieldValidator1"
            runat="server"
            ErrorMessage="Campo Obrigatório"
            ControlToValidate="txtNewPassword"
            Display="Dynamic" />
        <asp:RegularExpressionValidator
            CssClass="ErrorMessage"
            ID="RegularExpressionPassword"
            runat="server"
            ErrorMessage="Password inválida. Deve conter no mínimo 7 caracteres, um número, uma letra minúscula e um caracter especial."
            ControlToValidate="txtNewPassword"
            Display="Dynamic"
            ValidationExpression="((?=.*\d)(?=.*[a-z])(?=.*[\W]).{6,20})" />

        <br />
        <asp:Label runat="server" AssociatedControlID="txtConfirmPassword" Text="Confirmar Palavra-passe:" CssClass="label"></asp:Label>
        <br />
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="text-input" Style="width: 280px; margin-block: 10px"></asp:TextBox>
        <asp:RequiredFieldValidator
            CssClass="ErrorMessage"
            ID="RequiredFieldRepPassword"
            runat="server"
            ErrorMessage="Campo Obrigatório"
            ControlToValidate="txtConfirmPassword"
            Display="Dynamic" />
        <asp:CompareValidator
            CssClass="ErrorMessage"
            ID="CompareValidatorRepPassword"
            runat="server"
            ControlToValidate="txtConfirmPassword"
            Operator="Equal"
            ControlToCompare="txtNewPassword"
            ErrorMessage="As passwords não coincidem."
            Display="Dynamic" />

        <br />
        <asp:Button ID="btnChangePassword" runat="server" Text="Alterar" CssClass="btnChangePassword" OnClick="btnChangePassword_Click" Style="margin-top: 20px" />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="ErrorMessage"></asp:Label>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
</asp:Content>
