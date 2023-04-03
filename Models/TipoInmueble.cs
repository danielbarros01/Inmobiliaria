namespace Inmobiliaria.Models;

public class TipoInmueble
{
    public int Id { get; set; }
    public string Tipo { get; set; }

    public TipoInmueble() { }

     public override string ToString()
    {
        return $"{Tipo}";
    }

}