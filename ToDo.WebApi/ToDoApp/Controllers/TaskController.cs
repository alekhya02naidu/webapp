using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.BAL.Interfaces;
using ToDoApp.DB.Models;
using ToDoApp.ResponseModel;
using ToDoApp.ResponseModel.Enums;

namespace ToDoApp.Controllers;

[Route("[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TaskController> _logger;

    public TaskController(ITaskService taskService, ILogger<TaskController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    /// <summary>
    /// Generates a standardized API response.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    /// <param name="data">The response data.</param>
    /// <param name="status">The status of the response (default is Success).</param>
    /// <param name="errorCode">Optional error code in case of an error.</param>
    /// <returns>An ApiResponse object containing the specified data, status, and error code.</returns>
    private ApiResponse<T> GenerateResponse<T>(T data, ResponseStatus status = ResponseStatus.Success, ErrorCode? errorCode = null)
    {
        return new ApiResponse<T>
        (
            status,
            data,
            errorCode
        );
    }

    /// <summary>
    /// Gets all tasks from bussiness layer
    /// </summary>
    /// <returns>An ApiResponse containing a list of all tasks or an error status.</returns>
    [Authorize]
    [HttpGet]
    public async Task<ApiResponse<IEnumerable<TaskDto>>> GetAllTasks()
    {
        try
        {
            var tasks = await _taskService.GetAllTasks();
            return GenerateResponse(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all tasks");
            return GenerateResponse<IEnumerable<TaskDto>>(null, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    /// <summary>
    /// Retrieves tasks for the currently authenticated user.
    /// </summary>
    /// <returns>An ApiResponse containing a list of tasks for the specific user or an error status.</returns>
    [Authorize]
    [HttpGet("UserSpecificTask")]
    public async Task<ApiResponse<IEnumerable<TaskDto>>> GetTasksByUserIdAsync()
    {
        try 
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim == null)
            {
                return GenerateResponse<IEnumerable<TaskDto>>(null!, ResponseStatus.Error, ErrorCode.UnAuthorized);
            }
            var userId = int.Parse(userIdClaim.Value);

            var tasksForSpecificUser = await _taskService.GetTasksByUserIdAsync(userId);
            if (tasksForSpecificUser != null)
            {
                return GenerateResponse(tasksForSpecificUser);
            }
            else
            {
                return GenerateResponse<IEnumerable<TaskDto>>(null!, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching the tasks of a specific user");
            return GenerateResponse<IEnumerable<TaskDto>>(null!, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }
    
    /// <summary>
    /// Creates a new task for the currently authenticated user.
    /// </summary>
    /// <param name="taskDto">The task details.</param>
    /// <returns>An ApiResponse containing the ID of the newly created task or an error status.</returns>
    [Authorize]
    [HttpPost]
    public async Task<ApiResponse<int>> CreateTask([FromBody] TaskDto taskDto)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim == null)
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.UnAuthorized);
            }
            var userId = int.Parse(userIdClaim.Value);
            taskDto.UserId = userId;

            var taskId = await _taskService.InsertTask(taskDto);
            if (taskId > 0)
            {
                return GenerateResponse(taskId);
            }
            else
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating the task");
            return GenerateResponse(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    /// <summary>
    /// Updates an existing task for the currently authenticated user.
    /// </summary>
    /// <param name="taskDto">The task details to update.</param>
    /// <returns>An ApiResponse containing the ID of the updated task or an error status.</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ApiResponse<int>> UpdateTask(TaskDto taskDto)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim == null)
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.UnAuthorized);
            }
            var userId = int.Parse(userIdClaim.Value);
            taskDto.UserId = userId;
            
            var taskId = await _taskService.UpdateTask(taskDto);
            if(taskId > 0)
            {
                return GenerateResponse(taskId);
            }
            else
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating the task");
            return GenerateResponse(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }


    /// <summary>
    /// Deletes tasks with the specified IDs for the currently authenticated user.
    /// </summary>
    /// <param name="ids">An array of task IDs to delete.</param>
    /// <returns>An ApiResponse containing the number of tasks deleted or an error status.</returns>
    [Authorize]
    [HttpDelete("DeleteTasks")]
    public async Task<ApiResponse<int>> DeleteTasks([FromQuery] int[] ids)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim == null)
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.UnAuthorized);
            }
            var userId = int.Parse(userIdClaim.Value);

            var deletedCount = await _taskService.DeleteTask(ids);
            if (deletedCount > 0)
            {
                return GenerateResponse(deletedCount);
            }
            else
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting tasks");
            return GenerateResponse(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }
}