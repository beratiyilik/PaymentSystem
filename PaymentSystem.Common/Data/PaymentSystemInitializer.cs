using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PaymentSystem.Common.Data;
using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Managers;
using PaymentSystem.Common.Stores;
using PaymentSystem.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PaymentSystem.Common.Data
{
    public class PaymentSystemInitializer : DropCreateDatabaseAlways<PaymentSystemContext>
    {
        protected override void Seed(PaymentSystemContext context)
        {
            Init(context);
            base.Seed(context);
        }

        public static void Init(PaymentSystemContext context)
        {
            try
            {
                var userManager = new AppUserManager(new AppUserStore(context));
                var roleManager = new AppRoleManager(new AppRoleStore(context));

                if (!context.Roles.Any())
                {
                    /*
                    foreach (var role in GetPreconfiguredRoles())
                    {
                        context.Roles.Add(role);
                    }
                    */

                    foreach (var role in GetPreconfiguredRoles())
                    {
                        if (roleManager.FindByName(role.Name) != null)
                            continue;

                        roleManager.Create(role);
                    }
                }

                if (!context.Users.Any())
                {
                    foreach (var user in GetPreconfiguredUsers())
                    {
                        if (userManager.FindByName(user.UserName) != null)
                            continue;

                        var result = userManager.Create(user, DEFAULT_PASSWD);
                        result = userManager.SetLockoutEnabled(user.Id, false);
                    }
                }

                if (!context.UserRoles.Any())
                {
                    /*
                    foreach (var userRole in GetPreconfiguredUserRole())
                    {
                        context.UserRoles.Add(userRole);
                    }
                    */

                    // admin
                    userManager.AddToRole(ADMIN_ID, "Admin");
                    userManager.AddToRole(ADMIN_ID, "Membership");

                    // hangfire
                    userManager.AddToRole(HANGFIRE_ID, "Hangfire");

                    // berat
                    userManager.AddToRole(BERAT_ID, "Membership");

                    // sidus
                    userManager.AddToRole(SIDUS_ID, "Membership");

                    // zeynep
                    userManager.AddToRole(ZEYNEP_ID, "Membership");

                    // eymen
                    userManager.AddToRole(EYMEN_ID, "Membership");

                    // uras
                    userManager.AddToRole(URAS_ID, "Membership");
                }

                if (!context.AccrualLoan.Any())
                {
                    foreach (var accrualLoan in GetPreconfiguredAccrualLoan())
                    {
                        context.AccrualLoan.Add(accrualLoan);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static Guid ADMIN_ROL_ID { get => Guid.Parse("17C090B7-B095-4C64-B950-64782C5EC631"); }
        private static Guid MEMBERSHIP_ROL_ID { get => Guid.Parse("97FB9EFA-5C54-4199-9C77-F8521069A46C"); }
        private static Guid HANGFIRE_ROL_ID { get => Guid.Parse("93C1AE7E-0BFA-4DE4-9999-4B9682328847"); }

        private static Guid SYSTEM_USER_ID { get => Guid.Parse("5A1D0CFB-EFCC-47A4-9DB8-5A80EB69AEEC"); }
        private static Guid ADMIN_ID { get => Guid.Parse("7C9C673A-E8FA-4B08-BF93-5773F75C9EA7"); }
        private static Guid HANGFIRE_ID { get => Guid.Parse("5E6F41C6-9EC1-4245-9FE1-EE254C971214"); }
        private static Guid BERAT_ID { get => Guid.Parse("CA3225B8-32E1-4AC3-9280-331FF598FF94"); }
        private static Guid SIDUS_ID { get => Guid.Parse("E6F06F01-BED3-4826-90E9-F97F3576C8C1"); }
        private static Guid ZEYNEP_ID { get => Guid.Parse("70F3D138-8E25-467D-AAF2-1CC99A608A9B"); }
        private static Guid EYMEN_ID { get => Guid.Parse("32504230-4F81-4F22-82A2-2549EC63FA5B"); }
        private static Guid URAS_ID { get => Guid.Parse("7E1A9F82-1815-47C2-BDDF-ABB9FC909E3C"); }

        private static string DEFAULT_PASSWD { get => "123456"; }

        private static string ADMIN_PASSWD_HASH { get => SecurityUtilities.HashPassword(DEFAULT_PASSWD); }
        private static string HANGFIRE_PASSWD_HASH { get => SecurityUtilities.HashPassword(DEFAULT_PASSWD); }
        private static string BERAT_PASSWD_HASH { get => SecurityUtilities.HashPassword(DEFAULT_PASSWD); }
        private static string SIDUS_PASSWD_HASH { get => SecurityUtilities.HashPassword(DEFAULT_PASSWD); }
        private static string ZEYNEP_PASSWD_HASH { get => SecurityUtilities.HashPassword(DEFAULT_PASSWD); }
        private static string EYMEN_PASSWD_HASH { get => SecurityUtilities.HashPassword(DEFAULT_PASSWD); }
        private static string URAS_PASSWD_HASH { get => SecurityUtilities.HashPassword(DEFAULT_PASSWD); }


        private static IEnumerable<Role> GetPreconfiguredRoles()
        {
            return new List<Role>()
            {
                new Role()
                {
                    Id = ADMIN_ROL_ID,
                    Name = "Admin", // Manager
                    CreatedById = SYSTEM_USER_ID
                },
                new Role()
                {
                    Id = MEMBERSHIP_ROL_ID,
                    Name = "Membership",
                    CreatedById = SYSTEM_USER_ID
                },
                new Role()
                {
                    Id = HANGFIRE_ROL_ID,
                    Name = "Hangfire",
                    CreatedById = SYSTEM_USER_ID
                }
            };
        }

        private static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Id = SYSTEM_USER_ID,
                    FirstName = "System",
                    LastName = "User",
                    UserName = "systemuser",
                    CreatedById = SYSTEM_USER_ID,
                    UserType = Common.Enum.Enum.UserType.Manager,
                    EntityType = Common.Enum.Enum.EntityType.NaturalPerson
                },
                new User()
                {
                    Id = HANGFIRE_ID,
                    FirstName = "Hangfire",
                    LastName = "Hangfire",
                    UserName = "hangfire",
                    Email = "hangfire@domain.com",
                    CreatedById = SYSTEM_USER_ID,
                    // PasswordHash = HANGFIRE_PASSWD_HASH,
                    UserType = Common.Enum.Enum.UserType.Manager,
                    EntityType = Common.Enum.Enum.EntityType.NaturalPerson
                },
                new User()
                {
                    Id = ADMIN_ID,
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin",
                    Email = "admin@domain.com",
                    CreatedById = SYSTEM_USER_ID,
                    // PasswordHash = ADMIN_PASSWD_HASH,
                    UserType = Common.Enum.Enum.UserType.Manager,
                    EntityType = Common.Enum.Enum.EntityType.NaturalPerson
                },
                new User()
                {
                    Id = BERAT_ID,
                    FirstName = "Berat",
                    LastName = "İyilik",
                    UserName = "beratiyilik",
                    Email = "beratiyilik@domain.com",
                    CreatedById = SYSTEM_USER_ID,
                    // PasswordHash = BERAT_PASSWD_HASH,
                    UserType = Common.Enum.Enum.UserType.Membership,
                    EntityType = Common.Enum.Enum.EntityType.NaturalPerson,
                    NationalIdentificationNumber = TurkishIdentificationNumber.GenerateRandom()
                },
                new User()
                {
                    Id = SIDUS_ID,
                    FirstName = "Sidus",
                    LastName = "Tech",
                    UserName = "sidustech",
                    Email = "sisdustech@domain.com",
                    CreatedById = SYSTEM_USER_ID,
                    // PasswordHash = SIDUS_PASSWD_HASH,
                    UserType = Common.Enum.Enum.UserType.Membership,
                    EntityType = Common.Enum.Enum.EntityType.LegalEntity,
                    TaxNumber = "73780754562"
                },
                new User()
                {
                    Id = ZEYNEP_ID,
                    FirstName = "Zeynep",
                    LastName = "Unsal",
                    UserName = "zeynepunsal",
                    Email = "zeynepunsal@domain.com",
                    CreatedById = SYSTEM_USER_ID,
                    // PasswordHash = ZEYNEP_PASSWD_HASH,
                    UserType = Common.Enum.Enum.UserType.Membership,
                    EntityType = Common.Enum.Enum.EntityType.NaturalPerson,
                    NationalIdentificationNumber = TurkishIdentificationNumber.GenerateRandom()
                },
                new User()
                {
                    Id = EYMEN_ID,
                    FirstName = "Eymen",
                    LastName = "Unsal",
                    UserName = "eymenunsal",
                    Email = "eymenunsal@domain.com",
                    CreatedById = SYSTEM_USER_ID,
                    // PasswordHash = EYMEN_PASSWD_HASH,
                    UserType = Common.Enum.Enum.UserType.Membership,
                    EntityType = Common.Enum.Enum.EntityType.NaturalPerson,
                    NationalIdentificationNumber = TurkishIdentificationNumber.GenerateRandom()
                },
                new User()
                {
                    Id = URAS_ID,
                    FirstName = "Uras",
                    LastName = "Kucukkaya",
                    UserName = "uraskucukkaya",
                    Email = "uraskucukkaya@domain.com",
                    CreatedById = SYSTEM_USER_ID,
                    // PasswordHash = URAS_PASSWD_HASH,
                    UserType = Common.Enum.Enum.UserType.Membership,
                    EntityType = Common.Enum.Enum.EntityType.NaturalPerson,
                    NationalIdentificationNumber = TurkishIdentificationNumber.GenerateRandom()
                },
            };
        }

        private static IEnumerable<UserRole> GetPreconfiguredUserRole()
        {
            return new List<UserRole>()
            {
                new UserRole()
                {
                    RoleId = ADMIN_ROL_ID,
                    UserId = ADMIN_ID,
                    CreatedById = SYSTEM_USER_ID
                },
                new UserRole()
                {
                    RoleId = MEMBERSHIP_ROL_ID,
                    UserId = ADMIN_ID,
                    CreatedById = SYSTEM_USER_ID
                },
                new UserRole()
                {
                    RoleId = HANGFIRE_ROL_ID,
                    UserId = HANGFIRE_ID,
                    CreatedById = SYSTEM_USER_ID
                },
                new UserRole()
                {
                    RoleId = MEMBERSHIP_ROL_ID,
                    UserId = BERAT_ID,
                    CreatedById = SYSTEM_USER_ID
                },
                new UserRole()
                {
                    RoleId = MEMBERSHIP_ROL_ID,
                    UserId = SIDUS_ID,
                    CreatedById = SYSTEM_USER_ID
                },
                new UserRole()
                {
                    RoleId = MEMBERSHIP_ROL_ID,
                    UserId = ZEYNEP_ID,
                    CreatedById = SYSTEM_USER_ID
                },
            };
        }

        private static IEnumerable<AccrualLoan> GetPreconfiguredAccrualLoan()
        {
            return new List<AccrualLoan>()
            {
                new AccrualLoan()
                {
                    Type = Common.Enum.Enum.AccrualLoanType.Deposit,
                    MembershipId = BERAT_ID,
                    Amount = 100m,
                    ExpiryDate = DateTime.UtcNow,
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 100m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                        }
                    }
                },
                new AccrualLoan()
                {
                    Type = Common.Enum.Enum.AccrualLoanType.Deposit,
                    MembershipId = ZEYNEP_ID,
                    Amount = 100m,
                    ExpiryDate = DateTime.UtcNow,
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 100m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                        }
                    }
                },
                 new AccrualLoan()
                {
                    Type = Common.Enum.Enum.AccrualLoanType.Deposit,
                    MembershipId = EYMEN_ID,
                    Amount = 100m,
                    ExpiryDate = DateTime.UtcNow,
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 100m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                        }
                    }
                },
                  new AccrualLoan()
                {
                    Type = Common.Enum.Enum.AccrualLoanType.Deposit,
                    MembershipId = URAS_ID,
                    Amount = 100m,
                    ExpiryDate = DateTime.UtcNow,
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 100m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                        }
                    }
                },
                new AccrualLoan()
                {
                    Type = Common.Enum.Enum.AccrualLoanType.Deposit,
                    MembershipId = SIDUS_ID,
                    Amount = 100m,
                    ExpiryDate = DateTime.UtcNow,
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 100m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfPreviousMonth(),
                        }
                    }
                },
                new AccrualLoan()
                {
                    MembershipId = BERAT_ID,
                    Amount = 30m,
                    ExpiryDate = DateUtilities.GetFirstDayOfNextMonth().AddDays((double)(new Random().Next(4, 19))),
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetLastDayOfCurrentMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 30m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfNextMonth().AddDays(3),
                        }
                    }
                },
                new AccrualLoan()
                {
                    MembershipId = ZEYNEP_ID,
                    Amount = 70m,
                    ExpiryDate = DateUtilities.GetFirstDayOfNextMonth().AddDays((double)(new Random().Next(4, 19))),
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetLastDayOfCurrentMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        new CollectionRecord()
                        {
                            Amount = 40m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfNextMonth().AddDays(1),
                        },
                        new CollectionRecord()
                        {
                            Amount = 30m,
                            CreatedById = SYSTEM_USER_ID,
                            CreatedAt = DateUtilities.GetFirstDayOfNextMonth().AddDays(2),
                        }
                    }
                },
                new AccrualLoan()
                {
                    MembershipId = SIDUS_ID,
                    Amount = 50m,
                    ExpiryDate = DateUtilities.GetFirstDayOfNextMonth().AddDays((double)(new Random().Next(4, 19))),
                    CreatedById = SYSTEM_USER_ID,
                    CreatedAt = DateUtilities.GetLastDayOfCurrentMonth(),
                    CollectionRecords = new List<CollectionRecord>()
                    {
                        /* no payment */
                    }
                }
            };
        }
    }
}
