using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
 
namespace AplicacionPrueba.Models
{
    public class RepositorioPago
    {
        private readonly string connectionString;

        public RepositorioPago()
        {
            connectionString = "Server=localhost;Database=inmobiliaria_net;Uid=root;Pwd=";
        }
        
        public List<Pago> obtener()
        {
            var res = new List<Pago>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT p.Id, p.num_pago, p.Fecha , p.Importe, p.ContratoId, c.id_inquilino , c.id_inmueble FROM Pagos p INNER JOIN Contrato c ON p.id = c.Id WHERE p.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Pago
                        {
                            Id = reader.GetInt32(0),
                            Num_Pago = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato
                            {
                                id_inquilino = reader.GetInt32(5),
							    id_inmueble = reader.GetInt32(6),
                            },
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public Pago Buscar(int id)
        {
            Pago p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT p.Id, p.num_pago, p.Fecha, p.Importe,p.ContratoId,c.id_inquilino,c.id_inmueble FROM Pagos p INNER JOIN Contrato c ON p.ContratoId = c.Id WHERE p.Id= @id AND p.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    p = new Pago();
                    if(reader.Read())
                    {
                        p.Id = int.Parse(reader["id"].ToString());
                        p.Num_Pago = int.Parse(reader["Num_Pago"].ToString());
                        p.Fecha = DateTime.Parse(reader["Fecha"].ToString());
                        p.Importe = int.Parse(reader["Importe"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return p;
        }

        public IList<Pago> BuscarPorContrato(int id)
		{
			List<Pago> res = new List<Pago>();
			Pago entidad = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT p.Id, p.num_pago, p.Fecha,p.Importe,p.ContratoId,c.id_inquilino, c.id_inmueble FROM Pagos p INNER JOIN Contrato c ON p.ContratoId = c.id WHERE ContratoId=@id AND p.estado = 1";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Pago
						{
							Id = reader.GetInt32(0),
							Num_Pago=reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Contrato = new Contrato
							{
								Id = reader.GetInt32(4),
								id_inquilino = reader.GetInt32(5),
								id_inmueble = reader.GetInt32(6),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}


        public int Alta(Pago e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO pagos (Num_Pago, Fecha, Importe, ContratoId) VALUES (@Num_Pago,@fecha, @importe, @contratoId)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
					command.Parameters.AddWithValue("@Num_Pago", e.Num_Pago);
					command.Parameters.AddWithValue("@fecha", e.Fecha);
					command.Parameters.AddWithValue("@importe", e.Importe);
					command.Parameters.AddWithValue("@contratoId", e.ContratoId);
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }                
                string sql_ID = $"SELECT MAX(id) AS idUltimo FROM pagos";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Pago e)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update pagos set Num_Pago=@num_Pago,  Fecha=@fecha, Importe=@importe WHERE Id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@num_Pago",e.Num_Pago);
					command.Parameters.AddWithValue("@fecha", e.Fecha);
					command.Parameters.AddWithValue("@importe", e.Importe);
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
                string sql = $"update pagos set estado='0' where Id=@id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.UInt32);
                    command.Parameters["@id"].Value = idcon;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }
    }
}
    

