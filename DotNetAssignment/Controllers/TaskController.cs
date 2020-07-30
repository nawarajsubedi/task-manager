using DotNetAssignment.Models;
using DotNetAssignment.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotNetAssignment.Controllers
{
    [Authorize(Roles = "admin,user")]
    [RoutePrefix("Task")]
    public class TaskController : ApiController
    {
        private readonly ITodoTaskService taskService;

        public TaskController(ITodoTaskService todoTaskService)
        {
            this.taskService = todoTaskService;
        }

        // GET Task
        // GET Task?completed=true
        [Route("")]
        public async Task<IHttpActionResult> GetAll(bool? completed = null)
        {
            var tasks = await taskService.GetAllAsync(User.Identity.GetUserId(), User.IsInRole("admin"), completed);
            return Ok(tasks);
        }

        // GET Task/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var task = await taskService.GetById(id, User.Identity.GetUserId(), User.IsInRole("admin"));

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // POST Task
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] TaskViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Request body is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await taskService.Create(model, User.Identity.GetUserId());
            return Created(new Uri($"{Request.RequestUri.ToString().TrimEnd('/')}/{task.Id}"), task);
        }

        // PUT Task/5
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] TaskViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Request body is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await taskService.GetById(id, User.Identity.GetUserId(), User.IsInRole("admin"));

            if (task == null)
            {
                return NotFound();
            }

            model.Id = id;
            var updatedTask = await taskService.Update(model, User.Identity.GetUserId(), User.IsInRole("admin"));

            return Ok(updatedTask);
        }

        // PATCH Task/5
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> MarkAsCompleted(int id)
        {
            var task = await taskService.GetById(id, User.Identity.GetUserId(), User.IsInRole("admin"));

            if (task == null)
            {
                return NotFound();
            }
            var updatedTask = await taskService.MarkAsCompleted(id, User.Identity.GetUserId(), User.IsInRole("admin"));

            return Ok(updatedTask);
        }

        // DELETE Task/5
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var deleted = await taskService.Delete(id, User.Identity.GetUserId(), User.IsInRole("admin"));

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
