using BankTrackerApi.Application.Services.Cuenta;
using BankTrackerApi.Application.Shared.DTOs;

namespace BankTrackerApi.Presentation.Endpoints
{
    public static class CuentaEndpoints
    {
        public static void MapCuentaEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/cuentas")
                .WithTags("Cuentas");

            group.MapPost("/register", Register)
                .WithName("RegisterCuenta");

            group.MapPost("/login", Login)
                .WithName("LoginCuenta");

            group.MapGet("/me", GetCuenta)
                .WithName("GetCuenta")
                .RequireAuthorization();

            group.MapGet("/", GetAllCuentas)
                .WithName("GetAllCuentas")
                .RequireAuthorization();

            group.MapPut("/{id}", UpdateCuenta)
                .WithName("UpdateCuenta")
                .RequireAuthorization();

            group.MapDelete("/{id}", DeleteCuenta)
                .WithName("DeleteCuenta")
                .RequireAuthorization();
        }

        private static async Task<IResult> Register(CuentaRequest request, ICuentaService service)
        {
            try
            {
                var result = await service.RegisterAsync(request);

                return Results.Created($"{result.Id}", new ApiResponse<object>
                {
                    Success = true,
                    Message = "Cuenta registrada exitosamente",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        private static async Task<IResult> Login(LoginRequest request, ICuentaService service)
        {
            try
            {
                var result = await service.LoginAsync(request);

                return Results.Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Sesión iniciada exitosamente",
                    Data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Unauthorized();
            }
        }

        private static async Task<IResult> GetCuenta(ICuentaService service)
        {
            try
            {
                var result = await service.GetCuentaAsync();
                return Results.Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Cuenta obtenida exitosamente",
                    Data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Unauthorized();
            }
        }

        private static async Task<IResult> GetAllCuentas(ICuentaService service)
        {
            var result = await service.GetAllCuentasAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cuentas obtenidas exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> UpdateCuenta(int id, UpdateCuentaRequest request, ICuentaService service)
        {
            try
            {
                request.Id = id;
                var result = await service.UpdateCuentaAsync(request);

                return Results.Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Cuenta actualizada exitosamente",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        private static async Task<IResult> DeleteCuenta(int id, ICuentaService service)
        {
            await service.DeleteCuentaAsync(id);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cuenta eliminada correctamente",
                Data = new { Id = id }
            });
        }
    }
}
