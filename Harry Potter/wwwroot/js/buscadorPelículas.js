const url = 'https://localhost:44361/api/pelicula';
let películas = [];

function getPelículas() {
    const búsqueda = document.getElementById('nombrePelícula').value.trim();
    fetch(url + '/' + búsqueda)
        .then(response => response.json())
        .then(data => _tablaPelículas(data))
        .catch(error => console.error('No hay películas.', error));
}

function formularioEdición(id) {
    const película = películas.find(película => película.id === id);
    document.getElementById('edit-valoración').value = película.valoración;
    document.getElementById('edit-id').value = película.id;
    document.getElementById('edit-título').value = película.title;
    document.getElementById('edit-año').value = película.year;
    document.getElementById('edit-poster').src = película.poster;
    document.getElementById('edit-imdb').value = película.imdbID;
    document.getElementById('edit-tipo').value = película.type;
    document.getElementById('formularioValoración').style.display = 'block';
}

function actualizarValoración() {
    const películaId = document.getElementById('edit-id').value;
    const películaTitulo = document.getElementById('edit-título').value;
    const películaAño = document.getElementById('edit-año').value;
    const películaPoster = document.getElementById('edit-poster').src;
    const películaImdb = document.getElementById('edit-imdb').value;
    const películaTipo = document.getElementById('edit-tipo').value;
    var valoración = parseInt(document.getElementById('edit-valoración').value.trim(), 10);

    const película = {
        id: películaId,
        title: películaTitulo,
        year: películaAño,
        valoración: valoración,
        poster: películaPoster,
        imdbid: películaImdb,
        type: películaTipo
    };

    fetch(`${url}/${películaId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(película)
    })
        .then(() => getPelículas())
        .catch(error => console.error('No se ha podido actualizar la película.', error));

    cerrarFormulario();

    return false;
}

function cerrarFormulario() {
    document.getElementById('formularioValoración').style.display = 'none';
}

function _contadorPelículas(númeroPelículas) {
    const name = (númeroPelículas === 1) ? 'película' : 'películas';

    document.getElementById('counter').innerText = `${númeroPelículas} ${name}`;
}

function _tablaPelículas(data) {
    const cuerpo = document.getElementById('películas');
    cuerpo.innerHTML = '';

    _contadorPelículas(data.length);

    const button = document.createElement('button');

    data.forEach(película => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Cambiar valoración';
        editButton.setAttribute('onclick', `formularioEdición('${película.id}')`);

        let tr = cuerpo.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(película.title);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(película.year);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(película.valoración);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

    });

    películas = data;
}