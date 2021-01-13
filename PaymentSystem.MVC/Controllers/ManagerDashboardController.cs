using Newtonsoft.Json;
using PaymentSystem.MVC.ConstantValues;
using PaymentSystem.MVC.Controllers.Base;
using PaymentSystem.MVC.Filters;
using PaymentSystem.MVC.Models;
using PaymentSystem.MVC.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentSystem.MVC.Controllers
{
    public class ManagerDashboardController : BaseManagerApplicationController
    {
        // GET: ManagerDashboard
        public ActionResult Index()
        {
            if (TempData.ContainsKey("ViewBag.Message"))
            {
                ViewBag.Message = TempData["ViewBag.Message"].ToString();
                TempData.Remove("ViewBag.Message");
            }

            return View();
        }

        [HttpPost]
        public ActionResult RegisterMembership(UserViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {
                var response = PostAPIRequest<UserViewModel>(EndPoints.MEMBERSHIP_REGISTER_MEMBERSHIP, userViewModel);

                TempData["ViewBag.Message"] = response.ResultData.Id == Guid.Empty ? "Registration process did not complete successfully!" : response.Message;
            }

            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult EnquiryMembership(MembershipFilter filter)
        {

            if (ModelState.IsValid)
            {
                var response = PostAPIRequest<IEnumerable<UserViewModel>>(EndPoints.MEMBERSHIP_ENQUIRY_MEMBERSHIP, filter);

                TempData["ViewBag.Message"] = response.Message;

                return View(response.ResultData);
            }

            return Redirect("Index");
        }

        public ActionResult AccrualLoans()
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

        [HttpPost]
        public ActionResult DepositRefund(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = PostAPIRequest<object>(EndPoints.PAYMENT_SYSTEM_DEPOSIT_REFUND, model);

                TempData["ViewBag.Message"] = $"Deposit Refund {res.Message}";
            }

            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult UnsubcribeMembership(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = PostAPIRequest<object>(EndPoints.MEMBERSHIP_UNSUBCRIBE_MEMBERSHIP, model);

                TempData["ViewBag.Message"] = $"Unsubcribe Membership {res.Message}";
            }

            return Redirect("Index");
        }

    }
}