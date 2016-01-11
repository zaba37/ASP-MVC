using Ogloszenia_drobne.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Xml.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;
using System.Text.RegularExpressions;
using Ogloszenia_drobne.Migrations;
namespace Ogloszenia_drobne.Controllers
{
    public class AdvertisementController : Controller
    {
        // GET: Advertisement
        public ActionResult Index(string page, int? idCat)
        {
           
            var db = new ApplicationDbContext();
            if(User.Identity.IsAuthenticated)
            {
                int num = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).AdvOnPg;
                ViewBag.NumAdv = num;
     
            }
            else
            {
                ViewBag.NumAdv = 20;
            }

            var Categories = db.Category.ToList();
            ViewBag.cat = Categories;
            if (idCat != null)
            {

                List<Advertisement> adv = findAdvertisements((int)idCat);
                return View(adv);
            }
            else
                return View(db.Advertisement.ToList());
        }


        public ActionResult Filter (int? idCat)
        {
            var db = new ApplicationDbContext();
            var Categories = db.Category.ToList();
            ViewBag.NumAdv = 1;
            ViewBag.cat = Categories;
            return View("Index", db.Advertisement.Where(m => m.CategoryId == idCat).ToList());

        }
        // GET: Advertisement/Details/5
       
        public ActionResult Details(int id)
        {
            var db = new ApplicationDbContext();
            Advertisement adv = db.Advertisement.FirstOrDefault(u => u.AdvertisementId == id);
            adv.VisitCounter++;
            db.SaveChanges();
            return View(adv);
        }

