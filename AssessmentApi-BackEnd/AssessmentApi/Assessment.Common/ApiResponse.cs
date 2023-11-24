namespace Assessment.Common
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
        public ApiResponse() { }


        public static ApiResponse<T> Success(T data, string message)
        {
            return new ApiResponse<T> { Succeeded = true, Data = data, Message = message };
        }


        public static ApiResponse<T> Failed(T data, string message = null, List<string> errors = null)
        {
            return new ApiResponse<T> { Succeeded = false, Data = data, Message = message, Errors = errors };
        }

    }
}