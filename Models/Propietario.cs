using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Models
{
    public class Propietario
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public String Nombre { get; set; }
        [Required]
        public int Dni { get; set; }
        [Required]
        public String Direccion { get; set; }
        [Required]
        public int Tel { get; set; }
    }
}