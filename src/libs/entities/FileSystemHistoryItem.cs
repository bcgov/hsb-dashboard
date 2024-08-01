using System.Text.Json;

namespace HSB.Entities;
public class FileSystemHistoryItem : FileSystemHistoryItemSmall
{
    #region Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
    #endregion

    #region Constructors
    protected FileSystemHistoryItem() { }

    public FileSystemHistoryItem(string serverItemId, JsonDocument fileSystemItemData, JsonDocument configurationItemData)
        : base(serverItemId, fileSystemItemData, configurationItemData)
    {
    }

    public FileSystemHistoryItem(FileSystemItem entity)
        : base(entity)
    {
    }
    #endregion
}
