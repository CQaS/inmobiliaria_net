using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Models
{
    public class Inmueble
    {
        public int Id_inmu { get; set; }
        public String Direccion_in { get; set; }
        public String Uso { get; set; }
        public String Tipo { get; set; }
        public int ambientes { get; set; }
        public int precio { get; set; }
        public int id_propietario { get; set; }
        [ForeignKey("id_propietario")]
        public Propietario Duenio { get; set; }
    }
}
