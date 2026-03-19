using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BankTracker.SharedUI.Layout
{
    public partial class TopBar : ComponentBase
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        private async Task CerrarSesion()
        {
            await JS.InvokeVoidAsync("localStorage.removeItem", "token");
            await JS.InvokeVoidAsync("localStorage.removeItem", "usuarioNombre");

            Navigation.NavigateTo("/login");
        }
    }
}
