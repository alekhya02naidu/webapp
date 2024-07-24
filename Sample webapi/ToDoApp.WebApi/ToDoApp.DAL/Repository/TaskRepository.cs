
namespace ToDoApp.DAL.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly ToDoAppDbContext _dbContext;
    private readonly TaskMapper _taskMapper;

    /// <summary>
    /// Repository class implementing ITaskRepository for task data access operations.
    /// </summary>
    public TaskRepository(ToDoAppDbContext dbContext, TaskMapper taskMapper)
    {
        _dbContext = dbContext;
        _taskMapper = taskMapper;
    }

    /// <inheritdoc />
    public async Task<int> AddTask(TaskDto taskDto)
    {
        if (taskDto == null)
        {
            throw new ArgumentNullException(nameof(taskDto));
        }
        var task = _taskMapper.MapTaskDtoToTask(taskDto);
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
        return task.Id;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskDto>> GetAllTasks()
    {
        var tasks = await _dbContext.Tasks.ToListAsync();
        return tasks.Select(_taskMapper.MapTaskToTaskDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskDto>> GetTasksByUserId(int userId)
    {
        var tasks = await _dbContext.Tasks.Where(t => t.UserId == userId).ToListAsync();
        return tasks.Select(_taskMapper.MapTaskToTaskDto);
    }

    /// <inheritdoc />
    public async Task<int> UpdateTask(TaskDto taskDto)
    {
        var existingtask = await _dbContext.Tasks.FindAsync(taskDto.Id);
        if (existingtask == null)
        {
            return -1;
        }

        var updatedTask = _taskMapper.MapTaskDtoToExistingTask(taskDto, existingtask);
        await _dbContext.SaveChangesAsync();
        return updatedTask.Id;
    }

    /// <inheritdoc />
    public async Task<int> DeleteTask(int[] ids, int userId)
    {
        var taskToBeDeleted = await _dbContext.Tasks
                            .Where(t => ids.Contains(t.Id)  && t.UserId == userId).ToListAsync();

        if (taskToBeDeleted == null)
        {
            return 0;
        }
        _dbContext.Tasks.RemoveRange(taskToBeDeleted);
        return await _dbContext.SaveChangesAsync();
    }
}