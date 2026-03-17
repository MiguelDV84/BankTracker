
using BankTrackerShared.Core.Tipos;

namespace BankTrackerShared.Shared.DTOs

{
    public class MovimientoRequest
    {
        public decimal Cantidad { get; set; }
        public string Concepto { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
    }
}
