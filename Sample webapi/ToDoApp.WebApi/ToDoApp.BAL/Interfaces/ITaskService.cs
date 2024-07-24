namespace ToDoApp.BAL.Interfaces;

/// <summary>
/// Interface defining operations related to task management.
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Inserts a new task into the system.
    /// </summary>
    /// <param name="taskDto">The task details to insert.</param>
    /// <returns>The ID of the newly inserted task.</returns>
    Task<int> AddTask(TaskDto taskDto);

    /// <summary>
    /// Retrieves all tasks from the system.
    /// </summary>
    /// <returns>A collection of all tasks.</returns>
    Task<IEnumerable<TaskDto>> GetAllTasks();

    /// <summary>
    /// Retrieves tasks associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose tasks are to be retrieved.</param>
    /// <returns>A collection of tasks belonging to the specified user.</returns>
    Task<IEnumerable<TaskDto>> GetTasksByUserId(int userId);

    /// <summary>
    /// Updates an existing task in the system.
    /// </summary>
    /// <param name="taskDto">The task details to update.</param>
    /// <returns>The ID of the updated task.</returns>
    Task<int> UpdateTask(TaskDto taskDto);

    /// <summary>
    /// Deletes tasks identified by their IDs.
    /// </summary>
    /// <param name="ids">An array of task IDs to delete.</param>
    /// <returns>The number of tasks deleted.</returns
    Task<int> DeleteTask(int[] ids, int userId);
}