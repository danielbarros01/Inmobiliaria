namespace Inmobiliaria.Models;

public class Pago
{
    public int NumeroPago { get; set; }
    public DateTime? Fecha { get; set; }
    public int ContratoId { get; set; }

    public Contrato Contrato { get; set; }

    public Pago() {
     }

     public override string ToString()
    {
        return $"{NumeroPago} {Fecha} {Contrato}";
    }

}