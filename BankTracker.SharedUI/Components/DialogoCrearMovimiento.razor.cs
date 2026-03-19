using BankTrackerShared.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTracker.SharedUI.Components
{
    public partial class DialogoCrearMovimiento
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        // Modelo local para el formulario
        private MovimientoRequest model = new();

        private void Cancelar() => MudDialog.Cancel();

        private void Guardar()
        {
            // Al cerrar, devolvemos el objeto 'model' a la página principal
            MudDialog.Close(DialogResult.Ok(model));
        }
    }
}
