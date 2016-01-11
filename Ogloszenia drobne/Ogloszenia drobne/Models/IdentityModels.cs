using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
namespace Ogloszenia_drobne.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        public int AdvOnPg { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }

        public string Discriminator { get; set; }
       
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
            
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Advertisement> Advertisement{ get; set; }
        public DbSet<Attribute> Attribute { get; set; }
        public DbSet<AttributeValue> AttributeValue { get; set; }
        public DbSet<BannedWord> BannedWord{ get; set; }
        public DbSet<CategoryAttribute> CategoryAttribute { get; set; }
        public DbSet<Category> Category{ get; set; }
        public DbSet<File> File{ get; set; }
        public DbSet<Property> Property { get; set; }
        public DbSet<ShortMessage> ShortMessage{ get; set; }

        //public System.Data.Entity.DbSet<Ogloszenia_drobne.Models.ApplicationUser> ApplicationUsers { get; set; }


     
    }


    public class IdentityManager
    {
        public RoleManager<IdentityRole> LocalRoleManager
        {
            get
            {
                return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        
            }
        }


        public UserManager<ApplicationUser> LocalUserManager
        {
            get
            {
                return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            }
        }


        public ApplicationUser GetUserByID(string userID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = null;

            var query = from u in db.Users
                        where u.Id == userID
                        select u;

            if (query.Count() > 0)
                user = query.First();

            return user;
        }


        public ApplicationUser GetUserByName(string username)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = null;

            var query = from u in db.Users
                        where u.UserName == username
                        select u;

            if (query.Count() > 0)
                user = query.First();

            return user;
        }


        public bool RoleExists(string name)
        {
            var rm = LocalRoleManager;

            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = LocalRoleManager;
            var idResult = rm.Create(new IdentityRole(name));
            
            return idResult.Succeeded;
        }


        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = LocalUserManager;
            var idResult = um.Create(user, password);

            return idResult.Succeeded;
        }


        public bool AddUserToRole(string userId, string roleName)
        {
            var um = LocalUserManager;
            var idResult = um.AddToRole(userId, roleName);
            
            return idResult.Succeeded;
        }

        public bool RemoveFromRole(string userId,string roleName)
        {
            var um = LocalUserManager;
            var idResult = um.RemoveFromRole(userId, roleName);
            
            return idResult.Succeeded;
        }

        public IList<string> GetUserRoles(string userId)
        {
            var um = LocalUserManager;           
            return um.GetRoles(userId);
        }

        public bool InRole(string userId,string roleName)
        {
             var um = LocalUserManager;
             return um.IsInRole(userId, roleName);
        }

        public bool AddUserToRoleByUsername(string username, string roleName)
        {
            var um = LocalUserManager;

            string userID = um.FindByName(username).Id;
            var idResult = um.AddToRole(userID, roleName);

            return idResult.Succeeded;
        }


        public void ClearUserRoles(string userId)
        {
            var um = LocalUserManager;
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);

            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.RoleId);
            }
        }
    }

}