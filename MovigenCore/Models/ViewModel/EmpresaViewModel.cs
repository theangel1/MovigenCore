using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Models.ViewModel
{
    public class EmpresaViewModel
    {
        public Empresa Empresa { get; set; }

        public List<SelectListItem> Usuarios { get; set; }
    }
}
