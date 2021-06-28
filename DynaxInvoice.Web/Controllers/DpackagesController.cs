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
    public class DpackagesController : Controller
    {
        // GET: Dpackages
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllPackage()
        {
            IEnumerable<DynaxPackage> lst = new List<DynaxPackage>();
            int Count = 0;

            try
            {
                DynaxPackagesBL obj = new DynaxPackagesBL();
                lst = obj.GetPackageList();
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(s => new
            {
                id = s.Id,
                packageName = s.PackageName,
                packageDescription=s.PackageDescription,
                packageAmount = string.Format("{0:C}", s.PackageAmount) ,
                maxDiscount = string.Format("{0:C}", s.MaxDiscount),
                status = s.Status
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DynaxPackage ob)
        {
            try
            {
                ViewBag.Status = 0;
                DynaxPackagesBL obj = new DynaxPackagesBL();
                if (!ModelState.IsValid)
                    return View(ob);
                int id = obj.AddPackage(ob);
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
            DynaxPackage ob = new DynaxPackage();
            try
            {
                DynaxPackagesBL obj = new DynaxPackagesBL();
                ob = obj.GetPackageDetails(Id);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(ob);
        }
        
        [HttpPost]
        public ActionResult Edit(DynaxPackage ob)
        {
            ViewBag.Status = 0;

            try
            {
                if (!ModelState.IsValid)
                    return View(ob);

                DynaxPackagesBL objBl = new DynaxPackagesBL();
                bool flag = objBl.UpdatePackage(ob);
                if (flag == true)
                {
                    ModelState.Clear();
                    return Redirect(Url.Content("~/dpackages"));
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