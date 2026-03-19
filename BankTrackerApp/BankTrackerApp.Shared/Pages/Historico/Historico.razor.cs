using BankTrackerApp.Shared.Components;
using BankTrackerShared.Core.Tipos;
using BankTrackerShared.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace BankTrackerApp.Shared.Pages.Historico
{
    public partial class Historico
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Parameter] public bool _onSearcher { get; set; } = true;
        [Parameter] public bool _onPagerContent { get; set; } = true;
        [Parameter] public int _rowsPerPage{ get; set; } = 10;

        //
        private bool isAuthorized = false;

        //
        private List<MovimientoResponse>? listadoMovimientos;

        private string? mensajeError = null;

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
        private string searchString1 = "";

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
                mensajeError = null; // Limpiamos errores anteriores

                Elements = new List<Tabla>();

                Http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.GetAsync($"/api/movimientos");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MovimientoResponse>>>();
                    if (result != null && result.Success)
                    {
                        listadoMovimientos = result.Data;
                        Elements = result.Data.Select(m => new Tabla
                        {
                            Cantidad = m.Cantidad,
                            Concepto = m.Concepto ?? "Sin concepto",
                            Fecha = m.Fecha,
                            TipoMovimiento = m.TipoMovimiento
                        }).ToList();
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    mensajeError = "No tienes permiso para ver esta sección. Tu sesión puede haber caducado.";
                    isAuthorized = false; // Bloqueamos el acceso
                }
                else
                {
                    mensajeError = "Error al obtener los datos del servidor.";
                }
            }
            catch (Exception)
            {
                mensajeError = "No se pudo establecer conexión con el servidor. Verifica tu internet.";
            }
            StateHasChanged(); 
        }

        private async Task Reintentar()
        {
            var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
            if (!string.IsNullOrEmpty(token))
            {
                await CargarDatosHistorial(token);
            }
            else
            {
                Navigation.NavigateTo("/login");
            }
        }

        private bool FilterFunc1(Tabla tabla) => FilterFunc(tabla, searchString1);

        private bool FilterFunc(Tabla tabla, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            // 1. Buscar por Concepto
            if (tabla.Concepto.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            // 2. Buscar por Cantidad
            if ($"{tabla.Cantidad}".Contains(searchString))
                return true;

            // 3. Buscar por Fecha (formato día/mes/año)
            if (tabla.Fecha.ToString("dd/MM/yyyy").Contains(searchString))
                return true;

            // 4. Buscar por Tipo de Movimiento (Ingreso/Gasto)
            // Esto permite que si el usuario escribe "Gasto", aparezcan los gastos
            if (tabla.TipoMovimiento.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task AbrirDialogo()
        {
            // 1. Configuramos opciones del diálogo (tamaño, botón cerrar, etc.)
            var opciones = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            // 2. Lanzamos el componente que creamos antes
            var dialog = await DialogService.ShowAsync<DialogoCrearMovimiento>("Nuevo Registro", opciones);

            // 3. Esperamos a que el usuario pulse "Guardar" o "Cancelar"
            var result = await dialog.Result;

            if (!result.Canceled && result.Data is MovimientoRequest nuevoMovimiento)
            {
                // 4. Si guardó, llamamos a la función que hace el POST a la API
                await GuardarEnServidor(nuevoMovimiento);
            }
        }

        private async Task GuardarEnServidor(MovimientoRequest request)
        {

            try
            {
                mensajeError = null;

                var token = await JS.InvokeAsync<string>("localStorage.getItem", "token");
                Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await Http.PostAsJsonAsync("api/movimientos", request);

                if (response.IsSuccessStatusCode)
                {
                    await JS.InvokeVoidAsync("alert", "¡Guardado con éxito!");
                    await CargarDatosHistorial(token!); // Recargamos la tabla
                }
            }
            catch (Exception)
            {
                mensajeError = "Error de conexión con el servidor.";
                StateHasChanged();
            }
        }

        private string RowStyleFunc(Tabla element, int index)
        {
            // Supongamos que TipoMovimiento es un enum o string
            if (element.TipoMovimiento.Equals(TipoMovimiento.Gasto))
                return "background-color: #ffebee; color: #b71c1c;"; // Rojo claro

            if (element.TipoMovimiento.Equals(TipoMovimiento.Ingreso))
                return "background-color: #e8f5e9; color: #1b5e20;"; // Verde claro

            return ""; // Estilo por defecto
        }

    }
}
