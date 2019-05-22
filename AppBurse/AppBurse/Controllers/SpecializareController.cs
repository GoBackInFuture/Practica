using AppBurse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SpecializareController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: RamuraSpecializare

        [NonAction]
        public IEnumerable<SelectListItem> GetAni(int id)
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            Domeniu sp = new Domeniu();
            sp = db.Domenii.Find(id);
            int nr = sp.NrAniStudiu;
            // iteram prin categorii
            
            for (int index = 1; index <= nr; index++)
            {
              
            selectList.Add(new SelectListItem
                {
                    Value =index.ToString(),
                    Text = index.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New(int id)
        {
            New_subramura_and_all_Specializari_per_Domeniu ss = new New_subramura_and_all_Specializari_per_Domeniu();
            ss.SSpec = new Specializare_pe_Domeniu();
            ss.SSpec.SpecializareId = id;
            Domeniu sp = db.Domenii.Find(id);
            ss.SSpec.Domeniu = sp;
            ss.SSpec.Ani = GetAni(id);
            Session["nr"] = id;
            var spec = from spc in db.Specializari where spc.SpecializareId==id select spc;
            spec = spec.OrderBy(s => s.AnStudiu);
            var selectList = new List<Specializare_pe_Domeniu>();
            if(spec!=null)
            foreach (var p in spec)
                selectList.Add(p);
            ss.AllSpecializariSpec = selectList;
            return View(ss);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult New(int id,New_subramura_and_all_Specializari_per_Domeniu sp)
        {
            sp.SSpec.SpecializareId = Convert.ToInt32(Session["nr"]);
            Domeniu ssp = db.Domenii.Find(id);
            sp.SSpec.Domeniu = ssp;
            sp.SSpec.Ani = GetAni(Convert.ToInt32(Session["nr"]));
            int nn = Convert.ToInt32(Session["nr"]);
            var spec = from spc in db.Specializari where spc.SpecializareId == nn  select spc;
            spec = spec.OrderBy(s => s.AnStudiu);
            var selectList = new List<Specializare_pe_Domeniu>();
            foreach (var p in spec)
                selectList.Add(p);
            sp.AllSpecializariSpec = selectList;

            if (ModelState.IsValid)
            {
                sp.SSpec.SpecializareId = Convert.ToInt32(Session["nr"]);
                db.Specializari.Add(sp.SSpec);
                sp.SSpec.Ani = GetAni(Convert.ToInt32(Session["nr"]));
                db.SaveChanges();
                TempData["message"] = "Specializarea a fost adaugata!";
                return RedirectToAction("New");
            }
            else
                return View(sp);
        }

        public ActionResult Delete(int id,int id_rev)
        {
            Specializare_pe_Domeniu sp = db.Specializari.Find(id);

            db.Specializari.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("New", "Specializare",new { id = id_rev });
        }

        public ActionResult CompleteSpecializari()
        {
            //var prog = from pr in db.Programe_de_studiu select pr;
            //var prog = db.Programe_de_studiu.Select(m=>m.Program_de_studiuId);
            var prog = getPrograme();
            // iteram prin categorii
            foreach (var p in prog)
            {
                //var spec = db.Domenii.Where(m => m.Program_de_studiuId == p);
                // var spec = from sp in db.Domenii where sp.Program_de_studiuId == p.Program_de_studiuId select sp;
                var spec = getDomenii(p);
                foreach(var s in spec)
                {
                    var List  = from rams in db.Specializari where s.SpecializareId == rams.SpecializareId select rams.AnStudiu;
                    for(int i = 1; i <= s.NrAniStudiu; i++)
                    {
                        if (!List.Contains(i))
                        {
                            Specializare_pe_Domeniu ssp = new Specializare_pe_Domeniu();
                            ssp.AnStudiu = i;
                            ssp.SpecializareId = s.SpecializareId;
                            ssp.Subramura = s.Denumire;
                            db.Specializari.Add(ssp);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("FormatiuniFinalizate");
        }

        public List<int> getPrograme()
        {
            var prog = db.Programe_de_studiu.Select(m => m.Program_de_studiuId).ToList();
            return prog;
        }
        public List<Domeniu> getDomenii(int p)
        {
           var spec= db.Domenii.Where(m => m.Program_de_studiuId == p).ToList();
            return spec;
        }

        public ActionResult FormatiuniFinalizate()
        {
            
            FormatiuniFinalizate ff = new FormatiuniFinalizate();
            ff.AllPrograme = db.Programe_de_studiu.ToList();
            ff.AllDomenii = db.Domenii.ToList();
            ff.AllSpecializariSpec = db.Specializari.ToList();
            return View(ff);

        }
    }
}