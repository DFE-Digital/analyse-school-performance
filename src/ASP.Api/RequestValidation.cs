using ASP.Core.Helpers;
using ASP.Core.Results;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;

namespace ASP.Api
{
    public static class RequestValidation
    {
        public static Result<Done> RequiredHttpMethod(HttpRequest req, string[] allowedMethods)
        {
            if (!allowedMethods.Contains(req.Method))
            {
                return new MethodNotAllowedError(req.Method, allowedMethods);
            }

            return Result.Done;
        }

        public static Result<string> RequiredParameter(HttpRequest req, string parameterName)
        {
            var value = req.Query[parameterName];

            if (value.Count > 1)
            {
                return Error.Validation($@"The parameter ""{parameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Error.Validation($@"The parameter ""{parameterName}"" is missing.");
            }

            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Validation($@"The parameter ""{parameterName}"" should not be empty.");
            }

            return stringValue;
        }

        public static Result<Maybe<string>> OptionalParameter(HttpRequest req, string parameterName)
        {
            var value = req.Query[parameterName];

            if (value.Count > 1)
            {
                return Error.Validation($@"The parameter ""{parameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Maybe<string>.None;
            }

            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Validation($@"The parameter ""{parameterName}"" should not be empty.");
            }

            return Maybe<string>.Some(stringValue);
        }

        public static async Task<Result<TBody>> RequiredBodyAsync<TBody>(HttpRequest req)
        {
            using (var sr = new StreamReader(req.Body))
            {
                var content = await sr.ReadToEndAsync();
                if (content.Length == 0)
                    return Error.Validation("The request body is missing.");

                return JsonHelper.DeserializeIgnoringMissingMembers<TBody>(content)
                    .MapError(e => Error.Validation("The request body is not a JSON object."));
            }
        }
    }
}