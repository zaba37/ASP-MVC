using Ogloszenia_drobne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ogloszenia_drobne.Controllers
{
    public class TreeviewController : Controller
    {
        //
        // GET: /Treeview/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Simple()
        {
            List<Category> all = new List<Category>();
            using (var dc = new ApplicationDbContext())
            {
                all = dc.Category.OrderBy(a => a.Parent).ToList();
            }
            return View(all);
        }

    }
}