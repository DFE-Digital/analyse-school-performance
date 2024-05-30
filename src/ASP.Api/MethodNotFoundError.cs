using ASP.Core.Results;

namespace ASP.Api
{
    public class MethodNotAllowedError : Error
    {
        public string Method { get; }
        public string[] AllowedMethods { get; }

        public MethodNotAllowedError(string method, string[] allowedMethods)
            : base($"Bad request: the HTTP method {method} is not allowed.")
        {
            Method = method;
            AllowedMethods = allowedMethods;
        }
    }
}
