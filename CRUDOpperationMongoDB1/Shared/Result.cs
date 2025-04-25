namespace CRUDOpperationMongoDB1.Shared
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public bool Success { get; set; }
        public string Error { get; private set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public static Result Ok(string message, object? data = null) =>
            new Result { Success = true, Message = message, Data = data };
        public static Result Fail(string message, object? data = null) =>
            new Result { Success = false, Message = message, Data = data };
    }
}