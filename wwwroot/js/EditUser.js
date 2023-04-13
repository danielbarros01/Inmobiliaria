function mostrarImagen() {
    var archivo = document.getElementById("avatar-input").files[0];
    var imagen = document.getElementById("avatar-img");
    var lector = new FileReader();

    lector.onloadend = function() {
        imagen.src = lector.result;
    }

    if (archivo) {
        lector.readAsDataURL(archivo);
        document.querySelector(".custom-file-label").textContent = archivo.name;
    }
}