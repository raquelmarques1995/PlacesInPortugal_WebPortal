
const IDDistrito = {
    1: "1010500", /* Aveiro */
    2: "1020500", /* Beja */
    3: "1030300", /* Braga */
    4: "1040200", /* Bragança */
    5: "1050200", /* Castelo Branco */
    6: "1060300", /* Coimbra */
    7: "1070500", /* Évora */
    8: "1080500", /* Faro */
    9: "1090700", /* Guarda */
    10: "1100900", /* Leiria */
    11: "1110600", /* Lisboa */
    12: "1121400", /* Portalegre */
    13: "1131200", /* Porto */
    14: "1141600", /* Santarém */
    15: "1151200", /* Setúbal */
    16: "1160900", /* Viana do Castelo */
    17: "1171400", /* Vila Real */
    18: "1182300", /* Viseu */
    19: "3430100", /* Angra do Heroismo */
    20: "3470100", /* Horta */
    21: "3420300", /* Ponta Delgada */
    22: "2310300", /* Funchal */
}

//IMAGENS
const weatherImages = {
    0: "imgweather/w_ic_d_01anim.svg",
    1: "imgweather/w_ic_d_01anim.svg",
    2: "imgweather/w_ic_d_02anim.svg",
    3: "imgweather/w_ic_d_03anim.svg",
    4: "imgweather/w_ic_d_04anim.svg",
    5: "imgweather/w_ic_d_05anim.svg",
    6: "imgweather/w_ic_d_06anim.svg",
    7: "imgweather/w_ic_d_07anim.svg",
    8: "imgweather/w_ic_d_08anim.svg",
    9: "imgweather/w_ic_d_09anim.svg",
    10: "imgweather/w_ic_d_10anim.svg",
    11: "imgweather/w_ic_d_11anim.svg",
    12: "imgweather/w_ic_d_12anim.svg",
    13: "imgweather/w_ic_d_13anim.svg",
    14: "imgweather/w_ic_d_14anim.svg",
    15: "imgweather/w_ic_d_15anim.svg",
    16: "imgweather/w_ic_d_16anim.svg",
    17: "imgweather/w_ic_d_17anim.svg",
    18: "imgweather/w_ic_d_18anim.svg",
    19: "imgweather/w_ic_d_19anim.svg",
    20: "imgweather/w_ic_d_20anim.svg",
    21: "imgweather/w_ic_d_21anim.svg",
    22: "imgweather/w_ic_d_22anim.svg",
    23: "imgweather/w_ic_d_23anim.svg",
    24: "imgweather/w_ic_d_24anim.svg",
    25: "imgweather/w_ic_d_25anim.svg",
    26: "imgweather/w_ic_d_26anim.svg",
    27: "imgweather/w_ic_d_27anim.svg",
    28: "imgweather/w_ic_d_28anim.svg",
    29: "imgweather/w_ic_d_29anim.svg",
    30: "imgweather/w_ic_d_30anim.svg",
};

function fetchPrevisao(distritoId) {
    console.log(`Distrito ID: ${distritoId}, Código: ${IDDistrito[distritoId]}`);

    fetch(`https://api.ipma.pt/open-data/forecast/meteorology/cities/daily/${IDDistrito[distritoId]}.json`)
        .then((response) => {
            if (!response.ok) {
                document.getElementById("previsao").innerHTML =
                    '<tr><td colspan="4">Erro na previsão do tempo</td></tr>';
            }
            return response.json();
        })

        .then((data) => {
            let table = document.getElementById("previsao");

            data.data.forEach((day) => {
                //VERIFICAR SE ALGUM DOS VALORES RECEBIDOS DA API NÃO ESTÁ DEFINIDO
                const tMin = day.tMin !== undefined ? day.tMin + "ºC" : "N/A";
                const tMax = day.tMax !== undefined ? day.tMax + "ºC" : "N/A";
                const precipitaProb =
                    day.precipitaProb !== undefined ? day.precipitaProb : "N/A";

                //IMAGEM DA PREVISÃO DO TEMPO (IMAGEM POR DEFAULT)
                const weatherTypeImage =
                    weatherImages[day.idWeatherType] || "imgweather/w_ic_d_01anim.svg";

                //CONCATENAÇÃO DAS STRINGS QUE TÊM ESCRITO O HTML QUE IRÁ SER INSERIDO NA DIV ABAIXO DA PREVISãO DO TEMPO
                let row = table.insertRow(-1);

                let cellDate = row.insertCell(0);
                let cellImage = row.insertCell(1);
                let cellMin = row.insertCell(2);
                let cellMax = row.insertCell(3);

                cellDate.innerHTML = day.forecastDate;
                cellImage.innerHTML =
                    '<img src="' + weatherTypeImage + '" alt="" width=50px"/>'; //TODO - ver o alt da imagem
                cellMin.innerHTML = tMin;
                cellMax.innerHTML = tMax;
            });
        })
        .catch((error) => {
            console.error("Erro:", error);
            document.getElementById("previsao").innerHTML =
                '<tr><td colspan="4">Erro na previsão do tempo</td></tr>';
        });
}