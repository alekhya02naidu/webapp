namespace ToDoApp.BAL.Service;

/// <summary>
/// Service class implementing ITaskService for task management operations.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepo;

    public TaskService(ITaskRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    /// <inheritdoc />
    public async Task<int> AddTask(TaskDto taskDto)
    {
        return await _taskRepo.AddTask(taskDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskDto>> GetAllTasks()
    {
        return await _taskRepo.GetAllTasks();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskDto>> GetTasksByUserId(int userId)
    {
        return await _taskRepo.GetTasksByUserId(userId);
    }

    /// <inheritdoc />
    public async Task<int> UpdateTask(TaskDto taskDto)
    {
        return await _taskRepo.UpdateTask(taskDto);
    }

    /// <inheritdoc />
    public async Task<int> DeleteTask(int[] ids, int userId)
    {
        return await _taskRepo.DeleteTask(ids, userId);
    }
}