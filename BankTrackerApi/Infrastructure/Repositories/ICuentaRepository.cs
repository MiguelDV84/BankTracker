using BankTrackerApi.Core.Modelos;

namespace BankTrackerApi.Infrastructure.Repositories
{
    public interface ICuentaRepository
    {
        Task<IEnumerable<Cuenta>> GetCuentasAsync();
        Task<Cuenta> GetCuentaByDniClienteAsync(string numeroCuenta);
        Task<Cuenta> CreateCuentaAsync(Cuenta cuenta);
        Task UpdateCuentaAsync(Cuenta cuenta);
        Task DeleteCuentaAsync(int id);
    }
}
