using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
    public class New_subramura_and_all_Specializari_per_Domeniu
    {
        public Specializare_pe_Domeniu SSpec { get; set; }
        public IEnumerable<Specializare_pe_Domeniu> AllSpecializariSpec { get; set; }
    }
}