using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;
using TaskApi.Services;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : Controller
    {

        private readonly IListService _service;
        private readonly ILogService _logger;
        public ListController(IListService service, ILogService logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll( bool asc)
        {
            var user = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var id = int.Parse(user);
            var lists = await _service.GetAll(id, asc);
            if (lists == null || !lists.Any())
            {
                _logger.LogWarning("Couldnt reach all lists", new { user_id = id });
                return NotFound("No lists found.");
            }

            _logger.LogInfo("Reached all lists", new { user_id = id });
            Console.Write(lists);
            return Ok(lists);
        }
        [Authorize]
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetByName(string name)
        {
            var user = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var id = int.Parse(user);
            var lists = await _service.GetByName(name, id);
            _logger.LogInfo("Reached list by name", new { user_id = id });
            return Ok(lists);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddList(ListDTO newlist)
        {
            var user = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var id = int.Parse(user);
            newlist.user_id = id;
            await _service.AddList(newlist);
            _logger.LogInfo("added list", new { user_id = newlist.user_id, list_name = newlist.list_name });
            return Ok(new { msg = "added list!" });
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            await _service.DeleteList(id);
            _logger.LogInfo("Deleted list", new { list_id = id });
            return Ok(new { msg = "deleted list" });
        }
    }
}