using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Api;

public static class RequestQueryStringParameterValidationBuilderExtensions
{
    public static RequestQueryStringParameterValidationBuilder<string> IsRequired(this RequestQueryStringParameterValidationBuilder<Done> builder)
    {
        Result<string> result = builder.Result.Then(_ =>
        {
            var value = builder.Request.Query[builder.ParameterName];

            if (value.Count > 1)
            {
                return Error.Invalid($@"The query parameter ""{builder.ParameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Error.Invalid($@"The query parameter ""{builder.ParameterName}"" is missing.");
            }

            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Invalid($@"The query parameter ""{builder.ParameterName}"" should not be empty.");
            }

            return Result.Success(stringValue);
        });

        return new RequestQueryStringParameterValidationBuilder<string>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<List<string>> IsRequiredMultiParameter(this RequestQueryStringParameterValidationBuilder<Done> builder)
    {
        Result<List<string>> result = builder.Result.Then(_ =>
        {
            var values = builder.Request.Query[builder.ParameterName];

            if (values.Count == 0)
            {
                return Error.Invalid($@"The query parameter ""{builder.ParameterName}"" is missing.");
            }

            var validValues = values
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v!)
                .ToList();

            if (validValues.Count == 0)
            {
                return Error.Invalid($@"The query parameter ""{builder.ParameterName}"" should not be empty.");
            }

            return Result.Success(validValues);
        });

        return new RequestQueryStringParameterValidationBuilder<List<string>>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<Optional<string>> IsOptional(this RequestQueryStringParameterValidationBuilder<Done> builder)
    {
        Result<Optional<string>> result = builder.Result.Then(_ =>
        {
            var value = builder.Request.Query[builder.ParameterName];

            if (value.Count > 1)
            {
                return Error.Invalid($@"The query parameter ""{builder.ParameterName}"" is duplicated.");
            }

            if (value.Count == 0)
            {
                return Result.Success<Optional<string>>(new None<string>());
            }

            var stringValue = value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Error.Invalid($@"The query parameter ""{builder.ParameterName}"" should not be empty.");
            }

            return Result.Success<Optional<string>>(new Some<string>(stringValue));
        });

        return new RequestQueryStringParameterValidationBuilder<Optional<string>>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<Optional<string>> IsRequiredIf(this RequestQueryStringParameterValidationBuilder<Done> builder, bool required)
    {
        Result<Optional<string>> result = required
            ? IsRequired(builder).Result.Map(v => Optional<string>.Some(v))
            : IsOptional(builder).Result;

        return new RequestQueryStringParameterValidationBuilder<Optional<string>>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<TEnum> IsEnum<TEnum>(this RequestQueryStringParameterValidationBuilder<string> builder)
        where TEnum : struct, Enum
    {
        Result<TEnum> result = builder.Result.Then(value => ValidateEnum<TEnum>(value, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<TEnum>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<Optional<TEnum>> IsEnum<TEnum>(this RequestQueryStringParameterValidationBuilder<Optional<string>> builder)
        where TEnum : struct, Enum
    {
        Result<Optional<TEnum>> result = builder.Result.Then(value => ValidateEnum<TEnum>(value, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<Optional<TEnum>>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<int> IsNumeric(this RequestQueryStringParameterValidationBuilder<string> builder)
    {
        Result<int> result = builder.Result.Then(value => ValidateNumeric(value, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<int>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<Optional<int>> IsNumeric(this RequestQueryStringParameterValidationBuilder<Optional<string>> builder)
    {
        Result<Optional<int>> result = builder.Result.Then(value => ValidateNumeric(value, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<Optional<int>>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<string> HasLength(this RequestQueryStringParameterValidationBuilder<string> builder, int requiredLength)
    {
        Result<string> result = builder.Result.Then(value => ValidateLength(value, requiredLength, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<string>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<Optional<string>> HasLength(this RequestQueryStringParameterValidationBuilder<Optional<string>> builder, int requiredLength)
    {
        Result<Optional<string>> result = builder.Result.Then(value => ValidateLength(value, requiredLength, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<Optional<string>>(builder.Request, builder.ParameterName, result);
    }
    
    public static RequestQueryStringParameterValidationBuilder<string> IsDigits(this RequestQueryStringParameterValidationBuilder<string> builder)
    {
        Result<string> result = builder.Result.Then(value => ValidateDigits(value, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<string>(builder.Request, builder.ParameterName, result);
    }

    public static RequestQueryStringParameterValidationBuilder<Optional<string>> IsDigits(this RequestQueryStringParameterValidationBuilder<Optional<string>> builder)
    {
        Result<Optional<string>> result = builder.Result.Then(value => ValidateDigits(value, builder.ParameterName));

        return new RequestQueryStringParameterValidationBuilder<Optional<string>>(builder.Request, builder.ParameterName, result);
    }

    private static Result<string> ValidateLength(this string value, int requiredLength, string parameterName)
    {
        return value.Length == requiredLength
            ? Result.Success(value)
            : Error.Invalid($@"The query parameter ""{parameterName}"" must be exactly {requiredLength} characters long.");
    }
    
    private static Result<string> ValidateDigits(this string value, string parameterName)
    {
        return value.All(char.IsDigit)
            ? Result.Success(value)
            : Error.Invalid($@"The query parameter ""{parameterName}"" must contain only digits.");
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
            : Error.Invalid($@"The query parameter ""{parameterName}"" should be a whole number greater than or equal to 1.");
    }
}