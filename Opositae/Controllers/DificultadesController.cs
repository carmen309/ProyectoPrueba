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
using PagedList;

namespace Opositae.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DificultadesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Dificultades/
        public ViewResult Index(int? page)
        {
            var dificultades = db.Dificultads.ToList().OrderBy(dificultad => dificultad.Descripcion);
            int pageSize = Configuracion.tamañoPaginaDificultades;
            int pageNumber = (page ?? 1);
            return View(dificultades.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Dificultades/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dificultad dificultad = db.Dificultads.Find(id);
            if (dificultad == null)
            {
                return HttpNotFound();
            }
            return View(dificultad);
        }

        // GET: /Dificultades/Create
        public ActionResult Crear()
        {
            return View();
        }

        // POST: /Dificultades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include="Id,Descripcion,Activa")] Dificultad dificultad)
        {
            if (ModelState.IsValid)
            {
                db.Dificultads.Add(dificultad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dificultad);
        }

        // GET: /Dificultades/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dificultad dificultad = db.Dificultads.Find(id);
            if (dificultad == null)
            {
                return HttpNotFound();
            }
            return View(dificultad);
        }

        // POST: /Dificultades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include="Id,Descripcion,Activa")] Dificultad dificultad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dificultad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dificultad);
        }

        // GET: /Dificultades/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dificultad dificultad = db.Dificultads.Find(id);
            if (dificultad == null)
            {
                return HttpNotFound();
            }
            return View(dificultad);
        }

        // POST: /Dificultades/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dificultad dificultad = db.Dificultads.Find(id);
            db.Dificultads.Remove(dificultad);
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
