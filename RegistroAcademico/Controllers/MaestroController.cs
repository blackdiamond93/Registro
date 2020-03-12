using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroAcademico.Models;
using RegistroAcademico.Models.ViewModels;

namespace RegistroAcademico.Controllers
{
    public class MaestroController : Controller
    {
        List<ModelGrupo> grupo ;
        // GET: Maestro
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
            string Carnet = form["Codigo"];
            string pass = form["pass"];
            Database db = new Database();
            var Maestro = db.Docente.Where(e => e.Codigo_Doc == Carnet && e.pass == pass).Select(e => e.Nombres);
            Session["Codigo"] = Carnet;
            if (Maestro.Count() > 0)
            {
                CargarDropdownList();
                  Session["Maestro"] = Maestro.First();
                return View("Grupos");
            }

            else
            {
                return View("Login");
            }


        }
        public ActionResult Logout()
        {
            Session["Maestro"] = null;
            return View("Login");
        }
        
        public ActionResult Grupos()
        {
            if (Session["Maestro"]!=null)
            {
                CargarDropdownList();
                return View();
            }
            else
            {
                return View("Login");
            }
            
        }
        [HttpGet]
        public JsonResult Asignatura(int Id)
        {
            string c = Session["Codigo"].ToString();
            List<ElementJsonIntKey> lst = new List<ElementJsonIntKey>();
            using (Database db = new Database())
            {
                lst = (from d in db.Grupos_Asignado
                       join b in db.Asignatura on d.id_Asignatura equals b.idAsignatura
                       where d.id_Grupos == Id && d.id_Docente == c
                       select new ElementJsonIntKey
                       {
                           Value = b.Clava,
                           Text = b.Nombre
                       }
                       ).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Alumno(int Id)
        {
            string c = Session["Codigo"].ToString();
            List<ElementJsonIntKey> lst = new List<ElementJsonIntKey>();
            using (Database db = new Database())
            {
                lst = (from d in db.Detalle_Grupos_Asignado
                       join b in db.Grupos_Asignado on d.id_Grupos equals b.id_Grupos
                       where d.id_Grupos == Id && b.id_Docente == c
                       select new ElementJsonIntKey
                       {
                           Value = d.N_Carnet,
                           Text = d.N_Carnet.ToString()
                       }
                       ).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public class ElementJsonIntKey
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        [HttpGet]
        public ActionResult Nota()
        {
            if (Session["Maestro"]!=null)
            {
                return View("Grupos");
            }
            else
            {
                return View("Login");
            }

        }
        [HttpPost]
        public ActionResult Nota(FormCollection form)
        {
            int asignatura = Convert.ToInt32(form["Asignatura"]);
            Database db = new Database();
            var a = db.Asignatura.Where(e => e.Clava == asignatura).Select(e => e.idAsignatura);
         
            string asig = a.First();
            using ( db = new Database())
            {
                Nota nota = new Nota();
                nota.idAsignatura = asig;
                nota.N_Carnet = Convert.ToInt32(form["Alumnos"]);
                nota.Acumulado =Convert.ToInt32(form["Acumulado"]);
                nota.Examen = Convert.ToInt32(form["Examen"]);
                nota.Nota_final = (Convert.ToInt32(form["Acumulado"])+ Convert.ToInt32(form["Examen"]));
                nota.Rescate = 0;
                db.Nota.Add(nota);
                db.SaveChanges();
            }
            

            CargarDropdownList();
            return View("Grupos");
        }

        public  void CargarDropdownList()
        {
            string c = Session["Codigo"].ToString();
            using (Database db = new Database())
            {
                grupo = (from d in db.Grupos_Asignado
                         join g in db.Grupos on d.id_Grupos equals g.id_Grupos
                         where d.id_Docente == c
                         select new ModelGrupo
                         {
                             N_Carnet = d.id_Grupos,
                             Nombre = g.Grupo
                         }).ToList();
            }
            List<SelectListItem> listItems = grupo.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.N_Carnet.ToString()

                };
            });


            ViewBag.items = listItems;
        }
    }
}