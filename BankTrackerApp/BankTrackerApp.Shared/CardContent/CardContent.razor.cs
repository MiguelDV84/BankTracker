

using Microsoft.AspNetCore.Components;

namespace BankTrackerApp.Shared.CardContent
{
    public partial class CardContent : ComponentBase
    {
        [Parameter] public string Titulo { get; set; } = "Título por defecto";
        [Parameter] public string Cuerpo { get; set; } = "Contenido por defecto...";
        [Parameter] public string TextoBoton { get; set; } = "Saber más";
    }
}
