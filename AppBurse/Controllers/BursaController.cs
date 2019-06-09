using AppBurse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Controllers
{
	[Authorize]
    public class BursaController : Controller
    {
		private ApplicationDbContext db = ApplicationDbContext.Create();
		// GET: Bursa
		[Authorize(Roles = "User,Secretar,Administrator")]
		public ActionResult Index()
		{
			var burse = db.Burse;
			if (TempData.ContainsKey("message"))
			{
				ViewBag.message = TempData["message"].ToString();
			}
			ViewBag.Burse = burse;
			return View();
		}

		[Authorize(Roles = "User,Secretar,Administrator")]
		public ActionResult Show(int id)
		{
			Bursa bursa = db.Burse.Find(id);
			ViewBag.Bursa = bursa;
			ViewBag.afisareButoane = false;
			if (User.IsInRole("Secretar") || User.IsInRole("Administrator"))
			{
				ViewBag.afisareButoane = true;
			}
			ViewBag.esteAdmin = User.IsInRole("Administrator");
			return View(bursa);

		}

		[Authorize(Roles = "Administrator")]
		public ActionResult New()
		{
			Bursa bursa = new Bursa();
			return View(bursa);
		}

		[HttpPost]
		[Authorize(Roles = "Administrator")]
		public ActionResult New(Bursa bursa)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Burse.Add(bursa);
					db.SaveChanges();
					TempData["message"] = "Bursa a fost adaugata!";
					return RedirectToAction("Index");
				}
				else
				{
					return View(bursa);
				}
			}
			catch (Exception e)
			{
				return View(bursa);
			}
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
		{

			Bursa bursa = db.Burse.Find(id);
			ViewBag.Bursa = bursa;

			return View(bursa);
		}

		[HttpPut]
		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, Bursa requestBursa)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Bursa bursa = db.Burse.Find(id);

					if (TryUpdateModel(bursa))
					{
						bursa.Nume = requestBursa.Nume;
						bursa.Cuantum = requestBursa.Cuantum;
						bursa.MedieMinima = requestBursa.MedieMinima;
						db.SaveChanges();
						TempData["message"] = "Bursa a fost modificata!";
					}
					return RedirectToAction("Index");

				}
				else
				{
					return View();
				}

			}
			catch (Exception e)
			{
				return View();
			}
		}


		[HttpDelete]
		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
		{
			Bursa bursa = db.Burse.Find(id);

			db.Burse.Remove(bursa);
			db.SaveChanges();
			TempData["message"] = "Bursa a fost stersa!";
			return RedirectToAction("Index");
		}

	}
}