using PaymentSystem.Application.Interfaces;
using PaymentSystem.Application.Mapper;
using PaymentSystem.Application.Models;
using PaymentSystem.Common.Exceptions;
using PaymentSystem.Common.Filters;
using PaymentSystem.Common.Repositories;
using PaymentSystem.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentSystem.Application.Services
{
    public class PaymentSystemService : IPaymentSystemService
    {
        private readonly IAccrualLoanRepository _accrualLoanRepository;
        private readonly ICollectionRecordRepository _collectionRecordRepository;

        public PaymentSystemService(IAccrualLoanRepository accrualLoanRepository, ICollectionRecordRepository collectionRecordRepository)
        {
            _accrualLoanRepository = accrualLoanRepository ?? throw new ArgumentNullException(nameof(accrualLoanRepository));
            _collectionRecordRepository = collectionRecordRepository ?? throw new ArgumentNullException(nameof(collectionRecordRepository));
        }

        public AccrualLoanModel GetAccrualLoanById(Guid id)
        {
            var query = _accrualLoanRepository.GetAccrualLoans();

            var model = query.FirstOrDefault(m => m.Id == id);

            return ObjectMapper.Mapper.Map<AccrualLoanModel>(model);
        }

        public IEnumerable<AccrualLoanModel> GetAccrualLoans(AccrualLoanFilter filter)
        {
            var query = _accrualLoanRepository.GetAccrualLoans();

            query = filter != null && filter.MembershipId != Guid.Empty ? query.Where(m => m.MembershipId == filter.MembershipId) : query;

            query = filter != null && filter.Type.HasValue ? query.Where(m => m.Type == filter.Type.Value) : query;

            var mappedList = ObjectMapper.Mapper.Map<IEnumerable<AccrualLoanModel>>(query.ToList());

            return mappedList;
        }

        public UserModel GetMembershipPaymentInfo(Guid id)
        {
            var result = _accrualLoanRepository.GetMembershipPaymentInfo(id);
            return ObjectMapper.Mapper.Map<UserModel>(result);
        }

        public IEnumerable<UserModel> GetMembershipsWithPaymentInfo()
        {
            var result = _accrualLoanRepository.GetMembershipsWithPaymentInfo().ToList();
            return ObjectMapper.Mapper.Map<IEnumerable<UserModel>>(result);
        }

        public AccrualLoanModel Payment(AccrualLoanModel model)
        {
            var accrualLoan = _accrualLoanRepository.GetById(model.Id);

            var mappedAccrualLoan = ObjectMapper.Mapper.Map<AccrualLoanModel>(accrualLoan);

            if (!mappedAccrualLoan.HasLoan || model.Amount < 0 || model.Amount > mappedAccrualLoan.RemainingLoan)
            {
                // TODO: create new custumized exception 
                return model;
            }

            accrualLoan.CollectionRecords.Add(new Common.Entities.CollectionRecord
            {
                Amount = model.Amount,
                CreatedById = model.CreatedBy,
                CreatedAt = DateTime.UtcNow
            });

            _accrualLoanRepository.SaveChanges();

            return ObjectMapper.Mapper.Map<AccrualLoanModel>(accrualLoan);
        }

        public void DepositRefund(UserModel model)
        {
            var membershipId = model.Id;

            var orginalAccrualLoans = _accrualLoanRepository.GetAccrualLoans().Where(m => m.MembershipId == membershipId).ToList();

            var accrualLoans = ObjectMapper.Mapper.Map<IEnumerable<AccrualLoanModel>>(orginalAccrualLoans);

            if (accrualLoans.Where(m => m.Type == Common.Enum.Enum.AccrualLoanType.Invoice).Any(m => m.HasLoan))
            {
                // TODO: throw has loan
                // throw new MembershipHasLoanException(null);
                return;
            }

            var deposit = accrualLoans.FirstOrDefault(m => m.Type == Common.Enum.Enum.AccrualLoanType.Deposit);

            if (deposit == null || !deposit.CollectionRecords.Any())
            {
                // TODO: throw has no deposit
                // throw new MembershipHasNoDepositException(null);
                return;
            }

            if (deposit.CollectionRecords.Any(m => m.IsRefund) && deposit.CollectionRecords.Sum(m => m.Amount) == 0)
            {
                // TODO: throw deposit already refunded
                return;
            }

            var orginalDeposit = orginalAccrualLoans.FirstOrDefault(m => m.Id == deposit.Id);

            var orginalDepositCollectionRecord = orginalDeposit.CollectionRecords.FirstOrDefault();

            orginalDeposit.CollectionRecords.Add(new Common.Entities.CollectionRecord()
            {
                Amount = (-1) * (orginalDepositCollectionRecord.Amount),
                CreatedById = model.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                RefNumber = $"{orginalDepositCollectionRecord.RefNumber}-{StringUtilities.GenerateKey(3)}",
                IsRefund = true
            });

            _accrualLoanRepository.SaveChanges();

            return;
        }

    }
}
