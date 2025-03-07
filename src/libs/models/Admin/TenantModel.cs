﻿using System.Text.Json;
using HSB.Entities;

namespace HSB.Models.Admin;
public class TenantModel : SortableCodeAuditableModel<int>
{
    #region Properties
    public JsonDocument? RawData { get; set; }

    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";

    /// <summary>
    /// get/set - An array of organizations.
    /// </summary>
    public IEnumerable<OrganizationModel> Organizations { get; set; } = Array.Empty<OrganizationModel>();
    #endregion

    #region Constructors
    public TenantModel() { }

    public TenantModel(Tenant entity, bool includeOrganizations) : base(entity)
    {
        this.Id = entity.Id;

        this.ServiceNowKey = entity.ServiceNowKey;

        if (includeOrganizations)
        {
            this.Organizations = entity.OrganizationsManyToMany.Any() ? entity.OrganizationsManyToMany.Where(o => o.Organization != null).Select(o => new OrganizationModel(o.Organization!, false)) : this.Organizations;
            this.Organizations = entity.Organizations.Any() ? entity.Organizations.Select(o => new OrganizationModel(o, false)) : this.Organizations;
        }
    }

    public TenantModel(ServiceNow.ResultModel<ServiceNow.TenantModel> model, IEnumerable<OrganizationModel> organizations)
    {
        if (model.Data == null) throw new InvalidOperationException("Tenant data cannot be null");

        this.Name = model.Data.Name ?? model.Data.SysName ?? "";
        this.Code = model.Data.SysName ?? Guid.NewGuid().ToString();
        this.ServiceNowKey = model.Data.Id;

        this.Organizations = organizations;
    }
    #endregion

    #region Methods
    public Tenant ToEntity()
    {
        return (Tenant)this;
    }

    public static explicit operator Tenant(TenantModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        var tenant = new Tenant(model.Name)
        {
            Id = model.Id,
            Description = model.Description,
            Code = model.Code,
            ServiceNowKey = model.ServiceNowKey,
            RawData = model.RawData,
            IsEnabled = model.IsEnabled,
            SortOrder = model.SortOrder,
            Version = model.Version,
        };

        tenant.OrganizationsManyToMany.AddRange(model.Organizations.Select(o => new TenantOrganization(model.Id, o.Id)));

        return tenant;
    }
    #endregion
}
