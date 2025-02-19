using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Api;

public static class RequestRouteParameterValidationBuilderExtensions
{
    public static RequestRouteParameterValidationBuilder<string> IsRequired(this RequestRouteParameterValidationBuilder<Done> builder)
    {
        Result<string> result = builder.Result.Then(_ =>
        {
            var value = builder.Request.RouteValues[builder.ParameterName];

            if (value == null)
            {
                return Error.Invalid($@"The route parameter ""{builder.ParameterName}"" is missing.");
            }

            var stringValue = Uri.UnescapeDataString(value.ToString() ?? "");

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Invalid($@"The route parameter ""{builder.ParameterName}"" should not be empty.");
            }

            return Result.Success(stringValue);
        });

        return new RequestRouteParameterValidationBuilder<string>(builder.Request, builder.ParameterName, result);
    }

    public static RequestRouteParameterValidationBuilder<TEnum> IsEnum<TEnum>(this RequestRouteParameterValidationBuilder<string> builder)
        where TEnum : struct, Enum
    {
        Result<TEnum> result = builder.Result.Then(value => ValidateEnum<TEnum>(value, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<TEnum>(builder.Request, builder.ParameterName, result);
    }

    public static RequestRouteParameterValidationBuilder<Optional<TEnum>> IsEnum<TEnum>(this RequestRouteParameterValidationBuilder<Optional<string>> builder)
        where TEnum : struct, Enum
    {
        Result<Optional<TEnum>> result = builder.Result.Then(value => ValidateEnum<TEnum>(value, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<Optional<TEnum>>(builder.Request, builder.ParameterName, result);
    }

    public static RequestRouteParameterValidationBuilder<int> IsNumeric(this RequestRouteParameterValidationBuilder<string> builder)
    {
        Result<int> result = builder.Result.Then(value => ValidateNumeric(value, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<int>(builder.Request, builder.ParameterName, result);
    }

    public static RequestRouteParameterValidationBuilder<Optional<int>> IsNumeric(this RequestRouteParameterValidationBuilder<Optional<string>> builder)
    {
        Result<Optional<int>> result = builder.Result.Then(value => ValidateNumeric(value, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<Optional<int>>(builder.Request, builder.ParameterName, result);
    }

    public static RequestRouteParameterValidationBuilder<string> HasLength(this RequestRouteParameterValidationBuilder<string> builder, int requiredLength)
    {
        Result<string> result = builder.Result.Then(value => ValidateLength(value, requiredLength, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<string>(builder.Request, builder.ParameterName, result);
    }

    public static RequestRouteParameterValidationBuilder<Optional<string>> HasLength(this RequestRouteParameterValidationBuilder<Optional<string>> builder, int requiredLength)
    {
        Result<Optional<string>> result = builder.Result.Then(value => ValidateLength(value, requiredLength, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<Optional<string>>(builder.Request, builder.ParameterName, result);
    }
    
    public static RequestRouteParameterValidationBuilder<string> IsDigits(this RequestRouteParameterValidationBuilder<string> builder)
    {
        Result<string> result = builder.Result.Then(value => ValidateDigits(value, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<string>(builder.Request, builder.ParameterName, result);
    }

    public static RequestRouteParameterValidationBuilder<Optional<string>> IsDigits(this RequestRouteParameterValidationBuilder<Optional<string>> builder)
    {
        Result<Optional<string>> result = builder.Result.Then(value => ValidateDigits(value, builder.ParameterName));

        return new RequestRouteParameterValidationBuilder<Optional<string>>(builder.Request, builder.ParameterName, result);
    }

    private static Result<string> ValidateLength(this string value, int requiredLength, string parameterName)
    {
        return value.Length == requiredLength
            ? Result.Success(value)
            : Error.Invalid($@"The route parameter ""{parameterName}"" must be exactly {requiredLength} characters long.");
    }
    
    private static Result<string> ValidateDigits(this string value, string parameterName)
    {
        return value.All(char.IsDigit)
            ? Result.Success(value)
            : Error.Invalid($@"The route parameter ""{parameterName}"" must contain only digits.");
    }

    private static Result<TEnum> ValidateEnum<TEnum>(this string value, string parameterName)
        where TEnum : struct, Enum
    {
        return Enum.TryParse(value, true, out TEnum result)
                // Check if the parsed value is defined in the enum
                && Enum.IsDefined(typeof(TEnum), result)
            ? Result.Success(result)
            : Error.Invalid($@"""{value}"" is not a valid {parameterName}.");
    }

    private static Result<int> ValidateNumeric(this string value, string parameterName)
    {
        return int.TryParse(value, out int intValue) && intValue >= 1
            ? Result.Success(intValue)
            : Error.Invalid($@"The route parameter ""{parameterName}"" should be a whole number greater than or equal to 1.");
    }
}