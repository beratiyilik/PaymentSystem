using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using PaymentSystem.Application.Interfaces;
using PaymentSystem.Application.Mapper;
using PaymentSystem.Application.Models;
using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Exceptions;
using PaymentSystem.Common.Filters;
using PaymentSystem.Common.Repositories;
using PaymentSystem.Common.Stores;
using PaymentSystem.Common.Utilities;
using PaymentSystem.Common.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using PaymentSystem.Common.Data;

namespace PaymentSystem.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;

        public IdentityService(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        }

        public async Task<IEnumerable<UserModel>> GetAllUserAsync()
        {
            var userList = await _identityRepository.GetAllUserAsync();
            var mappedUserList = ObjectMapper.Mapper.Map<IEnumerable<UserModel>>(userList);
            return mappedUserList;
        }

        public UserModel Validate(string userNameOrEmail, string password)
        {
            try
            {
                var user = _identityRepository.Validate(userNameOrEmail, password);

                /*
                if (!SecurityUtilities.VerifyHashedPassword(user.PasswordHash, password))
                    throw new IncorrectLoginAttemptException(null);
                */

                if (user == null)
                    throw new IncorrectLoginAttemptException(null);

                user = _identityRepository.GetUserByUserNameOrEmail(userNameOrEmail);

                var userModel = new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    State = user.State,
                    Roles = user.Roles.Select(m => m.Role.Name).ToArray(),
                    UserType = user.UserType,
                    EntityType = user.EntityType
                };

                return userModel;
            }
            catch (UserNotFoundException)
            {
                // do not give unnecessary info to users
                throw new IncorrectLoginAttemptException(null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserModel RegisterMembership(UserModel userModel, Guid userId)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(userModel.Email);

                var membership = new User()
                {
                    CreatedById = userId,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    NationalIdentificationNumber = userModel.NationalIdentificationNumber,
                    TaxNumber = userModel.TaxNumber,
                    UserType = Common.Enum.Enum.UserType.Membership,
                    EntityType = userModel.EntityType,
                    Email = userModel.Email,
                    PasswordHash = SecurityUtilities.HashPassword(userModel.Password),
                    UserName = mailAddress.User
                };

                var membershipRoleId = _identityRepository.GetAllRoles().Where(m => m.Name.Contains("Membership")).Select(m => m.Id).FirstOrDefault();

                /* adding membership role */
                membership.Roles.Add(new UserRole
                {
                    RoleId = membershipRoleId,
                    CreatedById = userId
                });

                /* adding deposit record */
                membership.AccrualLoans.Add(new AccrualLoan
                {
                    Type = Common.Enum.Enum.AccrualLoanType.Deposit,
                    Amount = 100m,
                    ExpiryDate = DateTime.UtcNow,
                    CreatedById = userId,
                    CreatedAt = DateTime.UtcNow,
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 100m,
                            CreatedById = userId,
                            CreatedAt = DateTime.UtcNow,
                        }
                    }
                });

                membership = _identityRepository.AddUser(membership);

                var mappedMembership = ObjectMapper.Mapper.Map<UserModel>(membership);

                return mappedMembership;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<UserModel> GetMembershipByFilter(UserFilter filter)
        {
            var result = _identityRepository.GetUserByFilter(filter).ToList();
            return ObjectMapper.Mapper.Map<IEnumerable<UserModel>>(result);
        }

        public void UnsubcribeMembership(UserModel userModel, Guid userId)
        {
            var user = _identityRepository.GetUserByFilter(new UserFilter { Id = userModel.Id }).FirstOrDefault();

            var membership = ObjectMapper.Mapper.Map<UserModel>(user);

            if (membership.AccrualLoans.Where(m => m.Type == Common.Enum.Enum.AccrualLoanType.Invoice).Any(m => m.HasLoan))
            {
                // TODO: throw membership has loan
                return;
                // throw new MembershipHasLoanException();
            }

            var deposit = membership.AccrualLoans.FirstOrDefault(m => m.Type == Common.Enum.Enum.AccrualLoanType.Deposit);

            if (!(deposit != null && deposit.CollectionRecords.Any(m => m.IsRefund) && deposit.CollectionRecords.Sum(m => m.Amount) == 0))
            {
                // TODO: throw membership has avalable deposit
                return;
                // throw new MembershipHasDepositException(null);
            }

            user.State = Common.Enum.Enum.EntityState.Deleted;
            user.LastModifiedAt = DateTime.UtcNow;
            user.LastModifiedById = userId;

            _identityRepository.UpdateUser(user);
        }

        public UserModel GetUserByUserNameOrEmail(string userNameOrEmail)
        {
            /*
            var user = _identityRepository.GetUserByUserNameOrEmail(userNameOrEmail);
            return ObjectMapper.Mapper.Map<UserModel>(user);
            */

            var user = _identityRepository.GetUserByUserNameOrEmail(userNameOrEmail);

            var userModel = new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                State = user.State,
                Roles = user.Roles.Select(m => m.Role.Name).ToArray(),
                UserType = user.UserType,
                EntityType = user.EntityType
            };

            return userModel;
        }
    }
}
