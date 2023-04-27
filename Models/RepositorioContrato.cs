using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioContrato
{
    string connectionString = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=roque;";

    public RepositorioContrato() { }

    public int Alta(Contrato c)
    {
        int res = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string query = @"INSERT INTO 
            contratos (Desde, Hasta, Condiciones, Monto, inmueble_Id, inquilino_Id)
            VALUES (@Desde, @Hasta, @Condiciones, @Monto, @InmuebleId, @InquilinoId);
            SELECT LAST_INSERT_ID();
            UPDATE inmuebles SET Disponible = 0 WHERE id = @InmuebleId;
            ";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Desde", c.Desde);
                command.Parameters.AddWithValue("@Hasta", c.Hasta);
                command.Parameters.AddWithValue("@Condiciones", c.Condiciones);
                command.Parameters.AddWithValue("@Monto", c.Monto);
                command.Parameters.AddWithValue("@InmuebleId", c.Inmueble.Id);
                command.Parameters.AddWithValue("@InquilinoId", c.Inquilino.Id);
                connection.Open();

                res = Convert.ToInt32(command.ExecuteScalar());
                c.Id = res;

                connection.Close();
            }
        }
        return res;
    }

    public List<Contrato> GetContratos()
    {
        var list = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string query = @"
            SELECT c.Id,Desde,Hasta,Condiciones,Monto, inm.Id as InmuebleId ,inm.Direccion, tipos.Tipo, inq.Id as InquilinoId ,inq.Nombre, inq.Apellido
            FROM inmobiliaria.contratos c
            INNER JOIN inmuebles inm ON c.inmueble_Id = inm.Id
            INNER JOIN inquilinos inq ON c.inquilino_Id = inq.Id
            INNER JOIN tipos_inmueble tipos ON inm.tipo_inmueble_Id = tipos.Id";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(nameof(contrato.Id)),
                            Desde = reader.GetDateTime(nameof(contrato.Desde)),
                            Hasta = reader.GetDateTime(nameof(contrato.Hasta)),
                            Condiciones = reader.GetString(nameof(contrato.Condiciones)),
                            Monto = reader.GetDecimal(nameof(contrato.Monto)),
                            InmuebleId = reader.GetInt32(nameof(contrato.InmuebleId)),
                            InquilinoId = reader.GetInt32(nameof(contrato.InquilinoId)),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                Tipo = new TipoInmueble
                                {
                                    Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),
                                }
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            }
                        };

                        list.Add(contrato);
                    }
                }
                connection.Close();
            }

        }
        return list;
    }

    public Contrato GetContrato(int id)
    {
        Contrato? res = null;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
            SELECT c.Id,Desde,Hasta,Condiciones,Monto, inm.Id as InmuebleId ,inm.Direccion, inm.Uso,inm.Cantidad_ambientes, inm.Precio, tipos.Tipo, inq.Id as InquilinoId ,inq.Nombre, inq.Apellido, inq.Email, inq.Telefono
            FROM inmobiliaria.contratos c
            INNER JOIN inmuebles inm ON c.inmueble_Id = inm.Id
            INNER JOIN inquilinos inq ON c.inquilino_Id = inq.Id
            INNER JOIN tipos_inmueble tipos ON inm.tipo_inmueble_Id = tipos.Id
            WHERE c.Id = @Id";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            Desde = reader.GetDateTime(nameof(Contrato.Desde)),
                            Hasta = reader.GetDateTime(nameof(Contrato.Hasta)),
                            Condiciones = reader.GetString(nameof(Contrato.Condiciones)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                Tipo = new TipoInmueble
                                {
                                    Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),
                                },
                                Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                                Cantidad_ambientes = reader.GetInt32(nameof(Inmueble.Cantidad_ambientes)),
                                Precio = reader.GetDecimal(nameof(Inmueble.Precio))
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                Email = reader.GetString(nameof(Inquilino.Email)),
                                Telefono = reader.GetString(nameof(Inquilino.Telefono))
                            }
                        };
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

    public int Modificar(Contrato c)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE contratos SET 
            Desde=@Desde,
            Hasta=@Hasta,
            Condiciones=@Condiciones,
            Monto=@Monto,
            inmueble_Id=@InmuebleId, 
            inquilino_Id=@InquilinoId
            WHERE Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Desde", c.Desde);
                command.Parameters.AddWithValue("@Hasta", c.Hasta);
                command.Parameters.AddWithValue("@Condiciones", c.Condiciones);
                command.Parameters.AddWithValue("@Monto", c.Monto);
                command.Parameters.AddWithValue("@InmuebleId", c.InmuebleId);
                command.Parameters.AddWithValue("@InquilinoId", c.InquilinoId);

                command.Parameters.AddWithValue("@Id", c.Id);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    public int CancelarContrato(int id){
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"UPDATE contratos SET Hasta=@Hasta WHERE Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Hasta", DateTime.Now.AddDays(-1));
                command.Parameters.AddWithValue("@Id", id);
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
            string query = @"DELETE FROM contratos where Id = @Id;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    public Contrato obtenerIdPorInmueble(int idInmueble)
    {
        Contrato? res = null;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
            SELECT * FROM inmobiliaria.contratos c
            WHERE c.inmueble_Id = @Id AND c.Desde <= CURDATE() AND c.Hasta >= CURDATE()";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", idInmueble);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id))
                        };
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

    public List<Contrato> GetContratosInmueble(int idInmueble)
    {
        var list = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string query = @"
            SELECT c.Id,Desde,Hasta,Condiciones,Monto, inm.Id as InmuebleId ,inm.Direccion, tipos.Tipo, inq.Id as InquilinoId ,inq.Nombre, inq.Apellido
            FROM inmobiliaria.contratos c
            INNER JOIN inmuebles inm ON c.inmueble_Id = inm.Id
            INNER JOIN inquilinos inq ON c.inquilino_Id = inq.Id
            INNER JOIN tipos_inmueble tipos ON inm.tipo_inmueble_Id = tipos.Id
            WHERE inm.Id = @InmuebleId";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InmuebleId", idInmueble);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(nameof(contrato.Id)),
                            Desde = reader.GetDateTime(nameof(contrato.Desde)),
                            Hasta = reader.GetDateTime(nameof(contrato.Hasta)),
                            Condiciones = reader.GetString(nameof(contrato.Condiciones)),
                            Monto = reader.GetDecimal(nameof(contrato.Monto)),
                            InmuebleId = reader.GetInt32(nameof(contrato.InmuebleId)),
                            InquilinoId = reader.GetInt32(nameof(contrato.InquilinoId)),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                Tipo = new TipoInmueble
                                {
                                    Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),
                                }
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            }
                        };

                        list.Add(contrato);
                    }
                }
                connection.Close();
            }

        }
        return list;
    }

    public List<Contrato> GetContratosVigentes(){
        var list = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString))
        {

            /* utilize una subconsulta para obtener el Id del contrato con la fecha de finalización de contrato más cercana 
            a la fecha actual, para cada inmueble_Id. Luego, en la cláusula principal de la consulta, filtramos los resultados 
            utilizando la condición c.Id = subconsulta para obtener únicamente los registros que coinciden con los Id obtenidos 
            en la subconsulta. Finalmente, agrupamos los resultados por inmueble_Id utilizando la cláusula GROUP BY y ordenamos 
            los resultados por fecha de finalización de contrato. */

            string query = @"
            SELECT c.Id, Desde, Hasta, Condiciones, Monto, inm.Id as InmuebleId, inm.Direccion, tipos.Tipo, inq.Id as InquilinoId, inq.Nombre, inq.Apellido
            FROM contratos c
            INNER JOIN inmuebles inm ON c.inmueble_Id = inm.Id
            INNER JOIN inquilinos inq ON c.inquilino_Id = inq.Id
            INNER JOIN tipos_inmueble tipos ON inm.tipo_inmueble_Id = tipos.Id
            WHERE Hasta >= CURDATE() 
            AND c.Id = (SELECT Id FROM contratos c2 WHERE c2.inmueble_Id = c.inmueble_Id AND c2.Hasta >= CURDATE() ORDER BY c2.Hasta ASC LIMIT 1)
            GROUP BY inm.Id
            ORDER BY Hasta ASC";

            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(nameof(contrato.Id)),
                            Desde = reader.GetDateTime(nameof(contrato.Desde)),
                            Hasta = reader.GetDateTime(nameof(contrato.Hasta)),
                            Condiciones = reader.GetString(nameof(contrato.Condiciones)),
                            Monto = reader.GetDecimal(nameof(contrato.Monto)),
                            InmuebleId = reader.GetInt32(nameof(contrato.InmuebleId)),
                            InquilinoId = reader.GetInt32(nameof(contrato.InquilinoId)),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                Tipo = new TipoInmueble
                                {
                                    Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),
                                }
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            }
                        };

                        list.Add(contrato);
                    }
                }
                connection.Close();
            }

        }
        return list;
    }

    public Boolean GetContratoVigentePorInmueble(int idInmueble){

        var Vigente = false;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT * FROM inmobiliaria.contratos c
                WHERE c.inmueble_Id = @InmuebleId AND c.Desde <= CURDATE() AND c.Hasta >= CURDATE()";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@InmuebleId", idInmueble);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Vigente = true;
                    }
                }
                conn.Close();
            }
        }
        return Vigente;
    }

    public Boolean Vigente(int id){

        var Vigente = false;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"SELECT * FROM inmobiliaria.contratos c
            WHERE c.Id = @ContratoId AND Hasta >= CURDATE() 
            AND c.Id = (SELECT Id FROM contratos c2 WHERE c2.inmueble_Id = c.inmueble_Id AND c2.Hasta >= CURDATE() ORDER BY c2.Hasta ASC LIMIT 1);";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@ContratoId", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Vigente = true;
                    }
                }
                conn.Close();
            }
        }
        return Vigente;
    }

}