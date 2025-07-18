using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;
using TaskApi.Services;
using System.IdentityModel.Tokens.Jwt;
using TaskApi.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogService _logger;

        public TaskController(ITaskService taskService, ILogService logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll(int id, bool asc)
        {
            var tasks = await _taskService.GetAll(id, asc);
            if (tasks == null || !tasks.Any())
            {
                _logger.LogWarning("Not found tasks for getall", tasks);
                return NotFound("No tasks found.");
            }
            _logger.LogInfo("all tasks reached", new { list_id = id });
            return Ok(tasks);
        }

        [Authorize]
        [HttpGet]
        [Route("get/{id:int}")]
        public async Task<IActionResult> GetByName(int id, string name)
        {
            var task = await _taskService.GetByName(name, id);
            _logger.LogInfo("Got by name", new { list_id = id });
            return Ok(task);
        }

        [Authorize]
        [HttpGet]
        [Route("deadline/{id:int}")]
        public async Task<IActionResult> FilterByDeadline(DateTime? low, DateTime? high, int id)
        {
            var tasks = await _taskService.FilterByDeadline(low, high, id);
            _logger.LogInfo("Deadline filtered", new { list_id = id });
            return Ok(tasks);
        }

        [Authorize]
        [HttpPost]
        [Route("addtask")]
        public async Task<IActionResult> AddTask(TaskDTO addedTask)
        {
            var temp = await _taskService.AddTask(addedTask);
            _logger.LogInfo("Task added", new { task_id = temp.task_id });
            return Ok(new { msg = "done" });
        }

        //Task id ye TaskDTO ile ulaşamıyorum
        //bu yüzden TaskType gönderdim
        //insanlar fed de value veremese bile task creation dateleri değiştirme şansına sahip
        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateTask(TaskType updated)
        {
            var temp = await _taskService.UpdateTask(updated);
            _logger.LogInfo("Task updated", new { list_id = temp.list_id, task_id = temp.task_id });
            return Ok(new { msg = "done sir!" });
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var temp = await _taskService.DeleteTask(id);
            _logger.LogInfo("Task deleted", new { task_id = id });
            return Ok(new { msg = "sildik" });
        }

        [Authorize]
        [HttpPut]
        [Route("setdone/{id:int}")]
        public async Task<IActionResult> SetTaskDone(int id)
        {
            var boo = await _taskService.SetDone(id);
            if (boo == true)
            {
                _logger.LogInfo("Task marked as done!!!", new { task_id = id });
                return Ok(new { msg = "marked as done" });
            }

            _logger.LogWarning("Something went wrong while marking task as done", new { task_id = id });
            return BadRequest("we couldnt update the tasks status idk");
        }

    }
}