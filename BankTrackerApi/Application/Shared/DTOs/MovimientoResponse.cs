using BankTrackerApi.Core.Tipos;

namespace BankTrackerApi.Application.Shared.DTOs
{
    public class MovimientoResponse
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }
        public decimal Cantidad { get; set; }
        public string Concepto { get; set; }
        public DateTime Fecha { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
    }
}
