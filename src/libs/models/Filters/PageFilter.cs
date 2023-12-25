using Microsoft.Extensions.Primitives;
using HSB.Core.Extensions;

namespace HSB.Models.Filters;

public abstract class PageFilter
{
    #region Properties
    public int? Page { get; set; }

    public int? Quantity { get; set; }
    #endregion

    #region Constructors
    public PageFilter() { }

    public PageFilter(Dictionary<string, StringValues> queryParams)
    {
        var filter = new Dictionary<string, StringValues>(queryParams, StringComparer.OrdinalIgnoreCase);

        this.Page = filter.GetIntNullValue(nameof(this.Page));
        this.Quantity = filter.GetIntNullValue(nameof(this.Quantity));
    }
    #endregion
}
