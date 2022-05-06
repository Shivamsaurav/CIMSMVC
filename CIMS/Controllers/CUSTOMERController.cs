using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIMS.Models;
namespace CIMS.Controllers
{
    public class CUSTOMERController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerList()
        {
            CIMSEntities entities = new CIMSEntities();
            var res = entities.CARs.ToList();
            return View(res);
        }

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
        public ActionResult CarDetails(int id)
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
    }
}