//Hasta que los valores no coincidan no se activa el boton guardar nueva contraseña

const newPassword = document.getElementById('new-password');
const confirmPassword = document.getElementById('confirm-password');
const guardarBtn = document.getElementById('guardar-btn');

newPassword.addEventListener('input', validarContrasena);
confirmPassword.addEventListener('input', validarContrasena);

function validarContrasena() {
  if (newPassword.value === confirmPassword.value) {
    confirmPassword.setCustomValidity('');
    guardarBtn.disabled = false;
  } else {
    confirmPassword.setCustomValidity('Las contraseñas no coinciden');
    guardarBtn.disabled = true;
  }
}