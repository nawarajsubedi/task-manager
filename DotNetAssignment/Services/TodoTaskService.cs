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

        public async Task<TaskViewModel> Create(TaskViewModel model)
        {
            var task = new TodoTask
            {
                Name = model.Name,
                Description = model.Description,
                CreatedOn = DateTime.Now,
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
            await unitOfWork.Complete();

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
            await unitOfWork.Complete();

            return new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskViewModel> MarkAsCompleted(int id)
        {
            var task = await unitOfWork.TodoTask.Get(id);
            task.IsCompleted = true;
            await unitOfWork.Complete();

            return new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskViewModel> Update(TaskViewModel model)
        {
            var task = await unitOfWork.TodoTask.Get(model.Id);
            task.Name = model.Name;
            task.Description = model.Description;
            await unitOfWork.Complete();

            return model;
        }
    }
}