using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
	public class LoginView
	{
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string Clave { get; set; }
	}
}