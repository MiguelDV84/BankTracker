namespace BankTrackerShared.Core.Modelos
{
    public class Cuenta
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public string NombreTitular { get; set; }
        public string DniCliente { get; set; }
        public decimal Saldo { get; set; }
        public string PasswordHash { get; set; }
        public List<Movimientos> Movimientos { get; set; }

    }
}
