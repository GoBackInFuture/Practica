using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Models
{
    public class Domeniu
    {
        [Key]
        public int SpecializareId { get; set; }
        [Required(ErrorMessage = "Denumirea este obligatorie")]
        public string Denumire { get; set; }
        [Required][Range(1, Int32.MaxValue,ErrorMessage = "Numarul anilor de studiu e minim 1")]
        public int NrAniStudiu { get; set; }
        [Required(ErrorMessage ="Programul de studiu e obligatoriu")]
        public int Program_de_studiuId { get; set; }


        public IEnumerable<SelectListItem> Programe { get; set; }



        public virtual Program_de_studiu Program_de_studiu { get; set; }
        public virtual ICollection<Specializare_pe_Domeniu> SubramuraDomeniu { get; set; }

       
    }
}