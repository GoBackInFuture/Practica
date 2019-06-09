using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
	public class Buget
	{
		[Key]
		public int BugetId { get; set; }
		public int BugetLunar { get; set; }
		public int BugetSocial { get; set; }
		public int AnScolar { get; set; }
	}
}