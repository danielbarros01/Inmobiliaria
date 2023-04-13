using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{
    public class Contrato
    {
        [Display(Name = "Nº")]
        public int Id { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime Desde { get; set; }
        [Display(Name = "Vencimiento")]
        public DateTime Hasta { get; set; }
        [Display(Name = "Condiciones y requisitos")]
        public string Condiciones { get; set; }
        public Decimal Monto { get; set; }

        public int InmuebleId { get; set; }
        public int InquilinoId { get; set; }

        //[ForeignKey("inmueble_Id")]
        public Inmueble Inmueble { get; set; }

        //[ForeignKey("inquilino_Id")]
        public Inquilino Inquilino { get; set; }


        public string ToString()
        {
            return $"Contrato Nº{Id} del inquilno {Inquilino.ToString()} con {Inmueble.ToString()}";
        }

        public string ParsearFecha(DateTime fecha)
        {
            string fechaFormateada = fecha.ToString("dd/MM/yyyy");
            return fechaFormateada;
        }
    }
}