using System.Text.Json;
using HSB.Entities;

namespace HSB.Models.Dashboard;

public class FileSystemHistoryItemModel : AuditableModel
{
    #region Properties
    public long Id { get; set; }

    #region ServiceNow Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
    public string ServiceNowKey { get; set; } = "";
    public string ServerItemServiceNowKey { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string Name { get; set; } = "";
    public int InstallStatus { get; set; }
    public string Label { get; set; } = "";
    public string Category { get; set; } = "";
    public string Subcategory { get; set; } = "";
    public string StorageType { get; set; } = "";
    public string MediaType { get; set; } = "";
    public string VolumeId { get; set; } = "";
    public int Capacity { get; set; }
    public float DiskSpace { get; set; }
    public string Size { get; set; } = "";
    public long SizeBytes { get; set; }
    public long? UsedSizeBytes { get; set; }
    public int AvailableSpace { get; set; }
    public string FreeSpace { get; set; } = "";
    public long FreeSpaceBytes { get; set; }
    #endregion
    #endregion

    #region Constructors
    public FileSystemHistoryItemModel() { }

    public FileSystemHistoryItemModel(FileSystemHistoryItem entity) : base(entity)
    {
        this.Id = entity.Id;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.ServerItemServiceNowKey = entity.ServerItemServiceNowKey;
        this.ClassName = entity.ClassName;
        this.Name = entity.Name;
        this.InstallStatus = entity.InstallStatus;
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

    public FileSystemHistoryItemModel(FileSystemHistoryItemSmall entity) : base(entity)
    {
        this.Id = entity.Id;

        this.ServiceNowKey = entity.ServiceNowKey;
        this.ServerItemServiceNowKey = entity.ServerItemServiceNowKey;
        this.ClassName = entity.ClassName;
        this.Name = entity.Name;
        this.InstallStatus = entity.InstallStatus;
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
        string ServerItemServiceNowKey,
        ServiceNow.ResultModel<ServiceNow.FileSystemModel> fileSystemItemModel,
        ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> configurationItemModel)
    {
        if (fileSystemItemModel.Data == null) throw new InvalidOperationException("File System Item data cannot be null");
        if (configurationItemModel.Data == null) throw new InvalidOperationException("Configuration item data cannot be null");

        this.ServiceNowKey = fileSystemItemModel.Data.Id;
        this.ServerItemServiceNowKey = ServerItemServiceNowKey;

        this.ClassName = fileSystemItemModel.Data.ClassName ?? "";
        this.Name = fileSystemItemModel.Data.Name ?? "";
        this.InstallStatus = int.Parse(fileSystemItemModel.Data.InstallStatus ?? "0");
        this.Label = fileSystemItemModel.Data.Label ?? "";
        this.Category = fileSystemItemModel.Data.Category ?? "";
        this.Subcategory = fileSystemItemModel.Data.Subcategory ?? "";
        this.StorageType = fileSystemItemModel.Data.StorageType ?? "";
        this.MediaType = fileSystemItemModel.Data.MediaType ?? "";
        this.VolumeId = fileSystemItemModel.Data.VolumeId ?? "";
        this.Capacity = !String.IsNullOrWhiteSpace(fileSystemItemModel.Data.Capacity) ? Int32.Parse(fileSystemItemModel.Data.Capacity) : 0;
        this.DiskSpace = !String.IsNullOrWhiteSpace(fileSystemItemModel.Data.DiskSpace) ? float.Parse(fileSystemItemModel.Data.DiskSpace) : 0;
        this.Size = fileSystemItemModel.Data.Size ?? "";
        this.SizeBytes = !String.IsNullOrWhiteSpace(fileSystemItemModel.Data.SizeBytes) ? long.Parse(fileSystemItemModel.Data.SizeBytes) : 0;
        this.UsedSizeBytes = !String.IsNullOrWhiteSpace(fileSystemItemModel.Data.UsedSizeBytes) ? long.Parse(fileSystemItemModel.Data.UsedSizeBytes) : null;
        this.AvailableSpace = !String.IsNullOrWhiteSpace(fileSystemItemModel.Data.AvailableSpace) ? Int32.Parse(fileSystemItemModel.Data.AvailableSpace) : 0;
        this.FreeSpace = fileSystemItemModel.Data.FreeSpace ?? "";
        this.FreeSpaceBytes = !String.IsNullOrWhiteSpace(fileSystemItemModel.Data.FreeSpaceBytes) ? long.Parse(fileSystemItemModel.Data.FreeSpaceBytes) : 0;
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

        return new FileSystemHistoryItem(model.ServerItemServiceNowKey, model.RawData, model.RawDataCI)
        {
            Id = model.Id,
            ServiceNowKey = model.ServiceNowKey,
            ClassName = model.ClassName,
            Name = model.Name,
            InstallStatus = model.InstallStatus,
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
