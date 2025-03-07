﻿using System.Text.Json;
using HSB.Entities;

namespace HSB.Models.Admin;
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

    public IEnumerable<TenantModel> Tenants { get; set; } = Array.Empty<TenantModel>();
    #endregion

    #region Constructors
    public OrganizationModel() { }

    public OrganizationModel(Organization entity, bool includeTenants, bool includeChildren = false) : base(entity)
    {
        this.Id = entity.Id;
        this.ParentId = entity.ParentId;
        if (!includeChildren) this.Parent = entity.Parent != null ? new OrganizationModel(entity.Parent, false) : null;
        if (includeChildren) this.Children = entity.Children.Select(c => new OrganizationModel(c, false));

        if (includeTenants)
        {
            this.Tenants = entity.TenantsManyToMany.Any() ? entity.TenantsManyToMany.Where(t => t.Tenant != null).Select(t => new TenantModel(t.Tenant!, false)).ToArray() : this.Tenants;
            this.Tenants = entity.Tenants.Any() ? entity.Tenants.Select(t => new TenantModel(t, false)).ToArray() : this.Tenants;
        }

        this.ServiceNowKey = entity.ServiceNowKey;
    }

    public OrganizationModel(ServiceNow.ResultModel<ServiceNow.ClientOrganizationModel> model)
    {
        if (model.Data == null) throw new InvalidOperationException("Organization data cannot be null");

        this.Name = model.Data.Name ?? "";
        this.Code = model.Data.OrganizationCode ?? Guid.NewGuid().ToString();
        this.ServiceNowKey = model.Data.Id;
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

        var entity = new Organization(model.Name)
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
        entity.TenantsManyToMany.AddRange(model.Tenants.Select(t => new TenantOrganization(t.Id, entity.Id)));

        return entity;
    }
    #endregion
}
