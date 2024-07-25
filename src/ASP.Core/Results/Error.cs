namespace ASP.Core.Results
{
    public abstract class Error
    {
        public abstract string ErrorType { get; }
        public string Message { get; }
        public string MessagePrefix => $"{ErrorType}: ";

        public Error(string message)
        {
            Message = message;
        }

        public static Error NotFound(string message)
        {
            return new NotFoundError(message);
        }

        public static Error Unexpected(string message, string? stackTrace)
        {
            return new UnexpectedError(message, stackTrace);
        }

        public static Error Invalid(string message)
        {
            return new ValidationError(message);
        }

        public static Error NotAllowed(string message)
        {
            return new NotAllowedError(message);
        }

        public override string ToString() => $"{MessagePrefix}{Message}";

        public abstract Error MapMessage(Func<string, string> mapFunction);

        public abstract Task<Error> MapMessage(Func<string, Task<string>> mapFunction);
    }
}
