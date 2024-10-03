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
            => new NotFoundError(message);

        public static Error Unexpected(string message, string? stackTrace)
            => new UnexpectedError(message, stackTrace);

        public static Error Invalid(string message)
            => new ValidationError(message);

        public static Error NotAllowed(string message)
            => new NotAllowedError(message);

        public override string ToString() 
            => $"{MessagePrefix}{Message}";

        public abstract Error MapMessage(Func<string, string> mapFunction);

        public abstract Task<Error> MapMessage(Func<string, Task<string>> mapFunction);

        public override bool Equals(object? other)
            => other is Error error && error.ErrorType == ErrorType && error.Message == Message;

        public override int GetHashCode()
            => HashCode.Combine(ErrorType, Message);
    }
}
