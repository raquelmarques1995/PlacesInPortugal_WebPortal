<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="pesquisa.aspx.cs" Inherits="projectoLocais.pesquisa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <section class="pesquisar col-9">
        <h2>Pesquisa</h2>
        <table>
            <tr>
                <td class="label">Nome</td>
                <td colspan="5">
                    <asp:TextBox
                        ID="textName"
                        runat="server">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">Distrito</td>
                <td>
                    <asp:DropDownList
                        ID="DropDownDistrito"
                        runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="listDistrito_SelectedIndexChanged"
                        Style="width: 200px;">
                    </asp:DropDownList>
                </td>
                <td class="label">Concelho</td>
                <td>
                    <asp:DropDownList
                        ID="DropDownConcelho"
                        runat="server"
                        Style="width: 250px;">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Button ID="searchButton" runat="server" class="btn-dark" Text="Pesquisar" Style="padding: 5px 15px;" OnClick="buttonPesquisar_Click" />
                </td>
            </tr>
        </table>
    </section>

    <asp:DataList ID="ListaLocais" runat="server" RepeatDirection="Horizontal">
        <ItemTemplate>
            <div class="card">
                <a href='<%#"local.aspx?id=" + Eval("LocalID") %>'>
                    <asp:Image ImageUrl='<%# Eval("PrimeiraFoto") %>' runat="server" ID="imagem" class="card-img" />
                </a>
                <div class="card-body">
                    <h5 class="card-title">
                        <a href="local.aspx?id=<%# Eval("LocalID") %>" class="link">
                            <asp:Label ID="nome" runat="server" Text='<%# Eval("NomeLocal") %>' CssClass="card-title" />
                        </a>
                    </h5>
                    <p class="card-text">
                        <asp:Label ID="concelho" runat="server" Text='<%# Eval("Concelho") %>' />
                        <br />
                        <asp:Label ID="distrito" runat="server" Text='<%# Eval("Distrito") %>' />
                    </p>
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>

    <div class="navegadores col-12 text-center my-5">
        <asp:LinkButton Text="<<" runat="server" ID="linkPrimeira" CssClass="text-decoration-none fs-5" OnClick="linkFirst_click" />

        <asp:LinkButton Text="<" runat="server" ID="linkAnterior" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkPrevious_click" />

        <asp:LinkButton Text=">" runat="server" ID="linkSeguinte" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkNext_click" />

        <asp:LinkButton Text=">>" runat="server" ID="linkUltima" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkLast_click" />
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script>
        // Acrescenta uma div cinza nos Locais sem foto
        const fotosLocal = Array.from(document.querySelectorAll(".card-img"))
        const semFotos = fotosLocal.filter((item, idx) => item.src === "")
        semFotos.forEach(item => item.parentElement.classList.add("skeleton"))

        //Para correção da grid quando tem apenas alguns cards no ecrã
        const table = document.querySelector('#mainContent_ListaLocais');
        const itemsPerPage = parseInt(table.getAttribute('data-pagesize')); // Lê o valor do PageSize
        const items = document.querySelectorAll('#mainContent_ListaLocais .card');
        if (items.length % itemsPerPage !== 0) {
            const emptySlots = itemsPerPage - (items.length % itemsPerPage);
            const tbody = table.children[0];
            const tr = tbody.children[0];
            for (let i = 0; i < emptySlots; i++) {
                const placeholder = document.createElement('div');
                const td = document.createElement("td");
                placeholder.className = 'card placeholder';
                td.appendChild(placeholder);
                tr.appendChild(td);
            }
        }
    </script>
</asp:Content>
