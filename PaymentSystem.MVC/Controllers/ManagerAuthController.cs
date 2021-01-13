using PaymentSystem.MVC.Controllers.Base;
using PaymentSystem.MVC.Models;
using System.Web.Mvc;

namespace PaymentSystem.MVC.Controllers
{
    public class ManagerAuthController : BaseManagerApplicationController
    {
        public ManagerAuthController() { }

        public ActionResult Login()
        {
            var managerLoginInfo = GetLoginInfo();

            if (managerLoginInfo != null && !managerLoginInfo.IsExpired)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AppUser userViewModel)
        {
            if (ModelState.IsValid)
            {

                var result = GetToken(userViewModel.UserName, userViewModel.Password, userViewModel.UserType.ToString());

                if (result != null)
                {
                    SetLoginInfo(result);

                    return RedirectToAction("Dashboard");
                }

            }
            return View(userViewModel);
        }

        public ActionResult Logout()
        {
            ClearLoginInfo();

            return RedirectToAction("Login");
        }

        public ActionResult Dashboard()
        {
            var managerLoginInfo = GetLoginInfo();

            if (managerLoginInfo != null && managerLoginInfo.IsExpired)
            {
                return RedirectToAction("Login");
            }

            // return View();
            return RedirectToAction("Index", "ManagerDashboard");
        }
    }
}