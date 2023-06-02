using System.Security.Claims;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Utilities;

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
        [Authorize(Policy = "Administrador")]
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
                //u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;

                int res = Repo.Alta(u);

                if (u.AvatarFile != null && u.Id > 0)
                {
                    u.AvatarRuta = u.AvatarFile.GuardarImagen(environment.WebRootPath, u.Id, "avatar");
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
            try
            {
                if (User.IsInRole("Empleado") && int.Parse(User.FindFirstValue("Id")) != id)
                {
                    ViewBag.Error = "No tienes acceso a este usuario";
                    return RedirectToAction("Restringido", "Home");
                }

                var user = Repo.ObtenerPorId(id);

                return View(user);
            }
            catch (Exception e)
            {
                throw;
            }

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
                    u.AvatarRuta = ImagenExtensions.GuardarImagen(u.AvatarFile, environment.WebRootPath, u.Id, "avatar");
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
        [Authorize(Policy = "Administrador")]
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
            try
            {
                if (User.IsInRole("Empleado") && int.Parse(User.FindFirstValue("Id")) != id)
                {
                    ViewBag.Error = "No tienes acceso a este usuario";
                    return RedirectToAction("Restringido", "Home");
                }

                var user = Repo.ObtenerPorId(id);
                return View(user);
            }
            catch (System.Exception)
            {

                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(int id, Usuario u, string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (User.IsInRole("Empleado") && int.Parse(User.FindFirstValue("Id")) != u.Id)
                {
                    ViewBag.Error = "No tienes acceso a este usuario";
                    return RedirectToAction("Restringido", "Home");
                }

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


        [HttpGet]
        [Route("Perfil")]
        public ActionResult Perfil()
        {
            var user = Repo.ObtenerPorEmail(User.Identity.Name);
            return View("Edit", user);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    //Contraseña
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: login.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));
                    //----------------

                    //EMAIL
                    var user = Repo.ObtenerPorEmail(login.Email);

                    if (user == null || user.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        ViewBag.Error = "El email o la clave no son correctos";
                        return View();
                    }
                    //----------------

                    //Claims
                    var claims = new List<Claim>
                    {
                        new Claim("Id", user.Id+""),
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("FullName", user.Nombre + " " + user.Apellido),
                        new Claim(ClaimTypes.Role, user.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //----------------

                    //Se utiliza el método HttpContext.SignInAsync para crear una cookie de autenticación para el usuario.
                    await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity));


                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                Console.WriteLine(ex);
                return View();
            }
        }

        // GET: /salir
        [Route("Salir", Name = "Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}