using DynaxInvoice.BL;
using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynaxInvoice.Web.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class DfacilityController : Controller
    {
        // GET: Dfacility
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllFacility()
        {
            IEnumerable<DynaxFacility> lst = new List<DynaxFacility>();
            int Count = 0;

            try
            {
                DynaxFacilityBL objSt = new DynaxFacilityBL();
                lst = objSt.GetFacilityList();
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(s => new
            {
                id = s.Id,
                facility= s.Facility,
                status=s.Status
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DynaxFacility fc)
        {
            try
            {
                ViewBag.Status = 0;
                DynaxFacilityBL objSt = new DynaxFacilityBL();
                if (!ModelState.IsValid)
                    return View(fc);
                int id = objSt.AddFacility(fc);
                if (id > 0)
                {
                    ViewBag.Status = id;
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            DynaxFacility objSt = new DynaxFacility();
            try
            {
                DynaxFacilityBL objBl = new DynaxFacilityBL();
                objSt = objBl.GetFacilityDetails(Id);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(objSt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DynaxFacility fc)
        {
            ViewBag.Status = 0;

            try
            {
                if (!ModelState.IsValid)
                    return View(fc);

                DynaxFacilityBL objBl = new DynaxFacilityBL();
                bool flag = objBl.UpdateFacility(fc);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/dfacility"));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = 1;
                Response.Write(ex);
            }
            return View();
        }
    }
}