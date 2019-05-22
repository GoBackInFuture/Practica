using AppBurse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DomeniuController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [NonAction]
        public IEnumerable<SelectListItem> GetAllPrograms()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Programe_de_studiu select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.Program_de_studiuId.ToString(),
                    Text = category.Denumire.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        // GET: Specializare
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult New()
        {
            
            New_specializare_and_new_Domenii sp = new New_specializare_and_new_Domenii();
            sp.Dom = new Domeniu();

            sp.Dom.Programe = GetAllPrograms();

            
            var spec = from spc in db.Domenii select spc;
            spec = spec.OrderBy(s => s.Program_de_studiuId);

            var selectList = new List<Domeniu>();
            foreach (var p in spec)
            selectList.Add(p);
            sp.AllDomenii = selectList;



            return View(sp);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult New(New_specializare_and_new_Domenii specializare)
        {
            specializare.Dom.Programe = GetAllPrograms();


            var spec = from spc in db.Domenii select spc;
            spec = spec.OrderBy(s => s.Program_de_studiuId);

            var selectList = new List<Domeniu>();
            foreach (var p in spec)
                selectList.Add(p);
            specializare.AllDomenii = selectList;

            if (ModelState.IsValid)
            {   
                db.Domenii.Add(specializare.Dom);
                specializare.Dom.Programe = GetAllPrograms();
                db.SaveChanges();
                TempData["message"] = "Specializarea a fost adaugata!";
                return RedirectToAction("New");
            }
            else
                return View(specializare);
              
           
        }

        public ActionResult Delete(int id)
        {
            Domeniu sp = db.Domenii.Find(id);

            db.Domenii.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("New", "Domeniu");
        }

    
    }
}
