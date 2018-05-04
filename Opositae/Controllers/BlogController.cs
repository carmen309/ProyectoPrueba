using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class BlogController : Controller
    {
        public void cargaListaCategoriasBlog()
        {
            //Categorías de las preguntas
            var categoriasDB = db.CategoriasBlog.ToList();
            SelectList selecCategorias = new SelectList(categoriasDB, "Id", "Nombre");
            IEnumerable<SelectListItem> envio = selecCategorias.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            });

            ViewBag.Categoria = envio;
        }

        private ApplicationDbContext db = new ApplicationDbContext();



        public IPagedList<Blog> cargaBlog(int? cat, int? page)
        {
            if (cat == null) { cat = 0; }

            //Carga todas las categoría para mostrar en la lista del filtro
            ViewBag.FiltroCategoria = db.CategoriasBlog.ToList();

            IOrderedEnumerable<Blog> blog;

            switch (cat)
            {
                case 0: //Carga todas las entradas de blog
                    blog = db.Blogs.ToList().OrderByDescending(blg => blg.Id);
                    break;
                default: //Carga las entradas de blog pertenecientes a una categoría
                    blog = db.Blogs.ToList().Where(blg => blg.CategoriaBlogId == cat).OrderByDescending(blg => blg.Id);
                    break;
            }

            ViewBag.CategoriaActual = cat;

            ICollection<Blog> blognew = new List<Blog>();
            foreach (var item in blog)
            {
                item.CategoriaBlog = db.CategoriasBlog.Find(item.CategoriaBlogId);
                blognew.Add(item);
            }

            int pageSize = Configuracion.tamañoPaginaBlog;

            int pageNumber = (page ?? 1);

            return blognew.ToPagedList(pageNumber, pageSize);
        }

        // GET: /Blog/
        public ActionResult Index(int? cat, int? page)
        {
            IPagedList<Blog> blog = cargaBlog(cat, page);
            cargaListaCategoriasBlog();

            return View(blog);
        }

        // GET: /Blog/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: /Blog/Create
        public ActionResult Crear()
        {
            cargaListaCategoriasBlog();
            return View();
        }

        // POST: /Blog/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//SUMMERNOTE
        public ActionResult Crear([Bind(Include="Id,Fecha,Titulo,Contenido,Imagen,ImagenAlin,Destacada,Activa")] Blog blog,string[] Categoria, HttpPostedFileBase imagen)
        {
            string carpeta = db.carpetaImagenes;
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imagen.FileName);
                    var path = Path.Combine(Server.MapPath("/" + carpeta), "blog_"+ imagen.FileName);
                    blog.Imagen =carpeta+"/blog_"+ imagen.FileName;
                    imagen.SaveAs(path);
                }
                int cat = Convert.ToInt16(Categoria[0]);
                CategoriaBlog cb = db.CategoriasBlog.Find(cat);
                blog.CategoriaBlog = cb;
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: /Blog/Edit/5
        public ActionResult Editar(int? id)
        {
            string carpeta = db.carpetaImagenes;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.imagePath = blog.Imagen;
            if (blog.Imagen != "")
            {
                ViewBag.Imagen = blog.Imagen;
            }

            cargaListaCategoriasBlog();
            return View(blog);
        }

        // POST: /Blog/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//SUMMERNOTE
        public ActionResult Editar([Bind(Include="Id,Fecha,Titulo,Contenido,Imagen,ImagenAlin,Destacada,Activa")] Blog blog,string[] Categoria, HttpPostedFileBase imagen)
        {
            //string carpeta = db.carpetaImagenes;
            string carpeta = @"\Content\UploadedImagesp"; //Carpeta donde se guardaran las imagenes
            var A = Request;
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                   // var pathEliminar = Path.Combine(Server.MapPath("/" + carpeta), " " + blog.Imagen);
                    try
                    {
                        // System.IO.File.Delete(pathEliminar);
                        
                        string NombreImagen = blog.Imagen.Split('/').Last();
                        System.IO.File.Delete(Path.Combine(Server.MapPath(carpeta), NombreImagen));//Eliminamos imagen

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    var fileName = Path.GetFileName(imagen.FileName);
                    var path = Path.Combine(Server.MapPath("/" + carpeta), "blog_" + imagen.FileName);
                    blog.Imagen = carpeta + "/blog_" + imagen.FileName;
                    imagen.SaveAs(path);
                }
                int cat = Convert.ToInt16(Categoria[0]);
                CategoriaBlog cb = db.CategoriasBlog.Find(cat);
                blog.CategoriaBlog = cb;
                blog.CategoriaBlogId = cb.Id;

                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: /Blog/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: /Blog/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Blog blog = db.Blogs.Find(id);

            string carpeta = @"\Content\UploadedImagesp"; //Carpeta donde se guardaran las imagenes
            string NombreImagen = blog.Imagen.Split('/').Last();
            System.IO.File.Delete(Path.Combine(Server.MapPath(carpeta), NombreImagen));//Eliminamos imagen

            

            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      
        [AllowAnonymous]
        public ActionResult SacarNoticia(int? id)
        {
             Blog blog = db.Blogs.Find(id);
            return Json(new MiNoticia(blog),JsonRequestBehavior.AllowGet);


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
