using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovigenCore.Data;
using MovigenCore.Extensiones;
using MovigenCore.Models;

namespace MovigenCore.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductosController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        /*
         * Descripcion del metodo
         * - Traer productos en base a un nombre de producto el cual se ingresara en un modal de la vista
         * de pedidos.
         * Tomar en cuenta como parametro la id de la empresa. Ojo con el stock         
         */
       
        public JsonResult GetProducts()
        {
            int empresaID = GetEmpresaId();

            var productsFromDb = _db.Prodmae.Where(p => p.EmpresaId == empresaID);

            var _TotalRecords = productsFromDb.Count();

            var data = productsFromDb.ToList();

            return Json(new { recordsFiltered = _TotalRecords, data });

        }

        [HttpPost]
        public async Task<List<Prodmae>> GetProductosAjaxPorCodigo(string codigo)
        {
            int empresaID = GetEmpresaId();

            if (!string.IsNullOrEmpty(codigo))
            {
                var appProd = await _db.Prodmae.Where(c => c.Prcod.ToLower() == codigo.ToLower().Trim() &&
                c.EmpresaId == empresaID).ToListAsync();
                return appProd;
            }
            return null;
        }

        public int GetEmpresaId()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _db.ApplicationUser.Where(e => e.Id == currentUserId).FirstOrDefault();
            return _db.Empresa.Where(e => e.Id == user.EmpresaId).Select(e => e.Id).First();
        }

    }


}