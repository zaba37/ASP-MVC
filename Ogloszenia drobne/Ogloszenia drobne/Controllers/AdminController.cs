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

        [HttpPost]
        public ActionResult AddBannedWord (BannedWord bannedWord)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.BannedWord.Add(bannedWord);
                    db.SaveChanges();
                }
            }


            return RedirectToAction("BannedWords");
        }

        [HttpPost]
        public ActionResult DeleteBannedWord(BannedWord bannedWord)
        {
            try
            {              
                    using (var db = new ApplicationDbContext())
                    {
                        var word = db.BannedWord.FirstOrDefault(m => m.BannedWordId == bannedWord.BannedWordId);
                        db.BannedWord.Remove(word);
                        db.SaveChanges();
                    }               
            }

            catch
            {

            }
            return RedirectToAction("BannedWords");
        }

        public ActionResult AddShortMessage()
        {
            return View();
        }
        public ActionResult EditShortMessage()
        {
            return View();
        }

        public ActionResult BannedWords()
        {
            var db = new ApplicationDbContext();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach(var item in db.BannedWord)
            {
                listItems.Add(new SelectListItem
                {
                    Text =item.Word,
                    Value = item.BannedWordId.ToString()
                });
            }
            ViewBag.WordList = listItems;

            return View();
        }
        public ActionResult Users()
        {
            var db = new ApplicationDbContext();

            return View(db.Users.ToList());
        }
        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    ApplicationUser usr = db.Users.FirstOrDefault(m => m.Id == id);

                    db.Users.Remove(usr);
                    db.SaveChanges();
                }
            }
            catch
            {
                return RedirectToAction("Users");
            }
           return RedirectToAction("Users");
        }

        public ActionResult EditUser(string id)
        {
            var db = new ApplicationDbContext();
            ApplicationUser usr;
            if (id != null)
            {
                usr = db.Users.FirstOrDefault(m => m.Id == id);

            }

            else
            {
                return HttpNotFound();
            }
            return View(usr);
        }


        [HttpPost]
        public ActionResult EditUser(ApplicationUser usr)
        {

            var db = new ApplicationDbContext();
            ApplicationUser user = db.Users.FirstOrDefault(m => m.Id == usr.Id); ;
            user.Email = usr.Email;
            user.AdvOnPg = usr.AdvOnPg;
            user.PhoneNumber = usr.PhoneNumber;
            db.SaveChanges();
            return RedirectToAction("Users");
        }
    }
}