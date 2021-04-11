using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Models
{
    public class RepositorioContrato
    {
        private readonly string connectionString;

        public RepositorioContrato()
        {
            connectionString = "Server=localhost;Database=inmobiliaria_net;Uid=root;Pwd=";
        }
        
        public List<Contrato> obtener()
        {
            var res = new List<Contrato>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {//id nombre dni direccion tel
                string sql = $"SELECT c.id, c.fe_ini, c.fe_fin, c.monto, c.id_inquilino, i.Nombre, c.id_inmueble, e.direccion_in, e.id_propietario FROM Contrato c INNER JOIN Inquilinos i ON i.id = c.id_inquilino INNER JOIN Inmueble e ON e.id_Inmu = c.id_inmueble WHERE c.estado = 1";
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
                            monto = reader.GetInt32(3),

							id_inquilino = reader.GetInt32(4),
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(4),
								Nombre = reader.GetString(5),
							},

							id_inmueble = reader.GetInt32(6),
							Inmueble = new Inmueble
							{
								Id_inmu = reader.GetInt32(6),
								Direccion_in= reader.GetString(7),
								id_propietario = reader.GetInt32(8),
							},
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }
        
    }
}
    

