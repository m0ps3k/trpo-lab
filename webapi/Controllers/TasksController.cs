using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskApi.Data.Requests;
using TaskApi.Data.Responses;
using TaskApi.Interfaces;

namespace TaskApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TasksController(ITaskService taskService) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await taskService.GetTasks(UserID);
        if (!response.Success) return UnprocessableEntity(response);
        return Ok(response.Tasks.Select(t => new TaskResponse
        {
            Id = t.Id, IsCompleted = t.IsCompleted, Name = t.Name, Ts = t.Ts
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Post(TaskRequest request)
    {
        var task = new Data.Entities.Task
        {
            IsCompleted = request.IsCompleted, Ts = DateTime.Now, Name = request.Name, UserId = UserID
        };
        var response = await taskService.SaveTask(task);
        if (!response.Success) return UnprocessableEntity(response);
        var saved = response.Task!;
        return Ok(new TaskResponse
        {
            Id = saved.Id, IsCompleted = saved.IsCompleted, Name = saved.Name, Ts = saved.Ts
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await taskService.DeleteTask(id, UserID);
        if (!response.Success) return UnprocessableEntity(response);
        return Ok(response.TaskId);
    }
}
