using ASP.Core.Optionality;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;

namespace ASP.Api;

public class RequestQueryStringParameterValidationBuilder<T> : RequestParameterValidationBuilder<T>
{
    public RequestQueryStringParameterValidationBuilder(HttpRequest request, string parameterName, Result<T> result)
        : base(request, parameterName, result)
    {
    }

    public override string ParameterType => "query parameter";

    public override Result<string> GetRequiredSingleValue()
    {
        return Core.Results.Result.Success(Core.Results.Result.Done).Then(_ =>
        {
            var value = Request.Query[ParameterName];

            if (value.Count > 1)
            {
                return Error.Invalid($@"The {ParameterType} ""{ParameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Error.Invalid($@"The {ParameterType} ""{ParameterName}"" is missing.");
            }

            var stringValue = value.ToString();

            return Core.Results.Result.Success(stringValue);
        });
    }

    public override Result<List<string>> GetRequiredMultiValue()
    {
        return Core.Results.Result.Success(Core.Results.Result.Done).Then(_ =>
        {
            var values = Request.Query[ParameterName];

            if (values.Count == 0)
            {
                return Error.Invalid($@"The {ParameterType} ""{ParameterName}"" is missing.");
            }

            var validValues = values
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v!)
                .ToList();

            return Core.Results.Result.Success(validValues);
        });
    }

    public override Result<Optional<string>> GetOptionalSingleValue()
    {
        return Core.Results.Result.Success(Core.Results.Result.Done).Then(_ =>
        {
            var value = Request.Query[ParameterName];

            if (value.Count > 1)
            {
                return Error.Invalid($@"The {ParameterType} ""{ParameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Core.Results.Result.Success<Optional<string>>(new None<string>());
            }

            var stringValue = value.ToString();

            return Core.Results.Result.Success<Optional<string>>(new Some<string>(stringValue));
        });
    }

    public override RequestParameterValidationBuilder<TNext> Next<TNext>(Result<TNext> nextResult)
    {
        return new RequestQueryStringParameterValidationBuilder<TNext>(Request, ParameterName, nextResult);
    }
}