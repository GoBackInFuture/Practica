using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
    public class Program_de_studiu
    {
        [Key]
        public int Program_de_studiuId { get; set; }
        [Required(ErrorMessage ="Denumirea este obligatorie")]
        public string Denumire { get; set; }

        public virtual ICollection<Domeniu> Specializare { get; set; }
    }
}