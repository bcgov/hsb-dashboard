using System.Security.Claims;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using HSB.Entities;
using HSB.DAL.Extensions;

namespace HSB.DAL.Services;

public class OrganizationService : BaseService<Organization>, IOrganizationService
{
    #region Constructors
    public OrganizationService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<OrganizationService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<Organization> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<Organization, bool>> predicate,
        System.Linq.Expressions.Expression<Func<Organization, T>>? sort = null,
        int? take = null,
        int? skip = null)
    {
        var query = (from org in this.Context.Organizations
                     join uo in this.Context.UserOrganizations on org.Id equals uo.OrganizationId
                     where uo.UserId == userId
                     select org)
            .Where(predicate)
            .Distinct();

        if (sort != null)
            query = query.OrderBy(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }

    public IEnumerable<Organization> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<Organization, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {

        var query = (from org in this.Context.Organizations
                     join uo in this.Context.UserOrganizations on org.Id equals uo.OrganizationId
                     where uo.UserId == userId
                     select org)
            .Where(predicate)
            .Distinct();

        if (sort?.Any() == true)
            query = query.OrderByProperty(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }

    #endregion
}
