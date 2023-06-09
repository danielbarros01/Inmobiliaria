using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioInmueble
{
    string connectionString = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=roque;";

    public RepositorioInmueble() { }

    public int Alta(Inmueble i)
    {
        int res = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO 
            inmuebles (Direccion, Uso, Cantidad_ambientes, Coordenadas, Precio, Disponible, propietario_Id, tipo_inmueble_Id)
            VALUES (@Direccion, @Uso, @Cantidad_ambientes, @Coordenadas, @Precio, @Disponible,@Propietario,@Tipo);
            SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Direccion", i.Direccion);
                command.Parameters.AddWithValue("@Uso", i.Uso);
                command.Parameters.AddWithValue("@Cantidad_ambientes", i.Cantidad_ambientes);
                command.Parameters.AddWithValue("@Coordenadas", i.Coordenadas);
                command.Parameters.AddWithValue("@Precio", i.Precio);
                command.Parameters.AddWithValue("@Disponible", i.Disponible);
                command.Parameters.AddWithValue("@Propietario", i.Propietario.Id);
                command.Parameters.AddWithValue("@Tipo", i.Tipo.Id);
                connection.Open();

                res = Convert.ToInt32(command.ExecuteScalar());
                i.Id = res;

                connection.Close();
            }
        }
        return res;
    }

    public List<Inmueble> GetInmuebles()
    {
        var list = new List<Inmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string query = @"
            SELECT i.Id, i.Direccion, i.Uso, i.Cantidad_ambientes, i.Coordenadas, i.Precio, i.Disponible, i.ImagenRuta,i.propietario_Id, p.nombre, p.Apellido, t.Tipo
            FROM inmuebles i
            INNER JOIN propietarios p ON i.propietario_id = p.Id
            INNER JOIN tipos_inmueble t ON i.tipo_inmueble_Id = t.Id;";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Cantidad_ambientes = reader.GetInt32(nameof(Inmueble.Cantidad_ambientes)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            ImagenRuta = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.ImagenRuta))) ? null : reader.GetString(nameof(Inmueble.ImagenRuta)),
                            PropietarioId = reader.GetInt32("propietario_Id"),
                            Propietario = new Propietario
                            {
                                Nombre = reader.GetString(nameof(Inmueble.Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Inmueble.Propietario.Apellido))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Tipo.Id)),/* No devuelve el verdadero Id */
                                Tipo = reader.GetString(nameof(Inmueble.Tipo.Tipo)),
                            }
                        };

                        list.Add(inmueble);
                    }
                }
                connection.Close();
            }

        }
        return list;
    }

    public Inmueble GetInmueble(int id)
    {
        Inmueble? res = null;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
            SELECT i.Id, i.Direccion, i.Uso, i.Cantidad_ambientes, i.Coordenadas, i.Precio, i.Disponible, i.ImagenRuta,i.propietario_Id, p.nombre, p.Apellido,p.Telefono, p.Email, t.Tipo
            FROM inmuebles i
            INNER JOIN propietarios p ON i.propietario_id = p.Id
            INNER JOIN tipos_inmueble t ON i.tipo_inmueble_Id = t.Id
            WHERE i.Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Cantidad_ambientes = reader.GetInt32(nameof(Inmueble.Cantidad_ambientes)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            ImagenRuta = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.ImagenRuta))) ? null : reader.GetString(nameof(Inmueble.ImagenRuta)),
                            PropietarioId = reader.GetInt32("propietario_Id"),
                            Propietario = new Propietario
                            {
                                Nombre = reader.GetString(nameof(Inmueble.Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Inmueble.Propietario.Apellido)),
                                Telefono = reader.GetString(nameof(Inmueble.Propietario.Telefono)),
                                Email = reader.GetString(nameof(Inmueble.Propietario.Email))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Tipo.Id)),/* No devuelve el verdadero Id */
                                Tipo = reader.GetString(nameof(Inmueble.Tipo.Tipo)),
                            }
                        }; ;
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

    public int Modificar(Inmueble i)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE inmuebles SET 
            Direccion=@direccion,
            Uso=@uso,
            Cantidad_ambientes=@cantAmbientes,
            Coordenadas=@coordenadas,
            Precio=@precio, 
            Disponible=@disponible, 
            ImagenRuta=@imagen,
            propietario_Id=@propietarioId, 
            tipo_inmueble_Id=@tipoInmuebleId 
            WHERE Id = @Id;";

            if (i.ImagenRuta == null)
            {
                string updatedSqlQuery = query.Replace("ImagenRuta=@imagen,", string.Empty);
            }

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@direccion", i.Direccion);
                command.Parameters.AddWithValue("@uso", i.Uso);
                command.Parameters.AddWithValue("@cantAmbientes", i.Cantidad_ambientes);
                command.Parameters.AddWithValue("@coordenadas", i.Coordenadas);
                command.Parameters.AddWithValue("@precio", i.Precio);
                command.Parameters.AddWithValue("@disponible", i.Disponible);
                command.Parameters.AddWithValue("@imagen", i.ImagenRuta);
                command.Parameters.AddWithValue("@propietarioId", i.Propietario.Id);
                command.Parameters.AddWithValue("@tipoInmuebleId", i.Tipo.Id);

                command.Parameters.AddWithValue("@id", i.Id);
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
            string query = @"DELETE FROM inmuebles WHERE Id = @Id;";
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

    public List<Inmueble> GetDisponibles()
    {
        var list = new List<Inmueble>();

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
            SELECT i.Id, i.Direccion, i.Uso, i.Cantidad_ambientes, i.Coordenadas, i.Precio, i.Disponible, i.ImagenRuta, i.propietario_Id, p.nombre, p.Apellido, p.Telefono, t.Tipo
            FROM inmuebles i
            INNER JOIN propietarios p ON i.propietario_id = p.Id
            INNER JOIN tipos_inmueble t ON i.tipo_inmueble_Id = t.Id
            WHERE Disponible = 1;";

            using (var command = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Cantidad_ambientes = reader.GetInt32(nameof(Inmueble.Cantidad_ambientes)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            ImagenRuta = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.ImagenRuta))) ? null : reader.GetString(nameof(Inmueble.ImagenRuta)),
                            PropietarioId = reader.GetInt32("propietario_Id"),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("propietario_Id"),
                                Nombre = reader.GetString(nameof(Inmueble.Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Inmueble.Propietario.Apellido)),
                                Telefono = reader.GetString(nameof(Inmueble.Propietario.Telefono))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Tipo.Id)),/* No devuelve el verdadero Id */
                                Tipo = reader.GetString(nameof(Inmueble.Tipo.Tipo)),
                            }
                        };

                        list.Add(inmueble);
                    }
                }
                conn.Close();
            }
        }
        return list;
    }

    public List<Inmueble> GetInmueblesPropietario(int idPropietario)
    {
        var list = new List<Inmueble>();

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
            SELECT i.Id, i.Direccion, i.Uso, i.Cantidad_ambientes, i.Coordenadas, i.Precio, i.Disponible, i.ImagenRuta,i.propietario_Id, p.nombre, p.Apellido, p.Telefono, t.Tipo
            FROM inmuebles i
            INNER JOIN propietarios p ON i.propietario_id = p.Id
            INNER JOIN tipos_inmueble t ON i.tipo_inmueble_Id = t.Id
            WHERE p.Id = @IdPropietario;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@IdPropietario", idPropietario);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Cantidad_ambientes = reader.GetInt32(nameof(Inmueble.Cantidad_ambientes)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            ImagenRuta = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.ImagenRuta))) ? null : reader.GetString(nameof(Inmueble.ImagenRuta)),
                            PropietarioId = reader.GetInt32("propietario_Id"),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("propietario_Id"),
                                Nombre = reader.GetString(nameof(Inmueble.Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Inmueble.Propietario.Apellido)),
                                Telefono = reader.GetString(nameof(Inmueble.Propietario.Telefono))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Tipo.Id)),/* No devuelve el verdadero Id */
                                Tipo = reader.GetString(nameof(Inmueble.Tipo.Tipo)),
                            }
                        };

                        list.Add(inmueble);
                    }
                }
                conn.Close();
            }
        }
        return list;
    }

    public List<Inmueble> GetInmueblesPorFechas(DateTime fechaInicio, DateTime fechaFinal)
    {
        var list = new List<Inmueble>();

        using (var conn = new MySqlConnection(connectionString))
        {

            /* se utiliza una subconsulta que busca los contratos que se superponen con las fechas del nuevo contrato. 
            En esta subconsulta, se busca cualquier contrato que se solape con las fechas especificadas 
            La subconsulta devuelve una lista de los ID de los inmuebles que están ocupados durante el período especificado. 
            Luego, se utiliza la cláusula "NOT IN" para seleccionar todos los inmuebles que no estén en esa lista.*/

            var query = @"
            SELECT i.Id, i.Direccion, i.Uso, i.Cantidad_ambientes, i.Coordenadas, i.Precio, i.Disponible, i.ImagenRuta,i.propietario_Id, p.nombre, p.Apellido, p.Telefono, t.Tipo
            FROM inmuebles i
            INNER JOIN propietarios p ON i.propietario_id = p.Id
            INNER JOIN tipos_inmueble t ON i.tipo_inmueble_Id = t.Id
            WHERE i.Disponible = 1 
            OR i.Id NOT IN (
                SELECT c.inmueble_Id
                FROM contratos c
                WHERE (c.Desde <= @parametroHasta AND c.Hasta >= @parametroDesde) 
                OR (c.Desde >= @parametroDesde AND c.Hasta <= @parametroHasta) 
                OR (c.Desde <= @parametroDesde AND c.Hasta >= @parametroDesde)
            )";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@parametroDesde", fechaInicio);
                command.Parameters.AddWithValue("@parametroHasta", fechaFinal);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Cantidad_ambientes = reader.GetInt32(nameof(Inmueble.Cantidad_ambientes)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            ImagenRuta = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.ImagenRuta))) ? null : reader.GetString(nameof(Inmueble.ImagenRuta)),
                            PropietarioId = reader.GetInt32("propietario_Id"),
                            Propietario = new Propietario
                            {
                                Nombre = reader.GetString(nameof(Inmueble.Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Inmueble.Propietario.Apellido))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Tipo.Id)),/* No devuelve el verdadero Id */
                                Tipo = reader.GetString(nameof(Inmueble.Tipo.Tipo)),
                            }
                        };

                        list.Add(inmueble);
                    }
                }
                conn.Close();
            }
        }
        return list;
    }
}