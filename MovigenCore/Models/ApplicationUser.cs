using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name ="Vendedor")]
        public string Nombre { get; set; }

        [Display(Name = "Código Vendedor"), Required]
        public string CodVendedor { get; set; }

        [Required]
        public int EmpresaId { get; set; }        

        [NotMapped]
        public bool IsSuperAdmin { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }
    }
}
