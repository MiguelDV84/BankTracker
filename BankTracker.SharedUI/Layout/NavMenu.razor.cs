using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace BankTracker.SharedUI.Layout
{
    public partial class NavMenu : ComponentBase
    {
        private bool _isExpanded = true;

        private List<MenuItem> _menuItems = new();

        protected override void OnInitialized()
        {
            // Aquí cargamos la "configuración" de nuestro menú
            LoadMenus();

            // Ejemplo: Podrías forzar que inicie colapsado en móviles
            _isExpanded = true;
        }

        private void LoadMenus()
        {
            _menuItems = new List<MenuItem>
            {
                new MenuItem { Label = "Inicio", Link = "/", Icon = Icons.Material.Filled.Dashboard, Match = NavLinkMatch.All },
                new MenuItem { Label = "Estadísticas", Link = "/estadistica", Icon = Icons.Material.Filled.BarChart },
                new MenuItem { Label = "Historial", Link = "/historico", Icon = Icons.Material.Filled.ReceiptLong }
            };
        }

        private void ToggleDrawer() => _isExpanded = !_isExpanded;

        // Definición del modelo de datos
        public class MenuItem
        {
            public string Label { get; set; } = string.Empty;
            public string Link { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
            public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;
        }
    }
}