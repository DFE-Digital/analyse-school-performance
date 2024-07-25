namespace ASP.Core.Results
{
    public class UnexpectedError : Error
    {
        public override string ErrorType => "Unexpected";

        public string? StackTrace { get; }

        public UnexpectedError(string message, string? stackTrace)
            : base(message)
        {
            StackTrace = stackTrace;
        }

        public override Error MapMessage(Func<string, string> mapFunction)
        {
            return new UnexpectedError(mapFunction(Message), StackTrace);
        }

        public override async Task<Error> MapMessage(Func<string, Task<string>> mapFunction)
        {
            var message = await mapFunction(Message);
            return new UnexpectedError(message, StackTrace);
        }
    }
}
