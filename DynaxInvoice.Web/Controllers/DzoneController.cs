using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DynaxInvoice.BO;
using DynaxInvoice.BL;
namespace DynaxInvoice.Web.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class DzoneController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllZone()
        {
            IEnumerable<DynaxZone> lst = new List<DynaxZone>();
            int Count = 0;

            try
            {
                DynaxZoneBL obj = new DynaxZoneBL();
                lst = obj.GetZoneList(0);
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(s => new
            {
                id = s.Id,
                stateID = s.StateId,
                stateName=s.StateName,
                zoneName = s.ZoneName,
                status = s.Status
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            DynaxStateBL objState = new DynaxStateBL();
            var stateList = objState.StateList();
            ViewBag.stList = stateList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DynaxZone dz)
        {
            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;
                if (!ModelState.IsValid)
                    return View();
                DynaxZoneBL objZone = new DynaxZoneBL();
                int id = objZone.AddZone(dz);
                if (id > 0)
                {
                    ViewBag.Status = id;
                    ModelState.Clear();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            DynaxZone ob = new DynaxZone();
            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;
                DynaxZoneBL obj = new DynaxZoneBL();
                ob = obj.GetZoneDetails(Id);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(ob);
        }
        [HttpPost]
        public ActionResult Edit(DynaxZone ob)
        {
            ViewBag.Status = 1;

            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;
                if (!ModelState.IsValid)
                    return View(ob);

                DynaxZoneBL objBl = new DynaxZoneBL();
                bool flag = objBl.UpdateZone(ob);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/dzone"));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = 0;
                Response.Write(ex);
            }
            return View();
        }
    }
}