using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using DynaxInvoice.BO;
using DynaxInvoice.BL;
using System.Web;
namespace DynaxInvoice.Web.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index(string returnUrl)
        {
            var role = DynaxInvoice.Web.Models.ClaimsExtensions.GetRole(this.User);
            if (role != null)
                return Redirect("~/dashboard");

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel objLv)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(objLv);

                UserBL objUser = new UserBL();                
                var result =  objUser.UserLogin(objLv);
                if (result == null)
                    return View(objLv);

                var strName = result.FullName;
                string uName = strName.Substring(0, strName.IndexOf(' '));
                ViewBag.Name = uName;

                var role = GetRoleName(result.UserType);

                var identity = new ClaimsIdentity(new[]
                 {
                        new Claim(ClaimTypes.Sid, result.Id.ToString()),
                              new Claim(ClaimTypes.Name, uName),
                              new Claim(ClaimTypes.Role, role),
                               new Claim(ClaimTypes.Email,result.DealerId.ToString())
                          }, "ApplicationCookie");

                var context = Request.GetOwinContext();
                var authManager = context.Authentication;
                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(objLv.ReturnUrl));

            }
            catch (Exception ex)
            {
             Response.Write("Dynax:HomeController-userLogin() - " + ex.Message);
            }
            return View();
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "dashboard");
            }

            return returnUrl;
        }
        private string GetRoleName(int id)
        {
            var strRole = "";
            if(id==1)
            {
                strRole = "Super Admin";
            }
            else if (id == 2)
            {
                strRole = "Dealer Admin";
            }
            else if (id == 3)
            {
                strRole = "End-user";
            }
            return strRole;
        }
    }
}