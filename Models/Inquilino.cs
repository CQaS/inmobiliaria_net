using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Models
{
    public class Inquilino
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }
        [Required]
        public int Dni { get; set; }
        [Required]
        public String Nombre { get; set; }
        [Required, EmailAddress]
        public String Mail { get; set; }
        [Required]
        public String Direccion { get; set; }
        [Required]
        [Display(Name = "Tel. Inquilino")]
        public int tel_inquilino { get; set; }
        [Required]
        [Display(Name = "Lugar de Trabajo")]
        public String lugarTrabajo { get; set; }
        [Required]
        [Display(Name = "Nombred del Garante")]
        public String nom_garante { get; set; }
        [Required]
        [Display(Name = "DNI Garante")]
        public int dni_garante { get; set; }
        [Required]
        [Display(Name = "Tel. Garante")]
        public int tel_garante { get; set; }
    }
}
