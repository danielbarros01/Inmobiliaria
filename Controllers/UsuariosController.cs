using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly RepositorioUsuario Repo;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            Repo = new RepositorioUsuario();
        }

        // GET: Usuarios
        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            var usuarios = Repo.GetUsuarios();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            var user = Repo.ObtenerPorId(id);
            return View(user);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario u)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));
                u.Clave = hashed;
                u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;

                int res = Repo.Alta(u);

                if (u.AvatarFile != null && u.Id > 0)
                {
                    u.AvatarRuta = u.AvatarFile.GuardarImagen(environment.WebRootPath, u.Id);
                    Repo.Modificar(u);

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            var user = Repo.ObtenerPorId(id);

            return View(user);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario u)
        {
            try
            {
                u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;


                if (u.AvatarFile != null && u.Id > 0)
                {
                    u.AvatarRuta = ImagenExtensions.GuardarImagen(u.AvatarFile, environment.WebRootPath, u.Id);
                }

                Repo.Modificar(u);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            var user = Repo.ObtenerPorId(id);
            return View(user);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario u)
        {
            try
            {
                int res = Repo.Eliminar(id);

                //eliminar foto
                if (res != -1)
                {
                    var ruta = Path.Combine(environment.WebRootPath, "uploads", $"avatar_{id}" + Path.GetExtension(u.AvatarRuta));
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Usuarios/EditPassword/5
        //[Route("EditPassword", Name = "editPassword")]
        public ActionResult EditPassword(int id)
        {
            var user = Repo.ObtenerPorId(id);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(int id, Usuario u, string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: currentPassword,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));

                // Buscar en la base de datos el registro de usuario correspondiente al nombre de usuario o correo electrónico proporcionado por el usuario
                Usuario user = Repo.ObtenerPorId(u.Id); //Verificar que ande

                if (user != null)
                {
                    // Obtener el hash de contraseña almacenado en la base de datos para ese usuario
                    string passwordUser = user.Clave;

                    //El boton no se puede activar si no coinciden, pero si modificaran esto, el controlador se encarga del problema
                    if (newPassword != confirmPassword)
                    {
                        ViewBag.Error = "Las contraseñas no eran iguales";
                        return View(u);
                    }
                    // Comparar el hash de la contraseña ingresada por el usuario con el hash de la contraseña almacenado en la base de datos
                    if (hashedPassword == passwordUser)
                    {
                        string hashedNewPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: newPassword,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));

                        u.Clave = hashedNewPassword; //hashear
                        Repo.ModificarPassword(u);

                        TempData["Mensaje"] = "Contraseña cambiada";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = "La contraseña actual no es correcta";
                        return View(u);
                    }


                }

                ViewBag.Error = "Hubo un problema";
                return View(u);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(u);
            }
        }
    }

    public static class ImagenExtensions
    {
        public static string GuardarImagen(this IFormFile imagen, string webRootPath, int id)
        {
            if (imagen == null) return null;

            string path = Path.Combine(webRootPath, "uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = $"avatar_{id}{Path.GetExtension(imagen.FileName)}";//Ruta fisica del servidor
            string fullPath = Path.Combine(path, fileName);// /uploads/avatar_1.jpg //Ruta que voy a poner en mi BD

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                imagen.CopyTo(stream);
            }

            return Path.Combine("/uploads", fileName);
        }
    }

}