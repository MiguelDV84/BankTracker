namespace BankTrackerApi.Application.Shared.DTOs
{
    public class LoginRequest
    {
        public string DniCliente { get; set; }
        public string Password { get; set; }
    }
}
