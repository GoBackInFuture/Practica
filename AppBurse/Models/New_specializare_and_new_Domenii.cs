using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
    public class New_specializare_and_new_Domenii
    {
        public Domeniu Dom { get; set; }
        public IEnumerable<Domeniu> AllDomenii { get; set; }
    }
}