namespace ASP.Core.Results
{
    public class NotAllowedError : Error
    {
        public override string ErrorType => "Not allowed";

        public NotAllowedError(string message)
            : base(message)
        {
        }

        public override Error MapMessage(Func<string, string> mapFunction)
        {
            return new NotAllowedError(mapFunction(Message));
        }

        public override async Task<Error> MapMessage(Func<string, Task<string>> mapFunction)
        {
            var message = await mapFunction(Message);
            return new NotAllowedError(message);
        }
    }
}
