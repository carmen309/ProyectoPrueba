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
using Microsoft.AspNet.Identity;

namespace Opositae.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContactosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contactos
        public async Task<ActionResult> Index()
        {
            return View(await db.Contactoes.FirstAsync());
        }

        public ActionResult _EditarCuadrosContactos()
        {
            return View( db.Enlaces.Where(x => x.enlacePadre == "Contactos").ToList());
        }

        // GET: Contactos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = await db.Contactoes.FindAsync(id);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // GET: Contactos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contactos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Direccion,Correo,Mapa,RS_Facebook,RS_Twitter,RS_GooglePlus")] Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                
                db.Contactoes.Add(contacto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contacto);
        }

        // GET: Contactos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = await db.Contactoes.FindAsync(id);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // POST: Contactos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Direccion,Correo,Mapa,RS_Facebook,RS_Twitter,RS_GooglePlus")] Contacto contacto)
        {
           

            if (ModelState.IsValid)
            {
               
                db.Entry(contacto).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            var errores = ModelState.Values;
            if (contacto.Id == 0)
            {
                db.Contactoes.Add(contacto);
                db.SaveChangesAsync();
            }
            return View(contacto);
        }

        // GET: Contactos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = await db.Contactoes.FindAsync(id);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // POST: Contactos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contacto contacto = await db.Contactoes.FindAsync(id);
            db.Contactoes.Remove(contacto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarCuadrosContactos()
        {
            if (ModelState.IsValid)
            {
               
                //EDITAR CUADRO1
                Enlace C1 = db.Enlaces.Find(Int32.Parse(Request.Form["IdC1"]));
                C1.anteEnlace = Request.Form["TituloC1"];
                C1.texto = Request.Form["TextoC1"];
                C1.claseCss = Request.Form["CSSC1"];
                db.Entry(C1).State = EntityState.Modified;

                //EDITAR CUADRO2
                Enlace C2 = db.Enlaces.Find(Int32.Parse(Request.Form["IdC2"]));
                C2.anteEnlace = Request.Form["TituloC2"];
                C2.texto = Request.Form["TextoC2"];
                C2.claseCss = Request.Form["CSSC2"];
                db.Entry(C2).State = EntityState.Modified;

                //EDITAR CUADRO3
                Enlace C3 = db.Enlaces.Find(Int32.Parse(Request.Form["IdC3"]));
                C3.anteEnlace = Request.Form["TituloC3"];
                C3.texto = Request.Form["TextoC3"];
                C3.claseCss = Request.Form["CSSC3"];
                db.Entry(C3).State = EntityState.Modified;

                await db.SaveChangesAsync();  
            }
           
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> FormularioContac(string Nombre , string Correo , string Motivo , string Asunto , string Mensaje)
        {
            var dfasd = Request;

            //CREACIÓN DE NUEVO EMAIL DE CONTACTO
            EmailService emailService = new EmailService();
            var message = new IdentityMessage
            {
                Body = "El usuario con el nombre ( "+Nombre+" ) y con el correo ( "+Correo+" ) le ha escrito con el motivo ( "+Motivo+" ) y el asunto ( "+Asunto+" ) el siguiente mensaje : "+Mensaje ,
                Subject = "CORREO DE LA PÁGINA DE CONTACTO",
                Destination = "cgil@ebsoft.es",
            };
 
            Session["CorreoContactoEnviado"] = "true";
            
            await emailService.SendAsync(message);



            return RedirectToAction("Contacto", "Home");
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
