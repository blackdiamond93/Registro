using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroAcademico.Models;
using RegistroAcademico.Models.ViewModels;

namespace RegistroAcademico.Controllers
{
    public class AlumnoController : Controller
    {
        List<ViewModelHistorial> historials;
        // GET: Alumno
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            int Carnet = Convert.ToInt32(form["Carnet"]);
            string pass = form["pass"];
            Database db = new Database();


            var Alumno = db.Alumno.Where(e => e.N_Carnet == Carnet && e.pass == pass).Select(e => e.Nombres);


            using (db = new Database())
            {
                historials = (from d in db.Nota
                              join c in db.Alumno on d.N_Carnet equals c.N_Carnet
                              join a in db.Asignatura on d.idAsignatura equals a.idAsignatura
                              where d.N_Carnet == Carnet
                              select new ViewModelHistorial
                              {
                                  Nombre_Alumno = c.Nombres + c.Apellidos,
                                  Nombre_Asignatura = a.Nombre,
                                  Nota_Final = d.Nota_final.Value,
                                  Nota_Rescate = d.Rescate.Value,
                                  Clave_Asignatura = a.idAsignatura

                              }).ToList();

            }

            if (Alumno.Count() > 0)
            {

                Session["Alumno"] = Alumno.First();
                return View("Historial", historials);
            }

            else
            {
                return View("Login");
            }


        }
        public ActionResult Logout()
        {
            Session["Alumno"] = null;
            return View("Login");
        }
        public ActionResult Historial()
        {
            if (Session["Alumno"] != null)
            {
                var Nombre = Session["Alumno"].ToString();
                using (Database db = new Database())
                {
                    historials = (from d in db.Nota
                                  join c in db.Alumno on d.N_Carnet equals c.N_Carnet
                                  join a in db.Asignatura on d.idAsignatura equals a.idAsignatura
                                  where c.Nombres == Nombre
                                  select new ViewModelHistorial
                                  {
                                      Nombre_Alumno = c.Nombres + c.Apellidos,
                                      Nombre_Asignatura = a.Nombre,
                                      Nota_Final = d.Nota_final.Value,
                                      Nota_Rescate = d.Rescate.Value,
                                      Clave_Asignatura = a.idAsignatura

                                  }).ToList();

                }
                return View(historials);
            }
            else
            {
                return View("Index");
            }




        }
    }
}