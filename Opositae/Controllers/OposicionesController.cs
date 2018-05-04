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
    public class OposicionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void cargaCategorias()
        {
            ViewBag.Categorias = new SelectList(db.CategoriasOposiciones, "Id", "Nombre");
        }

        public void borraRelacionesOpoCat(int idOposicion)
        {
            foreach (var item in db.OposicionesCategorias.Where(x => x.Oposicion.Id == idOposicion))
            {
                db.OposicionesCategorias.Remove(item);
            }
            db.SaveChanges();
        }

        // GET: /Oposiciones/
        public ActionResult Index(int? page)
        {
            var oposiciones = db.Oposicions.ToList().OrderBy(oposicion => oposicion.Nombre);
            int pageSize = Configuracion.tamañoPaginaOposiciones;
            int pageNumber = (page ?? 1);
            return View(oposiciones.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Oposiciones/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oposicion oposicion = db.Oposicions.Find(id);
            if (oposicion == null)
            {
                return HttpNotFound();
            }
            return View(oposicion);
        }

        // GET: /Oposiciones/Create
        public ActionResult Crear()
        {
            Oposicion opo = new Oposicion();
            opo.Activa = true;

            cargaCategorias();

            return View(opo);
        }

        // POST: /Oposiciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "Id,Nombre,Activa")] Oposicion oposicion, int[] categoriasSeleccionadas)
        {
            if (ModelState.IsValid && categoriasSeleccionadas != null)
            {
                foreach (int catId in categoriasSeleccionadas)
                {
                    OposicionesCategorias opocat = new OposicionesCategorias();
                    opocat.Categoria = db.CategoriasOposiciones.Find(catId);
                    opocat.Oposicion = oposicion;
                    oposicion.OposicionesCategorias.Add(opocat);
                }

                db.Oposicions.Add(oposicion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oposicion);
        }

        // GET: /Oposiciones/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oposicion oposicion = db.Oposicions.Find(id);
            if (oposicion == null)
            {
                return HttpNotFound();
            }
            else 
            {
                cargaCategorias();
            }
            return View(oposicion);
        }

        // POST: /Oposiciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include="Id,Nombre,Activa")] Oposicion oposicion, int[] categoriasSeleccionadas)
        {

            borraRelacionesOpoCat(oposicion.Id);

            if (ModelState.IsValid && categoriasSeleccionadas != null)
            {

                
                //Insertar las nuevas coincidencias oposiciones-categorias
                foreach (int catId in categoriasSeleccionadas)
                {
                    OposicionesCategorias opocat = new OposicionesCategorias();
                    opocat.Categoria = db.CategoriasOposiciones.Find(catId);
                    opocat.Oposicion = oposicion;
                    db.OposicionesCategorias.Add(opocat);
                }

                db.Entry(oposicion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oposicion);
        }

        // GET: /Oposiciones/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oposicion oposicion = db.Oposicions.Find(id);
            if (oposicion == null)
            {
                return HttpNotFound();
            }
            return View(oposicion);
        }

        // POST: /Oposiciones/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Oposicion oposicion = db.Oposicions.Find(id);
            borraRelacionesOpoCat(oposicion.Id);
            db.Oposicions.Remove(oposicion);
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
