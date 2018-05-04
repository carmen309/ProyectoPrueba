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
    public class CategoriasBlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /CategoriasNoticias/
        public ActionResult Index(int? page)
        {
            var catblog = db.CategoriasBlog.ToList().OrderBy(categoriablog => categoriablog.Nombre);
            int pageSize = Configuracion.tamañoPaginaCategoriasBlog;
            int pageNumber = (page ?? 1);
            return View(catblog.ToPagedList(pageNumber, pageSize));
        }

        // GET: /CategoriasNoticias/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaBlog categoriablog = db.CategoriasBlog.Find(id);
            if (categoriablog == null)
            {
                return HttpNotFound();
            }
            return View(categoriablog);
        }

        // GET: /CategoriasNoticias/Create
        public ActionResult Crear()
        {
            return View();
        }

        // POST: /CategoriasNoticias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "Id,Nombre,Activa")] CategoriaBlog categoriablog)
        {
            if (ModelState.IsValid)
            {
                db.CategoriasBlog.Add(categoriablog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriablog);
        }

        // GET: /CategoriasNoticias/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaBlog categoriablog = db.CategoriasBlog.Find(id);
            if (categoriablog == null)
            {
                return HttpNotFound();
            }
            return View(categoriablog);
        }

        // POST: /CategoriasNoticias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include="Id,Nombre,Activa")] CategoriaBlog categoriablog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriablog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriablog);
        }

        // GET: /CategoriasNoticias/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaBlog categorianoticia = db.CategoriasBlog.Find(id);
            if (categorianoticia == null)
            {
                return HttpNotFound();
            }
            return View(categorianoticia);
        }

        // POST: /CategoriasNoticias/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaBlog categoriablog = db.CategoriasBlog.Find(id);
            db.CategoriasBlog.Remove(categoriablog);
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
