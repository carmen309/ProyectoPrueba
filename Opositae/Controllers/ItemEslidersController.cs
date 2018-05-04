using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Opositae.Models;
using IdentitySample.Models;

namespace BackEndGestion.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ItemEslidersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ItemEsliders
        public ActionResult Index()
        {
            ViewBag.padre = "Slideshow";
            return View(db.ItemEsliders.ToList());
        }

        // GET: ItemEsliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ItemEslider itemEslider = db.ItemEsliders.Find(id);
            ViewBag.imagenes = itemEslider.imagenSlider;

            if (itemEslider == null)
            {
                return HttpNotFound();
            }
            return View(itemEslider);
        }

        // GET: ItemEsliders/Create
        public ActionResult Create(string id)
        {
            ViewBag.padre = id;
            return View();
        }

        // POST: ItemEsliders/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace, [Bind(Include = "ItemEsliderId,titulo,subtitulo,texto")] ItemEslider itemEslider, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {

                itemEslider.enlace = enlace;

                foreach (var image in files)
                {
                    if (image != null && image.ContentLength > 0)
                    {

                        Imagen NuevaImagen = new Imagen();
                        NuevaImagen.GuardarImagen(image,db.carpetaImagenes,"Slider");
                        db.Imagenes.Add(NuevaImagen);
                        itemEslider.imagenSlider = NuevaImagen;
                    }
                }




              /*  string carpeta = db.carpetaImagenes;

                foreach (var image in files)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var nombre = Path.GetFileName(image.FileName);
                        Imagen imagen = new Imagen();
                        imagen.nombre = itemEslider.titulo+"_"+nombre; //Cambiaremos el nombre del archivo
                        imagen.carpeta = carpeta;
                        db.Imagenes.Add(imagen);
                        image.SaveAs(Path.Combine(Server.MapPath(carpeta), imagen.nombre));
                        itemEslider.imagenSlider = imagen;

                    }
                }*/

               
                db.ItemEsliders.Add(itemEslider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.padre = "Slideshow";
            return View(itemEslider);
        }

        // GET: ItemEsliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ItemEslider itemEslider = db.ItemEsliders.Find(id);
            ViewBag.imagenes = itemEslider.imagenSlider;
            
            if (itemEslider.imagenSlider == null) { ViewBag.imagenes = new Imagen(); }

            ViewBag.idit = id;
            if (itemEslider == null)
            {
                return HttpNotFound();
            }
            return View(itemEslider);
        }

        // POST: ItemEsliders/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnlaceId,texto ")] Enlace enlace, [Bind(Include = "ItemEsliderId,titulo,subtitulo,texto")] ItemEslider pitemEslider, IEnumerable<HttpPostedFileBase> files)
        {
            if (pitemEslider == null)
            {
                return RedirectToAction("index", "itemEsliders");
            }
            ItemEslider itemEslider = db.ItemEsliders.Find(pitemEslider.ItemEsliderId);
            if (ModelState.IsValid)
            {
                itemEslider.enlace = enlace;
                itemEslider.subtitulo = pitemEslider.subtitulo;
                itemEslider.titulo = pitemEslider.titulo;
                itemEslider.texto = pitemEslider.texto;
                string carpeta = db.carpetaImagenes;
                if (files.First() != null)
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath(carpeta) + "/" + itemEslider.imagenSlider.nombre);
                        db.Imagenes.Remove(itemEslider.imagenSlider);
                    }
                    catch
                    {

                    }

                }
                foreach (var image in files)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var nombre = Path.GetFileName(image.FileName);
                        Imagen imagen = new Imagen();
                        imagen.nombre = itemEslider.titulo + "_" + nombre; //Cambiaremos el nombre del archivo
                        imagen.carpeta = carpeta;
                        db.Imagenes.Add(imagen);
                        image.SaveAs(Path.Combine(Server.MapPath(carpeta), imagen.nombre));
                        itemEslider.imagenSlider = imagen;

                    }
                }

                db.Entry(itemEslider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemEslider);
        }

        // GET: ItemEsliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemEslider itemEslider = db.ItemEsliders.Find(id);
            ViewBag.imagenes = itemEslider.imagenSlider;
            if (itemEslider == null)
            {
                return HttpNotFound();
            }
            return View(itemEslider);
        }

        // POST: ItemEsliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemEslider itemEslider = db.ItemEsliders.Find(id);
            if (itemEslider.imagenSlider != null)
            {
                System.IO.File.Delete(Server.MapPath(db.carpetaImagenes) + "/" + itemEslider.imagenSlider.nombre);
                db.Imagenes.Remove(itemEslider.imagenSlider);
                db.Enlaces.Remove(itemEslider.enlace);
            }
            db.ItemEsliders.Remove(itemEslider);
            db.SaveChanges();
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
