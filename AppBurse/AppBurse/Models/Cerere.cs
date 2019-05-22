using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Models
{
	public class Cerere
	{
		[Key]
		public int CerereId { get; set; }
		[Required(ErrorMessage = "Va rugam sa introduceti specializarea dumneavoastra.")]
		public string Specializare { get; set; }
		[Required(ErrorMessage = "Va rugam sa introduceti anul in care studiati.")]
		public int An { get; set; }
		[Required(ErrorMessage = "Va rugam sa introduceti grupa in care studiati.")]
		public int Grupa { get; set; }
		[Required][Range(1,10,ErrorMessage = "Media este gresita.")]
		public double Medie { get; set; }
		[Required(ErrorMessage = "Va rugam sa introduceti numarul de credite pe care le-ati obtinut.")]
		[Display(Name = "Numar de credite")]
		public int NrCredite { get; set; }
		[Required(ErrorMessage = "Va rugam sa introduceti CNP-ul dumneavoastra.")]
		[MinLength(13,ErrorMessage = "Va rugam sa introduceti un CNP valid.")]
		[MaxLength(13,ErrorMessage = "Va rugam sa introduceti un CNP valid.")]
		public string CNP { get; set; }
		[Required(ErrorMessage = "Va rugam sa introduceti serie cartii de identitate.")]
		[MinLength(2,ErrorMessage = "Va rugam sa introduceti o serie valida.")]
		[MaxLength(2,ErrorMessage = "Va rugam sa introduceti o serie valida.")]
		[Display(Name = "Seria CI")]
		public string SerieCI { get; set; }
		[Required(ErrorMessage = "Va rugam sa introduceti numarul cartii de identitate.")]
		[MinLength(6,ErrorMessage = "Va rugam sa introduceti un numar valid al cartii de identitate.")]
		[MaxLength(6, ErrorMessage = "Va rugam sa introduceti un numar valid al cartii de identitate.")]
		[Display(Name = "Numar CI")]
		public string NumarCI { get; set; }
		[Required(ErrorMessage = "Va rugam sa selectati bursa dorita.")]
		[Display(Name = "Bursa ceruta")]
		public int IdBursaCeruta { get; set; }
		//public Bursa BursaCeruta { get; set; }
		public IEnumerable<SelectListItem> Burse { get; set; }
		public byte[] Document { set; get; }
		public string UserId { set; get; }
		public virtual ApplicationUser User { set; get; }
	}
}