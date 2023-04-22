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
            SELECT c.Id,Desde,Hasta,Condiciones,Monto, inm.Id as InmuebleId ,inm.Direccion, tipos.Tipo, inq.Id as InquilinoId ,inq.Nombre, inq.Apellido
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
                                }
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
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

    public int Eliminar(int id){
        int res = 0;

        using (var conn = new MySqlConnection(connectionString)){
            string query = "DELETE FROM contratos where Id = @Id";

            using(var command = new MySqlCommand(query, conn)){
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
            SELECT c.Id
            FROM inmobiliaria.contratos c
            INNER JOIN inmuebles inm ON c.inmueble_Id = inm.Id
            WHERE inm.Id = @Id";

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
}