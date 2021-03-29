using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
	{
		Propietario ObtenerPorEmail(string mail);
        IList<Propietario> BuscarPorNombre(string nombre);
    	}
}