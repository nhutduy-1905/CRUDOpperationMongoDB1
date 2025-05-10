namespace CRUDOpperationMongoDB1.Shared
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }


        public static Result<T> Success(T data)
          => new Result<T> { IsSuccess = true, Data = data };

        public static Result<T> Success(string message, T data)
            => new Result<T> { IsSuccess = true, ErrorMessage = message, Data = data };

        public static Result<T> Failure(string errorMessage)
            => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage };

        public static Result<T> Failure(string errorMessage, T data = default)
            => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage, Data = data };
    }
}
