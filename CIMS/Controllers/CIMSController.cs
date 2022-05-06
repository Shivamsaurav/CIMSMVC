using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIMS.Models;
using System.Data.Entity;

namespace CIMS.Controllers
{
    public class CIMSController : Controller
    {
        // REMOTE VALIDATION
        public JsonResult IsCarModelExist(string Model)
        {
            CIMSEntities db = new CIMSEntities();

            var validateModel = db.CARs.FirstOrDefault
                                (x => x.Model == Model);
            if (validateModel != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: CIMS
        public ActionResult Index()
        {
            CIMSEntities entities = new CIMSEntities();
            var res = entities.CARs.ToList();
            return View(res);
        }

        // GET: CIMS/Details/5
        public ActionResult ShowCarDetails(int id)
        {

            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var list_Manufacturer = dbmodel.Manufacturers.ToList();
                ViewBag.list_Manufacturer = new SelectList(list_Manufacturer, "ID", "Name");
                var list_Type = dbmodel.CarTypes.ToList();
                ViewBag.TypeId = new SelectList(list_Type, "ID", "Type");
                var list_Transmission = dbmodel.CarTransmissionTypes.ToList();
                ViewBag.list_Transmission = new SelectList(list_Transmission, "ID", "Name");

                return View(dbmodel.CARs.Where(cid => cid.ID.Equals(id)).FirstOrDefault());
            }

        }

        // GET: CIMS/Create
        public ActionResult AddNewCarDetails()
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var list_Manufacturer = dbmodel.Manufacturers.ToList();
                ViewBag.list_Manufacturer = new SelectList(list_Manufacturer, "ID", "Name");
                var list_Type = dbmodel.CarTypes.ToList();
                ViewBag.TypeId = new SelectList(list_Type, "ID", "Type");
                var list_Transmission = dbmodel.CarTransmissionTypes.ToList();
                ViewBag.list_Transmission = new SelectList(list_Transmission, "ID", "Name");


                return View();
            }

        }

        // POST: CIMS/Create
        [HttpPost]
        public ActionResult AddNewCarDetails(CAR collection)
        {

            try
            {
                // TODO: Add insert logic here

                using (CIMSEntities dbmodel = new CIMSEntities())
                {

                    dbmodel.CARs.Add(collection);

                    dbmodel.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                using (CIMSEntities dbmodel = new CIMSEntities())
                {

                    var list_Manufacturer = dbmodel.Manufacturers.ToList();
                    ViewBag.list_Manufacturer = new SelectList(list_Manufacturer, "ID", "Name");
                    var list_Type = dbmodel.CarTypes.ToList();
                    ViewBag.TypeId = new SelectList(list_Type, "ID", "Type");
                    var list_Transmission = dbmodel.CarTransmissionTypes.ToList();
                    ViewBag.list_Transmission = new SelectList(list_Transmission, "ID", "Name");

                    return View();
                }
            }
        }
    }
}