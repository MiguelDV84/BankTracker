using BankTrackerShared.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BankTrackerApp.Shared.Pages
{
    public partial class CuentaBancaria : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;
        private CuentaResponse _cuenta = new();
        private bool _isLoading = true;
        private string? _errorMessage;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) // Solo queremos cargar los datos la primera vez
            {
                try
                {
                    // Ahora sí podemos usar JavaScript de forma segura
                    var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

                    if (!string.IsNullOrEmpty(token))
                    {
                        Http.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);

                        await CargarDatosCuenta();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private async Task CargarDatosCuenta()
        {
            var response = await Http.GetFromJsonAsync<ApiResponse<CuentaResponse>>("api/cuentas/me");
            if (response is { Success: true })
            {
                _cuenta = response.Data;
                StateHasChanged();
            }
        }
    }
}
