
namespace ASP.Core.Results
{
    public class ValidationError : Error
    {
        public override string ErrorType => "Invalid";

        public ValidationError(string message)
            : base(message)
        {
        }

        public override Error MapMessage(Func<string, string> mapFunction)
        {
            return new ValidationError(mapFunction(Message));
        }

        public override async Task<Error> MapMessage(Func<string, Task<string>> mapFunction)
        {
            var message = await mapFunction(Message);
            return new ValidationError(message);
        }
    }
}
