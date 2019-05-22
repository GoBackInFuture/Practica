using AppBurse.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Controllers
{
    public class CerereController : Controller
    {
		private ApplicationDbContext db = ApplicationDbContext.Create();
		// GET: Cerere
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult Show(int id)
		{
			Cerere cerere = db.Cereri.Find(id);
			return View(cerere);
		}

		public ActionResult New()
		{
			var item = db.Burse.Include("Formular");
			CerereViewModel cerere = new CerereViewModel();
			cerere.Formular = new Cerere();
			cerere.Formular.Burse = GetToateBursele();
			cerere.Formular.UserId = User.Identity.GetUserId();
			return View(cerere);
		}

		[HttpPost]
		public ActionResult New(CerereViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (model.Document != null)
					{
						var uploadedFile = new byte[model.Document.InputStream.Length];
						model.Document.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
						model.Formular.Document = uploadedFile;
					}
					model.Formular.UserId = User.Identity.GetUserId();
					db.Cereri.Add(model.Formular);
					db.SaveChanges();
					TempData["message"] = "Cererea a fost trimisa!";
					return RedirectToAction("Index");
				}
				else
				{
					model.Formular.Burse = GetToateBursele();
					return View(model);
				}
			}
			catch (Exception e)
			{
				return View(model);
			}
		}

		[NonAction]
		public IEnumerable<SelectListItem> GetToateBursele()
		{
			var selectList = new List<SelectListItem>();
			
			var burse = from bur in db.Burse select bur;

			foreach (var bursa in burse)
			{
				selectList.Add(new SelectListItem
				{
					Value = bursa.BursaId.ToString(),
					Text = "Bursa " + bursa.Nume.ToString()
				});
			}

			return selectList;
		}
	}
}