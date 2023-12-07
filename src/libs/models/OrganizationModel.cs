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

    public OrganizationModel(Organization entity, bool includeChildren = false) : base(entity)
    {
        this.Id = entity.Id;
        this.ParentId = entity.ParentId;
        if (!includeChildren) this.Parent = entity.Parent != null ? new OrganizationModel(entity.Parent) : null;
        if (includeChildren) this.Children = entity.Children.Select(c => new OrganizationModel(c));

        this.ServiceNowKey = entity.ServiceNowKey;
        this.RawData = entity.RawData;
    }

    public OrganizationModel(ServiceNow.ResultModel<ServiceNow.ClientOrganizationModel> model)
    {
        if (model.Data == null) throw new InvalidOperationException("Organization data cannot be null");

        this.Name = model.Data.Name ?? "";
        this.Code = model.Data.OrganizationCode ?? Guid.NewGuid().ToString();
        this.ServiceNowKey = model.Data.Id;
        this.RawData = model.RawData;
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
