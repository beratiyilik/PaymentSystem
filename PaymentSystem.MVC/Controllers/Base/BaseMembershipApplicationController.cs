using PaymentSystem.MVC.ConstantValues;

namespace PaymentSystem.MVC.Controllers.Base
{
    public abstract class BaseMembershipApplicationController : BaseApplicationController
    {
        protected override string SESSION_KEY { get => Keys.MEMBERSHIP_SESSION_KEY; }
    }
}
