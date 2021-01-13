using AutoMapper;
using PaymentSystem.Application.Interfaces;
using PaymentSystem.Application.Mapper;
using PaymentSystem.Application.Models;
using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Repositories;
using PaymentSystem.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Services
{
    public class BillingService : IBillingService
    {
        private readonly IAccrualLoanRepository _accrualLoanRepository;

        private readonly IIdentityRepository _identityRepository;

        public BillingService(IAccrualLoanRepository accrualLoanRepository, IIdentityRepository identityRepository)
        {
            _accrualLoanRepository = accrualLoanRepository ?? throw new ArgumentNullException(nameof(accrualLoanRepository));
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        }

        public void TriggerMonthlyBilling(Guid userId)
        {
            var today = DateTime.Today;

            var endOfMonth = DateUtilities.GetLastDayOfCurrentMonth();

            if (today.Date != endOfMonth.Date)
            {
                // TODO: 
                return;
            }

            var orginalMemberships = _identityRepository.GetUserByFilter(null).ToList().Where(m => m.UserType == Common.Enum.Enum.UserType.Membership);

            var memberships = ObjectMapper.Mapper.Map<IEnumerable<UserModel>>(orginalMemberships);

            foreach (var ms in memberships)
            {
                var hasAlready = ms.AccrualLoans.Any(m => m.Type == Common.Enum.Enum.AccrualLoanType.Invoice && m.CreatedAt.Year == today.Year && m.CreatedAt.Month == today.Month);

                if (hasAlready)
                {
                    // TODO: 
                    continue;
                }

                var orginalMembership = orginalMemberships.FirstOrDefault(m => m.Id == ms.Id);

                var rnd = new Random();
                var generatedAmount = (decimal)rnd.Next(30, 70);
                var generatedDelayDays = rnd.Next(3, 17);

                orginalMembership.AccrualLoans.Add(new AccrualLoan()
                {
                    Amount = generatedAmount,
                    ExpiryDate = DateUtilities.GetFirstDayOfNextMonth().AddDays(generatedDelayDays),
                    CreatedById = userId,
                    CreatedAt = today.Date,
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        /* */
                    }
                });


                _identityRepository.UpdateUser(orginalMembership);

            }
        }
    }
}
