using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Models
{
    public class RepositorioInquilino : RepositorioBase
    {

        public RepositorioInquilino(IConfiguration configuration): base(configuration)
        {

        }
        
        public List<Inquilino> obtener()
        {
            var res = new List<Inquilino>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Nombre)}, Mail, {nameof(Inquilino.Direccion)}, {nameof(Inquilino.tel_inquilino)}, {nameof(Inquilino.lugarTrabajo)}, {nameof(Inquilino.nom_garante)}, {nameof(Inquilino.dni_garante)}, {nameof(Inquilino.tel_inquilino)} FROM inquilinos";
                
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
                            tel_inquilino = reader.GetInt32(5),
                            lugarTrabajo = reader.GetString(6),
                            nom_garante = reader.GetString(7),
                            dni_garante = reader.GetInt32(8),
                            tel_garante = reader.GetInt32(9),
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
            Inquilino Inq;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Nombre)}, Mail, {nameof(Inquilino.Direccion)}, {nameof(Inquilino.tel_inquilino)}, {nameof(Inquilino.lugarTrabajo)}, {nameof(Inquilino.nom_garante)}, {nameof(Inquilino.dni_garante)}, {nameof(Inquilino.tel_garante)}  FROM inquilinos where id=@idIn";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@idIn", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Inq = new Inquilino();
                    if(res.Read())
                    {
                        Inq.Id = int.Parse(res["id"].ToString());
                        Inq.Dni = int.Parse(res["dni"].ToString());
                        Inq.Nombre = res["nombre"].ToString();
                        Inq.Mail = res["mail"].ToString();
                        Inq.Direccion = res["direccion"].ToString();
                        Inq.tel_inquilino = int.Parse(res["tel_inquilino"].ToString());
                        Inq.lugarTrabajo = res["lugarTrabajo"].ToString();
                        Inq.nom_garante = res["nom_garante"].ToString();
                        Inq.dni_garante = int.Parse(res["dni_garante"].ToString());
                        Inq.tel_garante = int.Parse(res["tel_garante"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return Inq;
        }


        public int Alta(Inquilino e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO inquilinos(Dni, Nombre, mail, direccion, tel_inquilino, lugarTrabajo, nom_garante, dni_garante, tel_garante) VALUES (@dni,@nombre,@mail,@direccion,@tel_inquilino, @lugartrabajo, @nom_garante, @dni_garante,@tel_garante)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.Int32);
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters.Add("@mail", MySqlDbType.VarChar);
                    command.Parameters.Add("@direccion", MySqlDbType.VarChar);
                    command.Parameters.Add("@tel_inquilino", MySqlDbType.Int32);
                    command.Parameters.Add("@lugarTrabajo", MySqlDbType.VarChar);
                    command.Parameters.Add("@nom_garante", MySqlDbType.VarChar);
                    command.Parameters.Add("@dni_garante", MySqlDbType.Int32);
                    command.Parameters.Add("@tel_garante", MySqlDbType.Int32);
                    command.Parameters["@dni"].Value = e.Dni;
                    command.Parameters["@nombre"].Value = e.Nombre;
                    command.Parameters["@mail"].Value = e.Mail;
                    command.Parameters["@direccion"].Value = e.Direccion;
                    command.Parameters["@tel_inquilino"].Value = e.tel_inquilino;
                    command.Parameters["@lugarTrabajo"].Value = e.lugarTrabajo;
                    command.Parameters["@nom_garante"].Value = e.nom_garante;
                    command.Parameters["@dni_garante"].Value = e.dni_garante;
                    command.Parameters["@tel_garante"].Value = e.tel_garante;
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }
                
                string sql_ID = $"SELECT MAX(id) AS id FROM inquilinos";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
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
                string sql = $"update inquilinos set Dni=@dni, Nombre=@nombre, Mail=@mail, Direccion=@direccion, tel_inquilino=@tel_inquilino, lugartrabajo=@lugarTrabajo, nom_garante=@nom_garante, dni_garante=@dni_garante, tel_garante=@tel_garante where Id=@id";

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
                    command.Parameters.Add("@tel_inquilino", MySqlDbType.VarChar);
                    command.Parameters["@tel_inquilino"].Value = e.tel_inquilino;
                    command.Parameters.Add("@lugartrabajo", MySqlDbType.VarChar);
                    command.Parameters["@lugartrabajo"].Value = e.lugarTrabajo;
                    command.Parameters.Add("@nom_garante", MySqlDbType.VarChar);
                    command.Parameters["@nom_garante"].Value = e.nom_garante;
                    command.Parameters.Add("@dni_garante", MySqlDbType.VarChar);
                    command.Parameters["@dni_garante"].Value = e.dni_garante;
                    command.Parameters.Add("@tel_garante", MySqlDbType.VarChar);
                    command.Parameters["@tel_garante"].Value = e.tel_garante;
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
    

