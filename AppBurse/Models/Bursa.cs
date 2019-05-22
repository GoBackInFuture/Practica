using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
	public class Bursa
	{
		[Key]
		public int BursaId { get; set; }
		[Required]
		public string Nume { get; set; }
		[Required]
		public double Cuantum { get; set; }
		public double MedieMinima { get; set; }
	}
}