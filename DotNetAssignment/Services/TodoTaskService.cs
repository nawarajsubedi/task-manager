using DotNetAssignment.Domain;
using DotNetAssignment.Models;
using DotNetAssignment.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAssignment.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        readonly IUnitOfWork unitOfWork;

        public TodoTaskService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TaskViewModel> Create(TaskViewModel model, string userId)
        {
            var task = new TodoTask
            {
                Name = model.Name,
                Description = model.Description,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                IsCompleted = false
            };

            await unitOfWork.TodoTask.Add(task);
            await unitOfWork.Complete();

            model.Id = task.Id;

            return model;
        }

        public async Task<List<TaskViewModel>> GetAllAsync(string userId, bool isAdmin)
        {
            // admin can see all tasks, user only own tasks
            var tasks = isAdmin
                ? await unitOfWork.TodoTask.GetAll()
                : await unitOfWork.TodoTask.Find(x => x.CreatedBy == userId);

            return tasks.OrderByDescending(x => x.CreatedOn).Select(x => ToViewModel(x)).ToList();
        }

        public async Task<TaskViewModel> GetById(int id, string userId, bool isAdmin)
        {
            var task = await GetTask(id, userId, isAdmin);

            if (task == null)
            {
                return null;
            }

            return ToViewModel(task);
        }

        public async Task<TaskViewModel> MarkAsCompleted(int id, string userId, bool isAdmin)
        {
            var task = await GetTask(id, userId, isAdmin);

            if (task == null)
            {
                return null;
            }

            if (!task.IsCompleted)
            {
                task.IsCompleted = true;
                task.MarkBy = userId;
                await unitOfWork.Complete();
            }

            return ToViewModel(task);
        }

        public async Task<TaskViewModel> Update(TaskViewModel model, string userId, bool isAdmin)
        {
            var task = await GetTask(model.Id, userId, isAdmin);

            if (task == null)
            {
                return null;
            }

            task.Name = model.Name;
            task.Description = model.Description;
            task.ModifiedBy = userId;
            task.ModifiedOn = DateTime.UtcNow;
            await unitOfWork.Complete();

            return model;
        }

        public async Task<bool> Delete(int id, string userId, bool isAdmin)
        {
            var task = await GetTask(id, userId, isAdmin);

            if (task == null)
            {
                return false;
            }

            await unitOfWork.TodoTask.Remove(task);
            await unitOfWork.Complete();

            return true;
        }

        private async Task<TodoTask> GetTask(int id, string userId, bool isAdmin)
        {
            var task = await unitOfWork.TodoTask.Get(id);

            // other user's task is treated as not found
            if (task == null || (!isAdmin && task.CreatedBy != userId))
            {
                return null;
            }

            return task;
        }

        private TaskViewModel ToViewModel(TodoTask task)
        {
            return new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }
    }
}