using BankTrackerApi.Application.Shared.DTOs;
using BankTrackerApi.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankTrackerApi.Application.Services.Cuenta
{
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IConfiguration _config;

        public CuentaService(ICuentaRepository cuentaRepository, IConfiguration config)
        {
            _cuentaRepository = cuentaRepository;
            _config = config;
        }

        public async Task<CuentaResponse> RegisterAsync(CuentaRequest request)
        {
            var existingCuenta = await _cuentaRepository.GetCuentaByDniClienteAsync(request.DniCliente);
            if (existingCuenta != null)
            {
                throw new InvalidOperationException("El número de cuenta ya existe.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var nuevaCuenta = new Core.Modelos.Cuenta
            {
                NumeroCuenta = request.NumeroCuenta,
                DniCliente = request.DniCliente,
                NombreTitular = request.NombreTitular,
                Saldo = request.Saldo,
                PasswordHash = passwordHash
            };

            var cuentaCreada = await _cuentaRepository.CreateCuentaAsync(nuevaCuenta);

            return new CuentaResponse
            {
                Id = cuentaCreada.Id,
                DniCliente = cuentaCreada.DniCliente,
                NumeroCuenta = cuentaCreada.NumeroCuenta,
                NombreTitular = cuentaCreada.NombreTitular,
                Saldo = cuentaCreada.Saldo
            };
        }

        public async Task<CuentaResponse> LoginAsync(LoginRequest request)
        {
             var cuenta = await _cuentaRepository.GetCuentaByDniClienteAsync(request.DniCliente) ??
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, cuenta.PasswordHash))
            {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
            }

            var tokenData = GenerateJwtToken(cuenta);

            return new CuentaResponse
            {
                Id = cuenta.Id,
                DniCliente = cuenta.DniCliente,
                NumeroCuenta = cuenta.NumeroCuenta,
                NombreTitular = cuenta.NombreTitular,
                Saldo = cuenta.Saldo,
                Token = tokenData.Token,
                TokenExpiration = tokenData.Expiration
            };
        }

        public async Task<IEnumerable<CuentaResponse>> GetAllCuentasAsync()
        {
            var cuentas = await _cuentaRepository.GetCuentasAsync();
            return cuentas.Select(c => new CuentaResponse
            {
                Id = c.Id,
                NumeroCuenta = c.NumeroCuenta,
                NombreTitular = c.NombreTitular,
                Saldo = c.Saldo
            });
        }

        public async Task<CuentaResponse> UpdateCuentaAsync(UpdateCuentaRequest request)
        {
            var existingCuenta = await _cuentaRepository.GetCuentaByDniClienteAsync(request.DniCliente);
            if (existingCuenta == null)
            {
                throw new InvalidOperationException("La cuenta no existe.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var cuentaActualizada = new Core.Modelos.Cuenta
            {
                Id = request.Id,
                NumeroCuenta = request.NumeroCuenta,
                NombreTitular = request.NombreTitular,
                Saldo = request.Saldo,
                PasswordHash = passwordHash
            };

            await _cuentaRepository.UpdateCuentaAsync(cuentaActualizada);

            return new CuentaResponse
            {
                Id = cuentaActualizada.Id,
                NumeroCuenta = cuentaActualizada.NumeroCuenta,
                NombreTitular = cuentaActualizada.NombreTitular,
                Saldo = cuentaActualizada.Saldo
            };
        }

        public async Task DeleteCuentaAsync(int id)
        {
            await _cuentaRepository.DeleteCuentaAsync(id);
        }

        private dynamic GenerateJwtToken(Core.Modelos.Cuenta cuenta)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, cuenta.DniCliente),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, cuenta.DniCliente),
                new Claim(ClaimTypes.Name, cuenta.NombreTitular)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new
            {
                Token = tokenString,
                Expiration = expiration
            };
        }
    }
}

