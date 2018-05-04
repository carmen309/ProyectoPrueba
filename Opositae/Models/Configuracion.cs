using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opositae.Models
{
    static class Configuracion
    {
        private static int tamañoGeneralBackEnd = 10;

        public static int tamañoPaginaOposiciones = tamañoGeneralBackEnd;
        public static int tamañoPaginaCategoriasOposiciones = tamañoGeneralBackEnd;
        public static int tamañoPaginaDificultades = tamañoGeneralBackEnd;
        public static int tamañoPaginaPreguntas = tamañoGeneralBackEnd;
        public static int tamañoPaginaCategoriasBlog = tamañoGeneralBackEnd;
        public static int tamañoPaginaBlog = 6;

        public static int tamañoPaginaConvocatorias = 6;
        public static int tamañoPaginaLegislacion = 6;
    }
}