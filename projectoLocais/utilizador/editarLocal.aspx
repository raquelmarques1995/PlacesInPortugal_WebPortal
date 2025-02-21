<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="editarLocal.aspx.cs" Inherits="projectoLocais.utilizador.editarLocal"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.7.1/dist/leaflet.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2 class="title">Editar Local</h2>
    <h3 class="titlenomeLocal"><asp:Label ID="nomeLocal" runat="server" /></h3>
    <section class="editarLocal">
        <table style="margin-bottom: 60px;">
            <tr>
                <th class="legenda">Nome</th>
                <td colspan="3" class="controlo">
                    <asp:TextBox runat="server" ID="textName" CssClass="form-control" Width="600" />
                </td>
            </tr>
            <tr style="height: 120px;">
                <th class="legenda">Descrição</th>
                <td colspan="3" class="controlo">
                    <asp:TextBox runat="server" ID="textDescricao" CssClass="form-control" Width="600" TextMode="MultiLine" Height="100" />
                </td>
            </tr>
            <tr>
                <th class="legenda">Morada</th>
                <td colspan="3" class="controlo">
                    <asp:TextBox runat="server" ID="textMorada" CssClass="form-control" Width="600" />
                </td>
            </tr>
            <tr>
                <th class="legenda">Localidade</th>
                <td colspan="3" class="controlo">
                    <asp:TextBox runat="server" ID="textLocalidade" CssClass="form-control" Width="600" />
                </td>
            </tr>
            <tr>
                <th class="legenda">Distrito</th>
                <td class="controlo">
                    <asp:DropDownList runat="server" ID="DropDownDistrito" CssClass="form-select" Width="350px" DataTextField="Nome" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="DropDownDistrito_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Obrigatório" ControlToValidate="DropDownDistrito" InitialValue="Selecione um Distrito"
                        runat="server" Display="Dynamic" ForeColor="#CC0000" />
                </td>
                <th class="legenda">Concelho</th>
                <td class="controlo">
                    <asp:DropDownList runat="server" ID="DropDownConcelho" CssClass="form-select" Width="350px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ErrorMessage="Obrigatório" ControlToValidate="DropDownConcelho" InitialValue="Selecione um Concelho"
                        runat="server" Display="Dynamic" ForeColor="#CC0000" />
                </td>
            </tr>
            <tr>
                <td colspan="4" style="height:50px; vertical-align:bottom">Por favor indique a localização do local no mapa:</td>
            </tr>
            <tr style="height: 400px; vertical-align: middle;">
                <td colspan="4" style="text-align: center;">
                    <div class="justify-content-center">
                        <div class="col-12">
                            <div id="map" style="height: 350px" class="w-100" />
                            <asp:HiddenField ID="latitude" runat="server" />
                            <asp:HiddenField ID="longitude" runat="server" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr style="height: 100px;">
                <th class="legenda"></th>
                <td class="controlo">
                    <!--Button de Guardar o Local -->
                    <asp:Button Text="Guardar" runat="server" ID="buttonGuardar" CssClass="buttonEditarLocal" OnClick="buttonGuardar_Click" />
                    <!--Button de Cancelar o Local -->
                    <asp:Button Text="Cancelar" runat="server" ID="buttonCancelar" CssClass="buttonEditarLocal" OnClick="buttonCancelar_Click" />
                    <asp:Button Text="Eliminar" runat="server" ID="buttonEliminar" CssClass="buttonEditarLocal" OnClick="buttonEliminar_Click" />
                </td>
            </tr>
        </table>

        <h3>Fotos do Local</h3>
        <asp:DataList
            runat="server"
            ID="listFotos"
            RepeatColumns="2"
            RepeatDirection="Horizontal">
            <ItemTemplate>
                <table class="table table-borderless">
                    <tr style="height: 220px; vertical-align: middle;">
                        <td style="width: 450px; text-align: center;">
                            <img src='../<%# Eval("Ficheiro") %>' alt='<%# Eval("Legenda") %>' style="width: 350px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text='<%# Eval("Legenda") %>' runat="server" CssClass="fs-3" />
                        </td>
                    </tr>
                    <tr>
                        <td><%-- button que permite selecionar uma foto para editar a legenda ou para remover--%>
                            <asp:LinkButton
                                ID="buttonSelecionar"
                                runat="server"
                                CommandArgument='<%# Eval("ID") %>'
                                OnCommand="buttonSelecionar_Command"
                                CssClass="buttonEditarLocal"
                                BackColor="#D7D3BF">Selecionar
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>

        <table style="margin-bottom: 60px;">
            <tr>
                <th class="legenda">Selecionar foto</th>
                <td class="controlo">
                    <asp:FileUpload runat="server" ID="uploadFoto" CssClass="form-control w-100" />
                </td>
            </tr>
            <tr style="height: 120px;">
                <th class="legenda">legenda</th>
                <td class="controlo">
                    <asp:TextBox runat="server" ID="textLegenda" CssClass="form-control" Width="600" Height="100" />
                </td>
            </tr>
            <tr style="height: 100px;">
                <th class="legenda"></th>
                <td class="controlo">
                    <!--Button de Guardar Foto -->
                    <asp:Button Text="Guardar Foto" runat="server" ID="buttonGuardarFoto" CssClass="buttonEditarLocal" OnClick="buttonGuardarFoto_Click" />
                    <!--Button de Editar a Legenda da Foto -->
                    <asp:Button Text="Editar Legenda" runat="server" ID="buttonEditarLegenda" CssClass="buttonEditarLocal" OnClick="buttonEditarLegenda_Click" />
                    <!--Button de ELiminar Foto -->
                    <asp:Button Text="Eliminar Foto" runat="server" ID="buttonEliminarFoto" CssClass="buttonEditarLocal" OnClick="buttonEliminarFoto_Click" />
                    <!--Button de Cancelar Foto -->
                    <asp:Button Text="Cancelar Foto" runat="server" ID="buttonCancelarFoto" CssClass="buttonEditarLocal" OnClick="buttonCancelarFoto_Click" />
                </td>
            </tr>
        </table>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptContent" runat="server">
    <script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js"></script>
    <script> 
        // Obter valores da base de dados definidos no evento Load
        var lat = parseFloat('<%= Latitude %>') || 39.69484; // Valor padrão caso não haja coordenadas
        var lng = parseFloat('<%= Longitude %>') || -8.13031;
        var nome = '<%= Nome %>';

        // Inicializar o mapa centrado nas coordenadas carregadas
        let mapOptions = {
            center: [lat, lng],
            zoom: 11
        };
        let map = new L.map('map', mapOptions);
        let layer = new L.TileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 15 });
        map.addLayer(layer);

        // Criar o marcador inicial com popup
        let marker = L.marker([lat, lng]).addTo(map).openPopup();

        // Identificar os campos escondidos de latitude e longitude no formulário
        const latitudeField = '<%= latitude.ClientID %>';
        const longitudeField = '<%= longitude.ClientID %>';

        // Permitir que o utilizador mova o marcador ao clicar no mapa
        map.on('click', (event) => {
            if (marker !== null) {
                map.removeLayer(marker);
            }
            marker = L.marker([event.latlng.lat, event.latlng.lng]).addTo(map);

            // Atualizar os campos escondidos com as novas coordenadas
            document.getElementById(latitudeField).value = event.latlng.lat;
            document.getElementById(longitudeField).value = event.latlng.lng;
        });
    </script>
</asp:Content>
