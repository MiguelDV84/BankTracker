using BankTrackerApp.Shared.Services;
using BankTrackerShared.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BankTrackerApp.Shared.Components.Balance
{
    public partial class Balance : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IBalanceStateService StateService { get; set; } = default!;


        private CuentaResponse? _cuenta;
        private bool _isLoading = true;
        private string? _errorMessage;

        protected override void OnInitialized()
        {
            StateService.OnChange += HandleBalanceChanged;
        }

        private async void HandleBalanceChanged()
        {
            await InvokeAsync(async () =>
            {
                await CargarDatosCuenta();
                StateHasChanged();
            });
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InicializarDatos();
            }
        }

        private async Task InicializarDatos()
        {
            try
            {
                _isLoading = true;
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                if (!string.IsNullOrEmpty(token))
                {
                    Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    await CargarDatosCuenta();
                }
                else
                {
                    _errorMessage = "Sesión no válida.";
                }
            }
            catch (Exception ex)
            {
                _errorMessage = "Error de conexión.";
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task CargarDatosCuenta()
        {
            var response = await Http.GetFromJsonAsync<ApiResponse<CuentaResponse>>("api/cuentas/me");
            if (response is { Success: true })
            {
                _cuenta = response.Data;
            }
            else
            {
                _errorMessage = "No se encontró la información de la cuenta.";
            }
        }

        private void NavegarMovimientos()
        {
            Navigation.NavigateTo("/historico");
        }

        public void Dispose() => StateService.OnChange -= HandleBalanceChanged;
    }
}
