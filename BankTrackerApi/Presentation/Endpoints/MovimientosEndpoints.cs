using BankTrackerApi.Application.Services.Movimiento;
using BankTrackerApi.Application.Shared.DTOs;

namespace BankTrackerApi.Presentation.Endpoints
{
    public static class MovimientosEndpoints
    {
        public static void MapMovimientosEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/movimientos")
                .WithTags("Movimientos")
                .RequireAuthorization();

            group.MapPost("/", CreateMovimiento)
                .WithName("CreateMovimiento");

            group.MapGet("/", GetAllMovimientos)
                .WithName("GetAllMovimientos");

            group.MapPut("/{id}", UpdateMovimiento)
                .WithName("UpdateMovimiento");

            group.MapDelete("/{id}", DeleteMovimiento)
                .WithName("DeleteMovimiento");
        }

        private static async Task<IResult> CreateMovimiento(MovimientoRequest request, IMovimientoService service)
        {
            var result = await service.CreateMovimientoAsync(request);

            return Results.Created($"{result.Id}", new ApiResponse<object>
            {
                Success = true,
                Message = "Movimiento creado exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> GetAllMovimientos(IMovimientoService service)
        {
            var result = await service.GetAllMovimientosAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Movimientos obtenidos exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> UpdateMovimiento(int id, UpdateMovimientoRequest request, IMovimientoService service)
        {
            request.Id = id;
            var result = await service.UpdateMovimientoAsync(request);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Movimiento actualizado exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> DeleteMovimiento(int id, IMovimientoService service)
        {
            await service.DeleteMovimientoAsync(id);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Movimiento eliminado correctamente",
                Data = new { Id = id }
            });
        }
    }
}
