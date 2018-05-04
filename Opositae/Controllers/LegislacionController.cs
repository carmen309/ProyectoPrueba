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
    public class LegislacionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //CARGA LAS NORMAS EN INDEX TENIENDO EN CUENTA LA PAGINACIÓN
        public IPagedList<Legislacion> cargaLegislacion(int? page)
        {
            int pageSize = Configuracion.tamañoPaginaLegislacion;
            int pageNumber = (page ?? 1);
            IOrderedEnumerable<Legislacion> leg;
            leg = db.Legislacions.ToList().OrderBy(legislacion => legislacion.Norma);

            return leg.ToPagedList(pageNumber, pageSize);
        }

        public void cargaCategorias(Legislacion leg)
        {
            if (leg == null) { 
                ViewBag.Categorias = new SelectList(db.CategoriasOposiciones, "Id", "Nombre");
            }
            else { 
                var categoriasDB = db.CategoriasOposiciones.ToList();
                SelectList selecCategorias = new SelectList(categoriasDB, "Id", "Nombre");
                IEnumerable<SelectListItem> envio = selecCategorias.Select(x => new SelectListItem()
                {
                    Selected = (x.Value == leg.Categoria.Id.ToString()),
                    Text = x.Text,
                    Value = x.Value
                });
                ViewBag.Categorias = envio;
            }
        }

        // GET: /Legislacion/
        public ViewResult Index(int? page)
        {
            IPagedList<Legislacion> legislacion = cargaLegislacion(page);

            return View(legislacion);
        }

        // GET: /Legislacion/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Legislacion legislacion = db.Legislacions.Find(id);
            if (legislacion == null)
            {
                return HttpNotFound();
            }
            return View(legislacion);
        }

        // GET: /Legislacion/Create
        public ActionResult Crear()
        {
            cargaCategorias(null);
            return View();
        }

        // POST: /Legislacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include="Id,Norma,Enlace")] Legislacion legislacion, string Categorias)
        {
            if (ModelState.IsValid)
            {
                legislacion.Categoria = db.CategoriasOposiciones.Find(Convert.ToInt16(Categorias));
                db.Legislacions.Add(legislacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(legislacion);
        }

        // GET: /Legislacion/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Legislacion legislacion = db.Legislacions.Find(id);
            if (legislacion == null)
            {
                return HttpNotFound();
            }
            cargaCategorias(legislacion);
            return View(legislacion);
        }

        // POST: /Legislacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include="Id,Norma,Enlace")] Legislacion legislacion, string Categorias)
        {
            if (ModelState.IsValid)
            {
                legislacion.CategoriaId = Convert.ToInt16(Categorias);
                db.Entry(legislacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(legislacion);
        }

        // GET: /Legislacion/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Legislacion legislacion = db.Legislacions.Find(id);
            if (legislacion == null)
            {
                return HttpNotFound();
            }
            return View(legislacion);
        }

        // POST: /Legislacion/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Legislacion legislacion = db.Legislacions.Find(id);
            db.Legislacions.Remove(legislacion);
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
