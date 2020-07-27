using DotNetAssignment.Domain;
using DotNetAssignment.Domain.Repository;
using System.Threading.Tasks;

namespace DotNetAssignment.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            TodoTask = new TodoTaskRepository(_context);
        }


        public ITodoTaskRepository TodoTask { get; private set; }

        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
