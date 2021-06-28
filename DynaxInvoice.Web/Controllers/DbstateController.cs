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
    public class DbstateController : Controller
    {
        // GET: State
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllStates()
        {
            IEnumerable<DynaxState> lst = new List<DynaxState>();
            int Count = 0;           

            try
            {
                DynaxStateBL objSt = new DynaxStateBL();
                lst = objSt.StateList();
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(s => new
            {
                Id = s.Id,
                StateName = s.StateName                
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(DynaxState st)
        {
            try
            {
                DynaxStateBL objSt = new DynaxStateBL();
                if (!ModelState.IsValid)
                    return View(st);
                int id = objSt.AddState(st);
                if (id >0)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/dbstate"));
                }
            }
            catch(Exception ex){

            }
            return View();
        }
        [HttpGet]
        public  ActionResult Edit(int Id)
        {
            DynaxState objSt = new DynaxState();
            try
            {
                DynaxStateBL objBl = new DynaxStateBL();
                objSt = objBl.GetStateDetails(Id);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(objSt);
        }
        [HttpPost]
        public ActionResult Edit(DynaxState st)
        {
            ViewBag.Status = 0;
            try
            {
                if (!ModelState.IsValid)
                    return View(st);

                DynaxStateBL objBl = new DynaxStateBL();
                bool flag = objBl.UpdateState(st);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/dbstate"));
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