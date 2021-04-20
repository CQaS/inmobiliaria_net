using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "Codigo")]
        public int Id_inmu { get; set; }
        [Required]
        [Display(Name = "Direccion")]
        public String Direccion_in { get; set; }
        [Required]
        public String Uso { get; set; }
        [Required]
        public String Tipo { get; set; }
        [Required]
        public int ambientes { get; set; }
        [Required]
        public int precio { get; set; }
        public String foto { get; set; }
        [Display(Name = "Elige una Foto:")]
        public IFormFile FotoFile{ get; set; }
        [Display(Name = "Dueño")]
        public int id_propietario { get; set; }
        [ForeignKey(nameof(Inmueble.id_propietario))]
        public Propietario Duenio { get; set; }
        
        public Inquilino inquilino { get; set; }
        public Contrato contrato { get; set; }
        public Inmueble inmueble { get; set; }
    }
}
