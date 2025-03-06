namespace HSB.DAL.Services;

public interface IRefreshMaterializedViewsService : IBaseService
{
    Task RefreshAll();
}
