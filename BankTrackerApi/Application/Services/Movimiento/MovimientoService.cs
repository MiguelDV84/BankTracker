using BankTrackerApi.Application.Shared.DTOs;
using BankTrackerApi.Core.Modelos;
using BankTrackerApi.Core.Tipos;
using BankTrackerApi.Infrastructure.Repositories;
using System.Security.Claims;

namespace BankTrackerApi.Application.Services.Movimiento
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovimientoService(IMovimientoRepository movimientoRepository, IHttpContextAccessor httpContextAccessor, ICuentaRepository cuentaRepository)
        {
            _movimientoRepository = movimientoRepository;
            _httpContextAccessor = httpContextAccessor;
            _cuentaRepository = cuentaRepository;
        }

        public async Task<MovimientoResponse> CreateMovimientoAsync(MovimientoRequest request)
        {
            var dniCliente = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new UnauthorizedAccessException("No se ha encontrado el numero de cuenta en el token.");

            var cuenta = await _cuentaRepository.GetCuentaByDniClienteAsync(dniCliente);

            var nuevoMovimiento = new Movimientos
            {
                CuentaId = cuenta.Id,
                Cantidad = request.Cantidad,
                Concepto = request.Concepto,
                Fecha = DateTime.Now,
                TipoMovimiento = request.TipoMovimiento
            };

            var movimientoCreado = await _movimientoRepository.CreateMovimientoAsync(nuevoMovimiento);

            cuenta.Saldo+= request.Cantidad;
            await _cuentaRepository.UpdateCuentaAsync(cuenta);

            return new MovimientoResponse
            {
                Id = movimientoCreado.Id,
                CuentaId = movimientoCreado.CuentaId,
                Cantidad = movimientoCreado.Cantidad,
                Concepto = movimientoCreado.Concepto,
                Fecha = movimientoCreado.Fecha,
                TipoMovimiento = movimientoCreado.TipoMovimiento
            };
        }

        public async Task<IEnumerable<MovimientoResponse>> GetAllMovimientosAsync()
        {
            var movimientos = await _movimientoRepository.GetMovimientosAsync();
            return movimientos.Select(m => new MovimientoResponse
            {
                Id = m.Id,
                CuentaId = m.CuentaId,
                Cantidad = m.Cantidad,
                Concepto = m.Concepto,
                Fecha = m.Fecha,
                TipoMovimiento = m.TipoMovimiento
            });
        }

        public async Task<MovimientoResponse> UpdateMovimientoAsync(UpdateMovimientoRequest request)
        {
            var movimientoActualizado = new Movimientos
            {
                Id = request.Id,
                CuentaId = request.CuentaId,
                Cantidad = request.Cantidad,
                Concepto = request.Concepto,
                Fecha = request.Fecha,
                TipoMovimiento = request.TipoMovimiento
            };

            await _movimientoRepository.UpdateMovimientoAsync(movimientoActualizado);

            return new MovimientoResponse
            {
                Id = movimientoActualizado.Id,
                CuentaId = movimientoActualizado.CuentaId,
                Cantidad = movimientoActualizado.Cantidad,
                Concepto = movimientoActualizado.Concepto,
                Fecha = movimientoActualizado.Fecha,
                TipoMovimiento = movimientoActualizado.TipoMovimiento
            };
        }

        public async Task DeleteMovimientoAsync(int id)
        {
            await _movimientoRepository.DeleteMovimientoAsync(id);
        }
    }
}
