using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string UserId { get; set; }

        [Display(Name ="Rut Cliente"), Required]
        public string RutCliente { get; set; }

        [Display(Name = "Razon Social"),Required]
        public string RazonSocial { get; set; }

        [Display(Name = "Dirección Sucursal")]
        public string Sucursal { get; set; }

        [Display(Name = "Condicion de Pago")]
        public string CondicionPago { get; set; }

        [Display(Name = "Comentario Adic.")]
        public string ComentarioAdicional { get; set; }

        
        public double Total { get; set; }
        
        public DateTime Fecha { get; set; }

        [Display(Name = "Tipo Documento")]
        public string TipoDocumento { get; set; }

        [Display(Name = "¿Enviado?")]
        public bool Enviado { get; set; }               

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

     

    }
}
