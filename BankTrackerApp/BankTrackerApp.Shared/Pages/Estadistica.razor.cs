using BankTrackerShared.Core.Tipos;
using BankTrackerShared.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankTrackerApp.Shared.Pages
{
    public partial class Estadistica
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;

        private int _index = -1;
        private int _height = 350;
        private bool _matchBoundsToSize = false;
        private string[] _xAxisLabels = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        private List<ChartSeries<double>> _series = new();

        // Inicializamos las opciones
        private ChartOptions _axisChartOptions = new ChartOptions()
        {
            ChartPalette = new[] { "#2196F3", "#F44336" }
            
        };

        private List<MovimientoResponse>? movimientos;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
            if (!string.IsNullOrEmpty(token))
            {
                await CargarDatos(token);
                StateHasChanged();
            }
            else
            {
                Navigation.NavigateTo("/login");
            }
        }

        private async Task CargarDatos(string token)
        {
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Http.GetAsync("/api/movimientos");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MovimientoResponse>>>();
                if (result != null && result.Success)
                {
                    movimientos = result.Data;
                    CalcularSeries();
                }
            }
        }

        private void CalcularSeries()
        {
            if (movimientos == null) return;

            double[] ingresos = new double[12];
            double[] gastos = new double[12];

            foreach (var m in movimientos)
            {
                int mes = m.Fecha.Month - 1;
                if (m.TipoMovimiento == TipoMovimiento.Ingreso)
                    ingresos[mes] += (double)m.Cantidad;
                else
                    gastos[mes] += (double)m.Cantidad;
            }

            _series = new List<ChartSeries<double>>
            {
                new ChartSeries<double> { Name = "Ingresos", Data = ingresos },
                new ChartSeries<double> { Name = "Gastos", Data = gastos }
            };
        }
    }
}
