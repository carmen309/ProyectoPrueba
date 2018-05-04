using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opositae.Models;
using System.IO;
using IdentitySample.Models;

namespace Opositae.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NosotrosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void cargaIconosTips(Nosotros nosotros) {
            string ic1 = "fa-pencil-square-o";
            string ic2 = "fa-compress";
            string ic3 = "fa-paper-plane";
            string ic4 = "fa-folder-open";
            string ic5 = "fa-user";
            string ic6 = "fa-envelope-o";

            List<SelectListItem> icons1 = new List<SelectListItem>();
            icons1.Add(new SelectListItem() { Selected = (nosotros.Icono1 == ic1), Text = "Lápiz y cuadrado", Value = ic1 });
            icons1.Add(new SelectListItem() { Selected = (nosotros.Icono1 == ic2), Text = "Flechas apuntándose", Value = ic2 });
            icons1.Add(new SelectListItem() { Selected = (nosotros.Icono1 == ic3), Text = "Avión de papel", Value = ic3 });
            icons1.Add(new SelectListItem() { Selected = (nosotros.Icono1 == ic4), Text = "Carpeta abierta", Value = ic4 });
            icons1.Add(new SelectListItem() { Selected = (nosotros.Icono1 == ic5), Text = "Usuario", Value = ic5 });
            icons1.Add(new SelectListItem() { Selected = (nosotros.Icono1 == ic6), Text = "Carta", Value = ic6 });

            List<SelectListItem> icons2 = new List<SelectListItem>();
            icons2.Add(new SelectListItem() { Selected = (nosotros.Icono2 == ic1), Text = "Lápiz y cuadrado", Value = ic1 });
            icons2.Add(new SelectListItem() { Selected = (nosotros.Icono2 == ic2), Text = "Flechas apuntándose", Value = ic2 });
            icons2.Add(new SelectListItem() { Selected = (nosotros.Icono2 == ic3), Text = "Avión de papel", Value = ic3 });
            icons2.Add(new SelectListItem() { Selected = (nosotros.Icono2 == ic4), Text = "Carpeta abierta", Value = ic4 });
            icons2.Add(new SelectListItem() { Selected = (nosotros.Icono2 == ic5), Text = "Usuario", Value = ic5 });
            icons2.Add(new SelectListItem() { Selected = (nosotros.Icono2 == ic6), Text = "Carta", Value = ic6 });

            List<SelectListItem> icons3 = new List<SelectListItem>();
            icons3.Add(new SelectListItem() { Selected = (nosotros.Icono3 == ic1), Text = "Lápiz y cuadrado", Value = ic1 });
            icons3.Add(new SelectListItem() { Selected = (nosotros.Icono3 == ic2), Text = "Flechas apuntándose", Value = ic2 });
            icons3.Add(new SelectListItem() { Selected = (nosotros.Icono3 == ic3), Text = "Avión de papel", Value = ic3 });
            icons3.Add(new SelectListItem() { Selected = (nosotros.Icono3 == ic4), Text = "Carpeta abierta", Value = ic4 });
            icons3.Add(new SelectListItem() { Selected = (nosotros.Icono3 == ic5), Text = "Usuario", Value = ic5 });
            icons3.Add(new SelectListItem() { Selected = (nosotros.Icono3 == ic6), Text = "Carta", Value = ic6 });

            List<SelectListItem> icons4 = new List<SelectListItem>();
            icons4.Add(new SelectListItem() { Selected = (nosotros.Icono4 == ic1), Text = "Lápiz y cuadrado", Value = ic1 });
            icons4.Add(new SelectListItem() { Selected = (nosotros.Icono4 == ic2), Text = "Flechas apuntándose", Value = ic2 });
            icons4.Add(new SelectListItem() { Selected = (nosotros.Icono4 == ic3), Text = "Avión de papel", Value = ic3 });
            icons4.Add(new SelectListItem() { Selected = (nosotros.Icono4 == ic4), Text = "Carpeta abierta", Value = ic4 });
            icons4.Add(new SelectListItem() { Selected = (nosotros.Icono4 == ic5), Text = "Usuario", Value = ic5 });
            icons4.Add(new SelectListItem() { Selected = (nosotros.Icono4 == ic6), Text = "Carta", Value = ic6 });

