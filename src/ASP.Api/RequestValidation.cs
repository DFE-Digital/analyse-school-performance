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
        
        public static Result<TEnum> RequiredParameter<TEnum>(HttpRequest req, string parameterName) where TEnum : struct, Enum
        {
            var value = req.Query[parameterName];

            if (value.Count == 0)
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" is missing.");
            }

            if (value.Count > 1)
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" is duplicated.");
            }

            var stringValue = value.ToString().Trim();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" should not be empty.");
            }

            return ValidateEnumParameter<TEnum>(stringValue, parameterName);
        }
        
        public static Result<string> RequiredParameterIf(HttpRequest req, string parameterName, bool optional)
        {
            if (optional)
            {
                var value = req.Query[parameterName];
                return !string.IsNullOrWhiteSpace(value.ToString()) ? value.ToString() : string.Empty;
            }

            return RequiredParameter(req, parameterName);
        }

        public static Result<string?> OptionalParameter(HttpRequest req, string parameterName)
        {
            var value = req.Query[parameterName];

            if (value.Count > 1)
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Result.Success<string?>(null);
            }

            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" should not be empty.");
            }

            return stringValue;
        }

        public static async Task<Result<TBody>> RequiredBodyAsync<TBody>(HttpRequest req) where TBody : notnull
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

        public static Result<int?> NumericParameter(string? value, string parameterName)
        {
            if(value is null)
            {
                return Result.Success<int?>(null);
            }

            if (int.TryParse(value, out int intValue))
            {
                if (intValue >= 1)
                {
                    return intValue;
                }
                else
                {
                    return Error.Invalid($@"The parameter ""{parameterName}"" should be a whole number greater than or equal to 1.");
                }
            }
            else
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" should be a whole number greater than or equal to 1.");
            }
        }

        public static Result<string> RequiresParameterLengthToMatch(string value, string parameterName, int requiredLength)
        {
            if (value.Length != requiredLength)
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" must be exactly {requiredLength} characters long.");
            }

            return value;
        }


        public static Result<string> RequiresParameterToBeDigits(string value, string parameterName)
        {
            if (!value.All(char.IsDigit))
            {
                return Error.Invalid($@"The parameter ""{parameterName}"" must contain only digits.");
            }

            return value;
        }
        
        private static Result<TEnum> ValidateEnumParameter<TEnum>(string value, string parameterName) where TEnum : struct, Enum
        {
            if (Enum.TryParse<TEnum>(value, true, out TEnum result))
            {
                // Check if the parsed value is defined in the enum
                return Enum.IsDefined(typeof(TEnum), result)
                    ? result
                    : Error.Invalid($@"""{value}"" is not a valid ""{parameterName}"".");
            }

            return Error.Invalid($@"""{value}"" is not a valid ""{parameterName}"".");
        }
    }
}