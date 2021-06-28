using DynaxInvoice.BL;
using DynaxInvoice.BO;
using DynaxInvoice.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DynaxInvoice.Web.Controllers
{
    
    public class DuserController : Controller
    {
        [Authorize(Roles = "Super Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllUser()
        {
            IEnumerable<DynaxUser> lst = new List<DynaxUser>();
            int Count = 0;
            UserBL user = new UserBL();

            lst =  user.GetAllUser();
            Count = lst.Count();
            var objUserList = lst.Select(s => new
            {
                Id = s.Id,
                UserName = s.UserName,
                FullName = s.FullName,
                Email = s.Email,
                Mobile = s.Mobile,
                UserType = s.UserType,
                Status = s.Status
            });

            return Json(new
            {
                draw = 1,
                recordsTotal = Count,
                recordsFiltered = 10,
                data = objUserList
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Super Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            DynaxDealerBL objDealer = new DynaxDealerBL();
           var lst =objDealer.GetDealerList();
            ViewBag.lstDealer = lst;
            return View();
        }

        [Authorize(Roles = "Super Admin")]
        [HttpPost]
        public ActionResult Create(DynaxUser objUser)
        {
            DynaxDealerBL objDealer = new DynaxDealerBL();
            var lst = objDealer.GetDealerList();
            ViewBag.lstDealer = lst;
            ViewBag.Status = 1;

            if (!ModelState.IsValid)
                return View(objUser);

            UserBL user = new UserBL();
            var utility = new Utilities();
            objUser.Password = utility.Encrypt(objUser.Password);
            int flag =  user.AddUser(objUser);
            if (flag >0)
            {
                ModelState.Clear();
                ViewBag.Status = 0;
            }
            return View();
        }

        [Authorize(Roles = "Super Admin")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            DynaxUser objUser = new DynaxUser();
            try
            {
                DynaxDealerBL objDealer = new DynaxDealerBL();
                var lst = objDealer.GetDealerList();                
                ViewBag.lstDealer = lst;
                UserBL user = new UserBL();
                objUser =  user.GetUserById(Id);
                
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(objUser);
        }

        [Authorize(Roles = "Super Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DynaxUser objUser)
        {
            ViewBag.Status = 0;
            try
            {
                DynaxDealerBL objDealer = new DynaxDealerBL();
                var lst = objDealer.GetDealerList();
                ViewBag.lstDealer = lst;

                if (!ModelState.IsValid)
                    return View(objUser);

                UserBL user = new UserBL();
                var utility = new Utilities();
                objUser.Password = utility.Encrypt(objUser.Password);
                
                bool flag =  user.UpdateUser(objUser);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/duser"));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = 1;
                Response.Write(ex);
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {    
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(DViewModel obj)
        {
            var uid = Models.ClaimsExtensions.GetUserId(this.User);            
            var objUser = new UserBL();
           ViewBag.Flag= objUser.ChangePassword(uid.Value, obj.Password);
            return View();
        }
    }
}