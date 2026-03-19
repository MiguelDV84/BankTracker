

using Microsoft.AspNetCore.Components;

namespace BankTracker.SharedUI.CardContent
{
    public partial class CardContent : ComponentBase
    {
        [Parameter] public string Titulo { get; set; } = "Título por defecto";
        [Parameter] public string Cuerpo { get; set; } = "Contenido por defecto...";
        [Parameter] public string TextoBoton { get; set; } = "Saber más";
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public EventCallback OnClick { get; set; }
    }
}
