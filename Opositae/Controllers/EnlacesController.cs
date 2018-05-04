using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Opositae.Models;

namespace BackEndGestion.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnlacesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Enlaces
        public ActionResult Index()
        {
            return View(db.Enlaces.ToList());
        }


        // GET: Enlaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enlace enlace = db.Enlaces.Find(id);
            if (enlace == null)
            {
                return HttpNotFound();
            }
            return View(enlace);
        }

        // GET: Enlaces/Create
        public ActionResult Create(string id)
        {
            if (id != null)
            {
                ViewBag.padre = id;
            }
            return View();
        }

        // POST: Enlaces/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (db.Enlaces.Where(x => x.enlacePadre == enlace.enlacePadre && x.situacion == enlace.situacion).Count() > 0)
            {
                ModelState.AddModelError("situacion", "Ya existe un enlace de esta caja en la posición " + enlace.situacion);
            }
            if (ModelState.IsValid)
            {
                db.Enlaces.Add(enlace);
                db.SaveChanges();
                return RedirectToAction("Menu", "Menus", new { id = enlace.enlacePadre });
            }
            ViewBag.padre = enlace.enlacePadre;
            return View(enlace);
        }

        // GET: Enlaces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enlace enlace = db.Enlaces.Find(id);
            if (enlace == null)
            {
                return HttpNotFound();
            }
            return View(enlace);
        }

        // POST: Enlaces/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (db.Enlaces.Where(x => x.enlacePadre == enlace.enlacePadre && x.situacion == enlace.situacion && x.EnlaceId != enlace.EnlaceId).Count() > 0)
            {
                ModelState.AddModelError("situacion", "Ya existe un enlace de esta caja en la posición " + enlace.situacion);
            }
            if (ModelState.IsValid)
            {
                db.Entry(enlace).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Menu", "Menus", new { id = enlace.enlacePadre });
            }
            return View(enlace);
        }

        // GET: Enlaces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enlace enlace = db.Enlaces.Find(id);
            if (enlace == null)
            {
                return HttpNotFound();
            }
            return View(enlace);
        }

        // POST: Enlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enlace enlace = db.Enlaces.Find(id);
            db.Enlaces.Remove(enlace);
            db.SaveChanges();
            return RedirectToAction("Menu", "Menus", new { id = enlace.enlacePadre });
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
