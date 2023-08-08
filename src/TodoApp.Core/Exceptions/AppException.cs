namespace TodoApp.Core.Exceptions
{
    public class AppException : Exception
    {
        public string Code { get; }
        public int StatusCode { get; }
        public IDictionary<string, List<string>?>? Errors { get; }

        public AppException(string code, int statusCode, string message, IDictionary<string, List<string>?>? errors) : base(message)
        {
            Code = code;
            StatusCode = statusCode;
            Errors = errors;
        }
    }

    public class BadRequestError : AppException
    {
        public BadRequestError(string message, IDictionary<string, List<string>?>? errors) : base("BAD_REQUEST", 400, message, errors) { }
    }

    public class UnauthorizedError : AppException
    {
        public UnauthorizedError(string message) : base("UNAUTHORIZED", 401, message, null) { }
    }

    public class ForbiddenError : AppException
    {
        public ForbiddenError(string message) : base("FORBIDDEN", 403, message, null) { }
    }

    public class NotFoundError : AppException
    {
        public NotFoundError(string message) : base("NOT_FOUND", 404, message, null) { }
    }

    public class ConflictError : AppException
    {
        public ConflictError(string message) : base("CONFLICT", 409, message, null) { }
    }

    public class DatabaseError : AppException
    {
        public DatabaseError(string message) : base("DATABASE_ERROR", 500, message, null) { }
    }

    public class UnexpectedError : AppException
    {
        public UnexpectedError() : base("UNEXPECTED", 500, "Unexpected error", null) { }
    }

}