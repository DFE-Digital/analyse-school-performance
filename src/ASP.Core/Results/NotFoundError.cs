namespace ASP.Core.Results
{
    public class NotFoundError : Error
    {
        public override string ErrorType => "Not found";

        public NotFoundError(string message)
            : base(message)
        {
        }

        public override Error MapMessage(Func<string, string> mapFunction)
        {
            return new NotFoundError(mapFunction(Message));
        }

        public override async Task<Error> MapMessage(Func<string, Task<string>> mapFunction)
        {
            var message = await mapFunction(Message);
            return new NotFoundError(message);
        }
    }
}
