namespace BankTrackerApi.Core.Modelos
{
    public class Movimientos
    {
        private int Id { get; set; }
        private int CuentaId { get; set; }
        private decimal Cantidad { get; set; }
        private string Concepto { get; set; }
        private DateTime Fecha { get; set; }
        private string TipoMovimiento { get; set; }
    }
}
