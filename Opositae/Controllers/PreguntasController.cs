using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Opositae.Models;
using System.Collections;
using PagedList;
using System.IO;
using System.Text;
using IdentitySample.Models;

namespace Opositae.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PreguntasController : Controller
    {
        public void cargaListaCategoriasyDificultades()
        {
            //Categorías de las preguntas
            var categoriasDB = db.CategoriasOposiciones.ToList();
            SelectList selecCategorias = new SelectList(categoriasDB, "Id", "Nombre");
            IEnumerable<SelectListItem> envio = selecCategorias.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            });

            var dificultadesDB = db.Dificultads.ToList();
            SelectList selecDificultades = new SelectList(dificultadesDB, "Id", "Descripcion");
            IEnumerable<SelectListItem> envio2 = selecDificultades.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value
            });
            
            ViewBag.Categoria = envio;
            ViewBag.Dificultad = envio2;
        }

        public void cargaListaCategoriasyDificultades(string idCategoria, string idDificultad)
        {
            //Categorías de las preguntas
            var categoriasDB = db.CategoriasOposiciones.ToList();
            SelectList selecCategorias = new SelectList(categoriasDB, "Id", "Nombre");
            IEnumerable<SelectListItem> envio = selecCategorias.Select(x => new SelectListItem()
            {
                Selected = (x.Value == idCategoria),
                Text = x.Text,
                Value = x.Value
            });

            var dificultadesDB = db.Dificultads.ToList();
            SelectList selecDificultades = new SelectList(dificultadesDB, "Id", "Descripcion");
            IEnumerable<SelectListItem> envio2 = selecDificultades.Select(x => new SelectListItem()
            {
                Selected = (x.Value == idDificultad),
                Text = x.Text,
                Value = x.Value
            });
            
            ViewBag.Categoria = envio;
            ViewBag.Dificultad = envio2;
        }

        //CARGA LAS PREGUNTAS EN INDEX TENIENDO EN CUENTA LA PAGINACIÓN
        public IPagedList<Pregunta> cargaPreguntas(int? cat, int? dif, int? page)
        {
            if (cat == null) { cat = 0; }
            if (dif == null) { dif = 0; }

            //Carga todas las categorías y dificultades para mostrar en la lista del filtro
            ViewBag.FiltroCategoria = db.CategoriasOposiciones.ToList();
            ViewBag.FiltroDificultad = db.Dificultads.ToList();

            IOrderedEnumerable<Pregunta> preguntas;

            switch (cat)
            {
                case 0: //Ninguna categoría elegida
                    switch (dif)
                    {
                        case 0: //Ninguna dificultad elegida (Carga todo)
                            preguntas = db.Preguntas.ToList().OrderByDescending(pregunta => pregunta.Id);
                        break;
                        default: //Alguna dificultad elegida (Carga cualquier categoría con X dificultad)
                        preguntas = db.Preguntas.ToList().Where(pregunta => pregunta.DificultadId == dif).OrderByDescending(pregunta => pregunta.Id);
                        break;
                    }
                    
                    break;
                default: //Alguna categoría elegida
                    switch (dif)
                    {
                        case 0: //Ninguna dificultad elegida (Carga cualquier dificultad con X categoría)
                            preguntas = db.Preguntas.ToList().Where(pregunta => pregunta.CategoriaId == cat).OrderByDescending(pregunta => pregunta.Id);
                            break;
                        default: //Alguna dificultad elegida (Carga X categoría con Y dificutlad)
                            preguntas = db.Preguntas.ToList().Where(pregunta => pregunta.CategoriaId == cat && pregunta.DificultadId == dif).OrderByDescending(pregunta => pregunta.Id);
                            break;
                    }
                    
                    break;
            }

            ViewBag.CategoriaActual = cat;
            ViewBag.DificultadActual = dif;
            ViewBag.TotalPreguntas = preguntas.ToList().Count();

            int pageSize = Configuracion.tamañoPaginaPreguntas;

            int pageNumber = (page ?? 1);

            return preguntas.ToPagedList(pageNumber, pageSize);
        }
  

        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Preguntas/
        public ViewResult Index(int? cat, int? dif, int? page)
        {

            IPagedList<Pregunta> prgs = cargaPreguntas(cat, dif, page);
            cargaListaCategoriasyDificultades();

            return View(prgs);
        }

        
        // POST: /Preguntas/IndexFiltrado

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filtrado(string selCategorias, string selDificultades)
        {
            IPagedList<Pregunta> prgs = cargaPreguntas(Int32.Parse(selCategorias), Int32.Parse(selDificultades), 1);
            cargaListaCategoriasyDificultades();

            return View("Index", prgs);
        }

        // GET: /Preguntas/Details/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Preguntas.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            else
            {
                pregunta.Categoria = db.CategoriasOposiciones.Find(pregunta.CategoriaId);
                pregunta.Dificultad = db.Dificultads.Find(pregunta.DificultadId);
            }
            return View(pregunta);
        }

        // GET: /Preguntas/Create
        public ActionResult Crear()
        {
            cargaListaCategoriasyDificultades();

            return View();
            
        }

        // POST: /Preguntas/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include="Id, Texto, JustificacionRespuesta, Activa")] Pregunta pregunta, string categoria, string dificultad, string respuestaA, string respuestaB, string respuestaC, string respuestaD, string correcta)
        {
            if (ModelState.IsValid)
            {
                //Añade el objeto categoría a la clase
                //int CategoriaId = Convert.ToInt16(categoria);
                //Categoria cat = db.Categorias.Find(CategoriaId);
                //pregunta.Categoria = cat;
                pregunta.CategoriaId = Convert.ToInt16(categoria);
                pregunta.DificultadId = Convert.ToInt16(dificultad);
                //Añade respuestas
                pregunta.Respuestas.Add(new Respuesta(respuestaA, ("A"==correcta)));
                pregunta.Respuestas.Add(new Respuesta(respuestaB, ("B"==correcta)));
                pregunta.Respuestas.Add(new Respuesta(respuestaC, ("C"==correcta)));
                pregunta.Respuestas.Add(new Respuesta(respuestaD, ("D"==correcta)));

                if (!pregunta.hayPreguntasYRespuestasIguales())
                {
                    //Guardado
                    db.Preguntas.Add(pregunta);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    cargaListaCategoriasyDificultades();
                    ModelState.AddModelError("PreguntaRepe", "Ya hay una pregunta igual a la que estás intentando introducir");
                    return View(pregunta);
                }
            }
            cargaListaCategoriasyDificultades();
            return View(pregunta);
        }

        // GET: /Preguntas/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Preguntas.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            cargaListaCategoriasyDificultades(pregunta.CategoriaId.ToString(), pregunta.DificultadId.ToString());
            return View(pregunta);
        }

        // POST: /Preguntas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Texto, JustificacionRespuesta, Activa")] Pregunta pregunta, string categoria, string dificultad, string respuestaA, string respuestaB, string respuestaC, string respuestaD, string correcta, string idRespA, string idRespB, string idRespC, string idRespD)
        {
            if (ModelState.IsValid)
            {
                //Añade el objeto categoría a la clase
               // int CategoriaId = Convert.ToInt16(categoria);
               // Categoria catt = db.Categorias.Find(CategoriaId);
                ///pregunta.Categoria = catt;
                pregunta.CategoriaId = Convert.ToInt16(categoria);
                pregunta.DificultadId = Convert.ToInt16(dificultad);
                //Añade respuestas
                int respid = Convert.ToInt16(idRespA);
                Respuesta respA = db.Respuestas.Find(respid);
                respA.Texto = respuestaA;
                respA.Correcta = "A" == correcta;

                respid = Convert.ToInt16(idRespB);
                Respuesta respB = db.Respuestas.Find(respid);
                respB.Texto = respuestaB;
                respB.Correcta = "B" == correcta;

                respid = Convert.ToInt16(idRespC);
                Respuesta respC = db.Respuestas.Find(respid);
                respC.Texto = respuestaC;
                respC.Correcta = "C" == correcta;

                respid = Convert.ToInt16(idRespD);
                Respuesta respD = db.Respuestas.Find(respid);
                respD.Texto = respuestaD;
                respD.Correcta = "D" == correcta;

                pregunta.Respuestas.Add(respA);
                pregunta.Respuestas.Add(respB);
                pregunta.Respuestas.Add(respC);
                pregunta.Respuestas.Add(respD);

                //Guardado
                db.Entry(pregunta).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            cargaListaCategoriasyDificultades(pregunta.CategoriaId.ToString(), pregunta.DificultadId.ToString());
            return View(pregunta);
        }

        // GET: /Preguntas/Delete/5
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Preguntas.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            else
            {
                pregunta.Categoria = db.CategoriasOposiciones.Find(pregunta.CategoriaId);
                pregunta.Dificultad = db.Dificultads.Find(pregunta.DificultadId);
            }
            return View(pregunta);
        }

        // POST: /Preguntas/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pregunta pregunta = db.Preguntas.Find(id);

            foreach (var item in db.Respuestas.ToList().Where(Respuesta => Respuesta.Pregunta.Id == id)) 
            {
                db.Respuestas.Remove(item);
            }

            db.Preguntas.Remove(pregunta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _EstadisticasPreguntas()
        {
            int cantidad = 0;
            List<String> estadisticas = new List<String>();

            estadisticas.Add("<table class='tablaEstadisticas'>");
            cantidad = db.Preguntas.Count();
            estadisticas.Add("<tr class='negrita'><td>TOTAL PREGUNTAS</td><td>" + cantidad.ToString() + "</td></tr>");

            List<CategoriaOposicion> categorias = db.CategoriasOposiciones.ToList();
            List<Dificultad> dificultades = db.Dificultads.ToList();

            foreach (CategoriaOposicion cat in categorias)
            {
                estadisticas.Add("<tr><td colspan='2'><hr></td></tr>");
                cantidad = db.Preguntas.Count(Pregunta => Pregunta.CategoriaId == cat.Id);
                estadisticas.Add("<tr class='negrita'><td>" + cat.Nombre + "</td><td>" + cantidad.ToString() + "</td></tr>");
                foreach (Dificultad dif in dificultades)
                {
                    cantidad = db.Preguntas.Count(Pregunta => Pregunta.CategoriaId == cat.Id && Pregunta.DificultadId == dif.Id);
                    estadisticas.Add("<tr><td class='tabulado'>" + dif.Descripcion + "</td><td>" + cantidad.ToString() + "</td></tr>");
                }
            }
            estadisticas.Add("</table>");
            ViewBag.Estadisticas = estadisticas;

            return View();
        }

        private Pregunta creaPregunta(ArrayList lineas, int cat, int dif) 
        {
            Pregunta prg = new Pregunta();
            bool primera = true;
            bool correcta = false;
            string rspt = "";


            foreach (var linea in lineas)
            {
                string ln = linea.ToString();
                if (primera)
                {
                    int primerpunto = ln.IndexOf(".") + 1;
                    prg.Texto = ln.Substring(primerpunto).Trim();
                    primera = false;
                }
                else 
                {
                    int primerparentesis = ln.IndexOf(")") + 1;
                    rspt = ln.Substring(primerparentesis).Trim();
                    correcta = (rspt.EndsWith(" x") || rspt.EndsWith(" X")) ? true : false;
                    rspt = rspt.Replace(" x", "");
                    rspt = rspt.Replace(" X", "");

                    prg.Respuestas.Add(new Respuesta(rspt,correcta));
                }
            }

            prg.Activa = true;
            prg.CategoriaId = cat;
            prg.DificultadId = dif;
            prg.JustificacionRespuesta = "";
            return prg;
        }

        [HttpPost]
        public ActionResult Importacion(HttpPostedFileBase Archivo, int Categoria, int Dificultad)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase archv = Request.Files[0];
                    if (archv.ContentLength > 0)
                    {
                        //Guardado del archivo
                        string rutaArchivo;
                        rutaArchivo = Path.Combine(Server.MapPath("~/App_Data/Temporal"), Archivo.FileName);
                        archv.SaveAs(rutaArchivo);
                        //Lectura del archivo
                        StreamReader objReader = new StreamReader(rutaArchivo);
                        string sLine = "";

                        List<Pregunta> preguntas = new List<Pregunta>();

                        int contador = 0;
                        ArrayList lineas = new ArrayList();
                        while (sLine != null)
                        {
                            sLine = objReader.ReadLine();
                            if ((sLine != null) && (sLine.Trim() != ""))
                            {
                                lineas.Add(sLine);
                                contador++;
                                if (contador == 5)
                                {
                                    preguntas.Add(creaPregunta(lineas, Categoria, Dificultad));
                                    lineas.Clear();
                                    contador = 0;
                                }
                            }


                        }

                        contador++;
                        //Una vez recopiladas todas
                        db.Preguntas.AddRange(preguntas);
                        db.SaveChanges();
                        objReader.Close();
                        if (System.IO.File.Exists(rutaArchivo))
                        {
                            System.IO.File.Delete(rutaArchivo);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ha ocurrido un error y no se han podido importar las preguntas.";
            }

            IPagedList<Pregunta> prgs = cargaPreguntas(0, 0, 1);
            cargaListaCategoriasyDificultades();
            return View("Index", prgs);
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
