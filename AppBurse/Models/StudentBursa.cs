using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
	public class StudentBursa
	{
		[Key]
		public int StudentBursaId { get; set; }
		public int StudentId { get; set; }
		public int BursaId { get; set; }
	}
}