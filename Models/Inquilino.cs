namespace Inmobiliaria.Models;

public class Inquilino
{

    public int Id { get; set; }
    public int Dni { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }

    public Inquilino() { }

    public string ToString()
    {
        return this.Nombre + " " + this.Apellido;
    }
}