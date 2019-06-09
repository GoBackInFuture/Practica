using AppBurse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Controllers
{
    public class BugetController : Controller
    {
		private ApplicationDbContext db = ApplicationDbContext.Create();
		// GET: Buget
		public ActionResult Index()
        {
			var specializari = db.Specializari.ToList();
			Dictionary<Specializare_pe_Domeniu, List<Tuple<Student, Bursa>>> bigList = new Dictionary<Specializare_pe_Domeniu, List<Tuple<Student, Bursa>>>();
			foreach (Specializare_pe_Domeniu specializare in specializari)
			{
				bigList.Add(specializare, new List<Tuple<Student, Bursa>>());
				var studIdList = db.Studenti.Where(x => x.SubramuraSpecializareId == specializare.SubramuraSpecializareId)
					.Select(x => x.StudentId).ToList();
				var studburs = db.StudentBurse.Where(x => studIdList.Contains(x.StudentId)).ToList();
				foreach (StudentBursa sb in studburs)
				{
					Student student = db.Studenti.Find(sb.StudentId);
					Bursa bursa = db.Burse.Find(sb.BursaId);
					Tuple<Student, Bursa> tuple = new Tuple<Student, Bursa>(student, bursa);
					bigList[specializare].Add(tuple);
				}
				bigList[specializare].OrderByDescending(x => x.Item1.MedieAnAnterior_Admitere);
			}
            return View(bigList);
        }

		public ActionResult Distribution()
		{
			Buget buget = new Buget();
			return View(buget);
		}

		[HttpPut]
		public ActionResult Distribution(Buget buget)
		{
			int cuantumSocial = db.Burse.Where(x => x.Nume.Equals("Sociala")).First().Cuantum;
			int cuantumPerformanta = db.Burse.Where(x => x.Nume.Equals("Performanta")).First().Cuantum;
			int cuantumMerit2 = db.Burse.Where(x => x.Nume.Equals("Merit II")).First().Cuantum;
			int cuantumMerit = db.Burse.Where(x => x.Nume.Equals("Merit I")).First().Cuantum;
			int nrBurseSociale = SelectSociala(buget);
			int bugetRamas = buget.BugetLunar - nrBurseSociale * cuantumSocial;
			int nrBursePerformanta = SelectPerformanta();
			int nrBurseMerit2 = SelectMerit2();
			bugetRamas -= nrBursePerformanta * cuantumPerformanta + nrBurseMerit2 * cuantumMerit2;
			int Nrez = Convert.ToInt32(Math.Floor((double)bugetRamas / cuantumMerit));
			Dictionary<Tuple<int, int>, List<Student>> clasificare = new Dictionary<Tuple<int, int>, List<Student>>();
			Dictionary<Tuple<int, int>, int> nspec = new Dictionary<Tuple<int, int>, int>();
			var domenii = db.Domenii.ToList();

			int idMerit1 = db.Burse.Where(x => x.Nume.Equals("Merit I")).Select(x => x.BursaId).First();

			foreach (Domeniu domeniu in domenii)
			{
				if (domeniu.EsteClasSpec)
				{
					var specializari = db.Specializari.Where(x => x.SpecializareId == domeniu.SpecializareId).ToList();
					foreach (Specializare_pe_Domeniu specializare in specializari)
					{
						Tuple<int, int> item = new Tuple<int, int>(domeniu.SpecializareId, specializare.SubramuraSpecializareId);
						clasificare.Add(item, new List<Student>());
					}
				}
				else
				{
					Tuple<int, int> item = new Tuple<int, int>(domeniu.SpecializareId, -1);
					clasificare.Add(item, new List<Student>());
				}
			}

			int N = db.Studenti.Count();
			int nramas = Nrez - nrBursePerformanta + nrBurseMerit2;

			foreach (KeyValuePair<Tuple<int, int>, List<Student>> item in clasificare)
			{
				if (item.Key.Item2 != -1)
				{
					var studenti = db.Studenti.Where(stud => stud.SubramuraSpecializareId == item.Key.Item2)
						.Where(x => x.Integralist == 1).ToList();
					studenti.OrderByDescending(x => x.MedieAnAnterior_Admitere);
					clasificare.Add(item.Key, studenti);
				}
				else
				{
					var studenti = db.Studenti.Join(db.Specializari,
						student => student.SubramuraSpecializareId,
						spec => spec.SubramuraSpecializareId,
						(student, spec) => new { Student = student, Spec = spec })
						.Where(StudSpec => StudSpec.Spec.SpecializareId == item.Key.Item1)
						.Select(StudSpec => StudSpec.Student)
						.Where(x => x.Integralist == 1).ToList();
					studenti.OrderByDescending(x => x.MedieAnAnterior_Admitere);
					clasificare.Add(item.Key, studenti);
				}
			}

			foreach (KeyValuePair<Tuple<int, int>, List<Student>> item in clasificare)
			{
				int ns = Convert.ToInt32(Math.Floor((double)item.Value.Count() * Nrez / N));
				ns -= NrBurseCat(item.Key);
				nspec.Add(item.Key, ns);
				foreach (Student student in item.Value)
				{
					if (ns <= 0)
						break;
					if (db.StudentBurse.Any(x => x.StudentId == student.StudentId))
						continue;
					StudentBursa sb = new StudentBursa
					{
						StudentId = student.StudentId,
						BursaId = idMerit1
					};
					db.StudentBurse.Add(sb);
					--ns;
					--nramas;
					db.SaveChanges();
				}
			}

			List<Student> ultclas = db.Studenti.Where(x => x.MedieAnAnterior_Admitere >= 8)
				.Where(x => db.StudentBurse.Any(y => y.StudentId == x.StudentId) == false).ToList();

			foreach (Student student in ultclas)
			{
				if (nramas <= 0)
					break;
				StudentBursa sb = new StudentBursa
				{
					StudentId = student.StudentId,
					BursaId = idMerit1
				};
				db.StudentBurse.Add(sb);
				--nramas;
				db.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		[NonAction]
		public int SelectSociala(Buget buget)
		{
			return 0;
		}

		[NonAction]
		public int SelectPerformanta()
		{
			return 0;
		}

		[NonAction]
		public int SelectMerit2()
		{
			int idMerit2 = db.Burse.Where(x => x.Nume.Equals("Merit II")).Select(x => x.BursaId).First();
			int burseAcordate = 0;
			List<Student> studenti = db.Studenti.Where(x => x.MedieAnAnterior_Admitere == 10)
				.Where(x => db.StudentBurse.Any(y => y.StudentId == x.StudentId) == false).ToList();
			foreach (Student student in studenti)
			{
				StudentBursa sb = new StudentBursa
				{
					StudentId = student.StudentId,
					BursaId = idMerit2
				};
				db.StudentBurse.Add(sb);
				++burseAcordate;
				db.SaveChanges();
			}
			return 0;
		}

		[NonAction]
		public int NrBurseCat(Tuple<int, int> tuplu)
		{
			var studenti = new List<int>();
			if (tuplu.Item2 < 0)
				studenti = db.Studenti.Join(db.Specializari,
							student => student.SubramuraSpecializareId,
							spec => spec.SubramuraSpecializareId,
							(student, spec) => new { Student = student, Spec = spec })
							.Where(StudSpec => StudSpec.Spec.SpecializareId == tuplu.Item1)
							.Select(StudSpec => StudSpec.Student.StudentId)
							.ToList();
			else
				studenti = db.Studenti.Where(stud => stud.SubramuraSpecializareId == tuplu.Item2)
						.Select(x => x.StudentId).ToList();
			int nr = db.StudentBurse.Where(x => studenti.Contains(x.StudentId)).Count();
			return nr;
		}
	}
}