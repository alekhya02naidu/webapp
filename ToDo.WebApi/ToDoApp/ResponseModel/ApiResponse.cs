using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ToDoApp.ResponseModel.Enums;

namespace ToDoApp.ResponseModel;
public class ApiResponse<T>
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ResponseStatus Status { get; set; }
    public T Data { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public ErrorCode? ErrorCode { get; set; }

    /// <summary>
    /// Constructor for ApiResponse class.
    /// </summary>
    /// <param name="status">The status of the API response.</param>
    /// <param name="data">The data returned in the API response.</param>
    /// <param name="errorCode">Optional error code in case of an error.</param>
    public ApiResponse(ResponseStatus status, T data, ErrorCode? errorCode = null)
    {
        Status = status;
        Data = data;
        ErrorCode = errorCode;
    }
}

