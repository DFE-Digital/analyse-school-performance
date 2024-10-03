namespace ASP.Core.Optionality;

public class None<TValue> : Optional<TValue>
    where TValue : notnull
{
    public static readonly None<TValue> Instance = new None<TValue>();

    public override TValue GetValueOrDefault(TValue defaultValue)
        => defaultValue;

    public override bool HasValue
        => false;

    public override Optional<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        => Optional<TNextValue>.None;

    public override Task<Optional<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        => Task.FromResult(Optional<TNextValue>.None);

    public override Optional<TNextValue> Then<TNextValue>(Func<TValue, Optional<TNextValue>> onSome)
        => Optional<TNextValue>.None;

    public override Task<Optional<TNextValue>> Then<TNextValue>(Func<TValue, Task<Optional<TNextValue>>> onSome)
        => Task.FromResult(Optional<TNextValue>.None);

    public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSome, Func<TNextValue> onNone)
        => onNone();

    public override async Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSome, Func<Task<TNextValue>> onNone)
        => await onNone();

    public override void Switch(Action<TValue> onSome, Action onNone)
        => onNone();

    public override async Task Switch(Func<TValue, Task> onSome, Task onNone)
        => await onNone;

    public override void IfSome(Action<TValue> actionIfSome)
    { 
    }

    public override Task IfSome(Func<TValue, Task> actionIfSome)
        => Task.CompletedTask;

    public override void IfNone(Action actionIfNone)
        => actionIfNone();

    public override Task IfNone(Task actionIfNone)
        => actionIfNone;

    protected override bool Equals(Optional<TValue> opt)
        => opt is None<TValue>;

    public override string ToString()
        => "None";
}
