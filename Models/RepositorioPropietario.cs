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
                string sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Direccion)}  FROM propietarios";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Dni = reader.GetInt32(2),
                            Direccion = reader.GetString(3),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }


        public int Alta(Propietario e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO propietarios {nameof(Propietario.Nombre)}, {nameof(Propietario.Dni)} " +
                    $"VALUES (@nombre, @email)" +
                    $"SELECT SCOPE_IDENTITY()";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", e.Nombre);
                    command.Parameters.AddWithValue("@email", e.Dni);
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
    

