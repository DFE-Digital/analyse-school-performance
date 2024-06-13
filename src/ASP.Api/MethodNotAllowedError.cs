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
    }
}
