using DotNetAssignment.Models;
using DotNetAssignment.Services;
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
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var tasks = await taskService.GetAllAsync();
            return Ok(tasks);
        }

        // GET Task/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var tasks = await taskService.GetById(id);
            return Ok(tasks);
        }

        // POST Task
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await taskService.Create(model);
            return Created(new Uri($"{Request.RequestUri.ToString()}/{task.Id}"), task);
        }

        // PUT Task/5
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await taskService.GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            model.Id = id;
            var updatedTask = await taskService.Update(model);

            return Ok(updatedTask);
        }

        // PATCH Task/5
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> MarkAsCompleted(int id)
        {
            var task = await taskService.GetById(id);

            if (task == null)
            {
                return NotFound();
            }
            var updatedTask = await taskService.MarkAsCompleted(id);

            return Ok(updatedTask);
        }
    }
}
