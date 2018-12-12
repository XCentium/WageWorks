using Glass.Mapper.Sc;

namespace WageWorks.Foundation.ORM.Context
{
    public interface IControllerSitecoreContext : ISitecoreContext
    {
        T GetDataSource<T>() where T : class;
        T GetRenderingParameters<T>() where T : class;
        T GetControllerItem<T>(bool isLazy, bool inferType) where T : class;
        T GetRenderingItem<T>(bool isLazy, bool inferType) where T : class;
    }
}