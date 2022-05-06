using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIMS.Models;

namespace CIMS.Controllers
{
    public class ADMINController : Controller
    {
        public JsonResult IsNameExist(string name)
        {
            CIMSEntities db = new CIMSEntities();

            var validateName = db.Manufacturers.FirstOrDefault
                                (x => x.Name == name);
            if (validateName != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsContactNumberExist(string ContactPersonNumber)
        {
            CIMSEntities db = new CIMSEntities();

            var validatecontactnumber = db.Manufacturers.FirstOrDefault
                                (x => x.ContactPersonNumber == ContactPersonNumber);
            if (validatecontactnumber != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        static int count = 0;
        // GET: ADMIN
        public ActionResult Index()
        {
            return View();
        }

        //GET
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var res1 = (from a in dbmodel.ADMINs
                            where a.UserName == UserName
                            select a).ToList();
                bool res = false;
                if (res1.Count > 0)
                {
                    res = res1.ElementAt(0).Password.Equals(Password);
                }
                if (res)
                {
                    count = 0;
                    return RedirectToAction("Index");
                }
                else if (count >= 0 && count < 3 && res == false)
                {
                    ViewBag.Message = "INVALID CREDENTIALS ENTERED";
                    count++;
                    return View();
                }
                else
                {
                    return RedirectToAction("ForgotPassword1");
                }
            }
        }

        public ActionResult ForgotPassword1(string UserName)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                if (UserName != null)
                {
                    var res1 = (from a in dbmodel.ADMINs
                                where a.UserName == UserName
                                select a).ToList();
                    bool res = false;
                    if (res1.Count > 0)
                    {
                        res = res1.ElementAt(0).UserName.Equals(UserName);
                    }

                    if (res)
                    {
                        var res2 = res1.ElementAt(0).AdminId;
                        return RedirectToAction("ForgotPassword2", "ADMIN", new { id = res2 });
                    }
                    else
                    {
                        ViewBag.Message = "INVALID USERNAME";
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult ForgotPassword2(int id)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var question = (from a in dbmodel.ADMINs
                                where a.AdminId == id
                                select a).ToList();
                ViewBag.ques = question.ElementAt(0).SecurityQuestion.ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword2(int id, string Answer)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var question = (from a in dbmodel.ADMINs
                                where a.AdminId == id
                                select a).ToList();

                if (Answer != null)
                {
                    var res1 = (from a in dbmodel.ADMINs
                                where a.AdminId == id
                                select a).ToList();

                    var res = res1.ElementAt(0).Response.Equals(Answer);

                    if (res)
                    {
                        ViewBag.ques = question.ElementAt(0).SecurityQuestion.ToString();
                        ViewBag.Password = "YOUR PASSWORD IS-> " + res1.ElementAt(0).Password;
                        return View();
                    }
                    else
                    {
                        ViewBag.ques = question.ElementAt(0).SecurityQuestion.ToString();
                        ViewBag.Message = "INVALID INPUT ENTERED";
                        return View();
                    }

                }
                else
                {
                    ViewBag.ques = question.ElementAt(0).SecurityQuestion.ToString();
                    return View();
                }
            }
        }

        //GET
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(ADMIN collection)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                try
                {
                    foreach (var item in dbmodel.ADMINs.ToList())
                    {
                        if (item.UserName == collection.UserName)
                        {
                            ViewBag.Username = "Username is already used ";
                        }

                    }
                    dbmodel.ADMINs.Add(collection);
                    dbmodel.SaveChanges();
                    return RedirectToAction("Index", "HOME");
                }
                catch (Exception)
                {
                    return View();
                }

            }
        }

        public ActionResult IndexManufacturer()
        {
            CIMSEntities entities = new CIMSEntities();
            var res = entities.Manufacturers.ToList();
            return View(res);
        }

        //GET:ADMINController
        public ActionResult CreateManufacturer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateManufacturer(Manufacturer collection)
        {
            try
            {
                // TODO: Add insert logic here

                using (CIMSEntities dbmodel = new CIMSEntities())
                {
                    foreach (var item in dbmodel.Manufacturers.ToList())
                    {
                        if (item.Name == collection.Name)
                        {
                            ViewBag.name = "MANUFACTURER ALREADY EXIST";
                        }
                        if (item.ContactPersonNumber == collection.ContactPersonNumber)
                        {
                            ViewBag.contact = "CONTACT PERSON NUMBER ALREADY EXIST";
                        }
                    }

                    dbmodel.Manufacturers.Add(collection);

                    dbmodel.SaveChanges();
                }
                return RedirectToAction("IndexManufacturer");
            }
            catch
            {

                return View();
            }
        }
    }
}