using FormBuilder.Core.IServices.FormBuilder;
using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using formBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FormBuilder.Services.Services.FormBuilder
{
    public class SapHanaConfigsService : ISapHanaConfigsService
    {
        private const string CacheKey = "sap-hana:active-connection-string";

        private readonly IunitOfwork _unitOfWork;
        private readonly IHanaSecretProtector _protector;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SapHanaConfigsService> _logger;

        public SapHanaConfigsService(
            IunitOfwork unitOfWork,
            IHanaSecretProtector protector,
            IMemoryCache cache,
            ILogger<SapHanaConfigsService> logger)
        {
            _unitOfWork = unitOfWork;
            _protector = protector;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string?> GetActiveConnectionStringAsync()
        {
            if (_cache.TryGetValue(CacheKey, out string? cached) && !string.IsNullOrWhiteSpace(cached))
            {
                return cached;
            }

            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.IsActive);

                if (entity == null || string.IsNullOrWhiteSpace(entity.ConnectionStringEncrypted))
                {
                    return null;
                }

                var plaintext = _protector.Unprotect(entity.ConnectionStringEncrypted);
                if (string.IsNullOrWhiteSpace(plaintext))
                {
                    return null;
                }

                // Cache for a short time to avoid DB hits on every request
                _cache.Set(CacheKey, plaintext, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

                return plaintext;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading active SAP HANA config from DB");
                return null;
            }
        }

        public async Task<string?> GetConnectionStringByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null || string.IsNullOrWhiteSpace(entity.ConnectionStringEncrypted))
                    return null;

                var plaintext = _protector.Unprotect(entity.ConnectionStringEncrypted);
                return string.IsNullOrWhiteSpace(plaintext) ? null : plaintext;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading SAP config {Id} connection string from DB", id);
                return null;
            }
        }

        public async Task<bool> SetActiveAsync(string name, string connectionString)
        {
            try
            {
                name = (name ?? string.Empty).Trim();
                connectionString = (connectionString ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(connectionString))
                {
                    return false;
                }

                // Deactivate any other active configs
                var activeList = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .Where(x => !x.IsDeleted && x.IsActive)
                    .ToListAsync();

                foreach (var item in activeList)
                {
                    item.IsActive = false;
                    item.UpdatedDate = DateTime.UtcNow;
                }

                var entity = new SAP_HANA_CONFIGS
                {
                    Name = name,
                    ConnectionStringEncrypted = _protector.Protect(connectionString),
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _unitOfWork.Repositary<SAP_HANA_CONFIGS>().Add(entity);
                await _unitOfWork.CompleteAsyn();

                // Invalidate cache
                _cache.Remove(CacheKey);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting active SAP HANA config");
                return false;
            }
        }

        public async Task<ServiceResult<IEnumerable<SapHanaConfigDto>>> GetAllAsync(bool includeInactive = true)
        {
            try
            {
                var query = _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted);

                if (!includeInactive)
                {
                    query = query.Where(x => x.IsActive);
                }

                var list = await query
                    .OrderByDescending(x => x.IsActive)
                    .ThenByDescending(x => x.CreatedDate)
                    .ToListAsync();

                var result = new List<SapHanaConfigDto>(list.Count);
                foreach (var entity in list)
                {
                    result.Add(ToDto(entity));
                }

                return ServiceResult<IEnumerable<SapHanaConfigDto>>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving SAP HANA configs");
                return ServiceResult<IEnumerable<SapHanaConfigDto>>.Error("Error retrieving SAP HANA configs");
            }
        }

        public async Task<ServiceResult<SapHanaConfigDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null)
                {
                    return ServiceResult<SapHanaConfigDto>.NotFound("SAP HANA config not found");
                }

                return ServiceResult<SapHanaConfigDto>.Ok(ToDto(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving SAP HANA config {Id}", id);
                return ServiceResult<SapHanaConfigDto>.Error("Error retrieving SAP HANA config");
            }
        }

        public async Task<ServiceResult<SapHanaConfigDto>> CreateAsync(CreateSapHanaConfigDto dto)
        {
            try
            {
                if (dto == null) return ServiceResult<SapHanaConfigDto>.BadRequest("DTO is required");

                var name = (dto.Name ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name)) return ServiceResult<SapHanaConfigDto>.BadRequest("Name is required");

                var connectionString = BuildConnectionString(
                    dto.IntegrationType,
                    dto.ConnectionString,
                    dto.Server,
                    dto.UserName,
                    dto.Password,
                    dto.Schema,
                    dto.MaxPoolSize,
                    dto.BaseUrl,
                    dto.AuthenticationMethod,
                    dto.CompanyDb,
                    dto.VerifySsl);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    return ServiceResult<SapHanaConfigDto>.BadRequest("Invalid config. Provide Service Layer (BaseUrl, CompanyDb, Username, Password) or HANA ODBC connection details.");
                }

                if (dto.IsActive)
                {
                    await DeactivateAllAsync();
                }

                var entity = new SAP_HANA_CONFIGS
                {
                    Name = name,
                    ConnectionStringEncrypted = _protector.Protect(connectionString),
                    IsActive = dto.IsActive,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _unitOfWork.Repositary<SAP_HANA_CONFIGS>().Add(entity);
                await _unitOfWork.CompleteAsyn();

                _cache.Remove(CacheKey);
                return ServiceResult<SapHanaConfigDto>.Ok(ToDto(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating SAP HANA config");
                return ServiceResult<SapHanaConfigDto>.Error("Error creating SAP HANA config");
            }
        }

        public async Task<ServiceResult<SapHanaConfigDto>> UpdateAsync(int id, UpdateSapHanaConfigDto dto)
        {
            try
            {
                if (dto == null) return ServiceResult<SapHanaConfigDto>.BadRequest("DTO is required");

                var entity = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<SapHanaConfigDto>.NotFound("SAP HANA config not found");

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    entity.Name = dto.Name.Trim();
                }

                // Update connection string with partial-merge support.
                // This allows changing flags like VerifySsl without resending password.
                var hasConnectionUpdates =
                    !string.IsNullOrWhiteSpace(dto.ConnectionString) ||
                    !string.IsNullOrWhiteSpace(dto.IntegrationType) ||
                    !string.IsNullOrWhiteSpace(dto.Server) ||
                    !string.IsNullOrWhiteSpace(dto.UserName) ||
                    !string.IsNullOrWhiteSpace(dto.Password) ||
                    !string.IsNullOrWhiteSpace(dto.Schema) ||
                    dto.MaxPoolSize.HasValue ||
                    !string.IsNullOrWhiteSpace(dto.BaseUrl) ||
                    !string.IsNullOrWhiteSpace(dto.AuthenticationMethod) ||
                    !string.IsNullOrWhiteSpace(dto.CompanyDb) ||
                    dto.VerifySsl.HasValue;

                if (hasConnectionUpdates)
                {
                    var currentConnectionString = _protector.Unprotect(entity.ConnectionStringEncrypted) ?? string.Empty;
                    var mergedConnectionString = MergeConnectionString(currentConnectionString, dto);
                    if (string.IsNullOrWhiteSpace(mergedConnectionString))
                    {
                        return ServiceResult<SapHanaConfigDto>.BadRequest("Invalid config update. Please provide valid connection details.");
                    }

                    entity.ConnectionStringEncrypted = _protector.Protect(mergedConnectionString);
                    _cache.Remove(CacheKey);
                }

                if (dto.IsActive.HasValue)
                {
                    if (dto.IsActive.Value)
                    {
                        await DeactivateAllAsync();
                        entity.IsActive = true;
                        _cache.Remove(CacheKey);
                    }
                    else
                    {
                        entity.IsActive = false;
                        _cache.Remove(CacheKey);
                    }
                }

                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<SAP_HANA_CONFIGS>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                return ServiceResult<SapHanaConfigDto>.Ok(ToDto(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating SAP HANA config {Id}", id);
                return ServiceResult<SapHanaConfigDto>.Error("Error updating SAP HANA config");
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<bool>.NotFound("SAP HANA config not found");

                entity.IsDeleted = true;
                entity.IsActive = false;
                entity.DeletedDate = DateTime.UtcNow;
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repositary<SAP_HANA_CONFIGS>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                _cache.Remove(CacheKey);
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting SAP HANA config {Id}", id);
                return ServiceResult<bool>.Error("Error deleting SAP HANA config");
            }
        }

        public async Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive)
        {
            try
            {
                var entity = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (entity == null) return ServiceResult<bool>.NotFound("SAP HANA config not found");

                if (isActive)
                {
                    await DeactivateAllAsync();
                    entity.IsActive = true;
                }
                else
                {
                    entity.IsActive = false;
                }

                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.Repositary<SAP_HANA_CONFIGS>().Update(entity);
                await _unitOfWork.CompleteAsyn();

                _cache.Remove(CacheKey);
                return ServiceResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling SAP HANA config {Id} active={IsActive}", id, isActive);
                return ServiceResult<bool>.Error("Error updating SAP HANA config status");
            }
        }

        private async Task DeactivateAllAsync()
        {
            var activeList = await _unitOfWork.AppDbContext.Set<SAP_HANA_CONFIGS>()
                .Where(x => !x.IsDeleted && x.IsActive)
                .ToListAsync();

            foreach (var item in activeList)
            {
                item.IsActive = false;
                item.UpdatedDate = DateTime.UtcNow;
            }
        }

        private string BuildConnectionString(
            string? integrationType,
            string? full,
            string? server,
            string? userName,
            string? password,
            string? schema,
            int? maxPoolSize,
            string? baseUrl,
            string? authenticationMethod,
            string? companyDb,
            bool? verifySsl)
        {
            var connectionString = (full ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                return connectionString;
            }

            var mode = NormalizeIntegrationType(integrationType);

            // Build Service Layer config payload encoded as key=value; pairs.
            if (string.Equals(mode, "ServiceLayer", StringComparison.OrdinalIgnoreCase))
            {
                var url = (baseUrl ?? string.Empty).Trim();
                var auth = string.IsNullOrWhiteSpace(authenticationMethod) ? "Session" : authenticationMethod.Trim();
                var db = (companyDb ?? string.Empty).Trim();
                var user = (userName ?? string.Empty).Trim();
                var pass = (password ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(url) ||
                    string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(pass))
                {
                    return string.Empty;
                }

                return $"Type=ServiceLayer;BaseUrl={url};AuthMethod={auth};CompanyDB={db};UserName={user};Password={pass};VerifySsl={(verifySsl ?? true)};";
            }

            var s = (server ?? string.Empty).Trim();
            var u = (userName ?? string.Empty).Trim();
            var p = (password ?? string.Empty).Trim();
            var sc = (schema ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(u) || string.IsNullOrWhiteSpace(p))
            {
                return string.Empty;
            }

            connectionString = $"Server={s};UserName={u};Password={p};";

            if (!string.IsNullOrWhiteSpace(sc))
            {
                connectionString += $"Current Schema={sc};";
            }

            if (maxPoolSize.HasValue && maxPoolSize.Value > 0)
            {
                connectionString += $"Max Pool Size={maxPoolSize.Value};";
            }

            return connectionString;
        }

        private string MergeConnectionString(string existingConnectionString, UpdateSapHanaConfigDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.ConnectionString))
            {
                return dto.ConnectionString.Trim();
            }

            var dict = ParseKeyValueConnectionString(existingConnectionString ?? string.Empty);

            var mode = NormalizeIntegrationType(
                !string.IsNullOrWhiteSpace(dto.IntegrationType)
                    ? dto.IntegrationType
                    : (dict.TryGetValue("Type", out var currentType) ? currentType : null));

            if (string.Equals(mode, "ServiceLayer", StringComparison.OrdinalIgnoreCase))
            {
                dict["Type"] = "ServiceLayer";

                if (!string.IsNullOrWhiteSpace(dto.BaseUrl)) dict["BaseUrl"] = dto.BaseUrl.Trim();
                if (!string.IsNullOrWhiteSpace(dto.AuthenticationMethod)) dict["AuthMethod"] = dto.AuthenticationMethod.Trim();
                if (!string.IsNullOrWhiteSpace(dto.CompanyDb)) dict["CompanyDB"] = dto.CompanyDb.Trim();
                if (!string.IsNullOrWhiteSpace(dto.UserName)) dict["UserName"] = dto.UserName.Trim();
                if (!string.IsNullOrWhiteSpace(dto.Password)) dict["Password"] = dto.Password.Trim();
                if (dto.VerifySsl.HasValue) dict["VerifySsl"] = dto.VerifySsl.Value.ToString();

                var url = dict.TryGetValue("BaseUrl", out var baseUrl) ? baseUrl : string.Empty;
                var auth = dict.TryGetValue("AuthMethod", out var authMethod) ? authMethod : "Session";
                var db = dict.TryGetValue("CompanyDB", out var companyDb) ? companyDb : string.Empty;
                var user = dict.TryGetValue("UserName", out var userName) ? userName : string.Empty;
                var pass = dict.TryGetValue("Password", out var password) ? password : string.Empty;
                var verifySsl = dict.TryGetValue("VerifySsl", out var verifySslValue) && bool.TryParse(verifySslValue, out var parsedVerifySsl)
                    ? parsedVerifySsl
                    : true;

                if (string.IsNullOrWhiteSpace(url) ||
                    string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(user) ||
                    string.IsNullOrWhiteSpace(pass))
                {
                    return string.Empty;
                }

                return $"Type=ServiceLayer;BaseUrl={url};AuthMethod={auth};CompanyDB={db};UserName={user};Password={pass};VerifySsl={verifySsl};";
            }

            // HanaOdbc merge
            dict["Type"] = "HanaOdbc";
            if (!string.IsNullOrWhiteSpace(dto.Server)) dict["Server"] = dto.Server.Trim();
            if (!string.IsNullOrWhiteSpace(dto.UserName)) dict["UserName"] = dto.UserName.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Password)) dict["Password"] = dto.Password.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Schema)) dict["Current Schema"] = dto.Schema.Trim();
            if (dto.MaxPoolSize.HasValue && dto.MaxPoolSize.Value > 0) dict["Max Pool Size"] = dto.MaxPoolSize.Value.ToString();

            var serverVal = dict.TryGetValue("Server", out var server) ? server : string.Empty;
            var userVal = dict.TryGetValue("UserName", out var userNameVal) ? userNameVal : string.Empty;
            var passVal = dict.TryGetValue("Password", out var passwordVal) ? passwordVal : string.Empty;
            var schemaVal = dict.TryGetValue("Current Schema", out var schema) ? schema : string.Empty;
            var maxPoolVal = dict.TryGetValue("Max Pool Size", out var maxPoolSize) ? maxPoolSize : null;

            if (string.IsNullOrWhiteSpace(serverVal) ||
                string.IsNullOrWhiteSpace(userVal) ||
                string.IsNullOrWhiteSpace(passVal))
            {
                return string.Empty;
            }

            var cs = $"Server={serverVal};UserName={userVal};Password={passVal};";
            if (!string.IsNullOrWhiteSpace(schemaVal)) cs += $"Current Schema={schemaVal};";
            if (!string.IsNullOrWhiteSpace(maxPoolVal)) cs += $"Max Pool Size={maxPoolVal};";
            return cs;
        }

        private SapHanaConfigDto ToDto(SAP_HANA_CONFIGS entity)
        {
            var dto = new SapHanaConfigDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                DeletedDate = entity.DeletedDate
            };

            // Derive non-secret fields from decrypted connection string (never return password)
            try
            {
                var cs = _protector.Unprotect(entity.ConnectionStringEncrypted);
                if (!string.IsNullOrWhiteSpace(cs))
                {
                    var dict = ParseKeyValueConnectionString(cs);
                    if (dict.TryGetValue("Type", out var type))
                    {
                        dto.IntegrationType = NormalizeIntegrationType(type);
                    }
                    else
                    {
                        dto.IntegrationType = "HanaOdbc";
                    }

                    if (dict.TryGetValue("Server", out var server)) dto.Server = server;
                    if (dict.TryGetValue("UserName", out var user)) dto.UserName = user;
                    if (dict.TryGetValue("Current Schema", out var schema)) dto.Schema = schema;
                    else if (dict.TryGetValue("Schema", out var schema2)) dto.Schema = schema2;
                    if (dict.TryGetValue("Max Pool Size", out var maxPool) && int.TryParse(maxPool, out var m)) dto.MaxPoolSize = m;

                    if (dict.TryGetValue("BaseUrl", out var baseUrl)) dto.BaseUrl = baseUrl;
                    if (dict.TryGetValue("AuthMethod", out var authMethod)) dto.AuthenticationMethod = authMethod;
                    if (dict.TryGetValue("CompanyDB", out var companyDb)) dto.CompanyDb = companyDb;
                    if (dict.TryGetValue("VerifySsl", out var verifySsl) && bool.TryParse(verifySsl, out var verify))
                    {
                        dto.VerifySsl = verify;
                    }
                }
            }
            catch
            {
                // Ignore parsing issues; return minimal metadata
            }

            return dto;
        }

        private Dictionary<string, string> ParseKeyValueConnectionString(string connectionString)
        {
            var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in parts)
            {
                var keyValue = part.Split('=', 2);
                if (keyValue.Length == 2)
                {
                    dict[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }
            return dict;
        }

        private static string NormalizeIntegrationType(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "HanaOdbc";
            return value.Equals("ServiceLayer", StringComparison.OrdinalIgnoreCase)
                ? "ServiceLayer"
                : "HanaOdbc";
        }
    }
}
