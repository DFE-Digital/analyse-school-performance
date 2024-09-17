namespace ASP.Core.Optionality;

public static class Optional
{
    public static Optional<TValue> FromNullable<TValue>(TValue? value)
    {
        return value is null
            ? Optional<TValue>.None
            : Optional<TValue>.Some(value);
    }
}

public abstract class Optional<TValue>
{
    public abstract TValue GetValueOrDefault(TValue defaultValue);

    public abstract bool HasValue { get; }

    public abstract Optional<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction);

    public abstract Task<Optional<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction);

    public abstract Optional<TNextValue> Then<TNextValue>(Func<TValue, Optional<TNextValue>> onSome);

    public abstract Task<Optional<TNextValue>> Then<TNextValue>(Func<TValue, Task<Optional<TNextValue>>> onSome);

    public abstract TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSome, Func<TNextValue> onNone);

    public abstract Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSome, Func<Task<TNextValue>> onNone);

    public abstract void Switch<TNextValue>(Action<TValue> onSome, Action onNone);

    public abstract Task Switch<TNextValue>(Func<TValue, Task> onSome, Task onNone);

    public abstract void IfSome(Action<TValue> actionIfSome);

    public abstract Task IfSome(Func<TValue, Task> actionIfSome);

    public abstract void IfNone(Action actionIfNone);

    public abstract Task IfNone(Task actionIfNone);

    public static Optional<TValue> Some(TValue value)
    {
        return new Some<TValue>(value);
    }

    public static Optional<TValue> None => new None<TValue>();
}
