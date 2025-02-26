namespace ASP.Core.Optionality;

public static class OptionalExtensions
{
    public static async Task<TValue> GetValueOrDefault<TValue>(this Task<Optional<TValue>> optionTask, TValue defaultValue)
        where TValue : notnull
    {
        var option = await optionTask;

        return option.GetValueOrDefault(defaultValue);
    }

    public static async Task<Optional<TNextValue>> Map<TValue, TNextValue>(this Task<Optional<TValue>> optionTask, Func<TValue, TNextValue> mapFunction)
        where TValue : notnull
        where TNextValue : notnull
    {
        var option = await optionTask;

        return option.Map(mapFunction);
    }

    public static async Task<Optional<TNextValue>> Map<TValue, TNextValue>(this Task<Optional<TValue>> optionTask, Func<TValue, Task<TNextValue>> mapFunction)
       where TValue : notnull
       where TNextValue : notnull
    {
        var option = await optionTask;
        var result = await option.Map(mapFunction);

        return result;
    }

    public static async Task<Optional<TNextValue>> Then<TValue, TNextValue>(this Task<Optional<TValue>> optionTask, Func<TValue, Optional<TNextValue>> onSome)
        where TValue : notnull
        where TNextValue : notnull
    {
        var option = await optionTask;

        return option.Then(onSome);
    }

    public static async Task<Optional<TNextValue>> Then<TValue, TNextValue>(this Task<Optional<TValue>> optionTask, Func<TValue, Task<Optional<TNextValue>>> onSome)
        where TValue : notnull
        where TNextValue : notnull
    {
        var option = await optionTask;
        var result = await option.Then(onSome);

        return result;
    }

    public static async Task<TNextValue> Match<TValue, TNextValue>(this Task<Optional<TValue>> optionTask, Func<TValue, TNextValue> onSome, Func<TNextValue> onNone)
        where TValue : notnull
        where TNextValue : notnull
    {
        var option = await optionTask;

        return option.Match(onSome, onNone);
    }

    public static async Task<TNextValue> Match<TValue, TNextValue>(this Task<Optional<TValue>> optionTask, Func<TValue, Task<TNextValue>> onSome, Func<Task<TNextValue>> onNone)
        where TValue : notnull
        where TNextValue : notnull
    {
        var option = await optionTask;
        var result = await option.Match(onSome, onNone);

        return result;
    }

    public static async Task Switch<TValue>(this Task<Optional<TValue>> optionTask, Action<TValue> onSome, Action onNone)
        where TValue : notnull
    {
        var option = await optionTask;

        option.Switch(onSome, onNone);
    }

    public static async Task Switch<TValue>(this Task<Optional<TValue>> optionTask, Func<TValue, Task> onSome, Task onNone)
        where TValue : notnull
    {
        var option = await optionTask;
        await option.Switch(onSome, onNone);
    }

    public static async Task IfSome<TValue>(this Task<Optional<TValue>> optionTask, Action<TValue> actionIfSome)
        where TValue : notnull
    {
        var option = await optionTask;

        option.IfSome(actionIfSome);
    }

    public static async Task IfSome<TValue>(this Task<Optional<TValue>> optionTask, Func<TValue, Task> actionIfSome)
        where TValue : notnull
    {
        var option = await optionTask;

        await option.IfSome(actionIfSome);
    }

    public static async Task IfNone<TValue>(this Task<Optional<TValue>> optionTask, Action actionIfNone)
        where TValue : notnull
    {
        var option = await optionTask;

        option.IfNone(actionIfNone);
    }

    public static async Task IfNone<TValue>(this Task<Optional<TValue>> optionTask, Task actionIfNone)
        where TValue : notnull
    {
        var option = await optionTask;

        await option.IfNone(actionIfNone);
    }
}
