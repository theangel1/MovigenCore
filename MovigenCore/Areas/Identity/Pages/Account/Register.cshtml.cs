using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MovigenCore.Data;
using MovigenCore.Models;
using MovigenCore.Utility;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MovigenCore.Areas.Identity.Pages.Account
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        //ctor
        public RegisterModel(
          UserManager<IdentityUser> userManager,
          SignInManager<IdentityUser> signInManager,
          ILogger<RegisterModel> logger,
          IEmailSender emailSender,
          RoleManager<IdentityRole> roleManager,
          ApplicationDbContext db
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Nombre { get; set; }

            [Display(Name = "¿Es Admin?")]
            public bool IsAdmin { get; set; }

            [Display(Name = "Código Vendedor"), Required]
            public string CodVendedor { get; set; }

            [Display(Name = "Empresa asociada"), Required]
            public int EmpresaId { get; set; }

        }

        public void OnGet(string returnUrl = null)
        {
            ViewData["temp"] = _db.Empresa.ToList();
            ReturnUrl = returnUrl;
            
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nombre = Input.Nombre,
                    EmpresaId = Input.EmpresaId,
                    CodVendedor = Input.CodVendedor
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(SD.SuperAdminEndUser))
                        await _roleManager.CreateAsync(new IdentityRole(SD.SuperAdminEndUser));

                    if (!await _roleManager.RoleExistsAsync(SD.AdminEndUser))
                        await _roleManager.CreateAsync(new IdentityRole(SD.AdminEndUser));

                    if (!await _roleManager.RoleExistsAsync(SD.VendedorEndUser))
                        await _roleManager.CreateAsync(new IdentityRole(SD.VendedorEndUser));

                    if (Input.IsAdmin)
                        await _userManager.AddToRoleAsync(user, SD.AdminEndUser);
                    else
                        await _userManager.AddToRoleAsync(user, SD.VendedorEndUser);

                    //Acá seteo las variables de sesion para que el usuario comience a usar 
                    //el sistema inmediatamente                   

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["temp"] = _db.Empresa.ToList();
            return Page();
        }
    }
}