            List<SelectListItem> icons5 = new List<SelectListItem>();
            icons5.Add(new SelectListItem() { Selected = (nosotros.Icono5 == ic1), Text = "Lápiz y cuadrado", Value = ic1 });
            icons5.Add(new SelectListItem() { Selected = (nosotros.Icono5 == ic2), Text = "Flechas apuntándose", Value = ic2 });
            icons5.Add(new SelectListItem() { Selected = (nosotros.Icono5 == ic3), Text = "Avión de papel", Value = ic3 });
            icons5.Add(new SelectListItem() { Selected = (nosotros.Icono5 == ic4), Text = "Carpeta abierta", Value = ic4 });
            icons5.Add(new SelectListItem() { Selected = (nosotros.Icono5 == ic5), Text = "Usuario", Value = ic5 });
            icons5.Add(new SelectListItem() { Selected = (nosotros.Icono5 == ic6), Text = "Carta", Value = ic6 });

            List<SelectListItem> icons6 = new List<SelectListItem>();
            icons6.Add(new SelectListItem() { Selected = (nosotros.Icono6 == ic1), Text = "Lápiz y cuadrado", Value = ic1 });
            icons6.Add(new SelectListItem() { Selected = (nosotros.Icono6 == ic2), Text = "Flechas apuntándose", Value = ic2 });
            icons6.Add(new SelectListItem() { Selected = (nosotros.Icono6 == ic3), Text = "Avión de papel", Value = ic3 });
            icons6.Add(new SelectListItem() { Selected = (nosotros.Icono6 == ic4), Text = "Carpeta abierta", Value = ic4 });
            icons6.Add(new SelectListItem() { Selected = (nosotros.Icono6 == ic5), Text = "Usuario", Value = ic5 });
            icons6.Add(new SelectListItem() { Selected = (nosotros.Icono6 == ic6), Text = "Carta", Value = ic6 });

            ViewBag.Icono1 = icons1;
            ViewBag.Icono2 = icons2;
            ViewBag.Icono3 = icons3;
            ViewBag.Icono4 = icons4;
            ViewBag.Icono5 = icons5;
            ViewBag.Icono6 = icons6;
        }



        // GET: /Nosotros/
        
        public ActionResult Index()
        {
            string carpeta = db.carpetaImagenes;
            Nosotros nosotros = db.Nosotros.Find(1);
            if (nosotros == null)
            {
                return HttpNotFound();
            }
            cargaIconosTips(nosotros);
            ViewBag.imagePath = carpeta+"/"+nosotros.Imagen;
            ViewBag.imageImagenFondoTipsPath = carpeta + "/" + nosotros.ImagenFondoTips;
            return View("Editar", nosotros);
        }
        public ActionResult UploadFiles()
        {
            Nosotros nosotros = db.Nosotros.Find(1);
            if (nosotros == null)
            {
                return HttpNotFound();
            }
            cargaIconosTips(nosotros);

            return View("_UploadFiles", nosotros);
        }

        // POST: /Nosotros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Index([Bind(Include = "Id,Titulo,Texto,Imagen,ImagenFondoTips,Icono1,Tip1,Icono2,Tip2,Icono3,Tip3,Icono4,Tip4,Icono5,Tip5,Icono6,Tip6")] Nosotros nosotros, HttpPostedFileBase Imagenr, HttpPostedFileBase ImagenFondoTipsr)
        {
            string carpeta = db.carpetaImagenes;
            if (ModelState.IsValid)
            {
                    if (Imagenr != null && Imagenr.ContentLength > 0)
                    {
                    var pathEliminar = Path.Combine(Server.MapPath("/" + carpeta), " " + nosotros.Imagen);
                    try
                    {
                        System.IO.File.Delete(pathEliminar);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    var fileName = Path.GetFileName(Imagenr.FileName);
                    var path = Path.Combine(Server.MapPath("/"+carpeta), "ima_" + Imagenr.FileName);
                    nosotros.Imagen = "/ima_"+ Imagenr.FileName;
                    Imagenr.SaveAs(path);
                }
                if (ImagenFondoTipsr != null && ImagenFondoTipsr.ContentLength > 0)
                {
                    var pathEliminar = Path.Combine(Server.MapPath("/" + carpeta), " " + nosotros.ImagenFondoTips);
                    try
                    {
                        System.IO.File.Delete(pathEliminar);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    var fileName = Path.GetFileName(ImagenFondoTipsr.FileName);
                    var path = Path.Combine(Server.MapPath("/"+carpeta), "imaFondo_" + ImagenFondoTipsr.FileName);
                    nosotros.ImagenFondoTips="/imaFondo_"+ImagenFondoTipsr.FileName; 
                    ImagenFondoTipsr.SaveAs(path);
                }
              
                db.Entry(nosotros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nosotros);
        }
        public ActionResult subirAjax(string id="")
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                var fullPath = Path.Combine(Server.MapPath("/" + db.carpetaImagenes), "imaFondo_" + file.FileName);
                file.SaveAs(fullPath);
            }

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
