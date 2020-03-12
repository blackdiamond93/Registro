using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegistroAcademico.Models.ViewModels
{
    public class ViewModelAlumno
    {
        
        public int N_Carnet { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string pass { get; set; }
        public Boolean Estado { get; set; }

        public int Nota_Final { get; set; }

        public string asignatura { get; set; }
    }
}