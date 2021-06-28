using DynaxInvoice.BL;
using DynaxInvoice.BO;
using DynaxInvoice.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynaxInvoice.Web.Controllers
{
    [Authorize]
    public class DcustomerController : Controller
    {
        // GET: Dcuatomer
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllCustomer()
        {
            IEnumerable<DynaxCustomer> lst = new List<DynaxCustomer>();
            int Count = 0;           
            var uid = Models.ClaimsExtensions.GetUserId(this.User);           

            try
            {
                DynaxCustomerBL obj = new DynaxCustomerBL();
                lst = obj.GetCustomerList(uid.Value);
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                var objUtility = new Utilities();
                var path = Server.MapPath("../log");
                objUtility.CreateLogFiles(path, ex.ToString());
            }

            
            var objCust = lst.Select(s => new
            {
                id = s.Id,
                companyName = s.CompanyName,
                address1 = s.Address1,
                address2 = s.Address2,
                city = s.City,
                pincode = s.Pincode,
                stateId = s.StateId,
                contactPerson = s.ContactPerson,
                designation = s.Designation,
                mobileNo = s.MobileNo,
                email = s.Email,
                gSTN = s.GSTN,
                dealerId = s.DealerId,
                userid = s.UserId,
                userName = s.UserName,
                dealerName = s.DealerName,
                stateName = s.StateName
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 50, data = objCust }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            DynaxStateBL objState = new DynaxStateBL();
            var stateList = objState.StateList();
            ViewBag.stList = stateList;

            DynaxCustomerBL objCust = new DynaxCustomerBL();
            var uid = Models.ClaimsExtensions.GetUserId(this.User);
            var userList = objCust.GetUserList(uid.Value);
            ViewBag.userList = userList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DynaxCustomer ob)
        {
            try
            {
                ViewBag.Status = 0;

                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxCustomerBL objCust = new DynaxCustomerBL();
                var uid = Models.ClaimsExtensions.GetUserId(this.User);
                var userList = objCust.GetUserList(uid.Value);
                ViewBag.userList = userList;

                DynaxCustomerBL obj = new DynaxCustomerBL();
                if (!ModelState.IsValid)
                    return View(ob);

                int id = obj.AddCustomer(ob);
                if (id > 0)
                {
                    ViewBag.Status = id;
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            DynaxCustomer ob = new DynaxCustomer();
            try
            {
                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxCustomerBL objCust = new DynaxCustomerBL();
                var uid = Models.ClaimsExtensions.GetUserId(this.User);
                var userList = objCust.GetUserList(uid.Value);
                ViewBag.userList = userList;

                DynaxCustomerBL obj = new DynaxCustomerBL();
                ob = obj.GetCustomerDetails(Id);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(ob);
        }
        [HttpPost]
        public ActionResult Edit(DynaxCustomer ob)
        {
            ViewBag.Status = 0;

            try
            {
                if (!ModelState.IsValid)
                    return View(ob);

                DynaxStateBL objState = new DynaxStateBL();
                var stateList = objState.StateList();
                ViewBag.stList = stateList;

                DynaxCustomerBL objCust = new DynaxCustomerBL();
                var uid = Models.ClaimsExtensions.GetUserId(this.User);
                var userList = objCust.GetUserList(uid.Value);
                ViewBag.userList = userList;

                DynaxCustomerBL objBl = new DynaxCustomerBL();
                bool flag = objBl.UpdateCustomer(ob);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/dcustomer"));
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