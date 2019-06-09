using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AppBurse.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

		public IEnumerable<SelectListItem> AllRoles { get; set; }
	}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

		public DbSet<Bursa> Burse { get; set; }
		public DbSet<Cerere> Cereri { get; set; }
        public DbSet<Program_de_studiu> Programe_de_studiu { get; set; }
        public DbSet<Domeniu> Domenii { get; set; }
        public DbSet<Specializare_pe_Domeniu> Specializari { get; set; }
        public DbSet<Student> Studenti { get; set; }
		public DbSet<StudentBursa> StudentBurse { get; set; }
		public DbSet<Buget> Bugete { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}