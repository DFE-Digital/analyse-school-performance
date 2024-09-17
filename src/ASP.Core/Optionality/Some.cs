namespace ASP.Core.Optionality
{
    public class Some<TValue> : Optional<TValue>
    {
        public TValue Value { get; }

        public Some(TValue value)
        {
            Value = value;
        }

        public override TValue GetValueOrDefault(TValue defaultValue)
        {
            return Value;
        }

        public override bool HasValue => true;

        public override Optional<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        {
            return new Some<TNextValue>(mapFunction(Value));
        }

        public override async Task<Optional<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        {
            var value = await mapFunction(Value);

            return new Some<TNextValue>(value);
        }

        public override Optional<TNextValue> Then<TNextValue>(Func<TValue, Optional<TNextValue>> onSome)
        {
            return onSome(Value);
        }

        public override async Task<Optional<TNextValue>> Then<TNextValue>(Func<TValue, Task<Optional<TNextValue>>> onSome)
        {
            return await onSome(Value);
        }

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSome, Func<TNextValue> onNone)
        {
            return onSome(Value);
        }

        public override async Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSome, Func<Task<TNextValue>> onNone)
        {
            return await onSome(Value);
        }

        public override void Switch<TNextValue>(Action<TValue> onSome, Action onNone)
        {
            onSome(Value);
        }

        public override async Task Switch<TNextValue>(Func<TValue, Task> onSome, Task onNone)
        {
            await onSome(Value);
        }

        public override void IfSome(Action<TValue> actionIfSome)
        {
            actionIfSome(Value);
        }

        public override async Task IfSome(Func<TValue, Task> actionIfSome)
        {
            await actionIfSome(Value);
        }

        public override void IfNone(Action actionIfNone)
        {
        }

        public override Task IfNone(Task actionIfNone)
        {
            return Task.CompletedTask;
        }
    }
}
