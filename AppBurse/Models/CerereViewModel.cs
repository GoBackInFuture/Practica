using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
	public class CerereViewModel
	{
		public Cerere Formular { get; set; }
		public HttpPostedFileBase Document { get; set; }
	}
}