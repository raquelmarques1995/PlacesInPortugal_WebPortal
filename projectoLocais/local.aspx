<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="local.aspx.cs" Inherits="projectoLocais.local1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.7.1/dist/leaflet.css" />
    <script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:Label ID="nomeLocal" runat="server" Text="Label" CssClass="nomeLocal"></asp:Label>
    <br />
    <asp:Label ID="localizacao" runat="server" Text="Label" CssClass="nomeLocalizacao"></asp:Label>

    <!-- Carousel -->
    <section id="carouselExampleIndicators" class="carousel slide carousel-css" data-bs-ride="carousel">
        <!-- Indicadores -->
        <div class="carousel-indicators">
            <asp:Repeater ID="RepeaterIndicators" runat="server">
                <ItemTemplate>
                    <button type="button" data-bs-target="#carouselExampleIndicators"
                        data-bs-slide-to="<%# Container.ItemIndex %>"
                        class="rounded-circle <%# Container.ItemIndex == 0 ? "active" : "" %>">
                    </button>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Imagens -->
        <div class="carousel-inner">
            <asp:Repeater ID="rptFotos" runat="server">
                <ItemTemplate>
                    <div class="carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                        <img class="carousel-item-image" src="<%# Eval("Ficheiro") %>" alt="<%# Eval("Legenda") %>">
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Botões de navegação -->
        <div>
            <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev" style="position: absolute; top: 50%; left: 5px; transform: translateY(-50%);">
                <span class="carousel-control-prev-icon rounded-circle bg-secondary" aria-hidden="true"></span>
                <span class="visually-hidden">Previous</span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next" style="position: absolute; top: 50%; right: 5px; transform: translateY(-50%);">
                <span class="carousel-control-next-icon rounded-circle bg-secondary" aria-hidden="true"></span>
                <span class="visually-hidden">Next</span>
            </button>
        </div>

    </section>
    <section class="descricaolocal">
        <h4>Descrição do Local</h4>
        <asp:Label CssClass="descricaotext" ID="descricao" runat="server" Text="Label"></asp:Label>
    </section>


    <div class="container">
    <div class="row">
        <!-- PREVISÕES -->
        <div class="previsao col-md-6">
                <p>Previsão do Tempo</p>
                <table id="previsao" class="table table-striped my-3">
                    <thead>
                        <tr>
                            <th>Data</th>
                            <th>Previsão</th>
                            <th>Mínima</th>
                            <th>Máxima</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Dados da previsão serão inseridos aqui -->
                    </tbody>
                </table>
        </div>
        <!-- MAPS -->
        <div class="maps col-md-6">
                <p>Localização</p>
                <div class="d-flex justify-content-center my-3">
                    <div class="w-100">
                        <div id="map" style="height: 400px"></div>
                    </div>
                </div>
        </div>
    </div>
</div>
        <h3>&nbsp;</h3>
    <section class="comentario">
        <h3>Quem já visitou?</h3>
        <asp:DataList runat="server" ID="listComentarios" CssClass="mt-3">
            <ItemTemplate>
                <div>
                    <table class="comentariotable">
                        <tr>
                            <td>
                                <asp:Label ID="comentario" runat="server" Text='<%# Eval("comentario") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:Label Text='<%# "Classificação - " + Eval("classificacao ")?.ToString() %>' runat="server" /><i class="bi bi-star-fill"></i>
                            </td>
                       </tr>
                        <tr>
                            <td>
                                <asp:Label ID="utilizador" runat="server" Text='<%# Eval("utilizador") %>' />
                            </td>
                        </tr>
                    </table>
              </div>
                 <hr class="separador">
            </ItemTemplate>
        </asp:DataList>
        <div class="navegadores">
            <asp:LinkButton Text="<<" runat="server" ID="linkPrimeira" CssClass="text-decoration-none fs-5" OnClick="linkPrimeira_Click" />
            <asp:LinkButton Text="<" runat="server" ID="linkAnterior" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkAnterior_Click" />
            <asp:LinkButton Text=">" runat="server" ID="linkSeguinte" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkSeguinte_Click" />
            <asp:LinkButton Text=">>" runat="server" ID="linkUltima" CssClass="ms-3 text-decoration-none fs-5" OnClick="linkUltima_Click" />
        </div>
    </section>
    <% if (mostrarComentario) { %>
    <section class="comentar">
        <div id="divComentario" runat="server">
            <h5>Escreva o seu comentário:</h5>
            <table>
                <tr>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="textComentario" CssClass="form-control border-secondary" TextMode="MultiLine"/>
                    </td>
                </tr>
                <tr>
                    <td style="width:50px">Classificação</td>
                    <td>
                        <asp:DropDownList ID="listClassificacao" runat="server" CssClass="form-select w-75">
                            <asp:ListItem Text="1" Value="1" />
                            <asp:ListItem Text="2" Value="2" />
                            <asp:ListItem Text="3" Value="3" />
                            <asp:ListItem Text="4" Value="4" />
                            <asp:ListItem Text="5" Value="5" />
                            <asp:ListItem Text="6" Value="6" />
                            <asp:ListItem Text="7" Value="7" />
                            <asp:ListItem Text="8" Value="8" />
                            <asp:ListItem Text="9" Value="9" />
                            <asp:ListItem Text="10" Value="10" />
                        </asp:DropDownList>
                    </td>
                    <td class="buttonComentar">
                        <asp:Button Text="Comentar" runat="server" ID="buttonComentar" OnClick="buttonComentar_Click" CssClass="btn" />
                    </td>
                </tr>
            </table>
        </div>
    </section>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"> </script>
    <script src="scripts/apiIPMA.js"></script>
    <script type="text/javascript">
        fetchPrevisao(<asp:Literal ID="IDdistrito" runat="server"></asp:Literal>)
    </script>
    <script> 
        // Obter valores das propriedades definidos no evento Load com os valores lidos da tabela 
        var latStr = '<%= Latitude %>';
        var lngStr = '<%= Longitude %>';
        var nome = '<%= Nome %>';

        // Converter para número, verificando se são valores válidos
        var lat = parseFloat(latStr);
        var lng = parseFloat(lngStr);
        var coordenadasValidas = !isNaN(lat) && !isNaN(lng);

        // Definir a posição inicial do mapa
        var coordenadasIniciais = coordenadasValidas ? [lat, lng] : [39.5, -8.0]; // Coordenadas padrão (Portugal)
        var zoomInicial = coordenadasValidas ? 11 : 6; // Ajustar zoom dependendo da presença das coordenadas

        // Criar o mapa
        var map = L.map('map').setView(coordenadasIniciais, zoomInicial);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 15 }).addTo(map);

        // Se houver coordenadas válidas, adicionar marcador e popup
        if (coordenadasValidas) {
            L.marker([lat, lng]).addTo(map).bindPopup(nome).openPopup();
        }
    </script>
</asp:Content>
