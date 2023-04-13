import { settearFecha } from './SettearInputFecha.js';


//Apenas cargo la pagina
window.onload = function() {
    //cargo los valores a numero y monto
    var select = document.getElementById("ContratoSelect");
    var value = select.value;
    datosExtra(value);

    //stteo fecha
    settearFecha("fecha-input");
};




//Metodo para cuando cambio de valor en el select
function datosExtra(id) {
    fetch(`DatosExtra/${id}`)
        .then(response => response.json())
        .then(data => {
            document.querySelector("#NumeroPago").value = data.siguienteNumeroPago;
            document.querySelector("#MontoPago").value = data.monto;
        })
        .catch(error => console.error(error));
}



