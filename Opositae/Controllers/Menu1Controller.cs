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
    

    public class Menu1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menu1
        public async Task<ActionResult> Index()
        {


            IEnumerable<Enlace> MisEnlaces = db.Enlaces.Where(x => x.enlacePadre == "Menu1").ToList();

            if (MisEnlaces.Count() == 0)
            {
                return HttpNotFound();
            }
           
            return View("Editar",MisEnlaces);
        }

        // POST: Menu1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {

            if (ModelState.IsValid)
            {
                //EDITAR CORREO
                Enlace Correo = db.Enlaces.Find(Int32.Parse(Request.Form["IdCorreo"]));
                Correo.claseCss = Request.Form["CSSCorreo"];
                Correo.texto = Request.Form["TextoCorreo"];
                Correo.controlador = Request.Form["ControladorCorreo"];
                Correo.accion = Request.Form["AccionCorreo"];
                db.Entry(Correo).State = EntityState.Modified;

                //EDITAR ENLACE1
                Enlace Enlace1 = db.Enlaces.Find(Int32.Parse(Request.Form["IdEnlace1"]));
                Enlace1.claseCss = Request.Form["CSSEnlace1"];
                Enlace1.controlador = Request.Form["ControladorEnlace1"];
                Enlace1.accion = Request.Form["AccionEnlace1"];
                db.Entry(Enlace1).State = EntityState.Modified;

                //EDITAR ENLACE2
                Enlace Enlace2 = db.Enlaces.Find(Int32.Parse(Request.Form["IdEnlace2"]));
                Enlace2.claseCss = Request.Form["CSSEnlace2"];
                Enlace2.controlador = Request.Form["ControladorEnlace2"];
                Enlace2.accion = Request.Form["AccionEnlace2"];
                db.Entry(Enlace2).State = EntityState.Modified;

                //EDITAR ENLACE3
                Enlace Enlace3 = db.Enlaces.Find(Int32.Parse(Request.Form["IdEnlace3"]));
                Enlace3.claseCss = Request.Form["CSSEnlace3"];
                Enlace3.controlador = Request.Form["ControladorEnlace3"];
                Enlace3.accion = Request.Form["AccionEnlace3"];
                db.Entry(Enlace3).State = EntityState.Modified;

                //EDITAR ENLACE LOGIN
                Enlace EnlaceLogin = db.Enlaces.Find(Int32.Parse(Request.Form["IdEnlaceLogin"]));
                EnlaceLogin.claseCss = Request.Form["CSSEnlaceLogin"];
                EnlaceLogin.texto = Request.Form["TextoEnlaceLogin"];
                EnlaceLogin.controlador = Request.Form["ControladorEnlaceLogin"];
                EnlaceLogin.accion = Request.Form["AccionEnlaceLogin"];
                db.Entry(EnlaceLogin).State = EntityState.Modified;

                //EDITAR ENLACE REGISTRO
                Enlace EnlaceRegistro = db.Enlaces.Find(Int32.Parse(Request.Form["IdEnlaceRegistro"]));
                EnlaceRegistro.claseCss = Request.Form["CSSEnlaceRegistro"];
                EnlaceRegistro.texto = Request.Form["TextoEnlaceRegistro"];
                EnlaceRegistro.controlador = Request.Form["ControladorEnlaceRegistro"];
                EnlaceRegistro.accion = Request.Form["AccionEnlaceRegistro"];
                db.Entry(EnlaceRegistro).State = EntityState.Modified;

                db.SaveChanges();//GUARDAR CAMBIOS
                return RedirectToAction("Index"); //VOLVER AL INDEX

            }

            return RedirectToAction("Index");

        }


        protected override void Dispose(bool disposing) //Elimina la basura
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
