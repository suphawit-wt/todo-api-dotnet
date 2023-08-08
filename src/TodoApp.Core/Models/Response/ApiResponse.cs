namespace TodoApp.Core.Models.Response
{
    public class ApiResponse<T>
    {
        public string? Code { get; set; } = "SUCCESS";

        public string? Message { get; set; } = null;

        public T? Data { get; set; }

        public IDictionary<string, List<string>?>? Errors { get; set; } = null;

        public ApiResponse() { }

        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(string message)
        {
            Message = message;
        }

    }
}