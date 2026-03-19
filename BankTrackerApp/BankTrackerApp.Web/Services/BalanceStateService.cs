using BankTracker;
using BankTracker.SharedUI.Services;

namespace BankTrackerApp.Web.Services
{
    public class BalanceStateService : IBalanceStateService
    {
        public event Action? OnChange;

        public void NotifyChange()
        {
            OnChange?.Invoke();
        }
    }
}
