using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class FileSystemHistoryItemModel : AuditableModel
{
    #region Properties
    public long Id { get; set; }

    #region ServiceNow Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
    public string ServiceNowKey { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string Name { get; set; } = "";
    public string Label { get; set; } = "";
    public string Category { get; set; } = "";
    public string Subcategory { get; set; } = "";
    public string StorageType { get; set; } = "";
    public string MediaType { get; set; } = "";
    public string VolumeId { get; set; } = "";
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
    public FileSystemHistoryItemModel() { }

    public FileSystemHistoryItemModel(FileSystemHistoryItem entity) : base(entity)
    {
        this.Id = entity.Id;

        this.RawData = entity.RawData;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.ClassName = entity.ClassName;
        this.Name = entity.Name;
        this.Label = entity.Label;
        this.Category = entity.Category;
        this.Subcategory = entity.Subcategory;
        this.StorageType = entity.StorageType;
        this.MediaType = entity.MediaType;
        this.VolumeId = entity.VolumeId;
        this.Capacity = entity.Capacity;
        this.DiskSpace = entity.DiskSpace;
        this.Size = entity.Size;
        this.SizeBytes = entity.SizeBytes;
        this.UsedSizeBytes = entity.UsedSizeBytes;
        this.AvailableSpace = entity.AvailableSpace;
        this.FreeSpace = entity.FreeSpace;
        this.FreeSpaceBytes = entity.FreeSpaceBytes;
    }

    public FileSystemHistoryItemModel(
        ServiceNow.ResultModel<ServiceNow.FileSystemModel> model,
        ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> configurationItemModel)
    {
        if (model.Data == null) throw new InvalidOperationException("File System Item data cannot be null");
        if (configurationItemModel.Data == null) throw new InvalidOperationException("Configuration item data cannot be null");

        this.RawData = model.RawData;
        this.RawDataCI = configurationItemModel.RawData;

        this.ServiceNowKey = model.Data.Id;
        this.ClassName = model.Data.ClassName ?? "";
        this.Name = model.Data.Name ?? "";
        this.Label = model.Data.Label ?? "";
        this.Category = model.Data.Category ?? "";
        this.Subcategory = model.Data.Subcategory ?? "";
        this.StorageType = model.Data.StorageType ?? "";
        this.MediaType = model.Data.MediaType ?? "";
        this.VolumeId = model.Data.VolumeId ?? "";
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
    public FileSystemHistoryItem ToEntity()
    {
        return (FileSystemHistoryItem)this;
    }

    public static explicit operator FileSystemHistoryItem(FileSystemHistoryItemModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new FileSystemHistoryItem(model.RawData, model.RawDataCI)
        {
            Id = model.Id,
            ServiceNowKey = model.ServiceNowKey,
            ClassName = model.ClassName,
            Name = model.Name,
            Label = model.Label,
            Category = model.Category,
            Subcategory = model.Subcategory,
            StorageType = model.StorageType,
            MediaType = model.MediaType,
            VolumeId = model.VolumeId,
            Capacity = model.Capacity,
            DiskSpace = model.DiskSpace,
            Size = model.Size,
            SizeBytes = model.SizeBytes,
            UsedSizeBytes = model.UsedSizeBytes,
            AvailableSpace = model.AvailableSpace,
            FreeSpace = model.FreeSpace,
            FreeSpaceBytes = model.FreeSpaceBytes,
            CreatedOn = model.CreatedOn,
            CreatedBy = model.CreatedBy,
            UpdatedOn = model.UpdatedOn,
            UpdatedBy = model.UpdatedBy,
            Version = model.Version,
        };
    }
    #endregion
}
