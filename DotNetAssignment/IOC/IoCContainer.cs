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
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            return container;
        }
    }
}