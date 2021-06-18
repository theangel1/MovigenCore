using System.ComponentModel.DataAnnotations.Schema;

namespace MovigenCore.Models
{
    public class Prodmae
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Prcod { get; set; }
        public string Prdes { get; set; }
        public string Prun { get; set; }
        public string Prvl1 { get; set; }
        public string Prvl2 { get; set; }
        public string Prvl3 { get; set; }
        public string Prvl4 { get; set; }
        public string Prvl5 { get; set; }
        public string Prila { get; set; }
        public string Prstk { get; set; }
        public string Prfam { get; set; }
        public string Unidxenv { get; set; }
        public string Descuento { get; set; }
        public string Prfracc { get; set; }
        public string Prtipoflete { get; set; }
        public string Prflete { get; set; }
        public string Prcantpermit { get; set; }        

        [ForeignKey("EmpresaId")]
        public virtual Empresa Empresa { get; set; }
    }
}
