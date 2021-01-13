using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Entities.Base;
using PaymentSystem.Common.Exceptions;
using PaymentSystem.Common.Filters;
using PaymentSystem.Common.Repositories;
using PaymentSystem.Common.Data;
using PaymentSystem.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Common.Managers;
using PaymentSystem.Common.Stores;

namespace PaymentSystem.Infrastructure.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        protected readonly PaymentSystemContext _dbContext;

        private readonly AppUserManager _userManager;

        public IdentityRepository(PaymentSystemContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            _userManager = new AppUserManager(new AppUserStore(dbContext));
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }

        public User GetUserByUserNameOrEmail(string userNameOrEmail)
        {
            var user = _dbContext.Set<User>().Include("Roles.Role").Where(m => m.UserName == userNameOrEmail || m.Email == userNameOrEmail).FirstOrDefault();

            if (user == null)
            {
                throw new UserNotFoundException($"user not found with {userNameOrEmail} credential!");
            }

            return user;
        }

        public User AddUser(User user)
        {

            _dbContext.Users.Add(user);

            var result = _dbContext.SaveChanges();

            return user;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _dbContext.Set<Role>().AsEnumerable();
        }

        public IEnumerable<User> GetUserByFilter(UserFilter userFilter)
        {
            var query = _dbContext.Set<User>()
                .Include("AccrualLoans.CollectionRecords")
                .Include("AccrualLoans.Membership")
                .Where(m => m.State == Common.Enum.Enum.EntityState.Active);

            if (userFilter == null)
                return query.AsEnumerable();

            query = query.Where(m => m.EntityType == userFilter.EntityType);

            if (!string.IsNullOrWhiteSpace(userFilter.NationalIdentificationNumber) && userFilter.EntityType == PaymentSystem.Common.Enum.Enum.EntityType.NaturalPerson)
            {
                query = query.Where(m => m.NationalIdentificationNumber == userFilter.NationalIdentificationNumber);
            }

            if (!string.IsNullOrWhiteSpace(userFilter.TaxNumber) && userFilter.EntityType == PaymentSystem.Common.Enum.Enum.EntityType.LegalEntity)
            {
                query = query.Where(m => m.TaxNumber == userFilter.TaxNumber);
            }

            return query.AsEnumerable();
        }

        public void UpdateUser(User user)
        {
            user.LastModifiedAt = DateTime.UtcNow;

            _dbContext.SaveChanges();
        }

        public User Validate(string userNameOrEmail, string password)
        {
            return _userManager.FindAsync(userNameOrEmail, password).Result;
        }
    }
}
