using ASP.Core.Results;
using Microsoft.AspNetCore.Http;

namespace ASP.Api;

public class RequestRouteParameterValidationBuilder<T>
{
    public HttpRequest Request { get; }
    public string ParameterName { get; }
    public Result<T> Result { get; }

    public RequestRouteParameterValidationBuilder(HttpRequest request, string parameterName, Result<T> result)
    {
        Request = request;
        ParameterName = parameterName;
        Result = result;
    }
}