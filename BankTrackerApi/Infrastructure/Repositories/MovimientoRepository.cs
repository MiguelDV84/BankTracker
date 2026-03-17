using BankTrackerApi.Core.Modelos;
using BankTrackerApi.Infrastructure.Data;
using Dapper;
using System.Data;

namespace BankTrackerApi.Infrastructure.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly DapperContext _context;

        public MovimientoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Movimientos> CreateMovimientoAsync(Movimientos movimiento)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_insert_movimiento";
            var parameters = new DynamicParameters();
            parameters.Add("@p_cantidad", movimiento.Cantidad);
            parameters.Add("@p_concepto", movimiento.Concepto);
            parameters.Add("@p_fecha", movimiento.Fecha);
            parameters.Add("@p_tipo_movimiento", movimiento.TipoMovimiento);
            parameters.Add("@p_cuenta_id", movimiento.CuentaId);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return movimiento;
        }

        public async Task DeleteMovimientoAsync(int id)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_delete_movimiento";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Movimientos>> GetMovimientosAsync()
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_get_movimientos";

            var movimientos = await connection.QueryAsync<Movimientos>(
                procedureName,
                commandType: CommandType.StoredProcedure
            );

            return movimientos;
        }

        public async Task UpdateMovimientoAsync(Movimientos movimiento)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_update_movimiento";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", movimiento.Id);
            parameters.Add("@p_cantidad", movimiento.Cantidad);
            parameters.Add("@p_concepto", movimiento.Concepto);
            parameters.Add("@p_fecha", movimiento.Fecha);
            parameters.Add("@p_tipo_movimiento", movimiento.TipoMovimiento);
            parameters.Add("@p_cuenta_id", movimiento.CuentaId);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
