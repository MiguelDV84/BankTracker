using BankTrackerApi.Application.Shared.DTOs;

namespace BankTrackerApi.Application.Services.Cuenta
{
    public interface ICuentaService
    {
        Task<CuentaResponse> RegisterAsync(CuentaRequest request);
        Task<CuentaResponse> LoginAsync(LoginRequest request);
        Task<IEnumerable<CuentaResponse>> GetAllCuentasAsync();
        Task<CuentaResponse> GetCuentaAsync();
        Task<CuentaResponse> UpdateCuentaAsync(UpdateCuentaRequest request);
        Task DeleteCuentaAsync(int id);
    }
}
