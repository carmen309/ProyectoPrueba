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
    public class CategoriasOposicionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Categorias/
        public ActionResult Index(int? page)
        {
            var categorias = db.CategoriasOposiciones.ToList().OrderBy(categoria => categoria.Nombre);
            int pageSize = Configuracion.tamañoPaginaCategoriasOposiciones;
            int pageNumber = (page ?? 1);
            return View(categorias.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Categorias/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaOposicion categoria = db.CategoriasOposiciones.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: /Categorias/Create
        public ActionResult Crear()
        {
            return View();
        }

        // POST: /Categorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include="Id,Nombre,Activa")] CategoriaOposicion categoriaoposicion)
        {
            if (ModelState.IsValid)
            {
                db.CategoriasOposiciones.Add(categoriaoposicion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriaoposicion);
        }

        // GET: /Categorias/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaOposicion categoria = db.CategoriasOposiciones.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: /Categorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include="Id,Nombre,Activa")] CategoriaOposicion categoriaoposicion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriaoposicion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriaoposicion);
        }

        // GET: /Categorias/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaOposicion categoria = db.CategoriasOposiciones.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: /Categorias/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaOposicion categoria = db.CategoriasOposiciones.Find(id);
            db.CategoriasOposiciones.Remove(categoria);
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
