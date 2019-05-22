using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required(ErrorMessage ="Numele este obligatoriu")][RegularExpression("^[A-Z].*",ErrorMessage ="Numele incep cu litera mare!")]
        public string Nume { get; set; }
        [Required(ErrorMessage ="Prenumele este obligatoriu")]
        [RegularExpression("^[A-Z].*",ErrorMessage ="Prenumele incep cu litera mare!")]
        public string Prenume { get; set; }
        
        [Required(ErrorMessage ="CNP este obligatoriu")]
        [MinLength(13,ErrorMessage ="CNP trebuie sa aiba 13 cifre")][MaxLength(13, ErrorMessage = "CNP trebuie sa aiba 13 cifre")]
        [Index(IsUnique =true)]
        public string CNP { get; set; }
        [Range(1, 10,ErrorMessage ="Media trebuie sa fie intre 1 si 10")]
        [DisplayName("Medie an Anterior/Admitere")]
        public float MedieAnAnterior_Admitere { get; set; }
        public int Integralist { get; set; }
        [Required(ErrorMessage ="Specializarea este obligatorie")]
        public int  SubramuraSpecializareId { get; set; }

        public IEnumerable<SelectListItem> AllDomenii { get; set; }


        public virtual Specializare_pe_Domeniu SpecializareStudent { get; set; }
       
    }
}