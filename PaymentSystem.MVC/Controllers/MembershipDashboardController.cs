using PaymentSystem.MVC.ConstantValues;
using PaymentSystem.MVC.Controllers.Base;
using PaymentSystem.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentSystem.MVC.Controllers
{
    public class MembershipDashboardController : BaseMembershipApplicationController
    {
        // GET: MembershipDashboard
        public ActionResult Index()
        {
            if (TempData.ContainsKey("ViewBag.Message"))
            {
                ViewBag.Message = TempData["ViewBag.Message"].ToString();
                TempData.Remove("ViewBag.Message");
            }

            return View();
        }

        
        public ActionResult AccrualLoans()
        {
            // token has already current user info
            // var filter = new { MembershipId = CurrentUser.Id };

            var response = GetAPIRequestAsync<IEnumerable<AccrualLoanViewModel>>(EndPoints.PAYMENT_SYSTEM_GET_ACCRUAL_LOANS);

            TempData["ViewBag.Message"] = response.Message;
            
            return View(response.ResultData);
        }

        public ActionResult CollectionRecords()
        {
            var response = GetAPIRequestAsync<IEnumerable<AccrualLoanViewModel>>(EndPoints.PAYMENT_SYSTEM_GET_ACCRUAL_LOANS);

            TempData["ViewBag.Message"] = response.Message;

            return View(response.ResultData);
        }

        [HttpPost]
        public ActionResult Payment(AccrualLoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = PostAPIRequest<AccrualLoanViewModel>(EndPoints.PAYMENT_SYSTEM_PAYMENT, model);

                TempData["ViewBag.Message"] = response.Message;
            }

            return RedirectToAction("AccrualLoans");
        }
    }
}