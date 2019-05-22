using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBurse.Models
{
    public class New_program_and_all_programs
    {
        public IEnumerable<Program_de_studiu> Programe_de_studiu { get; set; }
        public Program_de_studiu Program_studiu { get; set; }
    }
}