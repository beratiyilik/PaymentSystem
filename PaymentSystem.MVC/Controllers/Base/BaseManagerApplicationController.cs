using PaymentSystem.MVC.ConstantValues;

namespace PaymentSystem.MVC.Controllers.Base
{
    public abstract class BaseManagerApplicationController : BaseApplicationController
    {
        protected override string SESSION_KEY { get => Keys.MANAGER_SESSION_KEY; }
    }
}
