using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.carpetaImagenes = @"\Content\UploadedImagesp";
            this.noImagen = "noImagen.PNG";
        }
        public string carpetaImagenes { get; set; }
        public string noImagen { get; set; }
        public System.Data.Entity.DbSet<Opositae.Models.CategoriaOposicion> CategoriasOposiciones { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Respuesta> Respuestas { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Pregunta> Preguntas { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Dificultad> Dificultads { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Oposicion> Oposicions { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.OposicionesCategorias> OposicionesCategorias { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.CategoriaBlog> CategoriasBlog { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Blog> Blogs { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Nosotros> Nosotros { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Convocatoria> Convocatorias { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Legislacion> Legislacions { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Contacto> Contactoes { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.ItemEslider> ItemEsliders { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Imagen> Imagenes { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.Enlace> Enlaces { get; set; }

        public System.Data.Entity.DbSet<Opositae.Models.EnlaceMejorado> EnlaceMejoradoes { get; set; }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}