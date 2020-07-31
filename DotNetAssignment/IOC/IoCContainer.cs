using DotNetAssignment.Domain;
using DotNetAssignment.Domain.Repository;
using DotNetAssignment.Services;
using DotNetAssignment.UnitOfWorks;
using Unity;
using Unity.Lifetime;

namespace DotNetAssignment.IOC
{
    /// <summary>
    /// Registers all class with Interface to gain the dependency injection
    /// </summary>
    /// <returns></returns>
    public class IoCContainer
    {
        public static UnityContainer RegisterUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<ITodoTaskRepository, TodoTaskRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITodoTaskService, TodoTaskService>(new HierarchicalLifetimeManager());
            // one db context per request, disposed when request ends
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            return container;
        }
    }
}