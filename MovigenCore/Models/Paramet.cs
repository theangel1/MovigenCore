using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models
{
    public class Paramet
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Paiva { get; set; }
        public string Paven { get; set; }
        public string Panom { get; set; }
        public string Paedi { get; set; }
        public string Pacli { get; set; }
        public string Pag01 { get; set; }
        public string Pag02 { get; set; }
        public string Pag03 { get; set; }
        public string Pag04 { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }
    }
}
