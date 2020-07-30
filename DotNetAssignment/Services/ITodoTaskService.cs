using DotNetAssignment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetAssignment.Services
{
    public interface ITodoTaskService
    {
        Task<TaskViewModel> GetById(int id, string userId, bool isAdmin);
        Task<List<TaskViewModel>> GetAllAsync(string userId, bool isAdmin);
        Task<TaskViewModel> Create(TaskViewModel model, string userId);
        Task<TaskViewModel> Update(TaskViewModel model, string userId, bool isAdmin);
        Task<TaskViewModel> MarkAsCompleted(int id, string userId, bool isAdmin);
    }
}