        // GET: Advertisement/Create

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
          var db = new ApplicationDbContext();
            Advertisement ad = new Advertisement();
            ad.CategoryList = db.Category.ToList();
            return View(ad);
        }

        // POST: Advertisement/Create
        [HttpPost]
        
        public ActionResult Create(Advertisement advertisement, HttpPostedFileBase[] fileUpload2, HttpPostedFileBase[] fileUpload, string[] description, int? cat) 
        {
           
       
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new ApplicationDbContext())
                    {

                        if(!checkBannedWords(advertisement.Title)||!checkBannedWords(advertisement.Content))
                        {
                            return RedirectToAction("Index"); // TU DODAAC CO MA SIE STAC
                        }
                        
                        foreach(var desc in description)
                        {
                            if(!checkBannedWords(desc))
                            {
                                return RedirectToAction("Index"); // TU DODAAC CO MA SIE STAC
                            }
                        }

                        string login = User.Identity.GetUserName();
                        var user = db.Users.FirstOrDefault(u => u.UserName==login);
                        advertisement.User = user;
                        advertisement.UserId = user.Id;
                        advertisement.CategoryId = 1;
                        advertisement.Category = db.Category.FirstOrDefault(u => u.CategoryId == 1);
                        advertisement.AddedDate = DateTime.Now;
                        db.Advertisement.Add(advertisement);
                        db.SaveChanges();



                        int lastItemId = db.Advertisement.Max(item => item.AdvertisementId);

                        if (!Directory.Exists(Server.MapPath("~/advertisements")))
                        {         
                            Directory.CreateDirectory(Server.MapPath("~/advertisements"));
                        }
                        Directory.CreateDirectory(Server.MapPath("~/advertisements/" + lastItemId));
                        foreach (var file in fileUpload2)
                        {
                            String pathToDb = "~/advertisements/" + lastItemId + "/" + file.FileName;
                            String path = Server.MapPath(pathToDb);
                            file.SaveAs(path);
                            Models.File newFile = new Models.File();
                            newFile.AdvertisementId = lastItemId;
                            newFile.Advertisement = advertisement;
                            newFile.Path = pathToDb;
                            newFile.InDetails = true;
                            db.File.Add(newFile);
                            db.SaveChanges();
                        }

                        foreach (var file in fileUpload)
                        {

                            int i = 0;
                            String pathToDb = "~/advertisements/" + lastItemId + "/" + file.FileName;
                            String path = Server.MapPath(pathToDb);
                            file.SaveAs(path);
                            Models.File newFile = new Models.File();
                            newFile.AdvertisementId = lastItemId;
                            newFile.Advertisement = advertisement;
                            newFile.Path = pathToDb;
                            newFile.InDetails = false;

                            if (description[i] != "")
                                newFile.Description = description[i];
                            else
                                newFile.Description = null;

                            db.File.Add(newFile);
                            db.SaveChanges();
                            i++;
                        }
                        
                    }
                    return RedirectToAction("Index");
                }
               

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Advertisement/Edit/5
        public ActionResult Edit(int id)
        {
            var db = new ApplicationDbContext();
            Advertisement adv = db.Advertisement.FirstOrDefault(u => u.AdvertisementId == id);
            try
            {
                var help = adv.Files.Where(m => m.InDetails == false).ToList();
                
                ViewBag.NumberFile = db.Property.Find(1).Value - help.Count();


                help = adv.Files.Where(m => m.InDetails == true).ToList();

                ViewBag.NumberFile2 = db.Property.Find(2).Value - help.Count();
            }
            catch
            {
                ViewBag.NumberFile = 2;
                ViewBag.NumberFile2 = 2;
            }
            return View(adv);
        }

        // POST: Advertisement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Advertisement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Advertisement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private bool checkBannedWords(String input)
        {
            var db = new ApplicationDbContext();
            int counter=0;
            foreach(var word in db.BannedWord)
            {           
             counter = Regex.Matches(input, word.Word).Count;
             if (counter > 0)
                return false;
            }
            return true;
        }

        public ActionResult deleteFile(int? idFile, int idAdv)
        {

            try
            {
                if (idFile != null)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var file = db.File.FirstOrDefault(m => m.FileId == idFile);
                        db.File.Remove(file);
                        db.SaveChanges();
                    }
                }
            }

            catch
            {
                    
            }

            return RedirectToAction("Edit", new { id = idAdv });
        }


        public ActionResult Report(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                Advertisement adv = db.Advertisement.FirstOrDefault(m => m.AdvertisementId == id);
                adv.Reported = true;
                db.SaveChanges();
            }

          return  RedirectToAction("Details", new { id = id });
        }


        private List<Advertisement> findAdvertisements(int cat)
        {
            var db = new ApplicationDbContext();
            List<Advertisement> advs = db.Advertisement.Where(m => m.CategoryId == cat).ToList();//new List<Advertisement>();
            
            List<int> listId = new List<int>();
            List<Advertisement> helpList = db.Advertisement.ToList();
            listId.Add(cat);
            while(listId.Count>0)
            {
                foreach (var ad in helpList)
                {
                    if(ad.Category.Parent==listId[0])
                    {
                        advs.Add(ad);
                        listId.Add(ad.Category.CategoryId);
                    }
                }
                listId.RemoveAt(0);
            }
            return advs;

        }

        public JsonResult test(int? time)
        {

            var db = new ApplicationDbContext();

            var a = db.Advertisement.ToList();
            List<Ogloszenia_drobne.Models.Property> adv = new List<Ogloszenia_drobne.Models.Property>();
            Ogloszenia_drobne.Models.Property c = new Ogloszenia_drobne.Models.Property();

            c.Name = "a";
            c.PropertyId = 1;
            c.Value = 3;
            adv.Add(c);
            
            
           


            return Json(c, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(string nameFile)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes((Server.MapPath(nameFile)));
            string fileName = extractName(nameFile);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        
        public String extractName(string input)
        {
            char[] tab = input.ToArray();
   
          //  var last = Array.FindLast(tab,"/");

            //int last=input.FindLastIndex
            string lastItemOfSplit = input.Split(new char[] { @"/"[0] }).Last();
            return lastItemOfSplit;

        }
    
    }
}
