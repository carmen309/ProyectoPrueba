using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Opositae.Models;
using System.IO;

namespace Opositae.Controllers
{
    public class RecursosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recursos
        public async Task<ActionResult> Index()
        {
            return View(await db.Enlaces.ToListAsync());
        }

        // GET: Recursos/Details/5
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

        // GET: Recursos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recursos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
                enlace.enlacePadre = "Recursos";

                string carpeta = @"\Content\Archivos"; //Carpeta donde se guardaran los archivos/programas

                var NuevoArchivo = Request.Files["Archivo"];
                var NombreArchivo = Path.GetFileName(NuevoArchivo.FileName);
                NuevoArchivo.SaveAs(Path.Combine(Server.MapPath(carpeta), NombreArchivo)); //Guardar Archivo en la carpeta indicada

                enlace.accion = "/Content/Archivos/"+NombreArchivo; //Guardar direccion del archivo subido
                enlace.anteEnlace = NombreArchivo; //Guardar solo el nombre del archivo por si lo queremos eliminar despues

                 db.Enlaces.Add(enlace);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(enlace);
        }

        // GET: Recursos/Edit/5
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

        // POST: Recursos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnlaceId,texto,anteEnlace,accion,controlador,idEnviado,claseCss,enlacePadre,situacion")] Enlace enlace)
        {
            if (ModelState.IsValid)
            {
                Enlace MiEnlace = db.Enlaces.Find(enlace.EnlaceId);
                MiEnlace.texto = enlace.texto;
               

                if (Request.Files["Archivo"].FileName != "" ) //Si se ha pasado algun archivo
                {
                    string carpeta = @"\Content\Archivos"; //Carpeta donde se guardaran los archivos/programas

                    //Eliminamos el anterior archivo que teniamos
                    System.IO.File.Delete(Path.Combine(Server.MapPath(carpeta), MiEnlace.anteEnlace));

                    //Guardamos el nuevo archivo
                    var NuevoArchivo = Request.Files["Archivo"];
                    var NombreArchivo = Path.GetFileName(NuevoArchivo.FileName);
                    NuevoArchivo.SaveAs(Path.Combine(Server.MapPath(carpeta), NombreArchivo)); //Guardar Archivo en la carpeta indicada

                    MiEnlace.accion = "/Content/Archivos/" + NombreArchivo; //Guardar direccion del archivo subido
                    MiEnlace.anteEnlace = NombreArchivo;

                    db.Entry(MiEnlace).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                else
                {

                    db.Entry(MiEnlace).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                }

                return RedirectToAction("Index");
            }
            return View(enlace);
        }

        // GET: Recursos/Delete/5
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

        // POST: Recursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enlace enlace = await db.Enlaces.FindAsync(id);

            string carpeta = @"\Content\Archivos"; //Carpeta donde se guardaran los archivos/programas
            System.IO.File.Delete(Path.Combine(Server.MapPath(carpeta), enlace.anteEnlace));//Eliminamos tambien el archivo

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
