<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="areaPessoal.aspx.cs" Inherits="projectoLocais.utilizador.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2 class="title">Área Pessoal</h2>
    <section class="areaPessoal">
        <h3>
            <span>Olá</span>
            <asp:Label ID="userName" runat="server" />
        </h3>
        <section class="btncriarlocal">
            <asp:LinkButton
                ID="linkCriarLocal"
                runat="server"
                CommandArgument='<%# Eval("ID") %>'
                OnClick="linkCriarLocal_Click"
                CssClass="btn mt-4" BackColor="#D7D3BF"> Criar local </asp:LinkButton>
        </section>
        <section class="ListaLocais my-3">
            <asp:DataList runat="server" ID="ListaLocais" RepeatDirection="Horizontal">
                <ItemTemplate>
                    <div class="card">
                        <a href='<%# "../" + "local.aspx?id=" + Eval("LocalID") %>'>
                            <asp:Image ImageUrl='<%# "../" + Eval("PrimeiraFoto") %>' runat="server" ID="imagem" class="card-img" />
                        </a>
                        <div class="card-body">
                            <h5 class="card-title">
                                <a href="../ + local.aspx?id=<%# Eval("LocalID") %>" class="link">
                                    <asp:Label ID="nome" runat="server" Text='<%# Eval("NomeLocal") %>' CssClass="card-title" />
                                </a>
                            </h5>
                        </div>
                        <div class="buttonEditar" style="text-align:center">
                            <asp:LinkButton
                                ID="linkDetalhes" runat="server" CommandArgument='<%# Eval("LocalID") %>' OnCommand="linkDetalhes_Command" CssClass="btn mt-1 mb-2 align-content-center" BackColor="#D7D3BF" style="height:40px"> 
                            Editar local 
                            </asp:LinkButton>
                        </div>
                    </div>
                    
                </ItemTemplate>
            </asp:DataList>
        </section>
        <div id="navegadores" runat="server" class="navegadores col-12 text-center my-5">
            <asp:LinkButton Text="<<" runat="server" ID="linkPrimeira" CssClass="text-decoration-none fs-5" OnClick="linkPrimeira_Click" />
            <asp:LinkButton Text="<" runat="server" ID="linkAnterior" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkAnterior_Click" />
            <asp:LinkButton Text=">" runat="server" ID="linkSeguinte" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkSeguinte_Click" />
            <asp:LinkButton Text=">>" runat="server" ID="linkUltima" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkUltima_Click" />
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script>
        // Acrescenta uma div cinza nos Locais sem foto
        const fotosLocal = Array.from(document.querySelectorAll(".card-img"))
        const semFotos = fotosLocal.filter((item, idx) => item.src === !item.src || item.src.includes("null") || item.src.includes("undefined") || item.src.endsWith("/"))
        semFotos.forEach(item => {
            item.style.display = "none"; // Esconde a imagem quebrada
            item.parentElement.classList.add("skeleton"); // Aplica a classe 'skeleton' ao elemento pai
        });

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

