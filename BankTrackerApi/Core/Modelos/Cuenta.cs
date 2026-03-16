namespace BankTrackerApi.Core.Modelos
{
    public class Cuenta
    {
        private int Id { get; set; }
        private string NumeroCuenta { get; set; }
        private string NombreTitular { get; set; }
        private decimal Saldo { get; set; }
        private string PasswrodHash { get; set; }
        private List<Movimientos> Movimientos { get; set; }

    }
}
