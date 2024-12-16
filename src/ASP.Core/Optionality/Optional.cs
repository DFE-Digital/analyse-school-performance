namespace ASP.Core.Optionality;

public static class Optional
{
    public static Optional<TValue> FromNullable<TValue>(TValue? value)
        where TValue : notnull
    {
        return value is null
            ? Optional<TValue>.None
            : Optional<TValue>.Some(value);
    }

    public static Optional<TValue> FromNullable<TValue>(TValue? value) where TValue : struct
    {
        return value.HasValue
            ? Optional<TValue>.Some(value.Value)
            : Optional<TValue>.None;
    }

}

public abstract class Optional<TValue>
    where TValue : notnull
{
    public abstract TValue GetValueOrDefault(TValue defaultValue);

    public abstract bool HasValue { get; }

    public abstract Optional<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        where TNextValue : notnull;

    public abstract Task<Optional<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        where TNextValue : notnull;

    public abstract Optional<TNextValue> Then<TNextValue>(Func<TValue, Optional<TNextValue>> onSome)
        where TNextValue : notnull;

    public abstract Task<Optional<TNextValue>> Then<TNextValue>(Func<TValue, Task<Optional<TNextValue>>> onSome)
        where TNextValue : notnull;

    public abstract TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSome, Func<TNextValue> onNone)
        where TNextValue : notnull;

    public abstract Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSome, Func<Task<TNextValue>> onNone)
        where TNextValue : notnull;

    public abstract void Switch(Action<TValue> onSome, Action onNone);

    public abstract Task Switch(Func<TValue, Task> onSome, Task onNone);

    public abstract void IfSome(Action<TValue> actionIfSome);

    public abstract Task IfSome(Func<TValue, Task> actionIfSome);

    public abstract void IfNone(Action actionIfNone);

    public abstract Task IfNone(Task actionIfNone);

    public abstract TValue? ToNullable();

    protected abstract bool Equals(Optional<TValue> other);

    public override bool Equals(object? other)
        => other is Optional<TValue> result && Equals(result);

    public override int GetHashCode()
        => base.GetHashCode();

    public override string? ToString()
        => base.ToString();

    public static Optional<TValue> Some(TValue value)
        => new Some<TValue>(value);

    public static Optional<TValue> None
        => None<TValue>.Instance;
}
