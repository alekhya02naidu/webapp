namespace ToDoApp.DAL.Mapper;

/// <summary>
/// Mapper class for mapping TaskDto to Task entity.
/// </summary>
public class TaskMapper
{
    /// <summary>
    /// Maps a TaskDto object to a new Task entity.
    /// </summary>
    /// <param name="taskDto">The TaskDto object to map.</param>
    /// <returns>The mapped Task entity.</returns>
    public DB.Models.Task MapTaskDtoToTask(TaskDto taskDto)
    {
        var task = new DB.Models.Task
        {
            UserId = taskDto.UserId,
            Title = taskDto.Title,
            Description = taskDto.Description,
            IsCompleted = taskDto.IsCompleted,
            CreatedAt = taskDto.CreatedAt,
            UpdatedAt = taskDto.UpdatedAt
        };
        return task;
    }

    /// <summary>
    /// Maps a TaskDto object to an existing Task entity.
    /// </summary>
    /// <param name="taskDto">The TaskDto object containing updated data.</param>
    /// <param name="task">The existing Task entity to update.</param>
    /// <returns>The updated Task entity.</returns>
    public DB.Models.Task MapTaskDtoToExistingTask(TaskDto taskDto, DB.Models.Task task)
    {
        task.UserId = taskDto.UserId;
        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.IsCompleted = taskDto.IsCompleted;
        task.UpdatedAt = taskDto.UpdatedAt;
        return task;
    }

    /// <summary>
    /// Maps a Task object to a new TaskDto entity.
    /// </summary>
    /// <param name="task">The Task object to map.</param>
    /// <returns>The mapped TaskDto entity.</returns>
    public TaskDto MapTaskToTaskDto(DB.Models.Task task)
    {
        var taskDto = new TaskDto
        {
            Id = task.Id,
            UserId = task.UserId,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
        return taskDto;
    }
}