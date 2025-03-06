using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HSB.DAL.Services;

public class RefreshMaterializedViewsService : BaseService, IRefreshMaterializedViewsService
{
    #region Constructors
    public RefreshMaterializedViewsService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<RefreshMaterializedViewsService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public async Task RefreshAll()
    {
        // We are using ExecuteNonQueryAsync() rather than ExecuteSqlRaw() because we need to
        // refresh the materialized view, and the result of the command is -1. This causes issues
        // with ExecuteSqlRaw() because it expects a result set.
        using var connection = new NpgsqlConnection(this.Context.Database.GetDbConnection().ConnectionString);
        await connection.OpenAsync();
        using var command = new NpgsqlCommand("REFRESH MATERIALIZED VIEW \"mvServerHistoryItemsByMonth\"", connection);

        await command.ExecuteNonQueryAsync(); // Should return -1
    }
    #endregion
}
