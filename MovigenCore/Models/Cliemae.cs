using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models
{
    public class Cliemae
    {
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }
        
        public string Clven { get; set; }

        [Required, Display(Name ="Rut Cliente")]
        public string Clrut { get; set; }
        
        public string Clsuc { get; set; }
        
        public string Clnsu { get; set; }
        
        public string Cldsu { get; set; }

        [Required, Display(Name ="Nombre Cliente")]
        public string Clnom { get; set; }

        [Required, Display(Name ="Direccion")]
        public string Cldir { get; set; }

        [Display(Name = "Condicion de Pago")]
        public string Clcon { get; set; }

        [Display(Name = "Crédito disponible")]
        public string Clcre { get; set; }

        [Display(Name ="Observaciones")]
        public string Clobs { get; set; }
        
        public string Cld01 { get; set; }
        
        public string Cld02 { get; set; }
        
        public string Cld03 { get; set; }
        
        public string Cld04 { get; set; }
        
        public string Cld05 { get; set; }
        
        public string Cld06 { get; set; }
        
        public string Cld07 { get; set; }
        
        public string Cllpe { get; set; }
        
        public string Clord { get; set; }
        
        public string Clmcr { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }
    }
}
