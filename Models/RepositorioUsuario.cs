using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
 
namespace AplicacionPrueba.Models
{
    public class RepositorioUsuario
    {
        private readonly string connectionString;

        public RepositorioUsuario()
        {
            connectionString = "Server=localhost;Database=inmobiliaria_net;Uid=root;Pwd=";
        }

        public int Alta(Usuario e)
		{
			string avatarDefault = "/img/default.jpg";
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Usuarios (Nombre, Apellido, Avatar, Mail, Clave, Rol) VALUES (@nombre, @apellido, @avatar, @mail, @clave, @rol)";

				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					if(String.IsNullOrEmpty(e.Avatar))
							command.Parameters.AddWithValue("@avatar", avatarDefault);
					else
                    	command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@mail", e.Mail);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.Id = res;
					connection.Close();
				}

                string sql_ID = $"SELECT MAX(id) AS id FROM Usuarios";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
			}
			return res;
		}

		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Usuarios WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Usuario e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET Nombre=@nombre, Apellido=@apellido, Rol=@rol " +
					$"WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@rol", e.Rol);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int ModificarPass(Usuario e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET Clave=@clave " +
					$"WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
			
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Usuario> ObtenerTodos()
		{
			IList<Usuario> res = new List<Usuario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Avatar, Mail, Clave, Rol FROM Usuarios";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuario e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Mail = reader.GetString(3),
							Clave = reader.GetString(4),
							Rol = reader.GetInt32(5),
                            Avatar = reader["Avatar"].ToString(),
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Usuario ObtenerPorId(int id)
		{
			Usuario e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Avatar, Mail, Clave, Rol FROM Usuarios WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Mail = reader.GetString(3),
							Clave = reader.GetString(4),
							Rol = reader.GetInt32(5),
                            Avatar = reader["Avatar"].ToString(),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		public Usuario ObtenerPorEmail(string mail)
		{
			Usuario e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Avatar, Mail, Clave, Rol FROM Usuarios WHERE Mail=@mail";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = System.Data.CommandType.Text;
					command.Parameters.Add("@mail", MySqlDbType.VarChar).Value = mail;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Mail = reader.GetString(3),
							Clave = reader.GetString(4),
							Rol = reader.GetInt32(5),
							Avatar = reader["Avatar"].ToString(),
						};
					}
					connection.Close();
				}
			}
			return e;
		}   
    }
}
    

