using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovigenCore.Models
{
    public class Detalle
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Codigo { get; set; }

        [Required]
        public string Descripcion { get; set; }
        

        [NotMapped]
        public double Stock { get; set; }

        [Required]
        public double Precio { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cantidad { get; set; }
        public double Descuento { get; set; }
        public double Total { get; set; }

        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; }
    }
}
