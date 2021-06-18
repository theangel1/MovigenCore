using System.ComponentModel.DataAnnotations;

namespace MovigenCore.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required, MinLength(9), MaxLength(10)]
        public string Rut { get; set; }

        [Required, Display(Name = "Razon Social"), MinLength(5), MaxLength(60)]
        public string RazonSocial { get; set; }

        [Required, Display(Name = "Nombre Fantasía"), MinLength(5), MaxLength(60)]
        public string NombreFantasia { get; set; }

        [Required, MinLength(5), MaxLength(40)]
        public string Direccion { get; set; }

        [Required]
        public string EmailAdministrador { get; set; }

    }
       
}
