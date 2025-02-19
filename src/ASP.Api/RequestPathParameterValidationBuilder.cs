using ASP.Core.Optionality;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;

namespace ASP.Api;

public class RequestPathParameterValidationBuilder<T> : RequestParameterValidationBuilder<T>
{
    public RequestPathParameterValidationBuilder(HttpRequest request, string parameterName, Result<T> result)
        : base(request, parameterName, result)
    {
    }

    public override string ParameterType => "path parameter";

    public override Result<string> GetRequiredSingleValue()
    {
        return Core.Results.Result.Success(Core.Results.Result.Done).Then(_ =>
        {
            var value = Request.RouteValues[ParameterName];

            if (value == null)
            {
                return Error.Invalid($@"The {ParameterType} ""{ParameterName}"" is missing.");
            }

            var stringValue = Uri.UnescapeDataString(value.ToString() ?? "");

            return Core.Results.Result.Success(stringValue);
        });
    }

    public override Result<List<string>> GetRequiredMultiValue()
    {
        throw new InvalidOperationException("Path parameter validation does not support muliple values.");
    }

    public override Result<Optional<string>> GetOptionalSingleValue()
    {
        throw new InvalidOperationException("Path parameter validation does not support optional values.");
    }

    public override RequestParameterValidationBuilder<TNext> Next<TNext>(Result<TNext> nextResult)
    {
        return new RequestPathParameterValidationBuilder<TNext>(Request, ParameterName, nextResult);
    }
}