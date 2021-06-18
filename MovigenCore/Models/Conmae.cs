using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models
{
    public class Conmae
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Cocod { get; set; }
        public string Codes { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }
    }
}
