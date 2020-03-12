using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroAcademico.Models.ViewModels
{
    public class ViewModelNota
    {
        public string idAsignatura { get; set; }
        public int N_Carnet { get; set; }
        public int Acumulado { get; set; }
        public int Examen { get; set; }
        public int Nota_final { get; set; }
        public int Rescate { get; set; }
    }
}