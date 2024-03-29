﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Models
{
    public class RepositorioPropietario : RepositorioBase
    {

        public RepositorioPropietario(IConfiguration configuration): base(configuration)
        {

        }
        
        public List<Propietario> obtener()
        {
            var res = new List<Propietario>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Direccion)}, {nameof(Propietario.Tel)}  FROM propietarios WHERE estado = 1";
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
                            Tel = reader.GetInt32(4),
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
            Propietario Pro;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Direccion)}, {nameof(Propietario.Tel)}  FROM propietarios where id=@idP AND estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idP", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Pro = new Propietario();
                    if(res.Read())
                    {
                        Pro.Id = int.Parse(res["id"].ToString());
                        Pro.Dni = int.Parse(res["dni"].ToString());
                        Pro.Nombre = res["nombre"].ToString();
                        Pro.Direccion = res["direccion"].ToString();
                        Pro.Tel = int.Parse(res["tel"].ToString());
                    }
                    connection.Close();
                    
                }
            }
            return Pro;
        }


        public int Alta(Propietario e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO propietarios(Dni, Nombre, direccion, Tel) VALUES (@dni,@nombre,@direccion,@tel)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.Int32);
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters.Add("@direccion", MySqlDbType.VarChar);
                    command.Parameters.Add("@tel", MySqlDbType.Int32);
                    command.Parameters["@dni"].Value = e.Dni;
                    command.Parameters["@nombre"].Value = e.Nombre;
                    command.Parameters["@direccion"].Value = e.Direccion;
                    command.Parameters["@tel"].Value = e.Tel;
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }
                
                string sql_ID = $"SELECT MAX(id) AS id FROM propietarios";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
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
                string sql = $"update propietarios set Dni=@dni, Nombre=@nombre, Direccion=@direccion, Tel=@tel where Id=@id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.VarChar);
                    command.Parameters["@dni"].Value = e.Dni;
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters["@nombre"].Value = e.Nombre;
                    command.Parameters.Add("@direccion", MySqlDbType.VarChar);
                    command.Parameters["@direccion"].Value = e.Direccion;
                    command.Parameters.Add("@tel", MySqlDbType.VarChar);
                    command.Parameters["@tel"].Value = e.Tel;
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
                string sql = $"update propietarios SET estado = 0 where id=@idIn";

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
    

