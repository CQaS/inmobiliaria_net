using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Models
{
    public class RepositorioInquilino
    {
        private readonly string connectionString;

        public RepositorioInquilino()
        {
            connectionString = "Server=localhost;Database=inmobiliaria_net;Uid=root;Pwd=";
        }
        
        public List<Inquilino> obtener()
        {
            var res = new List<Inquilino>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Nombre)}, Mail, {nameof(Inquilino.Direccion)}  FROM inquilinos";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Dni = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            Mail = reader.GetString(3),
                            Direccion = reader.GetString(4),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public Inquilino Buscar(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Nombre)}, Mail, {nameof(Inquilino.Direccion)}  FROM inquilinos where id=@idIn";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@codigo", MySqlDbType.Int32);
                    command.Parameters["@idIn"].Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Inquilino Inq = new Inquilino();
                    if(res.Read())
                    {
                        Inq.Id = int.Parse(res["id"].ToString());
                        Inq.Dni = int.Parse(res["dni"].ToString());
                        Inq.Nombre = res["nombre"].ToString();
                        Inq.Mail = res["mail"].ToString();
                        Inq.Direccion = res["direccion"].ToString();
                    }
                    connection.Close();
                    return Inq;
                }
            }
        }


        public int Alta(Inquilino e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO inquilinos(Dni, Nombre, mail, direccion) VALUES (@dni,@nombre,@mail,@direccion)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.Int32);
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters.Add("@mail", MySqlDbType.VarChar);
                    command.Parameters.Add("@direccion", MySqlDbType.VarChar);
                    command.Parameters["@dni"].Value = e.Dni;
                    command.Parameters["@nombre"].Value = e.Nombre;
                    command.Parameters["@mail"].Value = e.Mail;
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
                
                string sql_ID = $"SELECT MAX(id) AS id FROM inquilinos";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteReader());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Inquilino e)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update inquilinos set Dni=@dni, Nombre=@nombre, Mail=@mail, Direccion=@direccion where Id=@id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.VarChar);
                    command.Parameters["@dni"].Value = e.Dni;
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters["@nombre"].Value = e.Nombre;
                    command.Parameters.Add("@mail", MySqlDbType.VarChar);
                    command.Parameters["@mail"].Value = e.Mail;
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
                string sql = $"delete from inquilinos where id=@idIn";

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
    

