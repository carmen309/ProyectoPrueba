using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Opositae.Models
{
    /*
     * 1. Oposicion
     * 2. CategoriaOposicion
     * 3. OposicionesCategorias
     * 4. Dificultad
     * 5. Pregunta
     * 6. Respuesta
     * 7. CategoriaBlog
     * 8. Blog
     * 9. Nosotros
     * 10. Convocatoria
     * 11. Legislacion
     * 12. Contacto
     * 13. Inicio
     * 14. Enlace
     * 15. Imagen
     * 16. ItemEslider
     */
    public static class Utilidades
    {
        public static string CollapseSpaces(string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }
        public static bool CadenasIguales(string cadena1, string cadena2)
        {
            string cad1 = CollapseSpaces(cadena1); string cad2 = CollapseSpaces(cadena2);
            // return db.Preguntas.Where(x=> Regex.Replace(x.Texto, " {2,}", " ").Equals(Regex.Replace(this.Texto, " {2,}", " "))).Count()>0;
            bool contiene = cad1.Equals(cad2, StringComparison.InvariantCultureIgnoreCase);
            return contiene;
        }
        public static bool ContieneRespuestas(ICollection<Respuesta> a, string n)
        {
            foreach (Respuesta item in a)
            {
                if (CadenasIguales(item.Texto, n))
                    return true;
            }
            return false;
        }
    }
    public class Oposicion
    {
        public Oposicion()
        {
            OposicionesCategorias = new List<OposicionesCategorias>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Required()]
        public string Nombre { get; set; }
        public virtual ICollection<OposicionesCategorias> OposicionesCategorias { get; set; }
        public bool Activa { get; set; }
    }

    public class CategoriaOposicion
    {
        public CategoriaOposicion()
        {
            OposicionesCategorias = new List<OposicionesCategorias>();
            Legislacion = new List<Legislacion>();
        }
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }
        [StringLength(255)]
        [Required()]
        public string Nombre { get; set; }
        public bool Activa { get; set; }
        public virtual ICollection<OposicionesCategorias> OposicionesCategorias { get; set; }
        public virtual ICollection<Legislacion> Legislacion { get; set; }

    }

    public class OposicionesCategorias
    {
        public int Id { get; set; }
        public virtual Oposicion Oposicion { get; set; }
        public virtual CategoriaOposicion Categoria { get; set; }
    }

    public class Dificultad
    {
        [Key]
        [Column(Order = 2)]
        public int Id { get; set; }
        [StringLength(50)]
        [Required()]
        [DisplayName("Dificultad")]
        public string Descripcion { get; set; }
        public bool Activa { get; set; }
    }

    public class Pregunta
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public Pregunta()
        {
            Respuestas = new List<Respuesta>();
        }
        [Key]
        public int Id { get; set; }
        [ForeignKey("Categoria")]
        [Column(Order = 1)]
        public int CategoriaId { get; set; }
        [ForeignKey("Dificultad")]
        [Column(Order = 2)]
        public int DificultadId { get; set; }
        [DisplayName("Pregunta")]
        [Required()]
        public string Texto { get; set; }
        public string JustificacionRespuesta { get; set; }
        public bool Activa { get; set; }
        public virtual ICollection<Respuesta> Respuestas { get; set; }
        public CategoriaOposicion Categoria { get; set; }
        public Dificultad Dificultad { get; set; }

        public string TextoCorto() {
            return Texto.Substring(0, 300) + "...";
        }
        public bool hayPreguntaIgual()
        {
            List<Pregunta> prs = db.Preguntas.ToList();
            //Pregunta pr = db.Preguntas.Where(x => x.Texto.Equals(this.Texto, StringComparison.InvariantCultureIgnoreCase)).First();
            foreach (Pregunta p in prs)
            {
                bool sonIguales = Utilidades.CadenasIguales(Utilidades.CollapseSpaces(p.Texto), Utilidades.CollapseSpaces(this.Texto));
                return sonIguales;
                //break;
            }
            return false;


        }
        public List<Pregunta> damePreguntasIguales()
        {
            List<Pregunta> pre = new List<Pregunta>();
            if (hayPreguntaIgual())
            {
                foreach (Pregunta p in db.Preguntas)
                {
                    if (Utilidades.CadenasIguales(Utilidades.CollapseSpaces(p.Texto), Utilidades.CollapseSpaces(this.Texto)))
                    {
                        pre.Add(p);
                    }
                }
            }
            return pre;
        }
        public bool hayPreguntasYRespuestasIguales()
        {
            bool variableDevuelta = true;
            if (damePreguntasIguales().Count > 0)
            {
                foreach (Pregunta x in damePreguntasIguales())
                {
                    foreach (Respuesta y in x.Respuestas)
                    {
                        if (!Utilidades.ContieneRespuestas(this.Respuestas, y.Texto))
                        {
                            variableDevuelta = false;
                            break;
                        }
                    }
                    if (!variableDevuelta)
                    {
                        break;
                    }
                }
            }
            else
            {
                variableDevuelta = false;
            }

            return variableDevuelta;
        }
    }

    public class Respuesta
    {
        public Respuesta()
        {
        }

        public Respuesta(string txtRespuesta, bool esCorrecta)
        {
            Texto = txtRespuesta;
            Correcta = esCorrecta;
        }

        public Respuesta(string id, string txtRespuesta, bool esCorrecta)
        {
            Id = Convert.ToInt16(id);
            Texto = txtRespuesta;
            Correcta = esCorrecta;
        }
        [Key]
        public int Id { get; set; }
        public virtual Pregunta Pregunta { get; set; }
        [DisplayName("Respuesta")]
        public string Texto { get; set; }
        public bool Correcta { get; set; }
    }


    public class CategoriaBlog
    {
        public CategoriaBlog()
        {
            Noticias = new List<Blog>();
        }
        public int Id { get; set; }
        [Required()]
        [StringLength(255)]
        public string Nombre { get; set; }
        public bool Activa { get; set; }
        public virtual ICollection<Blog> Noticias { get; set; }
    }


    public class Blog
    {
        [Key]
        public int Id { get; set; }
        public int CategoriaBlogId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }
        [Required()]
        [DisplayName("Título")]
        [StringLength(255)]
        public string Titulo { get; set; }
        [Required()]
        public string Contenido { get; set; }
        public string Imagen { get; set; }

        [DisplayName("Categoría")]
        public virtual CategoriaBlog CategoriaBlog { get; set; }
        public bool Destacada { get; set; }
        public bool Activa { get; set; }
    }

    public class Nosotros {
        public int Id { get; set; }
        [DisplayName("Título")]
        [StringLength(50)]
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Imagen { get; set; }
        [DisplayName("Imagen de fondo para la zona 'Tips'")]
        public string ImagenFondoTips { get; set; }
        [DisplayName("Icono tip 1")]
        public string Icono1 { get; set; }
        [DisplayName("Texto tip 1")]
        [StringLength(40)]
        public string Tip1 { get; set; }
        [DisplayName("Icono tip 2")]
        public string Icono2 { get; set; }
        [DisplayName("Texto tip 2")]
        [StringLength(40)]
        public string Tip2 { get; set; }
        [DisplayName("Icono tip 3")]
        public string Icono3 { get; set; }
        [DisplayName("Texto tip 3")]
        [StringLength(40)]
        public string Tip3 { get; set; }
        [DisplayName("Icono tip 4")]
        public string Icono4 { get; set; }
        [DisplayName("Texto tip 4")]
        [StringLength(40)]
        public string Tip4 { get; set; }
        [DisplayName("Icono tip 5")]
        public string Icono5 { get; set; }
        [DisplayName("Texto tip 5")]
        [StringLength(40)]
        public string Tip5 { get; set; }
        [DisplayName("Icono tip 6")]
        public string Icono6 { get; set; }
        [DisplayName("Texto tip 6")]
        [StringLength(40)]
        public string Tip6 { get; set; }
    }

    public class Convocatoria {
        public int Id { get; set; }
        [Required()]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }
        [DisplayName("Descripción")]
        [StringLength(60)]
        public string Descripcion { get; set; }
        [StringLength(30)]
        public string Entidad { get; set; }
        [StringLength(50)]
        public string Acceso { get; set; }
        [StringLength(7)]
        public string Plazas { get; set; }
        [StringLength(10)]
        public string Grupo { get; set; }
        public string Enlace { get; set; }
        public bool Activa { get; set; }
    }

    public class Legislacion {
        public int Id { get; set; }
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public virtual CategoriaOposicion Categoria { get; set; }
        [Required()]
        public string Norma { get; set; }
        [Required()]
        public string Enlace { get; set; }
    }

    public class Contacto {
        public int Id { get; set; }
        [Required()]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [Required()]
        public string Correo { get; set; }
        [Required()]
        [DisplayName("Mapa de Google")]
        public string Mapa { get; set; }
        [DisplayName("Facebook")]
        public string RS_Facebook { get; set; }
        [DisplayName("Twitter")]
        public string RS_Twitter { get; set; }
        [DisplayName("Google+")]
        public string RS_GooglePlus { get; set; }

    }

    public class Enlace
    {
        public int EnlaceId { get; set; }
        [Display(Name = "Enlace")]
        public string texto { get; set; }
        [Display(Name = "Texto")]
        public string anteEnlace { get; set; }
        [Display(Name = "Acción")]
        public string accion { get; set; }
        [Display(Name = "Controlador")]
        public string controlador { get; set; }
        [Display(Name = "Id del enlace")]
        public string idEnviado { get; set; }
        [Display(Name = "CalseCSS")]
        public string claseCss { get; set; }
        [Display(Name = "EnlacePadre")]
        public string enlacePadre { get; set; }
        [Display(Name = "Posición en una caja")]
        public int situacion { get; set; }

        public string Imprimete()
        {
            if (this.controlador != null && this.controlador.Split('[', ']').Count() > 1)
            {
                if (this.controlador.Split('[', ']')[1] == "http" || this.controlador.Split('[', ']')[1] == "file" || this.controlador.Split('[', ']')[1] == "ftp")
                {
                    //La url es la accion : es una url externa
                    return this.accion.ToString();
                }
            }

            return this.controlador + "/" + this.accion;
        }

        public static string cortarCadena(int num, string cadena)
        {
            if (cadena.Length >= num)
            {
                cadena = cadena.Substring(0, num) + " ...";
            }

            return cadena;
        }
    }

    public class Imagen //: Controller //Para 'server'
    {
        public int ImagenId { get; set; }
        public string carpeta { get; set; }
        public string nombre { get; set; }

        public string imagenPath()
        {

            // carpeta = carpeta.Replace( @"\" , @"/" ); //Para que este bien la url
            string carpeta = @"/Content/UploadedImagesp"; //Carpeta donde se guardaran los archivos/programas
            return carpeta + "/" + nombre;
        }

        public string GuardarImagen(HttpPostedFileBase FileImagen , string Carpeta ,string Categoria )
        {
            if(Categoria == "")
            {
                Categoria = "CategoriaPorDefecto";
            }
            if(Carpeta == "")
            {
                Carpeta = @"/Content/UploadedImagesp";
            }

            string NuevoNombre = Categoria + "_" + Path.GetFileName(FileImagen.FileName); //Cambiamos el nombre
            this.nombre = NuevoNombre;
            this.carpeta = Carpeta;

            FileImagen.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(carpeta), this.nombre));

            return "OK";
        }
    }

    public class ItemEslider
    {
        public int ItemEsliderId { get; set; }
        [Display(Name = "Titulo")]
        public string titulo { get; set; }
        [Display(Name = "Subtitulo")]
        public string subtitulo { get; set; }
        [Display(Name = "Texto")]
        public string texto { get; set; }
        [Display(Name = "Imagen del Slider")]
        public virtual Imagen imagenSlider { get; set; }
        public virtual Enlace enlace { get; set; }
    }

    public class EnlaceMejorado
    {
        public int EnlaceMejoradoId { get; set; }
        public virtual Enlace enlace { get; set; }
        [Display(Name = "Imagen")]
        public virtual Imagen imagen { get; set; }

    }

    public class Menu2
    {
        public Menu2()
        {

            MisEnlaces = new List<Enlace>();
            MisEnlacesMejorados = new List<EnlaceMejorado>();
            //Consulta en la base de datos para instanciar
            ApplicationDbContext db = new ApplicationDbContext();
            ICollection<Enlace> EnlacesBd = db.Enlaces.Where(x => x.enlacePadre == "Menu2").ToList();
            ICollection<EnlaceMejorado> EnlacesMejoradosBd = db.EnlaceMejoradoes.Where(x => x.enlace.enlacePadre == "Menu2Logo").ToList();

            foreach (var item in EnlacesBd)
            {
                MisEnlaces.Add(item);
            }

            foreach (var item in EnlacesMejoradosBd)
            {
                MisEnlacesMejorados.Add(item);
            }
        }
        public virtual ICollection<Enlace> MisEnlaces { get; set; }
        public virtual ICollection<EnlaceMejorado> MisEnlacesMejorados { get; set; }
    }

    public class PiePagina
    {
        public PiePagina()
        {
            EnlacesMenu2 = new Menu2();
            EnlacesPie = new List<Enlace>();
            UltimasEntradas = new List<Blog>();

            ApplicationDbContext db = new ApplicationDbContext();
            ICollection<Enlace> EnlacesBd = db.Enlaces.Where(x => x.enlacePadre == "Pie").ToList(); //Enlaces del Pie
            IOrderedEnumerable<Blog> EntradasBd = db.Blogs.Where(x => x.Activa == true).ToList().OrderByDescending(x => x.Fecha); //Ultimas entradas blog

            foreach (var item in EnlacesBd)
            {
                EnlacesPie.Add(item);
            }

            foreach (var item in EntradasBd.Take(6)) //Limitar a 6 entradas
            {
                UltimasEntradas.Add(item);
            }

        }
        public virtual ICollection<Enlace> EnlacesPie { get; set; }
        public virtual Menu2 EnlacesMenu2 { get; set; } //Enlaces iguales que los del menu2
        public virtual ICollection<Blog> UltimasEntradas { get; set; } //Ultimas entradas del blog
    }

    public class TotalContacto
    {
        public TotalContacto()
        {

            CuadrosInformacion = new List<Enlace>();
            MiContacto = new Contacto();
            //Consulta en la base de datos para instanciar
            ApplicationDbContext db = new ApplicationDbContext();
            ICollection<Enlace> EnlacesBd = db.Enlaces.Where(x => x.enlacePadre == "Contactos").ToList();
            MiContacto = db.Contactoes.First();

            foreach (var item in EnlacesBd)
            {
                CuadrosInformacion.Add(item);
            }
        }
        public virtual ICollection<Enlace> CuadrosInformacion { get; set; }
        public virtual Contacto MiContacto { get; set; }
    }


    //SERIALIZAR LA NOTICIA QUE QUIERO
    public class MiNoticia
    {

        public MiNoticia(Blog MiBlog)
        {
            this.Titulo = MiBlog.Titulo;
            this.Texto = MiBlog.Contenido;
            this.Imagen = MiBlog.Imagen;
        }
   
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Imagen { get; set; }

    }

    /*public class Logo
    {
        public int LogoId { get; set; }
        public virtual Imagen imagenLogo { get; set; }
        public virtual Enlace enlaceLogo { get; set; }

    }*/
}