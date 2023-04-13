using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models
{
    public class RepositorioUsuario
    {
        string connectionString = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=roque;";

        public RepositorioUsuario() { }

        public List<Usuario> GetUsuarios()
        {
            var list = new List<Usuario>();
            using (var conn = new MySqlConnection(connectionString))
            {
                var query = @"SELECT Id,Rol,Email,Nombre,Apellido,Clave,AvatarRuta FROM usuarios";

                using (var command = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario user = new Usuario
                            {
                                Id = reader.GetInt32(nameof(Usuario.Id)),
                                Nombre = reader.GetString(nameof(Usuario.Nombre)),
                                Apellido = reader.GetString(nameof(Usuario.Apellido)),
                                Email = reader.GetString(nameof(Usuario.Email)),
                                Clave = reader.GetString(nameof(Usuario.Clave)),
                                Rol = reader.GetInt32(nameof(Usuario.Rol))
                            };


                            if (!reader.IsDBNull(reader.GetOrdinal(nameof(Usuario.AvatarRuta))))
                            {
                                user.AvatarRuta = reader.GetString(nameof(Usuario.AvatarRuta));
                            }
                            else
                            {
                                user.AvatarRuta = "/uploads/imagenPorDefecto.png";
                            }

                            list.Add(user);
                        }
                    }
                    conn.Close();
                }
            }
            return list;
        }

        public int Alta(Usuario u)
        {
            int res = -1;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Usuarios 
					(Nombre, Apellido, AvatarRuta, Email, Clave, Rol) 
					VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);
					SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@nombre", u.Nombre);
                    command.Parameters.AddWithValue("@apellido", u.Apellido);
                    if (String.IsNullOrEmpty(u.AvatarRuta))
                    {
                        command.Parameters.AddWithValue("@avatar", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@avatar", u.AvatarRuta);
                    }

                    command.Parameters.AddWithValue("@email", u.Email);
                    command.Parameters.AddWithValue("@clave", u.Clave);
                    command.Parameters.AddWithValue("@rol", u.Rol);
                    conn.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    u.Id = res;
                    conn.Close();
                }
            }
            return res;
        }

        public int Modificar(Usuario u)
        {
            int res = -1;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Usuarios 
					SET Nombre=@nombre, Apellido=@apellido, AvatarRuta=@avatar, Email=@email, Clave=@clave, Rol=@rol
					WHERE Id = @id";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@nombre", u.Nombre);
                    command.Parameters.AddWithValue("@apellido", u.Apellido);
                    command.Parameters.AddWithValue("@avatar", u.AvatarRuta);
                    command.Parameters.AddWithValue("@email", u.Email);
                    command.Parameters.AddWithValue("@clave", u.Clave);
                    command.Parameters.AddWithValue("@rol", u.Rol);
                    command.Parameters.AddWithValue("@id", u.Id);

                    conn.Open();
                    res = command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }

        public int ModificarPassword(Usuario u)
        {
            int res = -1;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Usuarios 
					SET Clave=@clave
					WHERE Id = @id";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@clave", u.Clave);
                    command.Parameters.AddWithValue("@id", u.Id);

                    conn.Open();
                    res = command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }

        public Usuario ObtenerPorId(int id)
        {
            Usuario? u = null;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
					Id, Nombre, Apellido, AvatarRuta, Email, Clave, Rol 
					FROM Usuarios
					WHERE Id=@id";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            u = new Usuario
                            {
                                Id = reader.GetInt32(nameof(Usuario.Id)),
                                Nombre = reader.GetString(nameof(Usuario.Nombre)),
                                Apellido = reader.GetString(nameof(Usuario.Apellido)),
                                Email = reader.GetString(nameof(Usuario.Email)),
                                Clave = reader.GetString(nameof(Usuario.Clave)),
                                Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal(nameof(Usuario.AvatarRuta))))
                            {
                                u.AvatarRuta = reader.GetString(nameof(Usuario.AvatarRuta));
                            }
                            else
                            {
                                u.AvatarRuta = "/uploads/imagenPorDefecto.png";
                            }
                        }
                    }

                    conn.Close();
                }
            }
            return u;
        }

        public int Eliminar(int id)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = "DELETE FROM Usuarios WHERE Id = @id";
				using (var command = new MySqlCommand(sql, conn))
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
}