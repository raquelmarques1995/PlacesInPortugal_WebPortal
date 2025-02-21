<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="criar_conta.aspx.cs" Inherits="projectoLocais.criar_conta1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <h2 class="title">Criar Conta</h2>
    <section class="section-criarConta row">
        <div class="form-criarConta col-12 col-md-6">
            <table class="table-criarconta">
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
                    <!--Data de Nascimento-->
                    <td>Data de Nascimento</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox
                            ID="textBirthdate"
                            TextMode="Date"
                            Class="form-control me-2"
                            runat="server"
                            Style="width: 300px; display: inline">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator
                            CssClass="ErrorMessage"
                            ID="RequiredFieldData"
                            runat="server"
                            ErrorMessage="Campo Obrigatório"
                            ControlToValidate="textBirthdate"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator
                            CssClass="ErrorMessage"
                            ID="validarDataNascimento"
                            runat="server"
                            ErrorMessage="Data de nascimento inválida."
                            ControlToValidate="textBirthdate"
                            Display="Dynamic">
                        </asp:CompareValidator>
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
                <!--Password--->
                <tr>
                    <td>Password</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox
                            ID="textPassword"
                            TextMode="Password"
                            Class="form-control me-2"
                            runat="server"
                            Style="width: 300px; display: inline">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator
                            CssClass="ErrorMessage"
                            ID="RequiredFieldPassword"
                            runat="server"
                            ErrorMessage="Campo Obrigatório"
                            ControlToValidate="textPassword"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator
                            CssClass="ErrorMessage"
                            ID="RegularExpressionPassword"
                            runat="server"
                            ErrorMessage="Password inválida. Deve conter no mínimo 7 caracteres, um número, uma letra minúscula e um caracter especial."
                            ControlToValidate="textPassword"
                            Display="Dynamic"
                            ValidationExpression="((?=.*\d)(?=.*[a-z])(?=.*[\W]).{6,20})">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <!--Repetir Password--->
                <tr>
                    <td>Repita a Password</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox
                            ID="textPasswordrepeat"
                            TextMode="Password"
                            Class="form-control me-2"
                            runat="server"
                            Style="width: 300px; display: inline">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator
                            CssClass="ErrorMessage"
                            ID="RequiredFieldRepPassword"
                            runat="server"
                            ErrorMessage="Campo Obrigatório"
                            ControlToValidate="textPasswordrepeat"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator
                            CssClass="ErrorMessage"
                            ID="CompareValidatorRepPassword"
                            runat="server"
                            ControlToValidate="textPasswordrepeat"
                            Operator="Equal"
                            ControlToCompare="textPassword"
                            ErrorMessage="As passwords não coincidem."
                            Display="Dynamic">
                        </asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <asp:Button CssClass="buttonCriarConta" ID="ButtonCriarConta" runat="server" Text="Criar Conta" OnClick="ButtonCriarConta_Click" />
        </div>
        <div class="form-img col-12 col-md-6">
           <img class="imgCriarConta img-fluid" src="imagens/monsanto3.jpg" alt="Imagem de Linhares da Beira">
            </div>
    </section>
</asp:Content>
