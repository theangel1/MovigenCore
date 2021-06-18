using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models.ViewModel
{
    public class PedidosViewModel
    {        
        public Pedido Pedidos { get; set; }
        public List<Detalle> Detalles { get; set; }
    }
}
