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
    public class PiePaginaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PiePagina
        public async Task<ActionResult> Index()
        {
            return View(await db.Enlaces.Where(x => x.enlacePadre == "Pie").ToListAsync());
        }

        // GET: PiePagina/Details/5
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

        // GET: PiePagina/Create
        public ActionResult Create()
        {

           List<SelectListItem> Secciones = new List<SelectListItem>();
            Secciones.Add(new SelectListItem() { Text = "OPOSITAE", Value = "OPOSITAE" });
            Secciones.Add(new SelectListItem() { Text = "ULTIMAS CONVOCATORIAS", Value = "ULTIMAS CONVOCATORIAS" });
            Secciones.Add(new SelectListItem() { Text = "NUESTRAS REDES SOCIALES", Value = "NUESTRAS REDES SOCIALES" });

            ViewBag.Secciones = Secciones;


            return View();
        }

        // POST: PiePagina/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
            
                enlace.enlacePadre = "Pie";
                enlace.anteEnlace = Request.Form["Secciones"];



                db.Enlaces.Add(enlace);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(enlace);
        }

        // GET: PiePagina/Edit/5
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
            List<SelectListItem> Secciones = new List<SelectListItem>();

            Secciones.Add(new SelectListItem() { Text = "OPOSITAE", Value = "OPOSITAE" });
            Secciones.Add(new SelectListItem() { Text = "ULTIMAS CONVOCATORIAS", Value = "ULTIMAS CONVOCATORIAS" });
            Secciones.Add(new SelectListItem() { Text = "NUESTRAS REDES SOCIALES", Value = "NUESTRAS REDES SOCIALES"});

            foreach(var item in Secciones) //MANTENER SELECCIONADA SU SECCIÓN
            {
                if(item.Value == enlace.anteEnlace )
                {
                    item.Selected = true;
                }
            }

            ViewBag.Secciones = Secciones;
            return View(enlace);
        }

        // POST: PiePagina/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {

                enlace.enlacePadre = "Pie";
                enlace.anteEnlace = Request.Form["Secciones"];

                db.Entry(enlace).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(enlace);
        }

        // GET: PiePagina/Delete/5
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

        // POST: PiePagina/Delete/5
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
