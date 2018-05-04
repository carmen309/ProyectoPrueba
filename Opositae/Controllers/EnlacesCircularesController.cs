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
using IdentitySample.Models;

namespace Opositae.Controllers
{
    public class EnlacesCircularesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EnlacesCirculares
        public ActionResult Index()
        {
            return View(db.Enlaces.Where(x => x.enlacePadre == "EnlacesCirculares").ToList());
        }

        // GET: EnlacesCirculares/Details/5
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

        // GET: EnlacesCirculares/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnlacesCirculares/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
                

                enlace.enlacePadre = "EnlacesCirculares"; //Si creas un enlace en esta página quieres que sea un EnlaceCircular 
 
                db.Enlaces.Add(enlace);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(enlace);
        }

        // GET: EnlacesCirculares/Edit/5
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

        // POST: EnlacesCirculares/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
                enlace.enlacePadre = "EnlacesCirculares"; 

                db.Entry(enlace).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(enlace);
        }

        // GET: EnlacesCirculares/Delete/5
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

        // POST: EnlacesCirculares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enlace enlace = await db.Enlaces.FindAsync(id);
            db.Enlaces.Remove(enlace);
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
