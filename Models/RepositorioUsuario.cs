using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
 
namespace AplicacionPrueba.Models
{
    public class RepositorioUsuario : RepositorioBase
	{
		public RepositorioUsuario(IConfiguration configuration) : base(configuration)
		{

		}

        public int Alta(Usuario e)
		{
			string avatarDefault = "/img/default.jpg";
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Usuarios (Nombre, Apellido, Avatar, Mail, Clave, Pregunta, Rol) VALUES (@nombre, @apellido, @avatar, @mail, @clave, @pregunta, @rol)";

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
					command.Parameters.AddWithValue("@pregunta", e.Pregunta);
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
				string sql = $"UPDATE Usuarios SET estado = 0 WHERE Id = @id";
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
				string sql = $"UPDATE Usuarios SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Rol=@rol WHERE Id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@rol", e.Rol);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modifica(Usuario e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"CALL editarUsuario(@id, @nombre, @apellido, @rol, @clave)";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@rol", e.Rol);
					command.Parameters.AddWithValue("@clave", e.Clave);
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
				string sql = $"UPDATE Usuarios SET Clave=@clave WHERE Id = @id";
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
			var res = new List<Usuario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Avatar, Mail, Clave, Rol FROM Usuarios WHERE estado = 1";
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
                            Avatar = reader.GetString(3),
							Mail = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
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
				string sql = $"SELECT Id, Nombre, Apellido, Avatar, Mail, Clave, Rol FROM Usuarios WHERE Id=@id AND estado = 1";
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
                            Avatar = reader.GetString(3),
							Mail = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
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
							Avatar = reader.GetString(3),
							Mail = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}   
    }
}
    

