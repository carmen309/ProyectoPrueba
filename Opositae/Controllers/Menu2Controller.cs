using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opositae.Models;
using System.IO;
using IdentitySample.Models;

namespace Opositae.Controllers
{
    public class Menu2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //ACESO PARA EDITAR LOS ENLACES
        public ActionResult _EditarEnlaces()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Enlaces.Where(x => x.enlacePadre == "Menu2").ToList());
        }
        //ACCESO AL LOGO
        public ActionResult _EditarLogo()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            EnlaceMejorado enlaceMejorado = db.EnlaceMejoradoes.Find(db.EnlaceMejoradoes.Where(x => x.enlace.enlacePadre == "Menu2Logo").First().EnlaceMejoradoId);
            ViewBag.imagenes = enlaceMejorado.imagen;

            if (enlaceMejorado.imagen == null) { ViewBag.imagenes = new Imagen(); }

            return View(db.EnlaceMejoradoes.Where(x => x.enlace.enlacePadre == "Menu2Logo").ToList());
        }

        

        // GET: Menu2
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: Menu2/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enlace enlace = await db.Enlaces.FindAsync(id);
            if (enlace == null)
            {
                return HttpNotFound();
            }
            return View(enlace);
        }

        // GET: Menu2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu2/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
                //SU PADRE ES MENU2
                enlace.enlacePadre = "Menu2";

                db.Enlaces.Add(enlace);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(enlace);
        }

        // GET: Menu2/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enlace enlace = await db.Enlaces.FindAsync(id);
            if (enlace == null)
            {
                return HttpNotFound();
            }
            return View(enlace);
        }

        // POST: Menu2/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
                //SU PADRE ES MENU2
                enlace.enlacePadre = "Menu2";
                db.Entry(enlace).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(enlace);
        }

        // GET: Menu2/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enlace enlace = await db.Enlaces.FindAsync(id);
            if (enlace == null)
            {
                return HttpNotFound();
            }
            return View(enlace);
        }

        // POST: Menu2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enlace enlace = await db.Enlaces.FindAsync(id);
            db.Enlaces.Remove(enlace);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Menu2/EditarLogo/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarLogo(IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                EnlaceMejorado MiLogo = db.EnlaceMejoradoes.Find(Int32.Parse(Request.Form["IdLogo"]));

                //Datos del enlace
                MiLogo.enlace.texto = Request.Form["TextoEnlace"];
                MiLogo.enlace.accion = Request.Form["AccionEnlace"];
                MiLogo.enlace.controlador = Request.Form["ControladorEnlace"];
                MiLogo.enlace.claseCss = Request.Form["CSSEnlace"];

                //Datos de la imagen
                string carpeta = db.carpetaImagenes; //Dirección donde se guardan las imagenes

                if (files.First() != null) //Eliminar imagen si ya estba guardada
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath(carpeta) + "/" + MiLogo.imagen.nombre);
                        db.Imagenes.Remove(MiLogo.imagen);
                    }
                    catch
                    {

                    }

                }
                foreach (var image in files) //Guardar imagen 
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var nombre = Path.GetFileName(image.FileName);
                        Imagen imagen = new Imagen();
                        imagen.nombre = MiLogo.GetType().Name + "_" + MiLogo.GetHashCode() + "_" + image.FileName;
                        imagen.carpeta = carpeta;
                        db.Imagenes.Add(imagen);
                        image.SaveAs(Path.Combine(Server.MapPath(carpeta), imagen.nombre));
                        MiLogo.imagen = imagen;

                    }
                }

                 //GUARDAR DATOS
                 db.Entry(MiLogo).State = EntityState.Modified;
                 await db.SaveChangesAsync();
                 return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
