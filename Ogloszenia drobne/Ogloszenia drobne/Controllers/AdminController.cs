using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ogloszenia_drobne.Models;


namespace Ogloszenia_drobne.Controllers
{
    public class AdminController : Controller
    {
      
        // GET: Admin
        public ActionResult Index()
        {          
            return View();
        }

        public ActionResult AddBannedWord ()
        {
            return View();
        }

        public ActionResult DeleteBannedWord()
        {
            
            return View();
        }

        public ActionResult AddShortMessage()
        {
            return View();
        }
        public ActionResult EditShortMessage()
        {
            return View();
        }
    }
}