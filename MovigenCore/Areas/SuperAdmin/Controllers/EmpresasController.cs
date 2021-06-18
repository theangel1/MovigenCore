using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovigenCore.Data;
using MovigenCore.Models.ViewModel;
using MovigenCore.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace MovigenCore.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.SuperAdminEndUser)]    
    [Area("SuperAdmin")]
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public EmpresaViewModel EmpresaVM { get; set; }

        public EmpresasController(ApplicationDbContext db)
        {
            _db = db;
            EmpresaVM = new EmpresaViewModel();
        }

        // GET: Admin/Empresas
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadEmpresas()
        {
            var empresas = _db.Empresa;

            var _TotalRecords = empresas.Count();

            var data = empresas.ToList();

            return Json(new { recordsFiltered = _TotalRecords, data });
        }


        // GET: Admin/Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _db.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Admin/Empresas/Create
        public IActionResult Create()
        {
            EmpresaVM.Usuarios = _db.ApplicationUser.Select(a => new SelectListItem()
            {
                Value = a.Email,
                Text = a.Email
            }).ToList();

            return View(EmpresaVM);
        }

        // POST: Admin/Empresas/Create        
        [HttpPost, ValidateAntiForgeryToken, ActionName("Create")]       
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                var empresasFromdb = _db.Empresa.ToList();

                foreach (var item in empresasFromdb)
                {
                    if (item.Rut == EmpresaVM.Empresa.Rut)
                    {
                        ViewBag.message = "Empresa ya se encuentra registrada";
                        return View(EmpresaVM);
                    }

                }


                _db.Add(EmpresaVM.Empresa);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(EmpresaVM);
        }

        // GET: Admin/Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)            
                return NotFound();

            var objEmpresa = new EmpresaViewModel()
            {
                Empresa = await _db.Empresa.FirstOrDefaultAsync(e => e.Id == id),
            };

            if (objEmpresa == null) return NotFound();

            objEmpresa.Usuarios = _db.ApplicationUser.Select(a => new SelectListItem()
            {
                Value = a.Email,
                Text = a.Email
            }).ToList();

            return View(objEmpresa);

        }
 
        [HttpPost, ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id)
        {
            if (id != EmpresaVM.Empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(EmpresaVM.Empresa);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(EmpresaVM.Empresa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(EmpresaVM);
        }

        // GET: Admin/Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _db.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);

            if (empresa == null)           
                return NotFound();            

            return View(empresa);
        }

        // POST: Admin/Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _db.Empresa.FindAsync(id);
            _db.Empresa.Remove(empresa);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _db.Empresa.Any(e => e.Id == id);
        }
    }
}
