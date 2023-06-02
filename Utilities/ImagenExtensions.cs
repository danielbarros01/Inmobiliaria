namespace Inmobiliaria.Utilities;

public static class ImagenExtensions
{
    public static string GuardarImagen(this IFormFile imagen, string webRootPath, int id, string startName)
    {
        if (imagen == null) return null;

        string path = Path.Combine(webRootPath, "uploads");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = $"{startName}_{id}{Path.GetExtension(imagen.FileName)}";//Ruta fisica del servidor
        string fullPath = Path.Combine(path, fileName);// /uploads/avatar_1.jpg //Ruta que voy a poner en mi BD

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            imagen.CopyTo(stream);
        }

        return Path.Combine("/uploads", fileName);
    }
}