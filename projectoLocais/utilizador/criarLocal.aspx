<%@ Page Title="" Language="C#" MasterPageFile="~/modelo.Master" AutoEventWireup="true" CodeBehind="criarLocal.aspx.cs" Inherits="projectoLocais.utilizador.criarLocal" MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.7.1/dist/leaflet.css" />
    <script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <h2 class="title">Criar Local</h2>
    <section class="criarLocal">
        <div>
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
                    <td class="controlo" style="width:380px">
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
                <tr style="height: 600px; vertical-align: middle;">
                    <td colspan="4" style="text-align: center;">
                        <div id="map" style="width: 90%; height: 560px"></div>
                        <asp:HiddenField ID="latitude" runat="server" />
                        <asp:HiddenField ID="longitude" runat="server" />
                    </td>
                </tr>
                <tr style="height: 100px;">
                    <th class="legenda"></th>
                    <td class="controlo">
                        <!--Button de Guardar o Local -->
                        <asp:Button Text="Guardar" runat="server" ID="buttonGuardar" CssClass="buttonCriarLocal" OnClick="button_save_local" />
                        <!--Button de Cancelar o Local -->
                        <asp:Button Text="Cancelar" runat="server" ID="buttonCancelar" CssClass="buttonCriarLocal ms-4" OnClick="buttonCancelar_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div >
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
                                <img src='../<%# Eval("Ficheiro") %>' alt='<%# Eval("Legenda") %>' style="width: 350px; border-radius:10px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Text='<%# Eval("Legenda") %>' runat="server" style="font-size:20px"/>
                            </td>
                        </tr>
                        <tr>
                            <td><%-- button que permite selecionar uma foto para editar a legenda ou para remover--%>
                                <asp:LinkButton
                                    ID="buttonSelecionar"
                                    runat="server"
                                    CommandArgument='<%# Eval("ID") %>'
                                    OnCommand="buttonSelecionar_Command"
                                    CssClass="buttonCriarLocal"
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
                    <th class="legenda">Legenda</th>
                    <td class="controlo">
                        <asp:TextBox runat="server" ID="textLegenda" CssClass="form-control" Width="600" Height="100" />
                    </td>
                </tr>
                <tr style="height: 100px;">
                    <th class="legenda"></th>
                    <td class="controlo">
                        <!--Button de Guardar Foto -->
                        <asp:Button Text="Guardar Foto" runat="server" ID="buttonGuardarFoto" CssClass="buttonCriarLocal" OnClick="buttonGuardarFoto_Click" />
                        <!--Button de Editar a Legenda da Foto -->
                        <asp:Button Text="Editar Legenda" runat="server" ID="buttonEditarLegenda" CssClass="buttonCriarLocal ms-4" OnClick="buttonEditarLegenda_Click" />
                        <!--Button de ELiminar Foto -->
                        <asp:Button Text="Eliminar Foto" runat="server" ID="buttonEliminarFoto" CssClass="buttonCriarLocal ms-4" OnClick="buttonEliminarFoto_Click" />
                        <!--Button de Cancelar Foto -->
                        <asp:Button Text="Cancelar Foto" runat="server" ID="buttonCancelarFoto" CssClass="buttonCriarLocal ms-4" OnClick="buttonCancelarFoto_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </section>
    <script> 
        let mapOptions = {
            center: [39.69484, - 8.13031],
            zoom: 7
        }
        let map = new L.map('map', mapOptions);
        let layer = new L.TileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png');
        map.addLayer(layer);
        let marker = null;


        const latitudeField = '<%= latitude.ClientID %>';
        const longitudeField = '<%= longitude.ClientID %>';

        map.on('click', (event) => {
            if (marker !== null) {
                map.removeLayer(marker);
            }
            marker = L.marker([event.latlng.lat, event.latlng.lng]).addTo(map);

            // Put the values of latitude/longitude on the hiddenfields
            document.getElementById(latitudeField).value = event.latlng.lat;
            document.getElementById(longitudeField).value = event.latlng.lng;
        })
    </script>

</asp:Content>
