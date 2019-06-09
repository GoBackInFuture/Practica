using AppBurse.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Controllers
{
    [Authorize(Roles = "Administrator,Secretar")]
    public class StudentController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public string GetSpec(Specializare_pe_Domeniu sp)
        {
            var category = db.Specializari.Find(sp.SubramuraSpecializareId);
            return  String.Concat(category.Domeniu.Program_de_studiu.Denumire, "- An ", category.AnStudiu, "-", category.Subramura);
            
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllDomenii()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Specializari select cat;
            categories = categories.OrderBy(a => a.Domeniu.Program_de_studiuId).ThenBy(a=>a.AnStudiu).ThenBy(a=>a.Subramura);
            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown

                selectList.Add(new SelectListItem
                {
                    Value = category.SubramuraSpecializareId.ToString(),
                    Text = GetSpec(category)
                });
            }
            // returnam lista de categorii
            return selectList;
        }
      
       

        // GET: Student
        public ActionResult Index()
        {
            int x = 10;//sortare default
            Student st = new Student();
            st.Integralist = x;

            TempData["messageIndex"] = "Sortarea este implicita: Program de studiu->An studiu->Domeniu->Specializare->Integralist->Medie";
            ViewBag.message = TempData["messageIndex"].ToString();
            var selectList = new List<Student>();
            var produse = from prod in db.Studenti select prod;
            produse = produse.OrderBy(m => m.SpecializareStudent.Domeniu.Program_de_studiuId).ThenBy(m => m.SpecializareStudent.AnStudiu).
                ThenBy(m => m.SpecializareStudent.SpecializareId).ThenBy(m => m.SubramuraSpecializareId).ThenBy(m=>m.Integralist).
                ThenByDescending(m => m.MedieAnAnterior_Admitere);

            foreach (var p in produse)
            {
                selectList.Add(p);
            }
            selectList.Add(st);
            return View(selectList);
        }

        public ActionResult IndexSort(bool ProgramStudii=false, bool Domeniu=false , bool An=false )
        {
            int count = 0;
            if (ProgramStudii == true)
                count++;
            if (Domeniu == true)
                count++;
            if (An == true)
                count++;
            if (count == 0)
                return RedirectToAction("Index");

            int x;
            Student st = new Student();
            var selectList = new List<Student>();
            var produse = from prod in db.Studenti select prod;
            switch (count)
            {
                case 3:
                    x = 11;
                    st.Integralist = x;
                    TempData["messageIndex"] = "Criteriu sortare: Program de studiu->Domeniu->An studiu->Integralist->Medie";
                    ViewBag.message = TempData["messageIndex"].ToString();
                    produse = produse.OrderBy(m => m.SpecializareStudent.Domeniu.Program_de_studiuId).ThenBy(m => m.SpecializareStudent.Domeniu.SpecializareId).
                        ThenBy(m => m.SpecializareStudent.AnStudiu).ThenBy(m => m.Integralist).ThenByDescending(m => m.MedieAnAnterior_Admitere);
                    break;
                case 1:
                    if (ProgramStudii == true) { 
                        x = 12;
                        st.Integralist = x;
                        TempData["messageIndex"] = "Criteriu sortare: Program de studiu->Integralist->Medie";
                        ViewBag.message = TempData["messageIndex"].ToString();
                        produse = produse.OrderBy(m => m.SpecializareStudent.Domeniu.Program_de_studiuId).ThenBy(m => m.Integralist).ThenByDescending(m => m.MedieAnAnterior_Admitere);
                    }
                    if (Domeniu == true)
                    {
                        x = 13;
                        st.Integralist = x;
                        TempData["messageIndex"] = "Criteriu sortare: Domeniu->Integralist->Medie";
                        ViewBag.message = TempData["messageIndex"].ToString();
                        produse = produse.OrderBy(m => m.SpecializareStudent.SpecializareId).ThenBy(m => m.Integralist).ThenByDescending(m => m.MedieAnAnterior_Admitere);
                    }
                    if (An == true)
                    {
                        x = 14;
                        st.Integralist = x;
                        TempData["messageIndex"] = "Criteriu sortare: An->Integralist->Medie";
                        ViewBag.message = TempData["messageIndex"].ToString();
                        produse = produse.OrderBy(m => m.SpecializareStudent.AnStudiu).ThenBy(m => m.Integralist).ThenByDescending(m => m.MedieAnAnterior_Admitere);
                    }
                    break;
                case 2:
                    if(ProgramStudii==true && An == true)
                    {
                        x = 11;
                        st.Integralist = x;
                        TempData["messageIndex"] = "Criteriu sortare:Program de studiu->An->Integralist->Medie";
                        ViewBag.message = TempData["messageIndex"].ToString();
                        produse = produse.OrderBy(m=>m.SpecializareStudent.Domeniu.Program_de_studiuId).
                            OrderBy(m => m.SpecializareStudent.AnStudiu).ThenBy(m => m.Integralist).ThenByDescending(m => m.MedieAnAnterior_Admitere);

                    }
                    if (ProgramStudii == true && Domeniu == true)
                    {
                        x = 13;
                        st.Integralist = x;
                        TempData["messageIndex"] = "Criteriu sortare:Program de studiu->Domeniu->Integralist->Medie";
                        ViewBag.message = TempData["messageIndex"].ToString();
                        produse = produse.OrderBy(m => m.SpecializareStudent.Domeniu.Program_de_studiuId).
                            OrderBy(m => m.SpecializareStudent.Domeniu.SpecializareId).ThenBy(m => m.Integralist).ThenByDescending(m => m.MedieAnAnterior_Admitere);

                    }
                    if (An == true && Domeniu == true)
                    {
                        x = 11;
                        st.Integralist = x;
                        TempData["messageIndex"] = "Criteriu sortare:Domeniu->An->Integralist->Medie";
                        ViewBag.message = TempData["messageIndex"].ToString();
                        produse = produse.OrderBy(m => m.SpecializareStudent.Domeniu.SpecializareId).ThenBy(m=>m.SpecializareStudent.AnStudiu).
                            ThenBy(m => m.Integralist).ThenByDescending(m => m.MedieAnAnterior_Admitere);

                    }
                    break;
            }


            foreach (var p in produse)
            {
                selectList.Add(p);
            }
             selectList.Add(st);
            return View("Index",selectList);
        }

        public ActionResult New()
        {
            if(TempData.ContainsKey("message"))
            {
                TempData["message"] = "Studentul a fost adaugat!";
                ViewBag.message = TempData["message"].ToString();
            }
             Student st = new Student();
            st.AllDomenii = GetAllDomenii();
            return View(st);
        }

        

        [HttpPost]
       // [Authorize(Roles = "Administrator")]
        public ActionResult New(Student st)
        {
            TempData.Remove("message1");
            TempData.Remove("message");

            st.AllDomenii = GetAllDomenii();
            if (ModelState.IsValid)
            {
                var stud = db.Studenti;
                var ssC = stud.Where(m => m.CNP == st.CNP);
                if (ssC.Any())
                {
                    TempData["message1"] = "CNP-ul trebuie sa fie unic! Exista un alt student cu acelasi CNP!";
                    ViewBag.message = TempData["message1"].ToString();
                    return View(st);
                }
                db.Studenti.Add(st);
                st.AllDomenii = GetAllDomenii();
                db.SaveChanges();

                TempData["message"] = "Studentul a fost adaugat!";
                ViewBag.message = TempData["message"].ToString();

                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                
                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName =st.CNP;
                user.Email = st.CNP;

                string ss = st.CNP;
                char[] p1 = ss.ToCharArray(1, 2);
                char[] p2 = ss.ToCharArray(3, 2);
                char[] p3 = ss.ToCharArray(5, 2);
                string pp1 = new string(p1);
                string pp2 = new string(p2);
                string pp3 = new string(p3);
                string pass = string.Concat(pp3, "-",pp2,"-",pp1);
                
                //user.Email = pass;
                var adminCreated = UserManager.Create(user, pass);
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "User");
                }
               
                return RedirectToAction("New");
            }
            else
                return View(st);


        }

        public ActionResult Edit(int StudentId)
        {
            if (TempData.ContainsKey("message"))
            {
                TempData["message"] = "Studentul a fost editat cu succes!";
                ViewBag.message = TempData["message"].ToString();
            }
            Student st = new Student();
            st = db.Studenti.Find(StudentId);
            st.AllDomenii = GetAllDomenii();
            return View(st);
        }


        [HttpPost]
        // [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int StudentId, Student stReq)
        {
            TempData.Remove("message1");
            TempData.Remove("message");

            Student st = db.Studenti.Find(StudentId);
            var cnpVechi = st.CNP;
            st.Nume = stReq.Nume;
            st.Prenume = stReq.Prenume;
            st.Integralist = stReq.Integralist;
            st.MedieAnAnterior_Admitere = stReq.MedieAnAnterior_Admitere;
            st.AllDomenii = GetAllDomenii();
            st.SubramuraSpecializareId = stReq.SubramuraSpecializareId;
            st.CNP = stReq.CNP;
            if (ModelState.IsValid)
            {
                var stud = db.Studenti;
                var ssC = stud.Where(m => m.CNP == stReq.CNP && stReq.CNP!=st.CNP);
                if (ssC.Any())
                {
                    TempData["message1"] = "CNP-ul trebuie sa fie unic! Exista un alt student cu acelasi CNP!";
                    ViewBag.message = TempData["message1"].ToString();
                    return View(st);
                }
                db.SaveChanges();

                TempData["message"] = "Studentul a fost editat cu succes!";
                ViewBag.message = TempData["message"].ToString();

                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var users=from prod in db.Users select prod;
                var user = new ApplicationUser();
                user = users.Where(m => m.UserName == cnpVechi).First();
                user.UserName = st.CNP;
                user.Email = st.CNP;

                string ss = st.CNP;
                char[] p1 = ss.ToCharArray(1, 2);
                char[] p2 = ss.ToCharArray(3, 2);
                char[] p3 = ss.ToCharArray(5, 2);
                string pp1 = new string(p1);
                string pp2 = new string(p2);
                string pp3 = new string(p3);
                string pass = string.Concat(pp3, "-", pp2, "-", pp1);

               // user.Email = pass;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
                return View(st);


        }


    }
}
