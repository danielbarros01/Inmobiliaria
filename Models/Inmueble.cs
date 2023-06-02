using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{

    public enum enumUso
    {
        Residencial = 1,
        Comercial = 2
    }

    public class Inmueble
    {
        [Display(Name = "Nº")]
        public int Id { get; set; }
        [Display(Name = "Direcciòn")]
        public string Direccion { get; set; }

        public int Uso { get; set; }
        public string UsoNombre => Uso > 0 ? ((enumUso)Uso).ToString() : "";

        [Display(Name = "Ambientes")]
        public int Cantidad_ambientes { get; set; }
        public string Coordenadas { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }

        public string? ImagenRuta { get; set; }
        [Display(Name = "Imagen")]
        public IFormFile? ImagenFile { get; set; }
        
        public int PropietarioId { get; set; }

        //[ForeignKey(nameof(PropietarioId))] mal?
        public Propietario Propietario { get; set; }

        public TipoInmueble Tipo { get; set; }

        public string ToString()
        {
            return $"{this.Tipo.Tipo} en {this.Direccion}";
        }

        //metodo que devueleve los valores de enum independientemente del enum que sea
        public static IDictionary<int, string> ObtenerUsos()
        {
            SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
            Type tipoEnumUso = typeof(enumUso); //Obtengo el tipo del enum

            foreach (var valor in Enum.GetValues(tipoEnumUso)) //recorro todos los valores
            {
                usos.Add((int)valor, Enum.GetName(tipoEnumUso, valor));
            }
            return usos;
        }
    }
}