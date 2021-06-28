using DynaxInvoice.BL;
using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynaxInvoice.Web.Controllers
{
    [Authorize]
    public class DhotelsController : Controller
    {
        // GET: Dhotels
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllHotel()
        {
            IEnumerable<DynaxHotel> lst = new List<DynaxHotel>();
            int Count = 0;
            var uid = Models.ClaimsExtensions.GetUserId(this.User);
            try
            {
                DynaxHotelBL obj = new DynaxHotelBL();
                lst = obj.GetHotelList(uid.Value);
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(s => new
            {
                id = s.Id,
                companyId = s.CustomerId,
                hotelName = s.HotelName,
                hotelType=s.HotelType,
                address1 = s.Address1,
                address2 = s.Address2,
                city = s.City,
                pincode = s.Pincode,
                stateId = s.StateId,
                mobileNo = s.MobileNo,
                emailId = s.Email,
                landMark = s.Landmark,
                distLandmark = s.DistFromLandmark,
                ratePerNight = String.Format("{0:C}", s.RatePerNight),
                facility=s.FacilityId,
                companyName = s.CompanyName,
                stateName = s.StateName
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            DynaxStateBL objState = new DynaxStateBL();
            var stateList = objState.StateList();
            ViewBag.stList = stateList;

            var uid = Models.ClaimsExtensions.GetUserId(this.User);
            DynaxCustomerBL objCustomer = new DynaxCustomerBL();
            var CustomerList = objCustomer.GetCustomerList(uid.Value);
           
            ViewBag.cstList = CustomerList.ToList();

            DynaxFacilityBL objFac = new DynaxFacilityBL();
            var lstFacility = objFac.GetFacilityList();
            ViewBag.lstFac = lstFacility;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DynaxHotel ob)
        {
            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                var uid = Models.ClaimsExtensions.GetUserId(this.User);
                DynaxCustomerBL objCustomer = new DynaxCustomerBL();
                var CustomerList = objCustomer.GetCustomerList(uid.Value);
                ViewBag.cstList = CustomerList;

                ViewBag.Status = 0;
                DynaxHotelBL obj = new DynaxHotelBL();
                if (!ModelState.IsValid)
                    return View(ob);
                int id = obj.AddHotel(ob);
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
            DynaxHotel ob = new DynaxHotel();
            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                var uid = Models.ClaimsExtensions.GetUserId(this.User);
                DynaxCustomerBL objCustomer = new DynaxCustomerBL();
                var CustomerList = objCustomer.GetCustomerList(uid.Value);
                ViewBag.cstList = CustomerList.ToList();

                DynaxFacilityBL objFac = new DynaxFacilityBL();
                var lstFacility = objFac.GetFacilityList();
                ViewBag.lstFac = lstFacility;

                DynaxHotelBL obj = new DynaxHotelBL();
                ob = obj.GetHotelDetails(Id);
                ViewBag.facl = ob.FacilityId;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(ob);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DynaxHotel ob)
        {
            ViewBag.Status = 0;

            try
            {
                if (!ModelState.IsValid)
                    return View(ob);

                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                var uid = Models.ClaimsExtensions.GetUserId(this.User);
                DynaxCustomerBL objCustomer = new DynaxCustomerBL();
                var CustomerList = objCustomer.GetCustomerList(uid.Value);
                ViewBag.cstList = CustomerList;

                DynaxFacilityBL objFac = new DynaxFacilityBL();
                var lstFacility = objFac.GetFacilityList();
                ViewBag.lstFac = lstFacility;

                DynaxHotelBL objBl = new DynaxHotelBL();
                bool flag = objBl.UpdateHotel(ob);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/dhotels"));
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