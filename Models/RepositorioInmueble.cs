using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
 
namespace AplicacionPrueba.Models
{
    public class RepositorioInmueble
    {
        private readonly string connectionString;

        public RepositorioInmueble()
        {
            connectionString = "Server=localhost;Database=inmobiliaria_net;Uid=root;Pwd=";
        }
        
        public List<Inmueble> obtener()
        {
            var res = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inmueble.Id_inmu)}, {nameof(Inmueble.Direccion_in)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.ambientes)}, {nameof(Inmueble.precio)}, {nameof(Inmueble.id_propietario)}, p.Nombre FROM inmueble i INNER JOIN Propietarios p ON i.id_Propietario = p.id WHERE i.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inmueble
                        {
                            Id_inmu = reader.GetInt32(0),
                            Direccion_in = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            ambientes = reader.GetInt32(4),
                            precio = reader.GetInt32(5),
                            id_propietario = reader.GetInt32(6),
                            Duenio = new Propietario
                            {
                                
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            }
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public List<Inmueble> obtenerPorPropietario(int id)
        {
            var res = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inmueble.Id_inmu)}, {nameof(Inmueble.Direccion_in)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.ambientes)}, {nameof(Inmueble.precio)}, {nameof(Inmueble.foto)} FROM inmueble WHERE id_propietario = @id AND estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inmueble
                        {
                            Id_inmu = reader.GetInt32(0),
                            Direccion_in = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            ambientes = reader.GetInt32(4),
                            precio = reader.GetInt32(5),
                            foto = reader.GetString(6),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public Inmueble Buscar(int id)
        {
            Inmueble Inm;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inmueble.Id_inmu)}, {nameof(Inmueble.Direccion_in)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.ambientes)}, {nameof(Inmueble.precio)}, {nameof(Inmueble.id_propietario)}, {nameof(Inmueble.foto)} FROM inmueble where id_inmu=@idIn AND estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idIn", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Inm = new Inmueble();
                    if(res.Read())
                    {
                        Inm.Id_inmu = int.Parse(res["id_inmu"].ToString());
                        Inm.Direccion_in = res["Direccion_in"].ToString();
                        Inm.Uso = res["Uso"].ToString();
                        Inm.Tipo = res["tipo"].ToString();
                        Inm.ambientes = int.Parse(res["ambientes"].ToString());;
                        Inm.precio = int.Parse(res["precio"].ToString());
                        Inm.id_propietario = int.Parse(res["id_propietario"].ToString());
                        Inm.foto = res["foto"].ToString();
                    }
                    connection.Close();                                       
                }
            }
            return Inm;
        }


        public int Alta(Inmueble e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO inmueble (direccion_in, uso, tipo, ambientes, precio, id_propietario, foto) VALUES (@direccion_in, @uso, @tipo, @ambientes, @precio, @id_propietario, @foto)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
		            command.Parameters.AddWithValue("@direccion_in", e.Direccion_in);
                    command.Parameters.AddWithValue("@uso", e.Uso);
                    command.Parameters.AddWithValue("@tipo", e.Tipo);
                    command.Parameters.AddWithValue("@ambientes", e.ambientes);
                    command.Parameters.AddWithValue("@precio", e.precio);
                    command.Parameters.AddWithValue("@id_propietario", e.id_propietario);
                    command.Parameters.AddWithValue("@foto", e.foto);
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }

                string sql_ID = $"SELECT MAX(id_inmu) AS id_in FROM inmueble";                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Inmueble e)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update inmueble set Direccion_in=@Direccion_in, Uso=@Uso, tipo=@tipo, ambientes=@ambientes, precio=@precio where Id_inmu=@id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@Direccion_in", MySqlDbType.VarChar);
                    command.Parameters["@Direccion_in"].Value = e.Direccion_in;
                    command.Parameters.Add("@Uso", MySqlDbType.VarChar);
                    command.Parameters["@Uso"].Value = e.Uso;
                    command.Parameters.Add("@Tipo", MySqlDbType.VarChar);
                    command.Parameters["@Tipo"].Value = e.Tipo;
                    command.Parameters.Add("@ambientes", MySqlDbType.VarChar);
                    command.Parameters["@ambientes"].Value = e.ambientes;
                    command.Parameters.Add("@precio", MySqlDbType.VarChar);
                    command.Parameters["@precio"].Value = e.precio;
                    command.Parameters.Add("@id", MySqlDbType.VarChar);
                    command.Parameters["@id"].Value = e.Id_inmu;
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
                string sql = $"update inmueble set estado='0' where Id_inmu=@id_Inm";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Inm", MySqlDbType.UInt32);
                    command.Parameters["@id_Inm"].Value = idIn;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }
    }
}
    

