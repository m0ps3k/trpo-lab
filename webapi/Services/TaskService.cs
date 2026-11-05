using Microsoft.EntityFrameworkCore;
using TaskApi.Data.Entities;
using TaskApi.Data.Responses;
using TaskApi.Interfaces;

namespace TaskApi.Services;

public class TaskService(TasksDbContext tasksDbContext) : ITaskService
{
    public async Task<DeleteTaskResponse> DeleteTask(int taskId, int userId)
    {
        var task = await tasksDbContext.Tasks.FindAsync(taskId);
        if (task is null)
            return new() { Error = "Task not found", ErrorCode = "T01" };
        if (task.UserId != userId)
            return new() { Error = "You don't have access to delete this task", ErrorCode = "T02" };
        tasksDbContext.Tasks.Remove(task);
        if (await tasksDbContext.SaveChangesAsync() >= 0)
            return new() { Success = true, TaskId = task.Id };
        return new() { Error = "Unable to delete task", ErrorCode = "T03" };
    }

    public async Task<GetTasksResponse> GetTasks(int userId)
    {
        var tasks = await tasksDbContext.Tasks.Where(t => t.UserId == userId).ToListAsync();
        if (tasks.Count == 0)
            return new() { Error = "No tasks found for this user", ErrorCode = "T04" };
        return new() { Success = true, Tasks = tasks };
    }

    public async Task<SaveTaskResponse> SaveTask(Data.Entities.Task task)
    {
        await tasksDbContext.Tasks.AddAsync(task);
        if (await tasksDbContext.SaveChangesAsync() >= 0)
            return new() { Success = true, Task = task };
        return new() { Error = "Unable to save task", ErrorCode = "T05" };
    }
}
