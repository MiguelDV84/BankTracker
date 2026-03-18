using BankTrackerShared.Core.Tipos;
using BankTrackerShared.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankTrackerApp.Shared.Pages.Historico
{
    public partial class Historico
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;

        //
        private bool isAuthorized = false;

        //
        private List<MovimientoResponse>? listadoMovimientos;


        //
        public class Tabla
        {
            public decimal Cantidad { get; set; }
            public string Concepto { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public TipoMovimiento TipoMovimiento { get; set; }
        }

        private Tabla selectedItem1 = null;
        private IEnumerable<Tabla> Elements = new List<Tabla>();

        private bool dense = false;
        private bool hover = true;
        private bool striped = false;
        private bool bordered = false;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");

            if (!string.IsNullOrEmpty(token))
            {
                isAuthorized = true;
                await CargarDatosHistorial(token);

                StateHasChanged();
            }
            else
            {
                Navigation.NavigateTo("/login");
            }
        }

        private async Task CargarDatosHistorial(string token)
        {
            try
            {
                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetAsync($"/api/movimientos");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MovimientoResponse>>>();

                    if (result != null && result.Success)
                    {
                        listadoMovimientos = result.Data;

                        foreach (var m in listadoMovimientos)
                        {
                            Console.WriteLine($"Concepto: {m.Concepto} - Tipo: {m.TipoMovimiento}");
                        }

                        Elements = listadoMovimientos.Select(m => new Tabla
                        {
                            Cantidad = m.Cantidad,
                            Concepto = m.Concepto,
                            Fecha = m.Fecha,
                            TipoMovimiento = m.TipoMovimiento

                        });
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
