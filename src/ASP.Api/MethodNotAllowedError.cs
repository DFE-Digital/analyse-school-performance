using ASP.Core.Results;

namespace ASP.Api
{
    public class MethodNotAllowedError : Error
    {
        public override string ErrorType => "Method not allowed";

        public string Method { get; }
        public string[] AllowedMethods { get; }

        public MethodNotAllowedError(string method, string[] allowedMethods)
            : base($"The HTTP method {method} is not allowed.")
        {
            Method = method;
            AllowedMethods = allowedMethods;
        }

        public override Error MapMessage(Func<string, string> mapFunction)
        {
            return new MethodNotAllowedError(mapFunction(Message), AllowedMethods);
        }

        public override async Task<Error> MapMessage(Func<string, Task<string>> mapFunction)
        {
            var message = await mapFunction(Message);
            return new MethodNotAllowedError(message, AllowedMethods);
        }
    }
}
