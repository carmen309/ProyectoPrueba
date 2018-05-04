using IdentitySample.Models;
using Opositae.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Opositae.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult Nosotros()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string carpeta = db.carpetaImagenes;
            Nosotros nosotros = db.Nosotros.Find(1);
            ViewBag.imagePath = carpeta + "/" + nosotros.Imagen;

            return View(nosotros);
        }

        public ActionResult Blog(int? page , string mes , string anio)
        {
            
            ApplicationDbContext db = new ApplicationDbContext();
     

            if(db.Blogs.ToList().LongCount() == 0) //SI NO HAY NINGUNA NOTICIA AÚN....
            {
               
            }
            else
            {

                //SACAR FECHAS PARA SACAR EL LISTADO SEGUN LOS AÑOS (EN EL SELECT)
                IOrderedEnumerable<Blog> BlogDescendente;
                //BlogDescendente = db.Blogs.ToList().OrderByDescending(x => x.Fecha);
                BlogDescendente = db.Blogs.Where(x => x.Activa == true).ToList().OrderByDescending(x => x.Fecha);
                ViewBag.Nuevo = BlogDescendente.First().Fecha.Year;

                IOrderedEnumerable<Blog> BlogAscendente;
                BlogAscendente = db.Blogs.Where(x => x.Activa == true).ToList().OrderBy(x => x.Fecha);
                ViewBag.Antiguo = BlogAscendente.First().Fecha.Year;

            }
             
          
            IPagedList<Blog> blogs = cargaBlogs(page , mes , anio);
            return View(blogs);

        }

        public ActionResult _BlogsDestacados()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Blogs.Where(x => x.Destacada == true).Where(x => x.Activa == true)); //La entrada tiene que estar "Destacada" y "Activa"

        }

        public ActionResult Convocatorias(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IPagedList<Convocatoria> convocatorias = cargaConvocatorias(page);
            return View(convocatorias);
        }

        public ActionResult Legislacion(int? page, string cat)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            IPagedList<Legislacion> legislacion = cargaLegislaciones(page, cat);
            return View(legislacion);
        }

        public ActionResult Contacto()
        {
            TotalContacto contacto  = new TotalContacto();
            return View(contacto);
        }

        public ActionResult _Slider()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.ItemEsliders.ToList());
        }

        public ActionResult _EnlacesCirculares()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Enlaces.Where(x => x.enlacePadre == "EnlacesCirculares").ToList());
        }

        public ActionResult _CuadroPublicitario()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.EnlaceMejoradoes.Where(x => x.enlace.enlacePadre == "CuadroPublicitario").ToList());
        }

        public ActionResult _NosotrosIconos()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Enlaces.Where(x => x.enlacePadre == "NosotrosIconos").ToList());
        }

        public ActionResult _EnlaceAcceso()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Enlaces.Where(x => x.enlacePadre == "EnlaceAcceso").ToList());
        }

        public ActionResult _Menu1()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Enlaces.Where(x => x.enlacePadre == "Menu1").ToList());
        }

        public ActionResult _Menu2()
        {

            Menu2 MiMenu2 = new Menu2();
            return View(MiMenu2);
        }

        public ActionResult _PiePagina()
        {

            PiePagina MiPiePagina = new PiePagina();
            return View(MiPiePagina);
        }

        public ActionResult _CategoriasOposicion()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.CategoriasOposiciones.ToList());
        }

        public ActionResult _MensajeCabecera()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Enlaces.Where(x => x.enlacePadre == "MensajeCabecera").First());
        }

        public ActionResult Recursos()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Enlaces.Where(x => x.enlacePadre == "Recursos").ToList());
        }

        //CARGA LAS CONVOCATORIAS EN INDEX TENIENDO EN CUENTA LA PAGINACIÓN
        public IPagedList<Convocatoria> cargaConvocatorias(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int pageSize = Configuracion.tamañoPaginaConvocatorias;
            int pageNumber = (page ?? 1);
            IOrderedEnumerable<Convocatoria> conv;
            conv = db.Convocatorias.ToList().OrderByDescending(convocatoria => convocatoria.Fecha);

            return conv.ToPagedList(pageNumber, pageSize);
        }

        //CARGA LAS LEGISLACIONES EN INDEX TENIENDO EN CUENTA LA PAGINACIÓN
        public IPagedList<Legislacion> cargaLegislaciones(int? page, string cat)
        {
            ApplicationDbContext db = new ApplicationDbContext();


            int pageSize = Configuracion.tamañoPaginaLegislacion;
            int pageNumber = (page ?? 1);
            IEnumerable<Legislacion> legi;

            if(cat == null || cat == "all" ) //Si no se pasa categoria o la selecionada es la general....
            {
                legi = db.Legislacions.ToList();
            }
            else
            {
                legi = db.Legislacions.Where(x => x.Categoria.Nombre == cat).ToList();
            }

            return legi.ToPagedList(pageNumber, pageSize);
        }

        //CARGA LOS BLOGS EN INDEX TENIENDO EN CUENTA LA PAGINACIÓN
        public IPagedList<Blog> cargaBlogs(int? page , string mes , string anio)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int pageSize = Configuracion.tamañoPaginaBlog;
            int pageNumber = (page ?? 1);
            IOrderedEnumerable<Blog> blog;

            

            if ( ((mes!=null && mes!="all" ) && (anio != null && anio != "all")))
            {
                int intmes = Int32.Parse(0+mes);
                int intanio = Int32.Parse(anio);

                blog = db.Blogs.Where(x => x.Fecha.Month == intmes).ToList().OrderByDescending(x => x.Fecha);
                blog = blog.Where(x => x.Fecha.Year == intanio).Where(x => x.Activa == true).ToList().OrderByDescending(x => x.Fecha);
            }
            else if (mes != null && mes != "all")
            {
                int intmes = Int32.Parse(0 + mes);
                blog = db.Blogs.Where(x => x.Fecha.Month == intmes).Where(x => x.Activa == true).ToList().OrderByDescending(x => x.Fecha.Month);
            }
            else if (anio != null && anio != "all")
            {
                int intanio = Int32.Parse(anio);
                blog = db.Blogs.Where(x => x.Fecha.Year == intanio).Where(x => x.Activa == true).ToList().OrderByDescending(x => x.Fecha.Year);
            }
            else
            {
                blog = db.Blogs.Where(x => x.Activa == true ).ToList().OrderByDescending(x => x.Fecha); //Entradas activas y ordenadas
            }

            return blog.ToPagedList(pageNumber, pageSize);
        }

    }
}