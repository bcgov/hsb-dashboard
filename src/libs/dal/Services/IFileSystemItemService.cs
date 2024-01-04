using HSB.Entities;
using HSB.Models.Filters;

namespace HSB.DAL.Services;

public interface IFileSystemItemService : IBaseService<FileSystemItem>
{
    IEnumerable<FileSystemItem> Find(FileSystemItemFilter filter);

    IEnumerable<FileSystemItem> FindForUser(long userId, FileSystemItemFilter filter);

    FileSystemItem? FindForId(string key, long userId);
}
