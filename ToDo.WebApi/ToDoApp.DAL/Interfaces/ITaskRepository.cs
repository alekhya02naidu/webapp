using ToDoApp.DB.Models;

namespace ToDoApp.DAL.Interfaces;

/// <summary>
/// Interface defining operations related to task data access.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Inserts a new task into the database.
    /// </summary>
    /// <param name="taskDto">The TaskDto object representing the task to insert.</param>
    /// <returns>The ID of the newly inserted task.</returns>
    Task<int> InsertTask(TaskDto taskDto);

    /// <summary>
    /// Retrieves all tasks from the database.
    /// </summary>
    /// <returns>A collection of all tasks.</returns>
    Task<IEnumerable<TaskDto>> GetAllTasks();

    /// <summary>
    /// Retrieves tasks by user ID from the database.
    /// </summary>
    /// <param name="userId">The ID of the user whose tasks to retrieve.</param>
    /// <returns>A collection of tasks belonging to the specified user.</returns>
    Task<int> UpdateTask(TaskDto taskDto);

    /// <summary>
    /// Updates an existing task in the database.
    /// </summary>
    /// <param name="taskDto">The TaskDto object representing the task to update.</param>
    /// <returns>The ID of the updated task.</returns>
    Task<int> DeleteTask(int[] ids);

    /// <summary>
    /// Deletes tasks with the specified IDs from the database.
    /// </summary>
    /// <param name="ids">An array of task IDs to delete.</param>
    /// <returns>The number of tasks deleted.</returns>
    Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(int userId);
}