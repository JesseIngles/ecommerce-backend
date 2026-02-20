namespace Kimbito.Shared;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool IsSucess {get;set;}
    public string Message {get;set;} 
    public int StatusCode {get;set;}
    public ApiResponse(T? Data, bool Success, string Message, int StatusCode)
    {
        this.Data = Data;
        this.IsSucess = Success;
        this.Message = Message;
        this.StatusCode = StatusCode;
    }

    public static ApiResponse<T> Success(T data, int statusCode = 200, string message = "")
        => new ApiResponse<T>(data, true, message, statusCode);

    public static ApiResponse<T> Error(T data, int statusCode = 400, string message = "")
        => new ApiResponse<T>(data, false, message, statusCode);
}   