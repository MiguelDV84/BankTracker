using BankTrackerApi.Core.Modelos;

namespace BankTrackerApi.Infrastructure.Repositories
{
    public interface IMovimientoRepository
    {
        Task<IEnumerable<Movimientos>> GetMovimientosAsync();
        Task<Movimientos> CreateMovimientoAsync(Movimientos movimiento);
        Task UpdateMovimientoAsync(Movimientos movimiento);
        Task DeleteMovimientoAsync(int id);
    }
}
