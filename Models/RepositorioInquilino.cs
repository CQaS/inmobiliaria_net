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
                string sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Nombre)}, Mail  FROM inquilinos";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Mail = reader.GetString(2),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }


        public int Alta(Inquilino e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO inquilinos {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Mail)} " +
                    $"VALUES (@nombre, @email)" +
                    $"SELECT SCOPE_IDENTITY()";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", e.Nombre);
                    command.Parameters.AddWithValue("@email", e.Mail);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    e.Id = res;
                    connection.Close();
                }

            }
            return res;
        }
    }
}
    

