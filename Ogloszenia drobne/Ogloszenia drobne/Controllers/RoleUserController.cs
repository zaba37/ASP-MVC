using Ogloszenia_drobne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ogloszenia_drobne.Controllers
{
    public class RoleUserController : Controller
    {
        //
        // GET: /RoleUser/
        public string AddRoles()
        {
            IdentityManager im = new IdentityManager();

            im.CreateRole("Admin");
            im.CreateRole("User");

            return "Role stworzone";
        }

        public string AddUserToRole()
        {
            IdentityManager im = new IdentityManager();

            im.AddUserToRoleByUsername("Maciek", "Admin");
            im.AddUserToRoleByUsername("Piotrek", "User");

            return "Role przypisane";
        }
    }
}