using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
 
namespace AplicacionPrueba.Models
{
    public class RepositorioContrato : RepositorioBase
    {

        public RepositorioContrato(IConfiguration configuration): base(configuration)
        {

        }
        
        public List<Contrato> obtener()
        {
            var res = new List<Contrato>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, c.fe_ini, c.fe_fin, c.id_inquilino, i.Nombre, c.id_inmueble, e.direccion_in, e.id_propietario FROM Contrato c INNER JOIN Inquilinos i ON i.id = c.id_inquilino INNER JOIN Inmueble e ON e.id_Inmu = c.id_inmueble WHERE fe_fin >= Curdate() AND c.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            fe_ini = reader.GetDateTime(1),
                            fe_fin = reader.GetDateTime(2),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(4),
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion_in = reader.GetString(6)
                            }
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public List<Contrato> VerContratosXInmueble(int id)
        {
            var res = new List<Contrato>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT c.id, c.fe_ini, c.fe_fin, c.id_inquilino, i.Nombre, c.id_inmueble, e.direccion_in, e.id_propietario FROM Contrato c INNER JOIN Inquilinos i ON i.id = c.id_inquilino INNER JOIN Inmueble e ON e.id_Inmu = c.id_inmueble WHERE c.id_inmueble = @id_inmueble";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_inmueble", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            fe_ini = reader.GetDateTime(1),
                            fe_fin = reader.GetDateTime(2),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(4),
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion_in = reader.GetString(6)
                            }
                        };
                        res.Add(e);
                    }
                    connection.Close();                                       
                }
            }
            return res;
        }

        public Contrato Buscar(int id)
        {
            Contrato con;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT c.id, c.fe_ini, c.fe_fin, c.monto, inq.Nombre, inm.direccion_in, c.id_inmueble, c.id_inquilino FROM Contrato c INNER JOIN Inmueble inm ON c.id_inmueble = inm.id_Inmu INNER JOIN Inquilinos inq ON c.id_inquilino = inq.id WHERE c.id = @id AND c.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    con = new Contrato();
                    if(res.Read())
                    {
                        con.Id = int.Parse(res["id"].ToString());
                        con.fe_ini = DateTime.Parse(res["fe_ini"].ToString());
                        con.fe_fin = DateTime.Parse(res["fe_fin"].ToString());
                        con.monto = int.Parse(res["monto"].ToString());
                        con.id_inmueble = int.Parse(res["id_inmueble"].ToString());
                        con.id_inquilino = int.Parse(res["id_inquilino"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return con;
        }


        public int Alta(Contrato e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO contrato (fe_ini, fe_fin, monto, id_inmueble, id_inquilino) VALUES (@fe_ini, @fe_fin, @monto, @id_inmueble, @id_inquilino)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
					command.Parameters.AddWithValue("@fe_ini", e.fe_ini);
                    command.Parameters.AddWithValue("@fe_fin", e.fe_fin);
                    command.Parameters.AddWithValue("@monto", e.monto);
                    command.Parameters.AddWithValue("@id_inmueble", e.id_inmueble);
                    command.Parameters.AddWithValue("@id_inquilino", e.id_inquilino);
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }                
                string sql_ID = $"SELECT MAX(id) AS idUltimo FROM contrato";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Contrato e)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update contrato set fe_ini=@fe_ini, fe_fin=@fe_fin, monto=@monto, id_inmueble=@id_inmueble, id_inquilino=@id_inquilino where id=@id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fe_ini", e.fe_ini);
					command.Parameters.AddWithValue("@fe_fin", e.fe_fin);
                    command.Parameters.AddWithValue("@monto", e.monto);
					command.Parameters.AddWithValue("@id_inmueble", e.id_inmueble);
					command.Parameters.AddWithValue("@id_inquilino", e.id_inquilino);
					command.Parameters.AddWithValue("@id", e.Id);
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }

        public int Borrar(int idcon)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update contrato set estado='0' where Id=@id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.UInt32);
                    command.Parameters["@id"].Value = idcon;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }

                string multa = $"Select Multa(@id)";

                using (var command = new MySqlCommand(multa, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.UInt32);
                    command.Parameters["@id"].Value = idcon;
                    connection.Open();
                    i =  Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }

            }
            return i;
        }
    }
}
    
