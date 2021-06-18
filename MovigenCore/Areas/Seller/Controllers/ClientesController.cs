using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovigenCore.Data;
using MovigenCore.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovigenCore.Areas.Seller.Controllers
{
    [Authorize]
    [Area("Seller")]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClientesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {     
            return View();
        }

        // GET: Detalles
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var clientes = await _db.Cliemae
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (clientes == null)
                return NotFound();

            return View(clientes);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }     

        public IActionResult GetClientes()
        {
            string codigoVendedor = GetCodVendedor();
            int empresaID = GetEmpresaId();

            var clientes = _db.Cliemae.Include(c => c.Empresa).Where(c => c.EmpresaId == empresaID && c.Clven == codigoVendedor);
            var totalRecords = clientes.Count();
            var data = clientes.ToList();
            return Json(new { recordsFiltered = totalRecords, data });
        }
        

        // POST: Create        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliemae clientes)
        {
            int empresaID =  GetEmpresaId();            
            string codVendedor = GetCodVendedor();

            if (ModelState.IsValid)
            {                
                clientes.EmpresaId = empresaID;
                clientes.Clven = codVendedor;
                _db.Add(clientes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(clientes);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return NotFound();

            var clientes = await _db.Cliemae.FindAsync(id);

            if (clientes == null)
                return NotFound();

            return View(clientes);
        }

        // POST: Edit        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliemae cliemae)
        {
            var clienteFromDB = _db.Cliemae.Where(c => c.Id == cliemae.Id
            && c.Clven == cliemae.Clven && c.EmpresaId == cliemae.EmpresaId).FirstOrDefault();

            if (id != cliemae.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                clienteFromDB.Clnom = cliemae.Clnom;
                clienteFromDB.Cldir = cliemae.Cldir;
                clienteFromDB.Clobs = cliemae.Clobs;

                _db.Update(clienteFromDB);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliemae);
        }

        // GET: Seller/Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();
            

            var cliemae = await _db.Cliemae
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliemae == null)            
                return NotFound();            

            return View(cliemae);
        }

        // POST: Seller/Clientes/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliemae = await _db.Cliemae.FindAsync(id);
            _db.Cliemae.Remove(cliemae);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      

        //Metodo el cual me retorna un objecto cliente el cual se le agrega la condicion
        // de que haga match con el codigo del vendedor
        //¿puede que tenga que agregar condicion de empresa?
        
        public JsonResult GetClients()
        {            
            string codigoVendedor = GetCodVendedor();
            int empresaID = GetEmpresaId();

            var clientesFromDb = _db.Cliemae.Where(c => c.EmpresaId == empresaID && c.Clven.ToLower() == codigoVendedor.ToLower());

            var _TotalRecords = clientesFromDb.Count();

            var data = clientesFromDb.ToList();

            return Json(new { recordsFiltered = _TotalRecords, data });

        }


        //Custom functions
        public int GetEmpresaId()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _db.ApplicationUser.Where(e => e.Id == currentUserId).FirstOrDefault();
            return _db.Empresa.Where(e => e.Id == user.EmpresaId).Select(e => e.Id).First();
        }

        public string GetCodVendedor()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _db.ApplicationUser.Where(e => e.Id == currentUserId).FirstOrDefault();
            return user.CodVendedor;
        }
    }
}
