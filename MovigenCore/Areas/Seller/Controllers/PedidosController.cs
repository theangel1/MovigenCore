using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovigenCore.Data;
using MovigenCore.Extensiones;
using MovigenCore.Models;
using MovigenCore.Models.ViewModel;
using MovigenCore.Utility;

namespace MovigenCore.Areas.Seller.Controllers
{
    [Authorize]
    [Area("Seller")]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public PedidosViewModel PedidosVM { get; set; }


        public PedidosController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            PedidosVM = new PedidosViewModel()
            {
                Pedidos = new Pedido()
            };
        }


        public async Task<IActionResult> Index()
        {
            var pedido = _db.Pedido.Include(p => p.Empresa).
                Where(e => e.EmpresaId == GetEmpresaId() &&
                e.ApplicationUser.CodVendedor == GetCodVendedor() &&
                e.Enviado == false);

            return View(await pedido.ToListAsync());
        }

        public IActionResult Historial()
        {
            return View();
        }
        public JsonResult GetHistorial()
        {
            var pedido = _db.Pedido.Include(p => p.Empresa).
                  Where(e => e.EmpresaId == GetEmpresaId() &&
                  e.ApplicationUser.CodVendedor == GetCodVendedor() &&
                  e.Enviado == true);
            var totalRecords = pedido.Count();
            var data = pedido.ToList();

            return Json(new { recordsFiltered = totalRecords, data });
        }

        public IActionResult Create()
        {
            return View(PedidosVM);
        }


        #region Create Post
        [HttpPost, ActionName("Create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            double sumaTotalesDetalle = 0;
            double credito = 0;
            int pedidoId = 0;
            //int stockProducto;
            int empresaID = GetEmpresaId();
            string idVendedor = GetCodVendedor();

            PedidosVM.Pedidos.EmpresaId = empresaID;
            PedidosVM.Pedidos.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            PedidosVM.Pedidos.Fecha = DateTime.Now;

            //consulta a cliemae para restar el credito que tiene con el que usara
            var cliemaeFromDB = _db.Cliemae.Where(c => c.EmpresaId == empresaID &&
            c.Clven == idVendedor && c.Clrut == PedidosVM.Pedidos.RutCliente).FirstOrDefault();

            // credito = double.Parse(cliemaeFromDB.Clcre);

            if (!ModelState.IsValid)
                return View(PedidosVM);

            _db.Pedido.Add(PedidosVM.Pedidos);
            pedidoId = PedidosVM.Pedidos.Id;

            for (int i = 0; i < PedidosVM.Detalles.Count; i++)
            {
                //Validacion de cantidad vs stock y cantidad != 0
                /* if (PedidosVM.Detalles[i].Cantidad > PedidosVM.Detalles[i].Stock || PedidosVM.Detalles[i].Cantidad == 0)
                 {
                     ViewBag.Message = "Server: Cantidad debe ser menor al stock del producto";
                     return View();
                 }
                 */
                PedidosVM.Detalles[i].PedidoId = pedidoId;
                sumaTotalesDetalle += PedidosVM.Detalles[i].Total;

                /*var productoFromDB = _db.Prodmae.Where(p => p.EmpresaId == empresaID &&
                p.Prcod == PedidosVM.Detalles[i].Codigo).FirstOrDefault();
                stockProducto = int.Parse(productoFromDB.Prstk);
                stockProducto -= PedidosVM.Detalles[i].Cantidad;
                productoFromDB.Prstk = stockProducto.ToString();*/

                _db.Detalle.Add(PedidosVM.Detalles[i]);
            }
            PedidosVM.Pedidos.Total = sumaTotalesDetalle;

            //Validacion credito. Si el total del pedido es mayor al credito, retorna vista y mensaje de error

            /*
             * if (credito < PedidosVM.Pedidos.Total && PedidosVM.Pedidos.CondicionPago == "02")
            {
                ViewBag.Message = "Server: Cliente con credito insuficiente para efectuar la compra";
                return View();
            }*/

            //Si la condicion de pago es credito, resto a cliemae el monto, si no, no hago nada...
            //asi deberia ser la logica o no xd?

            /*
            if (PedidosVM.Pedidos.CondicionPago == "02")
            {
                credito -= sumaTotalesDetalle;
                cliemaeFromDB.Clcre = credito.ToString();
            }*/

            //Grabo en la base de datos
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        #endregion

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            PedidosViewModel objPedidoVM = new PedidosViewModel()
            {
                Pedidos = await _db.Pedido
                .Include(p => p.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id),
                Detalles = new List<Detalle>()
            };

            if (objPedidoVM.Pedidos == null)
                return NotFound();

            List<Detalle> objDetalle = _db.Detalle.Where(p => p.PedidoId == id).ToList();
            foreach (Detalle detallesObjetos in objDetalle)
            {
                objPedidoVM.Detalles.Add(detallesObjetos);
            }

            return View(objPedidoVM);

        }



        #region Edit Post
        // POST EDIT - La logica de esta edicion, incluye eliminar los detalles que estaban en la base de datos
        // y añadir el nuevo objeto de detalle(array) utilizando la misma id
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            double sumaTotalesDetalle = 0;
            string idVendedor = GetCodVendedor();
            /*double credito = 0;
            double creditoAux = 0;
            */

            //consulta a cliemae para restar el credito que tiene con el que usara
            var cliemaeFromDB = _db.Cliemae.Where(c => c.EmpresaId == PedidosVM.Pedidos.EmpresaId &&
            c.Clven == idVendedor && c.Clrut == PedidosVM.Pedidos.RutCliente).FirstOrDefault();

            //credito = double.Parse(cliemaeFromDB.Clcre);

            if (ModelState.IsValid)
            {
                var pedidoFromDb = _db.Pedido.Where(a => a.Id == PedidosVM.Pedidos.Id).FirstOrDefault();

                pedidoFromDb.RutCliente = PedidosVM.Pedidos.RutCliente;
                pedidoFromDb.RazonSocial = PedidosVM.Pedidos.RazonSocial;
                pedidoFromDb.Sucursal = PedidosVM.Pedidos.Sucursal;
                pedidoFromDb.CondicionPago = PedidosVM.Pedidos.CondicionPago;
                pedidoFromDb.ComentarioAdicional = PedidosVM.Pedidos.ComentarioAdicional;
                pedidoFromDb.Fecha = DateTime.Now;
                pedidoFromDb.TipoDocumento = PedidosVM.Pedidos.TipoDocumento;

                List<Detalle> detalleFromDb = _db.Detalle.Where(p => p.PedidoId == pedidoFromDb.Id).ToList();


                if (PedidosVM.Detalles.Count > 0)
                {
                    foreach (var detalleEliminar in detalleFromDb)
                    {
                        //Devolver stock y credito                                          
                        //Elimino todos los detalles asociados al pedido.
                        _db.Remove(detalleEliminar);
                    }

                    for (int i = 0; i < PedidosVM.Detalles.Count; i++)
                    {
                        var productoFromDB = _db.Prodmae.Where(p => p.Prcod == PedidosVM.Detalles[i].Codigo && p.EmpresaId == PedidosVM.Pedidos.EmpresaId).FirstOrDefault();

                        //Solo Dios sabe ahora que hacen estas lineas de codigo... 

                        //La idea es que actualizo el stock del producto en la base de datos.
                        /* int stockARestar = PedidosVM.Detalles[i].Cantidad < detalleFromDb[i].Cantidad ?
                             detalleFromDb[i].Cantidad - PedidosVM.Detalles[i].Cantidad :
                             PedidosVM.Detalles[i].Cantidad - detalleFromDb[i].Cantidad;

                         int newStock = int.Parse(productoFromDB.Prstk) + stockARestar;


                         productoFromDB.Prstk = newStock.ToString();

                         _db.Update(productoFromDB);*/

                        PedidosVM.Detalles[i].PedidoId = pedidoFromDb.Id;
                        sumaTotalesDetalle += PedidosVM.Detalles[i].Total;

                        _db.Detalle.Add(PedidosVM.Detalles[i]);
                    }
                }

                //Lógica devolucion de crédito
                /*double creditoAux2 = double.Parse(cliemaeFromDB.Clcre) + pedidoFromDb.Total;
                cliemaeFromDB.Clcre = creditoAux2.ToString();
                _db.Update(cliemaeFromDB);*/

                pedidoFromDb.Total = sumaTotalesDetalle;

                //Validacion credito. Si el total del pedido es mayor al credito, retorna vista y mensaje de error
                var cliemaeFromDBAux = _db.Cliemae.Where(c => c.EmpresaId == PedidosVM.Pedidos.EmpresaId &&
              c.Clven == idVendedor && c.Clrut == PedidosVM.Pedidos.RutCliente).FirstOrDefault();

                // creditoAux = double.Parse(cliemaeFromDBAux.Clcre);

                /*if (creditoAux < PedidosVM.Pedidos.Total && PedidosVM.Pedidos.CondicionPago == "02")
                {
                    ViewBag.Message = "Server: Cliente con crédito insuficiente para efectuar la compra";
                    return View();
                }*/

                //Si la condicion de pago es credito, resto a cliemae el monto, si no, no hago nada...

                /* if (PedidosVM.Pedidos.CondicionPago == "02")
                 {
                     creditoAux -= sumaTotalesDetalle;
                     cliemaeFromDB.Clcre = creditoAux.ToString();
                 }*/

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(PedidosVM);
        }
        #endregion
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var pedido = await _db.Pedido.Include(p => p.Empresa).FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null)
                return NotFound();

            return View(pedido);
        }


        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _db.Pedido.FindAsync(id);
            _db.Pedido.Remove(pedido);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            PedidosViewModel objPedidoVM = new PedidosViewModel()
            {
                Pedidos = await _db.Pedido
                .Include(p => p.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id),
                Detalles = new List<Detalle>()
            };

            if (objPedidoVM.Pedidos == null)
                return NotFound();

            List<Detalle> objDetalle = _db.Detalle.Where(p => p.PedidoId == id).ToList();
            foreach (Detalle detallesObjetos in objDetalle)
            {
                objPedidoVM.Detalles.Add(detallesObjetos);
            }

            return View(objPedidoVM);
        }


        //custom functions
        //Custom functions
        public int GetEmpresaId()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _db.ApplicationUser.Where(e => e.Id == currentUserId).FirstOrDefault();
            var empresaFromdb = _db.Empresa.Where(e => e.Id == user.EmpresaId).FirstOrDefault();

            return empresaFromdb.Id;
        }

        public string GetCodVendedor()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _db.ApplicationUser.Where(e => e.Id == currentUserId).FirstOrDefault();
            return user.CodVendedor;
        }


        [HttpPost]
        public IActionResult MetodoCustom(string dateAngel)
        {
            DateTime fecha = Convert.ToDateTime(dateAngel);

            int idEmpresa = GetEmpresaId();
            string idUsuario = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string idVendedor = GetCodVendedor();

            if (fecha < DateTime.Now.AddDays(-2))
            {
                ViewBag.Message = "No puede crear pedidos con mas de 2 dias de antiguedad.";
                return View();
            }
            else
            {
                List<Pedido> listaPedidos = ListarPedidosDia(idEmpresa, idUsuario, fecha);

                if (listaPedidos.Count < 1)
                {
                    ViewBag.Message = "No se encontraron pedidos en la fecha seleccionada";
                }
                else if (EnviarTxtPedidos(idEmpresa, idUsuario, idVendedor, fecha))
                {
                    ViewBag.Message = "Archivo creado y enviado";
                }
                else
                {
                    ViewBag.Message = "no se pudo crear y enviar el archivo";
                }

                return View();
            }

        }


        public bool EnviarTxtPedidos(int idEmpresa, string idUsuario, string idVendedor, DateTime dia)
        {

            //Crea lista de pedidos, a partir de la fecha, empresa y usuario
            List<Pedido> listaPedidos = ListarPedidosDia(idEmpresa, idUsuario, dia);

            var empresaFromdb = _db.Empresa.Where(e => e.Id == idEmpresa).FirstOrDefault();
            var rutEmpresa = empresaFromdb.Rut.Substring(0, 8);

            if (listaPedidos.Count > 0)
            {
                int contador = 1;

                List<string> lineasDetalles = new List<string>();
                foreach (Pedido ped in listaPedidos)
                {
                    List<Detalle> detalles = ListaDetallesPedido(ped.Id);
                    foreach (Detalle det in detalles)
                    {
                        string detalleString = ConvertirDetallePedidoString(ped, det, idVendedor, contador);
                        lineasDetalles.Add(detalleString);

                    }
                    contador++;

                }
                //Por cada pedido crea un string con sus datos
                string[] lineas = new string[lineasDetalles.Count];

                for (int i = 0; i < lineas.Length; i++)
                {
                    lineas[i] = lineasDetalles[i];
                }
                //Crea archivo a partir de las lineas de pedidos
                string webRootPath = _hostingEnvironment.WebRootPath;
                string nombreArchivo = "CoreFile" + DateTime.Now.ToString("ddMMHHyyyymmssFFF") + ".txt";
                string ruta = Path.Combine(webRootPath, SD.TxtFolder, nombreArchivo);


                /*From stackoverflow <3
                 * It looks like a namespace collision. The compiler is grabbing File from a different namespace
                 * than is expected. The File class that should make this work is in the System.IO namespace 
                 * and not the System.Web.Mvc.Controller namespace.*/
                System.IO.File.WriteAllLines(ruta, lineas);


                /*
                 FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://sisgenchile.com/"+ rutEmpresa+"/CoreFile" + DateTime.Now.ToString("ddMMHHyyyymmssFFF") + ".txt");
                 request.Method = WebRequestMethods.Ftp.UploadFile;
                 request.Credentials = new NetworkCredential("Developer@sisgenchile.com", "sisgenchile2020");
                 byte[] fileContents;

                 using (StreamReader sourceStream = new StreamReader(ruta))
                 {
                     fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                 }

                 request.ContentLength = fileContents.Length;
                 using (Stream requestStream = request.GetRequestStream())
                 {
                     requestStream.Write(fileContents, 0, fileContents.Length);
                 }
                 using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                 {
                     Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");

                     foreach (Pedido item in listaPedidos)
                     {
                         SetEnviado(item.Id);
                     }

                     return true;
                 } */

                //Enviarlo por Email

                return EnvioMail(listaPedidos, ruta, idEmpresa);
            }
            else
            {
                return false;
            }
        }

        private bool EnvioMail(List<Pedido> listaPedidos, string ruta, int idEmpresa)
        {
            //Traigo el email del usuario administrador.
            var empresaFromDB = _db.Empresa.Where(e => e.Id == idEmpresa).FirstOrDefault();
            var emailAdmin = empresaFromDB.EmailAdministrador;


            Attachment data = new Attachment(ruta, MediaTypeNames.Application.Octet);

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            // utilizamos el servidor SMTP de gmail

            client.EnableSsl = true;
            //client.Timeout = 10000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            // nos autenticamos con nuestra cuenta de gmail
            client.Credentials = new NetworkCredential("informatica@sisgenchile.cl", "T&7h55R666");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("informatica@sisgenchile.cl", "Movigen Core - " + empresaFromDB.NombreFantasia);
            mail.To.Add(emailAdmin);
            mail.To.Add("nicolas.cortes@sisgenchile.cl");

            //quick fix for madel
            if(empresaFromDB.Id == 10)
            {
                mail.CC.Add("osvaldogonzalocarmona@gmail.com");
            }

            mail.Subject = "Notificación Sistema Movigen";
            mail.BodyEncoding = Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.Attachments.Add(data);
            mail.IsBodyHtml = true;
            mail.Body = "Se ha generado un pedido con hora " + DateTime.Now.ToShortTimeString() + ". Debe " +
                "ingresar este archivo a Sisgen." +
                "<br>-Rut Empresa: " + empresaFromDB.Rut +
                "<br>-Razón Social: " + empresaFromDB.RazonSocial +
                "<br>-Código Vendedor: " + GetCodVendedor() +
                "<br> Atte.<br> Soluciones Informáticas Sisgen Chile";
            try
            {
                client.Send(mail);
                foreach (Pedido ped in listaPedidos)
                {
                    SetEnviado(ped.Id);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Listar pedidos del día NO enviados
        public List<Pedido> ListarPedidosDia(int idEmpresa, string idUsuario, DateTime dia)
        {
            var lista = (from consulta in _db.Pedido
                         where consulta.EmpresaId.Equals(idEmpresa) &&
                         consulta.UserId == idUsuario &&
                         consulta.Fecha.Day == dia.Day &&
                         consulta.Fecha.Month == dia.Month &&
                         consulta.Fecha.Year == dia.Year &&
                         consulta.Enviado == false
                         select consulta).ToList();
            return lista;
        }



        public List<Detalle> ListaDetallesPedido(int idPedido)
        {
            var lista = (from query in _db.Detalle
                         where query.PedidoId == idPedido
                         select query).ToList();

            List<Detalle> listaDetalles = null;
            if (lista.Count > 0)
            {
                listaDetalles = new List<Detalle>();
                foreach (Detalle detalle in lista)
                {
                    Detalle det = new Detalle();
                    det.Id = detalle.Id;
                    det.PedidoId = detalle.PedidoId;
                    det.Codigo = detalle.Codigo;
                    det.Descripcion = detalle.Descripcion;
                    det.Precio = detalle.Precio;
                    det.Cantidad = detalle.Cantidad;
                    det.Descuento = detalle.Descuento;
                    det.Total = detalle.Total;
                    listaDetalles.Add(det);
                }
            }
            return listaDetalles;
        }

        public bool SetEnviado(int idPedido)
        {
            Pedido pedido = _db.Pedido.FirstOrDefault(item => item.Id == idPedido);

            if (pedido != null)
            {
                pedido.Enviado = true;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Devolver string con datos del pedido en formato
        public string ConvertirDetallePedidoString(Pedido pedido, Detalle detalle, string codVend, int idTemporal)
        {
            string fecha;
            string hora;

            if (pedido.Fecha == null)
            {
                fecha = "        ";
                hora = "        ";
            }
            else
            {
                //Formato fecha
                fecha = pedido.Fecha.ToString("dd/MM/yy");
                //Formato hora
                hora = pedido.Fecha.Hour + ":" + pedido.Fecha.Minute + ":" + pedido.Fecha.Second;
                if (hora.Length < 8)
                {
                    int espacios = 8 - hora.Length;
                    string espacioBlanco = new string(' ', espacios);
                    hora += espacioBlanco;
                }
            }

            //Formato rut
            string rut = pedido.RutCliente.Trim();
            if (rut.Length < 10)
            {
                int espacios = 10 - rut.Length;
                string espacioBlanco = new string('0', espacios);
                rut = espacioBlanco + rut;
            }
            //Formato código producto
            string codProducto;

            if (detalle.Codigo == null)
            {
                int espacios = 15;
                string espacioBlanco = new string(' ', espacios);
                codProducto = espacioBlanco;
            }
            else if (detalle.Codigo.Length == 15)
            {
                codProducto = detalle.Codigo;
            }
            else if (detalle.Codigo.Length < 15)
            {
                int espacios = 15 - detalle.Codigo.Length;
                string espacioBlanco = new string(' ', espacios);
                codProducto = detalle.Codigo + espacioBlanco;
            }
            else
            {
                codProducto = detalle.Codigo.Substring(0, 15);
            }
            //Formato cantidad
            string cantidad;
            string cantidadDouble = string.Format(CultureInfo.InvariantCulture, "{0:0.0}", detalle.Cantidad);
            if (cantidadDouble.Length == 6)
            {
                cantidad = cantidadDouble;
            }
            else if (cantidadDouble.Length < 6)
            {
                int espacios = 6 - cantidadDouble.Length;
                string espacioBlanco = new string(' ', espacios);
                cantidad = espacioBlanco + cantidadDouble;
            }
            else
            {
                cantidad = cantidadDouble.Substring(0, 6);
            }
            //Formato código vendedor
            string codVendedor;

            if (codVend.Length == 4)
            {
                codVendedor = codVend;
            }
            else if (codVend.Length < 4)
            {
                int cantidadCeros = 4 - codVend.Length;
                string ceros = new string('0', cantidadCeros);
                codVendedor = ceros + codVend;
            }
            else
            {
                codVendedor = codVend.Substring(0, 4);
            }

            //Formato sucursal
            string sucursal;
            if (pedido.Sucursal.Length == 4)
            {
                sucursal = pedido.Sucursal;
            }
            else if (pedido.Sucursal.Length < 4)
            {
                int cantidadCeros = 4 - pedido.Sucursal.Length;
                string ceros = new string('0', cantidadCeros);
                sucursal = ceros + pedido.Sucursal;
            }
            else
            {
                //sucursal = pedido.Sucursal.Substring(0, 4);
                sucursal = "0000";
            }

            //Formato comentario
            string comentario = string.Empty;
            

            if (pedido.ComentarioAdicional == null)
            {
                int espacios = 20;
                string espacioBlanco = new string(' ', espacios);
                comentario = espacioBlanco;
            }
            else if (pedido.ComentarioAdicional.Length == 20)
            {
                comentario = pedido.ComentarioAdicional;
            }
            else if (pedido.ComentarioAdicional.Length < 20)
            
            {
                int espacios = 20 - pedido.ComentarioAdicional.Length;
                string espacioBlanco = new string(' ', espacios);
                comentario = pedido.ComentarioAdicional + espacioBlanco;
            }
            else
            {
                comentario = pedido.ComentarioAdicional.Substring(0, 20).Trim();
            }


            //Formato descuento
            string descuento;
            switch (detalle.Descuento.ToString().Length)
            {
                case 0: descuento = "00"; break;
                case 1: descuento = "0" + detalle.Descuento.ToString(); break;
                case 2: descuento = detalle.Descuento.ToString(); break;
                default: descuento = detalle.Descuento.ToString().Substring(0, 2); break;
            }

            string id = idTemporal.ToString();
            if (id.Length == 1)
            {
                id = "0" + id;
            }
            else if (id.Length > 2)
            {
                id = id.Substring(0, 2);
            }

            //Formato tipoDocumento
            string tipoDocumento;
            if (pedido.TipoDocumento == null)
            {
                tipoDocumento = " ";
            }
            else
            {
                switch (pedido.TipoDocumento.Length)
                {
                    case 0: tipoDocumento = " "; break;
                    case 1: tipoDocumento = pedido.TipoDocumento; break;
                    default: tipoDocumento = pedido.TipoDocumento.Substring(0, 1); break;
                }
            }
            //Formato condicionPago
            string condicionPago;
            switch (pedido.CondicionPago.Length)
            {
                case 0: condicionPago = "0 "; break;
                case 1: condicionPago = pedido.CondicionPago + " "; break;
                case 2: condicionPago = pedido.CondicionPago; break;
                default: condicionPago = pedido.CondicionPago.Substring(0, 2); break;
            }

            //Crear línea
            string lineaPedido = string.Empty;
            lineaPedido += fecha;
            lineaPedido += " ";
            lineaPedido += hora;
            lineaPedido += rut;
            lineaPedido += codProducto;
            lineaPedido += cantidad;
            lineaPedido += codVendedor;
            lineaPedido += sucursal;
            lineaPedido += new string('0', 6);
            lineaPedido += descuento;
            lineaPedido += new string('0', 15);
            lineaPedido += id;
            lineaPedido += tipoDocumento;
            lineaPedido += comentario;
            lineaPedido += condicionPago;
            return lineaPedido;
        }
    }
}
