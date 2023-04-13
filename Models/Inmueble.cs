using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{

    public class Inmueble
    {
        [Display(Name = "Nº")]
        public int Id { get; set; }
        [Display(Name = "Direcciòn")]
        public string Direccion { get; set; }
        public string Uso { get; set; }
        [Display(Name = "Ambientes")]
        public int Cantidad_ambientes { get; set; }
        public string Coordenadas { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
        public int PropietarioId { get; set; }

        //[ForeignKey(nameof(PropietarioId))] mal?
        public Propietario Propietario { get; set; }

        public TipoInmueble Tipo { get; set; }

        public string ToString()
        {
            return $"{this.Tipo.Tipo} en {this.Direccion}";
        }
    }
}