using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{

    public enum enRoles
    {
        Administrador = 1,
        Empleado = 2
    }

    public class Usuario
    {

        [Display(Name = "Còdigo")]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Clave { get; set; }

        [Display(Name = "Avatar")]
        public string? AvatarRuta { get; set; } //la url donde va estar guardada la foto

        [Display(Name = "Avatar")]
        public IFormFile? AvatarFile { get; set; }

        public int Rol { get; set; }
        public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

        //metodo que devueleve los valores de enum independientemente del enum que sea
        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRole = typeof(enRoles); //Obtengo el tipo del enum

            foreach (var valor in Enum.GetValues(tipoEnumRole)) //recorro todos los valores
            {
                roles.Add((int)valor, Enum.GetName(tipoEnumRole, valor));
            }
            return roles;
        }

    }
}