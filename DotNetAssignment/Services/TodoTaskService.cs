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

        public async Task<List<TaskViewModel>> GetAllAsync()
        {
            var tasks = await unitOfWork.TodoTask.GetAll();

            return tasks.Select(x => new TaskViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsCompleted = x.IsCompleted
            }).ToList();

        }

        public async Task<TaskViewModel> GetById(int id)
        {
            var task = await unitOfWork.TodoTask.Get(id);

            if (task == null)
            {
                return null;
            }

            return new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskViewModel> MarkAsCompleted(int id, string userId)
        {
            var task = await unitOfWork.TodoTask.Get(id);

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

            return new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskViewModel> Update(TaskViewModel model, string userId)
        {
            var task = await unitOfWork.TodoTask.Get(model.Id);

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
    }
}