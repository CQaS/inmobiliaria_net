using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Models
{
    public class Inquilino
    {
        public int Id { get; set; }
        public int Dni { get; set; }
        public String Nombre { get; set; }
        public String Mail { get; set; }
        public String Direccion { get; set; }
        public int tel_inquilino { get; set; }
        public String lugarTrabajo { get; set; }
        public String nom_garante { get; set; }
        public int dni_garante { get; set; }
        public int tel_garante { get; set; }
    }
}
