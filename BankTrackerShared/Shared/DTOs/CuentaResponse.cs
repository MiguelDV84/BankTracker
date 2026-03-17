namespace BankTrackerShared.Shared.DTOs
{
    public class CuentaResponse
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public string DniCliente { get; set; }
        public string NombreTitular { get; set; }
        public decimal Saldo { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}

