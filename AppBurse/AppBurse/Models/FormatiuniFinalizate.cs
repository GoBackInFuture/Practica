using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
    public class FormatiuniFinalizate
    {
        public IEnumerable<Program_de_studiu> AllPrograme { get; set; }
        public IEnumerable<Domeniu> AllDomenii { get; set; }
        public IEnumerable<Specializare_pe_Domeniu> AllSpecializariSpec { get; set; }
    }
}