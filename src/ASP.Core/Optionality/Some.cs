namespace ASP.Core.Optionality;

public class Some<TValue> : Optional<TValue>
    where TValue : notnull
{
    public TValue Value { get; }

    public Some(TValue value)
    {
        Value = value;
    }

    public override TValue GetValueOrDefault(TValue defaultValue)
        => Value;

    public override bool HasValue 
        => true;

    public override Optional<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        => new Some<TNextValue>(mapFunction(Value));

    public override async Task<Optional<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
    {
        var value = await mapFunction(Value);

        return new Some<TNextValue>(value);
    }

    public override Optional<TNextValue> Then<TNextValue>(Func<TValue, Optional<TNextValue>> onSome)
        => onSome(Value);

    public override async Task<Optional<TNextValue>> Then<TNextValue>(Func<TValue, Task<Optional<TNextValue>>> onSome)
        => await onSome(Value);

    public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSome, Func<TNextValue> onNone)
        => onSome(Value);

    public override async Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSome, Func<Task<TNextValue>> onNone)
        => await onSome(Value);

    public override void Switch(Action<TValue> onSome, Action onNone)
        => onSome(Value);

    public override async Task Switch(Func<TValue, Task> onSome, Task onNone)
        => await onSome(Value);

    public override void IfSome(Action<TValue> actionIfSome)
        => actionIfSome(Value);

    public override async Task IfSome(Func<TValue, Task> actionIfSome)
        => await actionIfSome(Value);

    public override void IfNone(Action actionIfNone)
    {
    }

    public override Task IfNone(Task actionIfNone)
        => Task.CompletedTask;

    protected override bool Equals(Optional<TValue> opt)
        => opt is Some<TValue> other && other.Value.Equals(Value);

    public override string ToString()
        => $"Some: {Value}";
}
