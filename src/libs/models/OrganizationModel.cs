using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class OrganizationModel : SortableCodeAuditableModel<int>
{
    #region Properties
    public int? ParentId { get; set; }
    public OrganizationModel? Parent { get; set; }
    public JsonDocument? RawData { get; set; }

    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";
    public IEnumerable<OrganizationModel> Children { get; set; } = Array.Empty<OrganizationModel>();
    #endregion

    #region Constructors
    public OrganizationModel() { }

    public OrganizationModel(Organization organization, bool includeChildren = false) : base(organization)
    {
        this.Id = organization.Id;
        this.ParentId = organization.ParentId;
        if (!includeChildren) this.Parent = organization.Parent != null ? new OrganizationModel(organization.Parent) : null;
        this.RawData = organization.RawData;
        if (includeChildren) this.Children = organization.Children.Select(c => new OrganizationModel(c));
    }
    #endregion

    #region Methods
    public Organization ToEntity()
    {
        return (Organization)this;
    }

    public static explicit operator Organization(OrganizationModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new Organization(model.Name)
        {
            Id = model.Id,
            Description = model.Description,
            Code = model.Code,
            ParentId = model.ParentId,
            ServiceNowKey = model.ServiceNowKey,
            RawData = model.RawData,
            IsEnabled = model.IsEnabled,
            SortOrder = model.SortOrder,
            Version = model.Version,
        };
    }
    #endregion
}
