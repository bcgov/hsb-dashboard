namespace HSB.Models.Filters;

using System.Linq.Expressions;
using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class UserFilter : PageFilter
{
    #region Properties
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool? IsEnabled { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public UserFilter() { }

    public UserFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Username = filter.GetStringValue(nameof(this.Username));
        this.Email = filter.GetStringValue(nameof(this.Email));
        this.FirstName = filter.GetStringValue(nameof(this.FirstName));
        this.LastName = filter.GetStringValue(nameof(this.LastName));
        this.IsEnabled = filter.GetBoolNullValue(nameof(this.IsEnabled)) ?? filter.GetBoolNullValue("enabled");

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.User> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.User>();
        if (this.Username != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Username, $"%{this.Username}%"));
        if (this.Email != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Email, $"%{this.Email}%"));
        if (this.FirstName != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.FirstName, $"%{this.FirstName}%"));
        if (this.LastName != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.LastName, $"%{this.LastName}%"));
        if (this.IsEnabled != null)
            predicate = predicate.And((u) => u.IsEnabled == this.IsEnabled);

        if (!predicate.IsStarted) return predicate.And((u) => true);

        return predicate;
    }
    #endregion
}
