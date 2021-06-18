using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using MovigenCore.Data;
using MovigenCore.Extensiones;
using MovigenCore.Models;
using MovigenCore.Utility;

namespace MovigenCore.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class GestionesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        public GestionesController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public int GetEmpresaId()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _db.ApplicationUser.Where(e => e.Id == currentUserId).FirstOrDefault();
            return _db.Empresa.Where(e => e.Id == user.EmpresaId).Select(e => e.Id).First();
        }

        [HttpPost]
        public IActionResult LoadProdmae()
        {

            var files = HttpContext.Request.Form.Files;
            string webRootPath = _hostingEnvironment.WebRootPath;
            var extension = Path.GetExtension(files[0].FileName);
            var empresaID = GetEmpresaId();

            string nombreArchivoSS = files[0].FileName.Substring(files[0].FileName.Length - 11);

            if (files.Count != 0 && extension == ".xml" && nombreArchivoSS == "prodmae.xml")
            {
                var uploads = Path.Combine(webRootPath, SD.XmlFolder);

                using (var filestream = new FileStream(Path.Combine(uploads, files[0].FileName), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                try
                {
                    XElement productosFromFile = XElement.Load(Path.Combine(uploads, files[0].FileName));

                    List<Prodmae> prodList = productosFromFile.Descendants("PRODMAE").Select(
                        p => new Prodmae
                        {
                            EmpresaId = empresaID,
                            Prcod = p.Element("PRCOD").Value,
                            Prdes = p.Element("PRDES").Value,
                            Prun = p.Element("PRUN").Value,
                            Prvl1 = p.Element("PRVL1").Value,
                            Prvl2 = p.Element("PRVL2").Value,
                            Prvl3 = p.Element("PRVL3").Value,
                            Prvl4 = p.Element("PRVL4").Value,
                            Prvl5 = p.Element("PRVL5").Value,
                            Prila = p.Element("PRILA").Value,
                            Prstk = p.Element("PRSTK").Value,
                            Prfam = p.Element("PRFAM").Value,
                            Unidxenv = p.Element("UNIDXENV").Value,
                            Descuento = p.Element("DESCUENTO").Value,
                            Prfracc = p.Element("PRFRACC").Value,
                            Prtipoflete = p.Element("PRTIPOFLETE").Value,
                            Prflete = p.Element("PRFLETE").Value,
                            Prcantpermit = p.Element("PRCANTPERMIT").Value,
                        }
                        ).ToList();

                    //en esta logica, podria eliminar todos los productos que estan en la bd antes de agregarlos


                    foreach (var i in prodList)
                    {
                        List<Prodmae> productosDb = _db.Prodmae.Where(p => p.EmpresaId == empresaID).ToList();

                        foreach (var item in productosDb)
                        {
                            _db.Remove(item);
                        }

                        _db.Add(i);
                    }
                    _db.SaveChanges();
                    ViewData["Mensaje"] = "Producto agregado correctamente";

                }

                catch (Exception ex)
                {

                    ViewData["Mensaje"] = "Archivo xml mal formado, consulte con un técnico de Sisgen.";
                }
                return View(nameof(Index));
            }
            else
            {
                ViewData["error"] = "Error al cargar productos, consulte con un técnico de Sisgen Chile";
                return View(nameof(Index));//enviar un mensaje de error si no se pude
            }


        }

        [HttpPost]
        public IActionResult LoadCliemae()
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _hostingEnvironment.WebRootPath;
            var extension = Path.GetExtension(files[0].FileName);
            var empresaID = GetEmpresaId();
            string nombreArchivoSS = files[0].FileName.Substring(files[0].FileName.Length - 11);

            if (files.Count != 0 && extension == ".xml" && nombreArchivoSS == "cliemae.xml")
            {
                var uploads = Path.Combine(webRootPath, SD.XmlFolder);

                using (var filestream = new FileStream(Path.Combine(uploads, files[0].FileName), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                try
                {
                    XElement clientesFromFile = XElement.Load(Path.Combine(uploads, files[0].FileName));

                    List<Cliemae> cliemaeList = clientesFromFile.Descendants("CLIEMAE").Select(
                    cliemae => new Cliemae
                    {
                        EmpresaId = empresaID,
                        Clven = cliemae.Element("CLVEN").Value,
                        Clrut = cliemae.Element("CLRUT").Value,
                        Clsuc = cliemae.Element("CLSUC").Value,
                        Clnsu = cliemae.Element("CLNSU").Value,
                        Cldsu = cliemae.Element("CLDSU").Value,
                        Clnom = cliemae.Element("CLNOM").Value,
                        Cldir = cliemae.Element("CLDIR").Value,
                        Clcon = cliemae.Element("CLCON").Value,
                        Clcre = cliemae.Element("CLCRE").Value,
                        Clobs = cliemae.Element("CLOBS").Value,
                        Cld01 = cliemae.Element("CLD01").Value,
                        Cld02 = cliemae.Element("CLD02").Value,
                        Cld03 = cliemae.Element("CLD03").Value,
                        Cld04 = cliemae.Element("CLD04").Value,
                        Cld05 = cliemae.Element("CLD05").Value,
                        Cld06 = cliemae.Element("CLD06").Value,
                        Cld07 = cliemae.Element("CLD07").Value,
                        Cllpe = cliemae.Element("CLLPE").Value,
                        Clord = cliemae.Element("CLORD").Value,
                        Clmcr = cliemae.Element("CLMCR").Value

                    }).ToList();


                    foreach (var i in cliemaeList)
                    {

                        List<Cliemae> clientesFromDB = _db.Cliemae.Where(c => c.EmpresaId == empresaID).ToList();

                        //Aca asumiremos de que nuestro cliente quiere formatear la database(tabla realmente)
                        //, y cargar nuevos datos de cliente. Podemos cambiar esta logica obviamente.. o no... :@
                        foreach (var item in clientesFromDB)
                        {
                            _db.Remove(item);
                        }

                        _db.Cliemae.Add(i);

                    }


                    ViewData["Mensaje"] = "Cliente agregado correctamente";
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewData["Mensaje"] = "Archivo xml mal formado, consulte con un técnico de Sisgen.";
                }
                return View(nameof(Index));
            }
            else
            {
                ViewData["error"] = "Error al cargar clientes, consulte con un técnico de Sisgen Chile";
                return View(nameof(Index));
            }

        }

        public IActionResult LoadCondmae()
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _hostingEnvironment.WebRootPath;
            var extension = Path.GetExtension(files[0].FileName);
            string nombreArchivoSS = files[0].FileName.Substring(files[0].FileName.Length - 11);

            if (files.Count != 0 && extension == ".xml" && nombreArchivoSS == "condmae.xml")
            {
                var uploads = Path.Combine(webRootPath, SD.XmlFolder);

                using (var filestream = new FileStream(Path.Combine(uploads, files[0].FileName), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                XElement condPagoFromFile = XElement.Load(Path.Combine(uploads, files[0].FileName));


                List<Conmae> conmaeList = condPagoFromFile.Descendants("CONDMAE").Select(
                    condmae => new Conmae
                    {
                        EmpresaId = GetEmpresaId(),
                        Cocod = condmae.Element("COCOD").Value,
                        Codes = condmae.Element("CODES").Value
                    }).ToList();

                foreach (var i in conmaeList)
                {
                    var v = _db.Conmae.Where(a => a.Id == i.Id).FirstOrDefault();

                    if (v != null)
                    {
                        v.EmpresaId = i.EmpresaId;
                        v.Cocod = i.Cocod;
                        v.Codes = i.Codes;
                    }
                    else
                        _db.Conmae.Add(i);

                    ViewData["Mensaje"] = "Condiciones de pago agregadas correctamente";
                    _db.SaveChanges();
                }


                return View(nameof(Index));
            }
            else
            {
                ViewData["error"] = "Error al cargar Condiciones de Pago, consulte con un técnico de Sisgen Chile";
                return View(nameof(Index));
            }

        }
    }
}