using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovigenCore.Data;
using MovigenCore.Models;
using MovigenCore.Models.ViewModel;
using MovigenCore.Utility;

namespace MovigenCore.Areas.Admin.Controllers
{    
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("SuperAdmin")]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public UserViewModel UserVM { get; set; }

        public UsuariosController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadUsers()
        {
            var usersFromDb = _db.ApplicationUser.Include(e => e.Empresa).ToList();
            var userList = new List<UserViewModel>();

            foreach (var item in usersFromDb)
            {
                var rolId = _db.UserRoles.First(m => m.UserId == item.Id);
                var roles = _db.Roles.First(m => m.Id == rolId.RoleId);

                UserViewModel usuario = new UserViewModel()
                {
                    User = item,
                    Rol = roles.NormalizedName
                };
                userList.Add(usuario);
            }
            var _TotalRecords = userList.Count();

            var data = userList.ToList();

            return Json(new { recordsFiltered = _TotalRecords, data });

        }

        //GET EDIT
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var _rolId = _db.UserRoles.First(m => m.UserId == id); // Trae el rol Id del usuario
            var _roles = _db.Roles.First(m => m.Id == _rolId.RoleId); // Trae el objeto rol del usuario
            var _rolesOb = _db.Roles.ToList(); // Trae una lista de todos los roles
            var empresas = _db.Empresa.ToList();


            UserVM = new UserViewModel()
            {
                User = await _db.ApplicationUser.FindAsync(id),
                Rol = _roles.NormalizedName,
                RolId = _roles.Id,
                RolList = _rolesOb,
                EmpresaList = empresas
            };
            return View(UserVM);
        }

        //POST EDIT
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByEmailAsync(UserVM.User.Email);
                var appUserFromDb = await _db.ApplicationUser.FindAsync(userFromDb.Id);
                var roles = _db.Roles.First(m => m.Id == UserVM.RolId);

                appUserFromDb.Nombre = UserVM.User.Nombre;
                appUserFromDb.EmpresaId = int.Parse( UserVM.EmpresaId);
                appUserFromDb.CodVendedor = UserVM.User.CodVendedor;

                var roleDel = await _userManager.GetRolesAsync(userFromDb);
                await _userManager.RemoveFromRolesAsync(userFromDb, roleDel.ToArray());
                await _userManager.AddToRoleAsync(userFromDb, roles.Name);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(UserVM);

        }

        //GET DELETE
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || id.Trim().Length == 0)
                return NotFound();

            var userFromDb = await _db.ApplicationUser.FindAsync(id);

            if (userFromDb == null)
                return NotFound();

            return View(userFromDb);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountControl(string id)
        {
            var UserFromDb = await _db.ApplicationUser.FindAsync(id);

            if (UserFromDb.LockoutEnabled == false)
            {
                //== Lo desactiva, usuario bloqueado
                UserFromDb.LockoutEnabled = true;
                var LockDate = new DateTime(2999, 01, 01);
                await _userManager.SetLockoutEndDateAsync(UserFromDb, LockDate);

            }
            else
            {
                //== Lo activa 
                UserFromDb.LockoutEnabled = true;
                UserFromDb.LockoutEnd = null;
                UserFromDb.LockoutEnabled = false;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}