using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBurse.Models
{
    public class Model_For_New_Account
    {
        public RegisterViewModel user_to_register { get; set; }
        public IEnumerable<SelectListItem> AllRoles { get; set; }
    }
}