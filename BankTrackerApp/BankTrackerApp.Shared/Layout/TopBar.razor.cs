using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BankTrackerApp.Shared.Layout
{
    public partial class TopBar : ComponentBase
    {
        [Parameter] public string NombrePerfil { get; set; } = "";
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        private async Task CerrarSesion()
        {
            await JS.InvokeVoidAsync("localStorage.removeItem", "token");
            await JS.InvokeVoidAsync("localStorage.removeItem", "usuarioNombre");

            Navigation.NavigateTo("/login");
        }

        protected override void OnParametersSet()
        {
            if (string.IsNullOrEmpty(NombrePerfil))
            {
                NombrePerfil = "Usuario";
            }
        }

        protected override async Task OnInitializedAsync()
        {
          await CargarNombre();
        }

        private async Task CargarNombre()
        {
            try
            {
                // Usamos await directamente, es mucho más limpio y seguro
                var nombreGuardado = await JS.InvokeAsync<string>("localStorage.getItem", "usuarioNombre");

                if (!string.IsNullOrEmpty(nombreGuardado))
                {
                    NombrePerfil = nombreGuardado;
                }
            }
            catch (Exception)
            {
                // Manejar error si el JS no está disponible aún
                NombrePerfil = "Usuario";
            }
        }
    }
}
