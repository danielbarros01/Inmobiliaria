using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioTipoInmueble
{

    string connectionString = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=roque;";

    public RepositorioTipoInmueble()
    { }


    public List<TipoInmueble> GetTipos()
    {
        var list = new List<TipoInmueble>();
        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,Tipo FROM tipos_inmueble";

            using (var command = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TipoInmueble tipoInmb = new TipoInmueble
                        {
                            Id = reader.GetInt32(nameof(TipoInmueble.Id)),//Me da el id
                            Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),//Es equivalente a reader.GetString(nameof("Tipo"))
                        };

                        list.Add(tipoInmb);
                    }
                }
                conn.Close();
            }
        }
        return list;
    }

    public TipoInmueble GetTipo(int id){
        TipoInmueble? res = null;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT Id,Tipo 
            FROM tipos_inmueble
            WHERE Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new TipoInmueble
                        {
                            Id = reader.GetInt32(nameof(TipoInmueble.Id)),//Me da el id
                            Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),//Es equivalente a reader.GetString(nameof("Tipo"))
                        };
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

    public int Alta(string tipoInmb)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO tipos_inmueble(Tipo)
                                VALUES(@tipo);
                                SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@tipo", tipoInmb);
                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar()); // devuelve el valor de la última fila insertada en la tabla. Este valor se convierte a un entero y se asigna a la variable res.
                conn.Close();
            }
        }
        return res;
    }

    public int Modificar(TipoInmueble tipoInmueble){
        int res = 0;

        using (var conn = new MySqlConnection(connectionString)){
            string query = @"UPDATE tipos_inmueble SET Tipo=@tipo WHERE Id = @id";

            using (var command = new MySqlCommand(query, conn)){
                command.Parameters.AddWithValue("@tipo", tipoInmueble.Tipo);
                command.Parameters.AddWithValue("@id", tipoInmueble.Id);
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
            string query = @"DELETE FROM tipos_inmueble WHERE Id = @Id;";
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