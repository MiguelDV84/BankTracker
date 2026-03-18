using BankTrackerShared.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankTrackerApp.Shared.Pages.Includes
{
    public partial class Login
    {
        private LoginRequest login = new();

        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;

        private async Task hacerLogin()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/api/cuentas/login", login);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<ApiResponse<CuentaResponse>>();

                    if (resultado != null && resultado.Success)
                    {
                        await JS.InvokeVoidAsync("localStorage.setItem", "token", resultado.Data.Token);
                        await JS.InvokeVoidAsync("localStorage.setItem", "usuarioNombre", resultado.Data.NombreTitular);

                        Navigation.NavigateTo("/");
                    }
                }
                else
                {
                    await JS.InvokeVoidAsync("alert", "Error al iniciar sesión. Verifica tus credenciales.");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Ocurrió un error: {ex.Message}");
            }
        }
    }
}
