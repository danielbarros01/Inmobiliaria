using System.Text.Json;
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioPago
{
    string connectionString = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=roque;";

    public RepositorioPago() { }

    public List<Pago> GetPagos()
    {
        var list = new List<Pago>();
        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
            SELECT NumeroPago,Fecha,contrato_Id, inq.Nombre, inq.Apellido, inmb.Direccion, tiposInmb.Tipo
            FROM pagos
            INNER JOIN contratos c  ON contrato_Id = c.Id
            INNER JOIN inquilinos inq ON c.inquilino_Id = inq.Id
            INNER JOIN inmuebles inmb ON c.inmueble_Id = inmb.Id
            INNER JOIN tipos_inmueble tiposInmb ON inmb.tipo_inmueble_Id = tiposInmb.Id";

            using (var command = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pago pago = new Pago
                        {
                            NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            ContratoId = reader.GetInt32("contrato_Id"),
                            Contrato = new Contrato{
                                Id = reader.GetInt32("contrato_Id"),
                                Inquilino = new Inquilino{
                                    Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                    Apellido = reader.GetString(nameof(Inquilino.Apellido))
                                },
                                Inmueble = new Inmueble{
                                    Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                    Tipo = new TipoInmueble{
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    }
                                }
                            }
                        };

                        list.Add(pago);
                    }
                }
                conn.Close();
            }
        }
        return list;
    }

    public int Alta(Pago p)
    {
        int res = 0;

        using (var connection = new MySqlConnection(connectionString))
        {

            /* SELECT COALESCE se almacena en una tabla temporal t y luego se utiliza el valor de NumeroPago de esa tabla temporal en la cláusula VALUES. */
            string query = @"
            INSERT INTO pagos (contrato_Id, NumeroPago, Fecha)
            VALUES (@ContratoId,
            (SELECT NumeroPago FROM (SELECT COALESCE(MAX(NumeroPago), 0) + 1 AS NumeroPago FROM pagos WHERE contrato_Id = @ContratoId) t),
            @Fecha);
            ";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ContratoId", p.ContratoId);
                command.Parameters.AddWithValue("@Fecha", p.Fecha);
                connection.Open();

                res = Convert.ToInt32(command.ExecuteScalar());
                p.NumeroPago = res;

                connection.Close();
            }
        }
        return res;
    }

    public Pago GetPago(int idPago, int idContrato)
    {
        Pago? res = null;

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
            SELECT NumeroPago,Fecha,contrato_Id, inq.Nombre, inq.Apellido, inmb.Direccion, tiposInmb.Tipo,c.Monto
            FROM pagos
            INNER JOIN contratos c  ON contrato_Id = c.Id
            INNER JOIN inquilinos inq ON c.inquilino_Id = inq.Id
            INNER JOIN inmuebles inmb ON c.inmueble_Id = inmb.Id
            INNER JOIN tipos_inmueble tiposInmb ON inmb.tipo_inmueble_Id = tiposInmb.Id
            WHERE NumeroPago = @IdPago && contrato_Id = @IdContrato;
            ";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@IdPago", idPago);
                command.Parameters.AddWithValue("@IdContrato", idContrato);

                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Pago
                        {
                            NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            ContratoId = reader.GetInt32("contrato_Id"),
                            Contrato = new Contrato{
                                Id = reader.GetInt32("contrato_Id"),
                                Inquilino = new Inquilino{
                                    Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                    Apellido = reader.GetString(nameof(Inquilino.Apellido))
                                },
                                Inmueble = new Inmueble{
                                    Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                    Tipo = new TipoInmueble{
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    }
                                }
                            }
                        };
                    }
                }
                conn.Close();
            }
        }
        return res;
    }

    public int Modificar(Pago p){
        int res = 0;

        using(var conn = new MySqlConnection(connectionString)){
            string query = @"
                UPDATE pagos SET Fecha = @Fecha WHERE NumeroPago = @NumeroPago && contrato_Id = @ContratoId;
            ";

            using(var command = new MySqlCommand(query, conn)){
                command.Parameters.AddWithValue("@Fecha", p.Fecha);
                command.Parameters.AddWithValue("@NumeroPago", p.NumeroPago);
                command.Parameters.AddWithValue("@ContratoId", p.ContratoId);

                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
                
            }
        }

        return res;
    }

    public int Eliminar(int idPago, int idContrato)
    {
        int res = 0;
        using (var conn = new MySqlConnection(connectionString))
        {
            string query = @"DELETE FROM pagos WHERE NumeroPago = @IdPago && contrato_Id = @ContratoId;";
            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@IdPago", idPago);
                command.Parameters.AddWithValue("@ContratoId", idContrato);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    //!!!!!!!!!!!!!!
    public Object GetDatosExtra(int id)
    {
        var data = new
        {
            Monto = 0,
            SiguienteNumeroPago = 0
        };

        using (var conn = new MySqlConnection(connectionString))
        {
            var query = @"
                SELECT 
                (SELECT Monto FROM contratos WHERE Id = @ContratoId) AS MontoContrato, 
                (SELECT COALESCE(MAX(NumeroPago), 0) + 1 FROM pagos WHERE contrato_Id = @ContratoId) AS SiguienteNumeroPago;";

            using (var command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@ContratoId", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        data = new
                        {
                            Monto = reader.GetInt32("MontoContrato"),
                            SiguienteNumeroPago = reader.GetInt32("SiguienteNumeroPago")
                        };
                    }
                }
                conn.Close();
            }
        }
        return data;
    }

}