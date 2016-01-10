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


        public ActionResult ShortMessageList()
        {
            var db = new ApplicationDbContext();
            return View(db.ShortMessage.ToList());
        }

        public ActionResult AddShortMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddShortMessage(ShortMessage msg)
        {
            msg.AddedDate = DateTime.Now;
            using (var db = new ApplicationDbContext())
            {
                db.ShortMessage.Add(msg);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult EditShortMessage(int id)
        {

            var db = new ApplicationDbContext();
            var msg = db.ShortMessage.FirstOrDefault(m => m.ShortMessageId == id);       
            return View(msg);
        }


        [HttpPost]
        public ActionResult EditShortMessage(ShortMessage msg)
        {

            using (var db = new ApplicationDbContext())
            {
                var message = db.ShortMessage.FirstOrDefault(m => m.ShortMessageId == msg.ShortMessageId);
                message.Title = msg.Title;
                message.Content = msg.Content;
                db.SaveChanges();
            }
            return RedirectToAction("ShortMessageList");
        }


        [HttpPost]
        public ActionResult DeleteShortMessage(int id)
        {

       
            using (var db = new ApplicationDbContext())
            {
                var msg = db.ShortMessage.FirstOrDefault(m => m.ShortMessageId == id);       
                db.ShortMessage.Remove(msg);
                db.SaveChanges();
            }
            return RedirectToAction("ShortMessageList");
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

        public ActionResult CategoryManager()
        {
            var db = new ApplicationDbContext();
            return View(db.Category.ToList());
        }



        [HttpPost]
        public ActionResult AddCategory(string nameCat,int? id )
        {

            using (var db = new ApplicationDbContext())
            {
                Category cat = new Category();
                cat.Name = nameCat;
                if (id != null)
                    cat.Parent = (int)id;
                else
                    cat.Parent = 0;
                db.Category.Add(cat);
                db.SaveChanges();
               
            }
            return RedirectToAction("CategoryManager");
        }

        public ActionResult DeleteCategory(int id)
        {

            using (var db = new ApplicationDbContext())
            {
                var cat = db.Category.FirstOrDefault(m => m.CategoryId == id);
                db.Category.Remove(cat);
                db.SaveChanges();

            }
            return RedirectToAction("CategoryManager");
        }


        public ActionResult ReportedAdvertisements()
        {

            var db = new ApplicationDbContext();
            return View(db.Advertisement.Where(m=>m.Reported==true).ToList());
        }

        [HttpGet]
        public ActionResult DeleteReport(int id)
        {
            using(var db= new ApplicationDbContext())
            {
                var adv = db.Advertisement.FirstOrDefault(m => m.AdvertisementId == id);
                adv.Reported = false;
                db.SaveChanges();
            }
            return RedirectToAction("ReportedAdvertisements");
        }
    }
}