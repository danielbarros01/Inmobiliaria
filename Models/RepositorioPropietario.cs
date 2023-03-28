using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioPropietario
{

    string connectionString = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=roque;";

    public RepositorioPropietario()
    { }

    public List<Propietario> GetPropietarios()
    {
        var list = new List<Propietario>();
        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,Dni,Nombre,Apellido,Email,Telefono FROM propietarios";

            using (var command = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propietario propietario = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),//Me da el id
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),//Es equivalente a reader.GetString(nameof("Dni"))
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono))
                        };

                        list.Add(propietario);
                    }
                }
                conn.Close();
            }
        }
        return list;
    }

    public int Alta(Propietario p)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO propietarios(Dni,Nombre,Apellido,Email, Telefono)
                                VALUES(@dni,@nombre, @apellido, @Email, @telefono);
                                SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@dni", p.Dni);
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@telefono", p.Telefono);
                command.Parameters.AddWithValue("@email", p.Email);
                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar()); // devuelve el valor de la última fila insertada en la tabla. Este valor se convierte a un entero y se asigna a la variable res.
                conn.Close();
            }
        }
        return res;
    }

    public Propietario GetPropietario(int id)
    {
        Propietario? res = null;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,Dni,Nombre,Apellido,Email,Telefono 
            FROM propietarios
            WHERE Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),//Me da el id
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),//Es equivalente a reader.GetString(nameof("Dni"))
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono))
                        };
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

    public int Modificar(Propietario p)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE propietarios SET 
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
            string query = @"DELETE FROM propietarios WHERE Id = @Id;";
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
