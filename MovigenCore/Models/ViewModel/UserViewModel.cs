using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models.ViewModel
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }

        public string Rol { get; set; }
        public string RolId { get; set; }
        public string Empresa { get; set; }
        public string EmpresaId{ get; set; }
        public IEnumerable<object> RolList { get; set; }
        public IEnumerable<object> EmpresaList { get; set; }
    }
}
