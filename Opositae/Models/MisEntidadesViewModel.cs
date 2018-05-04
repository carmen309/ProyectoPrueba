using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opositae.Models
{
    public class EstadisticaPregunta
    {   //Nivel de tabulación al que se va a mostrar
        public int tabulador { get; set; }
        //Texto a mostrar
        public string texto { get; set; }
        //Cantidad de preguntas relacionadas con esa categoría/dificultad/características
        public int cantidad { get; set; }
    }
}