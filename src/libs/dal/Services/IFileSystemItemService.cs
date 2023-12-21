using HSB.Entities;

namespace HSB.DAL.Services;

public interface IFileSystemItemService : IBaseService<FileSystemItem>
{
    IEnumerable<FileSystemItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<FileSystemItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<FileSystemItem, FileSystemItem>>? sort = null,
        int? take = null,
        int? skip = null);

    public IEnumerable<FileSystemItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<FileSystemItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null);
}
