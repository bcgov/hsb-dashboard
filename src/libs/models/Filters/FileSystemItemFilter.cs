namespace HSB.Models.Filters;

using HSB.Core.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

public class FileSystemItemFilter : PageFilter
{
    #region Properties
    public string? Name { get; set; }
    public string? ServiceNowKey { get; set; }
    public string? Category { get; set; }
    public string? Subcategory { get; set; }
    public string? ClassName { get; set; }

    public int? TenantId { get; set; }
    public int? OrganizationId { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public string? ServerItemServiceNowKey { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string[] Sort { get; set; } = Array.Empty<string>();
    #endregion

    #region Constructors
    public FileSystemItemFilter() { }

    public FileSystemItemFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryParams) : base(queryParams)
    {
        var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Name = filter.GetStringValue(nameof(this.Name));
        this.ClassName = filter.GetStringValue(nameof(this.ClassName));
        this.ServiceNowKey = filter.GetStringValue(nameof(this.ServiceNowKey));
        this.Category = filter.GetStringValue(nameof(this.Category));
        this.Subcategory = filter.GetStringValue(nameof(this.Subcategory));
        this.TenantId = filter.GetIntNullValue(nameof(this.TenantId));
        this.OrganizationId = filter.GetIntNullValue(nameof(this.OrganizationId));
        this.OperatingSystemItemId = filter.GetIntNullValue(nameof(this.OperatingSystemItemId));
        this.ServerItemServiceNowKey = filter.GetStringValue(nameof(this.ServerItemServiceNowKey));
        this.StartDate = filter.GetDateTimeNullValue(nameof(this.StartDate));
        this.EndDate = filter.GetDateTimeNullValue(nameof(this.EndDate));

        this.Sort = filter.GetStringArrayValue(nameof(this.Sort), new[] { nameof(FileSystemItemModel.Name) });
    }
    #endregion

    #region Methods
    public ExpressionStarter<Entities.FileSystemItem> GeneratePredicate()
    {
        var predicate = PredicateBuilder.New<Entities.FileSystemItem>();
        if (this.Name != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Name, $"%{this.Name}%"));
        if (this.ClassName != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ClassName, this.ClassName));
        if (this.ServiceNowKey != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ServiceNowKey, this.ServiceNowKey));
        if (this.Category != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Category, this.Category));
        if (this.Subcategory != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.Subcategory, this.Subcategory));
        if (this.TenantId != null)
            predicate = predicate.And((u) => u.ServerItem!.TenantId == this.TenantId);
        if (this.OrganizationId != null)
            predicate = predicate.And((u) => u.ServerItem!.OrganizationId == this.OrganizationId);
        if (this.OperatingSystemItemId != null)
            predicate = predicate.And((u) => u.ServerItem!.OperatingSystemItemId == this.OperatingSystemItemId);
        if (this.ServerItemServiceNowKey != null)
            predicate = predicate.And((u) => EF.Functions.Like(u.ServerItemServiceNowKey, this.ServerItemServiceNowKey));
        if (this.StartDate != null)
            predicate = predicate.And((u) => u.CreatedOn >= this.StartDate.Value.ToUniversalTime());
        if (this.EndDate != null)
            predicate = predicate.And((u) => u.CreatedOn <= this.EndDate.Value.ToUniversalTime());

        if (!predicate.IsStarted) return predicate.And((u) => true);
        return predicate;
    }
    #endregion
}
