using BankTrackerApi.Core.Modelos;
using BankTrackerApi.Infrastructure.Data;
using Dapper;
using System.Data;

namespace BankTrackerApi.Infrastructure.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {

        private readonly DapperContext _context;

        public CuentaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Cuenta> CreateCuentaAsync(Cuenta cuenta)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_insert_cuenta";
            var parameters = new DynamicParameters();
            parameters.Add("@p_numero_cuenta", cuenta.NumeroCuenta);
            parameters.Add("@p_nombre_titular", cuenta.NombreTitular);
            parameters.Add("@p_saldo", cuenta.Saldo);
            parameters.Add("@p_password_hash", cuenta.PasswordHash);
            parameters.Add("@p_dni_cliente", cuenta.DniCliente);

            await connection.ExecuteAsync(
                procedureName, 
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return cuenta;
        }

        public async Task DeleteCuentaAsync(int id)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_delete_cuenta";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Cuenta> GetCuentaByDniClienteAsync(string dniCliente)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_get_cuenta_dni";
            var parameters = new DynamicParameters();
            parameters.Add("@p_dni_cliente", dniCliente);

            var cuenta = await connection.QueryFirstOrDefaultAsync<Cuenta>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return cuenta;
        }

        public async Task<IEnumerable<Cuenta>> GetCuentasAsync()
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_get_cuentas";

            var cuentas = await connection.QueryAsync<Cuenta>(
                procedureName,
                commandType: CommandType.StoredProcedure
                );

            return cuentas;
        }

        public async Task UpdateCuentaAsync(Cuenta cuenta)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_update_cuenta";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", cuenta.Id);
            parameters.Add("@p_numero_cuenta", cuenta.NumeroCuenta);
            parameters.Add("@p_nombre_titular", cuenta.NombreTitular);
            parameters.Add("@p_saldo", cuenta.Saldo);
            parameters.Add("@p_password_hash", cuenta.PasswordHash);

            await connection.ExecuteAsync(  
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
