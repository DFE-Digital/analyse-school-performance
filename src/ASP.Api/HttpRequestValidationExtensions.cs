using ASP.Api.Client;
using ASP.Core.Results;
using ASP.Core.Text;
using Microsoft.AspNetCore.Http;

namespace ASP.Api;

public static class HttpRequestValidationExtensions
{
    public static Result<Done> ValidateHttpMethod(this HttpRequest req, string[] allowedMethods)
    {
        if (!allowedMethods.Contains(req.Method))
        {
            return new MethodNotAllowedError(req.Method, allowedMethods);
        }

        return Result.Done;
    }

    public static async Task<Result<TBody>> ValidateBodyAsync<TBody>(this HttpRequest req)
        where TBody : notnull
    {
        using (var sr = new StreamReader(req.Body))
        {
            var content = await sr.ReadToEndAsync();
            if (content.Length == 0)
                return Error.Invalid("The request body is missing.");

            return JsonHelper.DeserializeNotNull<TBody>(content, ignoreMissingMembers: true)
                .MapError(e => Error.Invalid("The request body is not a JSON object."));
        }
    }

    public static Result<T> ValidateQueryStringParameter<T>(this HttpRequest req, string parameterName, Func<RequestQueryStringParameterValidationBuilder<Done>, RequestQueryStringParameterValidationBuilder<T>> validate)
    {
        var builder = new RequestQueryStringParameterValidationBuilder<Done>(req, parameterName, Result.Done);

        return validate(builder).Result;
    }

    public static Result<T> ValidateRouteParameter<T>(this HttpRequest req, string parameterName, Func<RequestRouteParameterValidationBuilder<Done>, RequestRouteParameterValidationBuilder<T>> validate)
    {
        var builder = new RequestRouteParameterValidationBuilder<Done>(req, parameterName, Result.Done);

        return validate(builder).Result;
    }
} 