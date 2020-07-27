using DotNetAssignment.UnitOfWorks;

namespace DotNetAssignment.Domain.Repository
{
    public class TodoTaskRepository : Repository<TodoTask>, ITodoTaskRepository
    {
        public TodoTaskRepository(ApplicationDbContext context)
            : base(context)
        {

        }
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}