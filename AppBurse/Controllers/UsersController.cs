using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AppBurse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proict.Controllers
{
	[Authorize(Roles = "Administrator")]
	public class UsersController : Controller
	{
		private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Index()
		{
            var users=from user in db.Users
                      where 1==0
                      select user;
            var users1 = from user in db.Users
                        where 1 == 0
                        select user;
            List<IdentityRole> roles1 = db.Roles.Where(r => r.Name == "Secretar").ToList();
            List<IdentityRole>  roles = db.Roles.Where(r => r.Name == "Administrator").ToList();
            if (roles.Any())
            {
                var roleId = roles.First().Id;
                users= from user in db.Users
                       where user.Roles.Any(r => r.RoleId == roleId)
                       select user;
            }

            if (roles1.Any())
            {
                var roleId1 = roles1.First().Id;
                users1 = from user in db.Users
                        where user.Roles.Any(r => r.RoleId == roleId1)
                        select user;
            }
            /* var users = from user in db.Users
                         orderby user.UserName
                         select user;*/
            
            ViewBag.UsersList = users;
                ViewBag.UsersList1 = users1;
			return View();
		}

		
		[NonAction]
		public IEnumerable<SelectListItem> GetAllRoles()
		{
			var selectList = new List<SelectListItem>();
			var roles = from role in db.Roles select role;
			foreach (var role in roles)
			{
                if (role.Name != "User")
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = role.Id.ToString(),
                        Text = role.Name.ToString()
                    });
                }
			}
			return selectList;
		}

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRolesForNew()
        {
            var selectList = new List<SelectListItem>();
            var roles = from role in db.Roles select role;
            foreach (var role in roles)
            {
                if (role.Name != "User")
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = role.Name.ToString(),
                        Text = role.Name.ToString()
                    });
                }
            }
            return selectList;
        }

        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }

        [HttpPut]
		public ActionResult Edit(string id, ApplicationUser newData)
		{

			ApplicationUser user = db.Users.Find(id);
			user.AllRoles = GetAllRoles();
			var userRole = user.Roles.FirstOrDefault();
			ViewBag.userRole = userRole.RoleId;
			try
			{
				ApplicationDbContext context = new ApplicationDbContext();
				var roleManager = new RoleManager<IdentityRole>(new
			   RoleStore<IdentityRole>(context));
				var UserManager = new UserManager<ApplicationUser>(new
			   UserStore<ApplicationUser>(context));

				if (TryUpdateModel(user))
				{
					var roles = from role in db.Roles select role;
					foreach (var role in roles)
					{
						UserManager.RemoveFromRole(id, role.Name);
					}
					var selectedRole =
					db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
					UserManager.AddToRole(id, selectedRole.Name);
					db.SaveChanges();
				}
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				Response.Write(e.Message);
				return View(user);
			}

		}
        public ActionResult New()
        {
            Model_For_New_Account user = new Model_For_New_Account();
            user.AllRoles = GetAllRolesForNew();
            return View(user);
        }

        [HttpPut]
        public ActionResult New(Model_For_New_Account u,string newRole)
        {
            ApplicationUser user = new ApplicationUser();
            u.AllRoles = GetAllRolesForNew();
            try
            {
                   
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // se adauga utilizatorul administrator
                user.Email = u.user_to_register.Email;
                user.UserName = u.user_to_register.Email;
                user.AllRoles = GetAllRoles();

               // return JavaScript("window.open('"+ newRole+"')");

                if (u.user_to_register.Password==u.user_to_register.ConfirmPassword)
                {
                    //user.Email = pass;
                    var adminCreated = UserManager.Create(user,u.user_to_register.Password);
                    if (adminCreated.Succeeded)
                    {
                        UserManager.AddToRole(user.Id,newRole);
                    }
                }
                else return View(u);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                return View(u);
            }

        }

        public ActionResult Delete(string id)
        {
            ApplicationUser u = new ApplicationUser();
            u = db.Users.Find(id);
            db.Users.Remove(u);
            db.SaveChanges();
            return RedirectToAction("Index", "Users");
        }

    }
}