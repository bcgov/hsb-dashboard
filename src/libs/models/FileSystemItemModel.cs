using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class FileSystemItemModel : AuditableModel
{
    #region Properties
    public string ServiceNowKey { get; set; } = "";
    public string ServerItemServiceNowKey { get; set; } = "";

    #region ServiceNow Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
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
    public FileSystemItemModel() { }

    public FileSystemItemModel(FileSystemItem entity) : base(entity)
    {
        this.ServiceNowKey = entity.ServiceNowKey;
        this.ServerItemServiceNowKey = entity.ServerItemServiceNowKey;

        this.RawData = entity.RawData;

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

    public FileSystemItemModel(string serverItemServiceNowKey
    , ServiceNow.ResultModel<ServiceNow.FileSystemModel> fileSystemItemModel
    , ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> configurationItemModel)
    {
        if (fileSystemItemModel.Data == null) throw new InvalidOperationException("File System Item data cannot be null");
        if (configurationItemModel.Data == null) throw new InvalidOperationException("Configuration item data cannot be null");

        this.ServiceNowKey = fileSystemItemModel.Data.Id;
        this.ServerItemServiceNowKey = serverItemServiceNowKey;

        this.RawData = fileSystemItemModel.RawData;
        this.RawDataCI = configurationItemModel.RawData;

        this.ClassName = fileSystemItemModel.Data.ClassName ?? "";
        this.Name = fileSystemItemModel.Data.Name ?? "";
        this.Label = fileSystemItemModel.Data.Label ?? "";
        this.Category = fileSystemItemModel.Data.Category ?? "";
        this.Subcategory = fileSystemItemModel.Data.Subcategory ?? "";
        this.StorageType = fileSystemItemModel.Data.StorageType ?? "";
        this.MediaType = fileSystemItemModel.Data.MediaType ?? "";
        this.VolumeId = fileSystemItemModel.Data.VolumeId ?? "";
        this.Capacity = fileSystemItemModel.Data.Capacity ?? "";
        this.DiskSpace = fileSystemItemModel.Data.DiskSpace ?? "";
        this.Size = fileSystemItemModel.Data.Size ?? "";
        this.SizeBytes = fileSystemItemModel.Data.SizeBytes ?? "";
        this.UsedSizeBytes = fileSystemItemModel.Data.UsedSizeBytes ?? "";
        this.AvailableSpace = fileSystemItemModel.Data.AvailableSpace ?? "";
        this.FreeSpace = fileSystemItemModel.Data.FreeSpace ?? "";
        this.FreeSpaceBytes = fileSystemItemModel.Data.FreeSpaceBytes ?? "";
    }
    #endregion

    #region Methods
    public FileSystemItem ToEntity()
    {
        return (FileSystemItem)this;
    }

    public static explicit operator FileSystemItem(FileSystemItemModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new FileSystemItem(model.ServerItemServiceNowKey, model.RawData, model.RawDataCI)
        {
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
