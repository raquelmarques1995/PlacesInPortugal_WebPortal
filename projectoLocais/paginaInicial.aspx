<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="paginaInicial.aspx.cs" Inherits="projectoLocais.paginaInicial"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <section class="section-paginainicial row">

        <!-- Locais com melhor pontuação -->
        <h2>Top de Locais</h2>
        <div class="topLocais d-flex overflow-auto">
            
            <asp:DataList ID="listLocaisTop" runat="server" RepeatDirection="Vertical">
                <ItemTemplate>
                    <div class="topCard">
                        <!-- número de ordem e Nome -->
                        <div class="ordem">
                            <span>#<%# Eval("NumeroOrdem") %>:</span>
                            <a href='local.aspx?id=<%# Eval("LocalId") %>'><%# Eval("LocalNome") %> </a>
                        </div>
                        <a href='local.aspx?id=<%# Eval("LocalId") %>'>
                            <img class="imgTop" src='<%# Eval("PrimeiraImagem") %>' alt='<%# Eval("LocalNome") %>' />
                        </a>
                        <!-- classificação -->
                        <div class="classificacao">
                            Classificação: <%# Eval("MediaClassificacao") %> <i class="bi bi-star-fill"></i>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>

        <!-- Locais -->
        <div class="Locais col-12">
            <h2>Locais</h2>
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
                <asp:LinkButton Text="<<" runat="server" ID="linkPrimeira" CssClass="text-decoration-none fs-5" OnClick="linkPrimeira_Click" />
                <asp:LinkButton Text="<" runat="server" ID="linkAnterior" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkAnterior_Click" />
                <asp:LinkButton Text=">" runat="server" ID="linkSeguinte" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkSeguinte_Click" />
                <asp:LinkButton Text=">>" runat="server" ID="linkUltima" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkUltima_Click" />
            </div>
        </div>
    </section>
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
