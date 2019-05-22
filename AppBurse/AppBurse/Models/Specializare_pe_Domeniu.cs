using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Models
{
    public class Specializare_pe_Domeniu
    {
        [Key]
        public int SubramuraSpecializareId { get; set; }      
        public int AnStudiu { get; set; }
        [Required(ErrorMessage = "Denumirea este obligatorie")]
        [DisplayName("Specializare")]
        public string Subramura { get; set; }
        [Required]
        public int SpecializareId { get; set; }

        public IEnumerable<SelectListItem> Ani { get; set; }

        public IEnumerable<SelectListItem> SpecializariSpec { get; set; }

        public virtual Domeniu Domeniu { get; set; }

    }
}