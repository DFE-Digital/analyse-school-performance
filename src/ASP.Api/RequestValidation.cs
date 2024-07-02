using ASP.Core.Helpers;
using ASP.Core.Results;
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
                return Error.Invalid($@"The parameter ""{parameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" is missing.");
            }

            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" should not be empty.");
            }

            return stringValue;
        }

        public static Result<Maybe<string>> OptionalParameter(HttpRequest req, string parameterName)
        {
            var value = req.Query[parameterName];

            if (value.Count > 1)
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Maybe<string>.None;
            }

            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" should not be empty.");
            }

            return Maybe<string>.Some(stringValue);
        }

        public static async Task<Result<TBody>> RequiredBodyAsync<TBody>(HttpRequest req)
        {
            using (var sr = new StreamReader(req.Body))
            {
                var content = await sr.ReadToEndAsync();
                if (content.Length == 0)
                    return Error.Invalid("The request body is missing.");

                return JsonHelper.DeserializeIgnoringMissingMembers<TBody>(content)
                    .MapError(e => Error.Invalid("The request body is not a JSON object."));
            }
        }

        public static int GetNumericParameterOrDefault(Maybe<string> inputValue, int defaultValue)
        {
            var stringValue = inputValue.ToNullable();

            // Attempt to parse the parameter, use default value if parsing fails
            if (int.TryParse(stringValue, out int parsedValue))
            {
                return parsedValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static Result<Maybe<string>> NumericParameter(Maybe<string> maybeValue, string parameterName)
        {
            var stringValue = maybeValue.ToNullable();
            return ValidateNumericParameter(stringValue, parameterName);
        }

        public static Result<Maybe<string>> NumericParameter(string value, string parameterName)
        {
            return ValidateNumericParameter(value, parameterName);
        }

        private static Result<Maybe<string>> ValidateNumericParameter(string? value, string parameterName)
        {
            if (value == null) return Maybe<string>.None;

            if (int.TryParse(value, out int intValue))
            {
                if (intValue > 1)
                {
                    return Maybe<string>.Some(value);
                }
                else
                {
                    return Error.Invalid(
                        $@"Bad request: parameter ""{parameterName}"" should be a whole number greater than 1.");
                }
            }
            else
            {
                return Error.Invalid(
                    $@"Bad request: parameter ""{parameterName}"" should be a whole number greater than 1.");
            }
        }
    }
}