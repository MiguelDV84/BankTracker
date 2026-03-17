using BankTrackerApi.Application.Shared.DTOs;

namespace BankTrackerApi.Application.Services.Movimiento
{
    public interface IMovimientoService
    {
        Task<MovimientoResponse> CreateMovimientoAsync(MovimientoRequest request);
        Task<IEnumerable<MovimientoResponse>> GetAllMovimientosAsync();
        Task<MovimientoResponse> UpdateMovimientoAsync(UpdateMovimientoRequest request);
        Task DeleteMovimientoAsync(int id);
    }
}
