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
    public class EnlacesMejoradosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EnlacesMejorados
        public ActionResult Index()
        {
            return View(db.EnlaceMejoradoes.Where(x => x.enlace.enlacePadre == "CuadroPublicitario").ToList());
        }

        // GET: EnlacesMejorados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnlaceMejorado enlaceMejorado = db.EnlaceMejoradoes.Find(id);
            ViewBag.imagenes = enlaceMejorado.imagen;

            if (enlaceMejorado == null)
            {
                return HttpNotFound();
            }
            return View(enlaceMejorado);
        }

        // GET: EnlacesMejorados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnlacesMejorados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnlaceMejoradoId")] EnlaceMejorado enlaceMejorado, [Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {

                enlaceMejorado.enlace = enlace;

                string carpeta = db.carpetaImagenes; //Dirección donde se guardan las imagenes

                foreach (var image in files)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var nombre = Path.GetFileName(image.FileName);
                        Imagen imagen = new Imagen();
                        imagen.nombre = enlace.texto + "_" + nombre; //Cambiaremos el nombre del archivo
                        imagen.carpeta = carpeta;
                        db.Imagenes.Add(imagen);
                        image.SaveAs(Path.Combine(Server.MapPath(carpeta), imagen.nombre));
                        enlaceMejorado.imagen = imagen;

                    }
                }

                db.EnlaceMejoradoes.Add(enlaceMejorado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(enlaceMejorado);
        }

        // GET: EnlacesMejorados/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnlaceMejorado enlaceMejorado = db.EnlaceMejoradoes.Find(id);
            ViewBag.imagenes = enlaceMejorado.imagen;

            if (enlaceMejorado.imagen == null) { ViewBag.imagenes = new Imagen(); }

            ViewBag.idit = id;

            if (enlaceMejorado == null)
            {
                return HttpNotFound();
            }
            return View(enlaceMejorado);
        }

        // POST: EnlacesMejorados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnlaceMejoradoId")] EnlaceMejorado enlaceMejorado , [Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace , IEnumerable<HttpPostedFileBase> files)
        {
            if (enlaceMejorado == null)
            {
                return RedirectToAction("index", "EnlacesMejorados");
            }
          
          
            if (ModelState.IsValid)
            {

                if (enlace.situacion == 10)//Si el enlace mejorado es el cuadro publicitario....
                {
                    enlaceMejorado.enlace = enlace; //Guardar datos del enlace 
                    enlaceMejorado.enlace.enlacePadre = "CuadroPublicitario"; //Sera un cuadro publicitario
                }
                else
                {
                    enlaceMejorado.enlace = enlace; //Guardar datos del enlace 
                }

               

                string carpeta = db.carpetaImagenes; //Direccion donde se guardan las imagenes

                if (files.First() != null) //Eliminar imagen si ya estba guardada
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath(carpeta) + "/" + enlaceMejorado.imagen.nombre);
                        db.Imagenes.Remove(enlaceMejorado.imagen);
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
                        imagen.nombre = "EnlaceMejorado_"+ enlaceMejorado.GetHashCode()+ nombre; //Cambiaremos el nombre del archivo
                        imagen.carpeta = carpeta;
                        db.Imagenes.Add(imagen);
                        image.SaveAs(Path.Combine(Server.MapPath(carpeta), imagen.nombre));
                        enlaceMejorado.imagen = imagen;

                    }
                }

                db.Entry(enlace).State = EntityState.Modified;
                db.Entry(enlaceMejorado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enlaceMejorado);
        }

        // GET: EnlacesMejorados/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnlaceMejorado enlaceMejorado = await db.EnlaceMejoradoes.FindAsync(id);
            ViewBag.imagenes = enlaceMejorado.imagen;
            if (enlaceMejorado == null)
            {
                return HttpNotFound();
            }
            return View(enlaceMejorado);
        }

        // POST: EnlacesMejorados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EnlaceMejorado enlaceMejorado = await db.EnlaceMejoradoes.FindAsync(id);
            if (enlaceMejorado.imagen != null)
            {
                System.IO.File.Delete(Server.MapPath(db.carpetaImagenes) + "/" + enlaceMejorado.imagen.nombre);
                db.Imagenes.Remove(enlaceMejorado.imagen);
                db.Enlaces.Remove(enlaceMejorado.enlace);
            }
            db.EnlaceMejoradoes.Remove(enlaceMejorado);
            await db.SaveChangesAsync();
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
