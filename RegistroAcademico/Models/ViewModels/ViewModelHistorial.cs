using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroAcademico.Models.ViewModels
{
    public class ViewModelHistorial
    {
        public string Nombre_Alumno { get; set; }

        public string Nombre_Asignatura { get; set; }

        public int Nota_Final { get; set; }

        public  Nullable<int>  Nota_Rescate { get; set; }

        public string Clave_Asignatura { get; set; }
    }
}