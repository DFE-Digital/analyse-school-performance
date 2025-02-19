using ASP.Core.Optionality;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;

namespace ASP.Api;

public abstract class RequestParameterValidationBuilder<T>
{
    public HttpRequest Request { get; }
    public string ParameterName { get; }
    public Result<T> Result { get; }

    public RequestParameterValidationBuilder(HttpRequest request, string parameterName, Result<T> result)
    {
        Request = request;
        ParameterName = parameterName;
        Result = result;
    }

    public abstract string ParameterType { get; }
    public abstract Result<string> GetRequiredSingleValue();
    public abstract Result<List<string>> GetRequiredMultiValue();
    public abstract Result<Optional<string>> GetOptionalSingleValue();
    public abstract RequestParameterValidationBuilder<TNext> Next<TNext>(Result<TNext> nextResult);
}
