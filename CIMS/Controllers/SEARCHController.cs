using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIMS.Models;
using System.Data.Entity;
namespace CIMS.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Search(string Value, string ManufacturerId, string TypeId)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var list_Manufacturer = dbmodel.Manufacturers.ToList();
                ViewBag.list_Manufacturer = new SelectList(list_Manufacturer, "ID", "Name");
                var list_Type = dbmodel.CarTypes.ToList();
                ViewBag.TypeId = new SelectList(list_Type, "ID", "Type");
                var list_Transmission = dbmodel.CarTransmissionTypes.ToList();
                ViewBag.list_Transmission = new SelectList(list_Transmission, "ID", "Name");

                ViewBag.Name = (from n in dbmodel.Manufacturers
                                select n.Name).Distinct().ToList();

                ViewBag.Type = (from n in dbmodel.CarTypes
                                select n.Type).Distinct().ToList();

                if (!string.IsNullOrEmpty(Value))
                {
                    var res = dbmodel.CARs.Where(model => model.Model.StartsWith(Value)).ToList();
                    ModelState.Clear();
                    return View(res);
                }
                else if (!string.IsNullOrEmpty(ManufacturerId) && !string.IsNullOrEmpty(TypeId))
                {
                    var res = (from data in dbmodel.CARs
                               join data2 in dbmodel.Manufacturers on data.ManufacturerId equals data2.ID
                               join data3 in dbmodel.CarTypes on data.TypeId equals data3.ID
                               where ManufacturerId == data2.Name && TypeId == data3.Type
                               select data).ToList();
                    ModelState.Clear();
                    return View(res);
                }
                else
                {
                    return View(dbmodel.CARs.ToList());
                }
            }

        }

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

        // GET: CIMS/Edit/5
        public ActionResult UpdateCarDetails(int id)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var list_Manufacturer = dbmodel.Manufacturers.ToList();
                ViewBag.list_Manufacturer = new SelectList(list_Manufacturer, "ID", "Name");
                var list_Type = dbmodel.CarTypes.ToList();
                ViewBag.TypeId = new SelectList(list_Type, "ID", "Type");
                var list_Transmission = dbmodel.CarTransmissionTypes.ToList();
                ViewBag.list_Transmission = new SelectList(list_Transmission, "ID", "Name");
                return View(dbmodel.CARs.Where(cid => cid.ID == id).FirstOrDefault());
            }
        }

        // POST: CIMS/Edit/5
        [HttpPost]
        public ActionResult UpdateCarDetails(int id, CAR collection)
        {
            try
            {
                // TODO: Update logic here
                using (CIMSEntities dbmodel = new CIMSEntities())
                {

                    dbmodel.Entry(collection).State = EntityState.Modified;

                    dbmodel.SaveChanges();
                }
                return RedirectToAction("Search");
            }
            catch
            {
                using (CIMSEntities dbmodel = new CIMSEntities())
                {
                    ViewBag.Reason = "Failed to edit the record";
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

        // GET: CIMS/Delete/5
        public ActionResult DeleteCarDetails(int id)
        {
            using (CIMSEntities dbmodel = new CIMSEntities())
            {
                var list_Manufacturer = dbmodel.Manufacturers.ToList();
                ViewBag.list_Manufacturer = new SelectList(list_Manufacturer, "ID", "Name");
                var list_Type = dbmodel.CarTypes.ToList();
                ViewBag.TypeId = new SelectList(list_Type, "ID", "Type");
                var list_Transmission = dbmodel.CarTransmissionTypes.ToList();
                ViewBag.list_Transmission = new SelectList(list_Transmission, "ID", "Name");

                return View(dbmodel.CARs.Where(cid => cid.ID == id).FirstOrDefault());
            }
        }

        // POST: CIMS/Delete/5
        [HttpPost]
        public ActionResult DeleteCarDetails(int id, CAR collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (CIMSEntities dbmodel = new CIMSEntities())
                {
                    CAR car = dbmodel.CARs.Where(c => c.ID == id).FirstOrDefault();

                    dbmodel.CARs.Remove(car);

                    dbmodel.SaveChanges();
                }
                return RedirectToAction("Search");
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