using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Models
{
    public class Propietario
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public int Dni { get; set; }
        public String Direccion { get; set; }
        public int Tel { get; set; }
    }
}