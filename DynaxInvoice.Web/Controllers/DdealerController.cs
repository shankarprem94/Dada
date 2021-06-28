using DynaxInvoice.BL;
using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynaxInvoice.Web.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class DdealerController : Controller
    {
        // GET: Ddealer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllDealer()
        {
            IEnumerable<DynaxDealer> lst = new List<DynaxDealer>();
            int Count = 0;

            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxZoneBL objZone = new DynaxZoneBL();
                var zoneList = objZone.GetZoneList(0);
                ViewBag.zoneList = stateList;
                DynaxDealerBL obj = new DynaxDealerBL();
                lst = obj.GetDealerList();
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(s => new
            {
                id = s.Id,
                companyName = s.DealerName,
                companyAddress1 = s.DealerAddress1,
                companyAddress2 = s.DealerAddress2,
                city = s.City,
                stateId = s.StateId,
                pincode = s.Pincode,
                zoneId = s.ZoneId,
                emailId = s.EmailId,
                mobileNo = s.MobileNo,
                contactPerson = s.ContactPerson,
                accountNo = s.AccountNo,
                accountName = s.AccountName,
                bankName = s.BankName,
                iFSCCode = s.IFSCCode,
                joinDate = String.Format("{0:dd MMM yyyy}", s.JoinDate),
                endDate = s.EndDate,
                status = s.Status
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            DynaxStateBL objState = new DynaxStateBL();
            var stateList = objState.StateList();
            ViewBag.stList = stateList;

            DynaxZoneBL objZone = new DynaxZoneBL();
            var zoneList = objZone.GetZoneList(0);
            ViewBag.zoneList = zoneList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DynaxDealer ob)
        {
            try
            {
                ViewBag.Status = 0;

                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxZoneBL objZone = new DynaxZoneBL();
                var zoneList = objZone.GetZoneList(0);
                ViewBag.zoneList = zoneList;

                DynaxDealerBL obj = new DynaxDealerBL();

                if (!ModelState.IsValid)
                    return View(ob);
                ob.JoinDate = DateTime.Now;
                ob.EndDate = DateTime.Now.AddDays(365);
                int id = obj.AddDealer(ob);
                if (id > 0)
                {
                    ViewBag.Status = id;
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            DynaxDealer ob = new DynaxDealer();
            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxZoneBL objZone = new DynaxZoneBL();
                var zoneList = objZone.GetZoneList(0);
                ViewBag.zoneList = zoneList;

                DynaxDealerBL obj = new DynaxDealerBL();
                ob = obj.GetDealerDetails(Id);
                ViewBag.DealerZone = ob.ZoneId;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(ob);
        }
        [HttpPost]
        public ActionResult Edit(DynaxDealer ob)
        {
            ViewBag.Status = 0;

            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxZoneBL objZone = new DynaxZoneBL();
                var zoneList = objZone.GetZoneList(0);
                ViewBag.zoneList = zoneList;

                if (!ModelState.IsValid)
                    return View(ob);

                DynaxDealerBL objBl = new DynaxDealerBL();
                bool flag = objBl.UpdateDealer(ob);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/ddealer"));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = 1;
                Response.Write(ex);
            }
            return View();
        }

        [HttpGet]
        public ActionResult DealerDetail(int id)
        {
            DynaxDealer ob = new DynaxDealer();
            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxZoneBL objZone = new DynaxZoneBL();
                var zoneList = objZone.GetZoneList(0);
                ViewBag.zoneList = zoneList;

                DynaxDealerBL obj = new DynaxDealerBL();
                ob = obj.GetDealerDetails(id);

                ViewBag.DealerZone = ob.ZoneId;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(ob);            
        }

        [HttpGet]
        public ActionResult UploadLogo()
        {
            DynaxDealerBL obj = new DynaxDealerBL();
            var lst = obj.GetDealerList();
            ViewBag.dealerList = lst;
            return View();
        }

        [HttpPost]
        public ActionResult UploadLogo(DealerLogo objLogo)
        {
            DynaxDealerBL obj = new DynaxDealerBL();
            var lst = obj.GetDealerList();
            ViewBag.dealerList = lst;

            if (!ModelState.IsValid)
                return View(objLogo);

            try
            {
                if (objLogo.FileName != null)
                {
                    string path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName((objLogo.DealerId + ".jpg")));
                    objLogo.FileName.SaveAs(path);
                    ViewBag.Status = 1;
                }
            }
            catch(Exception ex)
            {
                Response.Write(ex);
            }

            return View();
        }
    }
}