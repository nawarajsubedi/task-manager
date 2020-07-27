using DotNetAssignment.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace DotNetAssignment.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoTaskRepository TodoTask { get; }
        Task<int> Complete();
    }
}
