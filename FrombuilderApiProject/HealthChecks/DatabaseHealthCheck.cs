using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Core.Models;

namespace FormBuilder.API.HealthChecks
{
    /// <summary>
    /// Health check for FormBuilder database connectivity
    /// </summary>
    public class FormBuilderDbHealthCheck : IHealthCheck
    {
        private readonly FormBuilderDbContext _context;
        private readonly ILogger<FormBuilderDbHealthCheck> _logger;

        public FormBuilderDbHealthCheck(
            FormBuilderDbContext context,
            ILogger<FormBuilderDbHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Try to connect to database
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                
                if (!canConnect)
                {
                    return HealthCheckResult.Unhealthy("Cannot connect to FormBuilder database");
                }

                // Try a simple query
                await _context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);

                return HealthCheckResult.Healthy("FormBuilder database is accessible");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed for FormBuilder database");
                return HealthCheckResult.Unhealthy("FormBuilder database health check failed", ex);
            }
        }
    }

    /// <summary>
    /// Health check for Auth database connectivity
    /// </summary>
    public class AuthDbHealthCheck : IHealthCheck
    {
        private readonly AkhmanageItContext _context;
        private readonly ILogger<AuthDbHealthCheck> _logger;

        public AuthDbHealthCheck(
            AkhmanageItContext context,
            ILogger<AuthDbHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                
                if (!canConnect)
                {
                    return HealthCheckResult.Unhealthy("Cannot connect to Auth database");
                }

                await _context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);

                return HealthCheckResult.Healthy("Auth database is accessible");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed for Auth database");
                return HealthCheckResult.Unhealthy("Auth database health check failed", ex);
            }
        }
    }
}
