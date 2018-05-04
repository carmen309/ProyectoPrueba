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
    public class ConvocatoriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //CARGA LAS CONVOCATORIAS EN INDEX TENIENDO EN CUENTA LA PAGINACIÓN
        public IPagedList<Convocatoria> cargaConvocatorias(int? page)
        {
            int pageSize = Configuracion.tamañoPaginaConvocatorias;
            int pageNumber = (page ?? 1);
            IOrderedEnumerable<Convocatoria> conv;
            conv = db.Convocatorias.ToList().OrderByDescending(convocatoria => convocatoria.Fecha);

            return conv.ToPagedList(pageNumber, pageSize);
        }

        public void cargaTiposAcceso(Convocatoria conv) {
            List<SelectListItem> acc = new List<SelectListItem>();
            if (conv == null)
            {
                
                acc.Add(new SelectListItem() { Text = "Libre", Value = "Libre" });
                acc.Add(new SelectListItem() { Text = "Concurso oposición", Value = "Concurso oposición" });
                acc.Add(new SelectListItem() { Text = "Oposición", Value = "Oposición" });
            }
            else {
                acc.Add(new SelectListItem() { Selected = (conv.Acceso=="Libre"), Text = "Libre", Value = "Libre" });
                acc.Add(new SelectListItem() { Selected = (conv.Acceso == "Concurso oposición"), Text = "Concurso oposición", Value = "Concurso oposición" });
                acc.Add(new SelectListItem() { Selected = (conv.Acceso == "Oposición"), Text = "Oposición", Value = "Oposición" });
            }
            ViewBag.Acceso = acc;
        }

        // GET: /Convocatorias/
        public ViewResult Index(int? page)
        {
            IPagedList<Convocatoria> convocatorias = cargaConvocatorias(page);
            
            return View(convocatorias);
        }

        // GET: /Convocatorias/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convocatoria convocatoria = db.Convocatorias.Find(id);
            if (convocatoria == null)
            {
                return HttpNotFound();
            }
            return View(convocatoria);
        }

        // GET: /Convocatorias/Create
        public ActionResult Crear()
        {
            cargaTiposAcceso(null);

            return View();
        }

        // POST: /Convocatorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include="Id,Fecha,Descripcion,Entidad,Acceso,Plazas,Grupo,Enlace,Activa")] Convocatoria convocatoria)
        {
            if (ModelState.IsValid)
            {
                db.Convocatorias.Add(convocatoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(convocatoria);
        }

        // GET: /Convocatorias/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convocatoria convocatoria = db.Convocatorias.Find(id);
            if (convocatoria == null)
            {
                return HttpNotFound();
            }
            cargaTiposAcceso(convocatoria);
            return View(convocatoria);
        }

        // POST: /Convocatorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include="Id,Fecha,Descripcion,Entidad,Acceso,Plazas,Grupo,Enlace,Activa")] Convocatoria convocatoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(convocatoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(convocatoria);
        }

        // GET: /Convocatorias/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convocatoria convocatoria = db.Convocatorias.Find(id);
            if (convocatoria == null)
            {
                return HttpNotFound();
            }
            return View(convocatoria);
        }

        // POST: /Convocatorias/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Convocatoria convocatoria = db.Convocatorias.Find(id);
            db.Convocatorias.Remove(convocatoria);
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
