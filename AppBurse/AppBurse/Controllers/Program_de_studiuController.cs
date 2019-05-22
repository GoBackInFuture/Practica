using AppBurse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class Program_de_studiuController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Program_de_studiu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            TempData["message"] = "Pentru o configurare corecta trebuie sa parcurgeti toti pasii, inclusiv cel de finalizare!";
            ViewBag.message = TempData["message"].ToString();
            New_program_and_all_programs ps = new New_program_and_all_programs();
            ps.Program_studiu = new Program_de_studiu();
            var prg = from prgs in db.Programe_de_studiu select prgs;

            var selectList = new List<Program_de_studiu>();
            foreach (var p in prg)
                selectList.Add(p);
            ps.Programe_de_studiu = selectList;
            return View(ps);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult New(New_program_and_all_programs ps)
        {
            var prg = from prgs in db.Programe_de_studiu select prgs;

            var selectList = new List<Program_de_studiu>();
            foreach (var p in prg)
                selectList.Add(p);
            ps.Programe_de_studiu = selectList;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Programe_de_studiu.Add(ps.Program_studiu);
                    db.SaveChanges();
                    TempData["message"] = "Programul de studiu a fost adaugat!";
                    return RedirectToAction("New");
                }
                else
                {
                    return View(ps);
                }
            }
            catch (Exception e)
            {
                return View(ps);
            }
        }

        // GET: Program_de_studiu/Delete/5
        public ActionResult Delete(int id)
        {
            Program_de_studiu ps = db.Programe_de_studiu.Find(id);

                    db.Programe_de_studiu.Remove(ps);
                    db.SaveChanges();
                
            

            return RedirectToAction("New", "Program_de_studiu");
        }
    }
}
