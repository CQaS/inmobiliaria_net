using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Models
{
    public class RepositorioPropietario
    {
        private readonly string connectionString;

        public RepositorioPropietario()
        {
            connectionString = "Server=localhost;Database=inmobiliaria_net;Uid=root;Pwd=";
        }
        
        public List<Propietario> obtener()
        {
            var res = new List<Propietario>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Direccion)}  FROM propietarios";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Direccion = reader.GetString(3),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public Propietario Buscar(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Direccion)}  FROM propietarios where id=@idIn";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@codigo", MySqlDbType.Int32);
                    command.Parameters["@idIn"].Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Propietario Inq = new Propietario();
                    if(res.Read())
                    {
                        Inq.Id = int.Parse(res["id"].ToString());
                        Inq.Dni = int.Parse(res["dni"].ToString());
                        Inq.Nombre = res["nombre"].ToString();
                        Inq.Direccion = res["direccion"].ToString();
                    }
                    connection.Close();
                    return Inq;
                }
            }
        }


        public int Alta(Propietario e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO propietarios(Dni, Nombre, direccion) VALUES (@dni,@nombre,@mail,@direccion)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.Int32);
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters.Add("@direccion", MySqlDbType.VarChar);
                    command.Parameters["@dni"].Value = e.Dni;
                    command.Parameters["@nombre"].Value = e.Nombre;
                    command.Parameters["@direccion"].Value = e.Direccion;
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();

                    /*command.Parameters.AddWithValue("@nombre", e.Nombre);
                    command.Parameters.AddWithValue("@email", e.Mail);
                    connection.Open();
                    //res = Convert.ToInt32(command.ExecuteScalar());
                    command.ExecuteScalar();
                    //e.Id = res;
                    connection.Close();*/
                }
                
                string sql_ID = $"SELECT MAX(id) AS id FROM propietarios";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteReader());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Propietario e)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update propietarios set Dni=@dni, Nombre=@nombre, Direccion=@direccion where Id=@id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.VarChar);
                    command.Parameters["@dni"].Value = e.Dni;
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters["@nombre"].Value = e.Nombre;
                    command.Parameters.Add("@direccion", MySqlDbType.VarChar);
                    command.Parameters["@direccion"].Value = e.Direccion;
                    command.Parameters.Add("@id", MySqlDbType.VarChar);
                    command.Parameters["@id"].Value = e.Id;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }

        public int Borrar(int idIn)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"delete from propietarios where id=@idIn";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idIn", MySqlDbType.UInt32);
                    command.Parameters["@idIn"].Value = idIn;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }
    }
}
    

