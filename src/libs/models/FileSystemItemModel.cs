using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class FileSystemItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public int ConfigurationItemId { get; set; }
    public JsonDocument? RawData { get; set; }


    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string Label { get; set; } = "";
    public string Category { get; set; } = "";
    public string SubCategory { get; set; } = "";
    public string StorageType { get; set; } = "";
    public string MediaType { get; set; } = "";
    public string VolumeId { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string Capacity { get; set; } = "";
    public string DiskSpace { get; set; } = "";
    public string Size { get; set; } = "";
    public string SizeBytes { get; set; } = "";
    public string UsedSizeBytes { get; set; } = "";
    public string AvailableSpace { get; set; } = "";
    public string FreeSpace { get; set; } = "";
    public string FreeSpaceBytes { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    public FileSystemItemModel() { }

    public FileSystemItemModel(FileSystemItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.ConfigurationItemId = entity.ConfigurationItemId;
        this.RawData = entity.RawData;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.Name = entity.Name;
        this.Label = entity.Label;
        this.Category = entity.Category;
        this.SubCategory = entity.SubCategory;
        this.StorageType = entity.StorageType;
        this.MediaType = entity.MediaType;
        this.VolumeId = entity.VolumeId;
        this.ClassName = entity.ClassName;
        this.Capacity = entity.Capacity;
        this.DiskSpace = entity.DiskSpace;
        this.Size = entity.Size;
        this.SizeBytes = entity.SizeBytes;
        this.UsedSizeBytes = entity.UsedSizeBytes;
        this.AvailableSpace = entity.AvailableSpace;
        this.FreeSpace = entity.FreeSpace;
        this.FreeSpaceBytes = entity.FreeSpaceBytes;
    }

    public FileSystemItemModel(ServiceNow.ResultModel<ServiceNow.FileSystemModel> model, int configurationItemId)
    {
        if (model.Data == null) throw new InvalidOperationException("File System Item data cannot be null");

        this.ConfigurationItemId = configurationItemId;
        this.RawData = model.RawData;

        this.ServiceNowKey = model.Data.Id;
        this.Name = model.Data.Name ?? "";
        this.Label = model.Data.Label ?? "";
        this.Category = model.Data.Category ?? "";
        this.SubCategory = model.Data.SubCategory ?? "";
        this.StorageType = model.Data.StorageType ?? "";
        this.MediaType = model.Data.MediaType ?? "";
        this.VolumeId = model.Data.VolumeId ?? "";
        this.ClassName = model.Data.ClassName ?? "";
        this.Capacity = model.Data.Capacity ?? "";
        this.DiskSpace = model.Data.DiskSpace ?? "";
        this.Size = model.Data.Size ?? "";
        this.SizeBytes = model.Data.SizeBytes ?? "";
        this.UsedSizeBytes = model.Data.UsedSizeBytes ?? "";
        this.AvailableSpace = model.Data.AvailableSpace ?? "";
        this.FreeSpace = model.Data.FreeSpace ?? "";
        this.FreeSpaceBytes = model.Data.FreeSpaceBytes ?? "";
    }
    #endregion

    #region Methods
    public FileSystemItem ToEntity()
    {
        if (this.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new FileSystemItem(this.ConfigurationItemId, this.RawData)
        {
            Id = this.Id,
            CreatedOn = this.CreatedOn,
            CreatedBy = this.CreatedBy,
            UpdatedOn = this.UpdatedOn,
            UpdatedBy = this.UpdatedBy,
            Version = this.Version,
        };
    }
    #endregion
}
