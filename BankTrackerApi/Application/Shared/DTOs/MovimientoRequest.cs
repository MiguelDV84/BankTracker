using BankTrackerApi.Core.Tipos;

namespace BankTrackerApi.Application.Shared.DTOs
{
    public class MovimientoRequest
    {
        public decimal Cantidad { get; set; }
        public string Concepto { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
    }
}
