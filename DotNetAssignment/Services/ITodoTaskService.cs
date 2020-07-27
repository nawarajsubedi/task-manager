using DotNetAssignment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetAssignment.Services
{
    public interface ITodoTaskService
    {
        Task<TaskViewModel> GetById(int id);
        Task<List<TaskViewModel>> GetAllAsync();
        Task<TaskViewModel> Create(TaskViewModel model);
        Task<TaskViewModel> Update(TaskViewModel model);
        Task<TaskViewModel> MarkAsCompleted(int id);
    }
}