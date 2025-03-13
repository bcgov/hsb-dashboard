using System.Text.Json;
using HSB.Core.Models;
using Xunit;

namespace HSB.Entities.Tests
{

  public class FileSystemItemTest
  {

    // Utility function to generate a ServerItem object where IsVirtual is true
    private ServerItem GetVirtualServerItem()
    {
      return new ServerItem(1, 2, 3,
        JsonDocument.Parse("{ \"virtual\": true }"), JsonDocument.Parse("{}"));
    }

    // Utility function to generate a ServerItem object where IsVirtual is false
    private ServerItem GetPhysicalServerItem()
    {
      return new ServerItem(1, 2, 3,
        JsonDocument.Parse("{ \"virtual\": false }"), JsonDocument.Parse("{}"));
    }

    // Utility function to generate a FileSystemItem object
    private FileSystemItem GetFileSystemItemWithServerItemAndStorageType(ServerItem serverItem, string storageType)
    {
      return new FileSystemItem(serverItem, JsonDocument.Parse("{}"), JsonDocument.Parse("{}"))
      {
        StorageType = storageType
      };
    }

    private FileSystemItem GetFileSystemItemWithVolumeId(string volumeId)
    {
      return new FileSystemItem("abc", JsonDocument.Parse("{}"), JsonDocument.Parse("{}"))
      {
        VolumeId = volumeId
      };
    }


    [Fact]
    public void IsSAN_ShouldReturnTrue_WhenServerIsVirtualAndStorageTypeIsLogical()
    {
      // Arrange
      var serverItem = GetVirtualServerItem();
      var fileSystemItem = GetFileSystemItemWithServerItemAndStorageType(serverItem, "logical");

      // Assert
      Assert.True(fileSystemItem.IsSAN);
    }

    [Fact]
    public void IsSAN_ShouldReturnFalse_WhenServerIsVirtualAndStorageTypeIsNotLogical()
    {
      // Arrange
      var serverItem = GetVirtualServerItem();
      var fileSystemItem = GetFileSystemItemWithServerItemAndStorageType(serverItem, "network");

      // Assert
      Assert.False(fileSystemItem.IsSAN);
    }

    [Fact]
    public void IsSAN_ShouldReturnTrue_WhenServerIsPhysicalAndStorageTypeIsNetwork()
    {
      // Arrange
      var serverItem = GetPhysicalServerItem();
      var fileSystemItem = GetFileSystemItemWithServerItemAndStorageType(serverItem, "network");

      // Assert
      Assert.True(fileSystemItem.IsSAN);
    }

    [Fact]
    public void IsSAN_ShouldReturnFalse_WhenServerIsPhysicalAndStorageTypeIsNotNetwork()
    {
      // Arrange
      var serverItem = GetPhysicalServerItem();
      var fileSystemItem = GetFileSystemItemWithServerItemAndStorageType(serverItem, "logical");

      // Assert
      Assert.False(fileSystemItem.IsSAN);
    }

    [Fact]
    public void IsSAN_ShouldReturnFalse_WhenVolumeIdStartsWithCapitalC()
    {
      // Arrange
      var fileSystemItem = GetFileSystemItemWithVolumeId("C:\\");

      // Assert
      Assert.False(fileSystemItem.IsSAN);
    }

    [Fact]
    public void IsSAN_ShouldReturnFalse_WhenVolumeIdContainsRoot()
    {
      // Arrange
      var fileSystemItem = GetFileSystemItemWithVolumeId("root");

      // Assert
      Assert.False(fileSystemItem.IsSAN);
    }

    [Fact]
    public void IsSAN_ShouldReturnTrue_WhenVolumeIdDoesNotStartWithCOrContainRoot()
    {
      // Arrange
      var fileSystemItem = GetFileSystemItemWithVolumeId("D:\\");

      // Assert
      Assert.True(fileSystemItem.IsSAN);
    }
  }
}
