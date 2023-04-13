export function settearFecha(inputFecha) {
    // Obtener la fecha actual
    var today = new Date();

    var year = today.getFullYear();
    var month = ('0' + (today.getMonth() + 1)).slice(-2);
    var day = ('0' + today.getDate()).slice(-2);
    var hours = ('0' + today.getHours()).slice(-2);
    var minutes = ('0' + today.getMinutes()).slice(-2);

    var formattedDate = year + '-' + month + '-' + day + 'T' + hours + ':' + minutes;

    document.getElementById(inputFecha).setAttribute("max", formattedDate);
}