using BankTrackerShared.Shared.DTOs;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankTrackerApp.Shared.Pages.Includes
{
    public partial class Registro
    {
        private CuentaRequest crearCuenta = new();

        private async Task CrearCuenta()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("/api/cuentas/register", crearCuenta);

                if (response.IsSuccessStatusCode)
                {

                    var result = response.Content.ReadFromJsonAsync<ApiResponse<CuentaResponse>>();

                    if (result != null)
                    {
                        await JS.InvokeVoidAsync("alert", "Usuario creado exitosamente. Ahora puedes iniciar sesión.");
                        Navigation.NavigateTo("/login");
                    }
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("alert", $"Ocurrió un error: {ex.Message}");
            }
        }
    }
}
