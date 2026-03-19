using BankTrackerApp.Shared.Services;

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
