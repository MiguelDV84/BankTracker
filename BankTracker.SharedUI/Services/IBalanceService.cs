using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTracker.SharedUI.Services
{
    public interface IBalanceStateService
    {
        event Action OnChange;
        void NotifyChange();
    }
}
