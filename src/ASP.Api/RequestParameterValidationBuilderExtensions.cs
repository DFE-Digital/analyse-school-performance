using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Api;

public static class RequestParameterValidationBuilderExtensions
{
    public static RequestParameterValidationBuilder<string> IsRequired(this RequestParameterValidationBuilder<Done> builder)
    {
        Result<string> result = builder.GetRequiredSingleValue();

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<List<string>> IsRequiredMultiParameter(this RequestParameterValidationBuilder<Done> builder)
    {
        Result<List<string>> result = builder.GetRequiredMultiValue();

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<Optional<string>> IsOptional(this RequestParameterValidationBuilder<Done> builder)
    {
        Result<Optional<string>> result = builder.GetOptionalSingleValue();

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<Optional<string>> IsRequiredIf(this RequestParameterValidationBuilder<Done> builder, bool required)
    {
        Result<Optional<string>> result = required
            ? IsRequired(builder).Result.Map(v => Optional<string>.Some(v))
            : IsOptional(builder).Result;

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<string> IsNotEmpty(this RequestParameterValidationBuilder<string> builder)
    {
        Result<string> result = builder.Result.Then(value => ValidateNotEmpty(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<Optional<string>> IsNotEmpty(this RequestParameterValidationBuilder<Optional<string>> builder)
    {
        Result<Optional<string>> result = builder.Result.Then(value => ValidateNotEmpty(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<List<string>> IsNotEmpty(this RequestParameterValidationBuilder<List<string>> builder)
    {
        Result<List<string>> result = builder.Result.Then(value => ValidateNotEmpty(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<TEnum> IsEnum<TEnum>(this RequestParameterValidationBuilder<string> builder)
        where TEnum : struct, Enum
    {
        Result<TEnum> result = builder.Result.Then(value => ValidateEnum<TEnum>(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<Optional<TEnum>> IsEnum<TEnum>(this RequestParameterValidationBuilder<Optional<string>> builder)
        where TEnum : struct, Enum
    {
        Result<Optional<TEnum>> result = builder.Result.Then(value => ValidateEnum<TEnum>(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<int> IsNumeric(this RequestParameterValidationBuilder<string> builder)
    {
        Result<int> result = builder.Result.Then(value => ValidateNumeric(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<Optional<int>> IsNumeric(this RequestParameterValidationBuilder<Optional<string>> builder)
    {
        Result<Optional<int>> result = builder.Result.Then(value => ValidateNumeric(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<string> HasLength(this RequestParameterValidationBuilder<string> builder, int requiredLength)
    {
        Result<string> result = builder.Result.Then(value => ValidateLength(value, requiredLength, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<Optional<string>> HasLength(this RequestParameterValidationBuilder<Optional<string>> builder, int requiredLength)
    {
        Result<Optional<string>> result = builder.Result.Then(value => ValidateLength(value, requiredLength, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<string> IsDigits(this RequestParameterValidationBuilder<string> builder)
    {
        Result<string> result = builder.Result.Then(value => ValidateDigits(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    public static RequestParameterValidationBuilder<Optional<string>> IsDigits(this RequestParameterValidationBuilder<Optional<string>> builder)
    {
        Result<Optional<string>> result = builder.Result.Then(value => ValidateDigits(value, builder.ParameterType, builder.ParameterName));

        return builder.Next(result);
    }

    private static Result<string> ValidateNotEmpty(this string value, string parameterType, string parameterName)
    {
        return value.Length > 0
            ? Result.Success(value)
            : Error.Invalid($@"The {parameterType} ""{parameterName}"" should not be empty.");
    }

    private static Result<List<string>> ValidateNotEmpty(this List<string> value, string parameterType, string parameterName)
    {
        return value.Count > 0
            ? Result.Success(value)
            : Error.Invalid($@"The {parameterType} ""{parameterName}"" should not be empty.");
    }

    private static Result<string> ValidateLength(this string value, int requiredLength, string parameterType, string parameterName)
    {
        return value.Length == requiredLength
            ? Result.Success(value)
            : Error.Invalid($@"The {parameterType} ""{parameterName}"" must be exactly {requiredLength} characters long.");
    }
    
    private static Result<string> ValidateDigits(this string value, string parameterType, string parameterName)
    {
        return value.All(char.IsDigit)
            ? Result.Success(value)
            : Error.Invalid($@"The {parameterType} ""{parameterName}"" must contain only digits.");
    }

    private static Result<TEnum> ValidateEnum<TEnum>(this string value, string parameterType, string parameterName)
        where TEnum : struct, Enum
    {
        return Enum.TryParse(value, true, out TEnum result)
                // Check if the parsed value is defined in the enum
                && Enum.IsDefined(typeof(TEnum), result)
            ? Result.Success(result)
            : Error.Invalid($@"""{value}"" is not a valid {parameterName}.");
    }

    private static Result<int> ValidateNumeric(this string value, string parameterType, string parameterName)
    {
        return int.TryParse(value, out int intValue) && intValue >= 1
            ? Result.Success(intValue)
            : Error.Invalid($@"The {parameterType} ""{parameterName}"" should be a whole number greater than or equal to 1.");
    }
}