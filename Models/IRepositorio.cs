using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Models
{
    public interface IRepositorio<Pro>
	{
		int Alta(Pro p);
		int Borrar(int id);
		int Modificacion(Pro p);
		IList<Pro> obtener();
	}
}
    

