using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioInquilino
{

    string connectionString = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=roque;";

    public RepositorioInquilino()
    { }

    public List<Inquilino> GetInquilinos()
    {
        var list = new List<Inquilino>();
        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,Dni,Nombre,Apellido,Email,Telefono FROM inquilinos";

            using (var command = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),//Me da el id
                            Dni = reader.GetInt32(nameof(Inquilino.Dni)),//Es equivalente a reader.GetString(nameof("Dni"))
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono))
                        };

                        list.Add(inquilino);
                    }
                }
                conn.Close();
            }
        }
        return list;
    }

    public int Alta(Inquilino i)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO inquilinos(Dni,Nombre,Apellido,Email, Telefono)
                                VALUES(@dni,@nombre, @apellido, @Email, @telefono);
                                SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@dni", i.Dni);
                command.Parameters.AddWithValue("@nombre", i.Nombre);
                command.Parameters.AddWithValue("@apellido", i.Apellido);
                command.Parameters.AddWithValue("@telefono", i.Telefono);
                command.Parameters.AddWithValue("@email", i.Email);
                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar()); // devuelve el valor de la última fila insertada en la tabla. Este valor se convierte a un entero y se asigna a la variable res.
                i.Id = res;
                conn.Close();
            }
        }
        return res;
    }

    public Inquilino GetInquilino(int id)
    {
        Inquilino? res = null;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,Dni,Nombre,Apellido,Email,Telefono 
            FROM inquilinos
            WHERE Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),//Me da el id
                            Dni = reader.GetInt32(nameof(Inquilino.Dni)),//Es equivalente a reader.GetString(nameof("Dni"))
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono))
                        };
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

    public int Modificar(Inquilino p)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE inquilinos SET 
            Dni=@dni,
            Nombre=@nombre,
            Apellido=@apellido,
            Email=@email,
            Telefono=@telefono 
            WHERE Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@dni", p.Dni);
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@telefono", p.Telefono);
                command.Parameters.AddWithValue("@email", p.Email);
                command.Parameters.AddWithValue("@id", p.Id);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    public int Eliminar(int id)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"DELETE FROM inquilinos WHERE Id = @Id;";
            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }


    
}
