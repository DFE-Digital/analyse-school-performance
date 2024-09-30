namespace ASP.Core.Optionality
{
    public class None<TValue> : Optional<TValue>
    {
        public override TValue GetValueOrDefault(TValue defaultValue)
        {
            return defaultValue;
        }

        public override bool HasValue => false;

        public override Optional<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        {
            return Optional<TNextValue>.None;
        }

        public override Task<Optional<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        {
            return Task.FromResult(Optional<TNextValue>.None);
        }

        public override Optional<TNextValue> Then<TNextValue>(Func<TValue, Optional<TNextValue>> onSome)
        {
            return Optional<TNextValue>.None;
        }

        public override Task<Optional<TNextValue>> Then<TNextValue>(Func<TValue, Task<Optional<TNextValue>>> onSome)
        {
            return Task.FromResult(Optional<TNextValue>.None);
        }

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSome, Func<TNextValue> onNone)
        {
            return onNone();
        }

        public override async Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSome, Func<Task<TNextValue>> onNone)
        {
            return await onNone();
        }

        public override void Switch(Action<TValue> onSome, Action onNone)
        {
            onNone();
        }

        public override async Task Switch(Func<TValue, Task> onSome, Task onNone)
        {
            await onNone;
        }

        public override void IfSome(Action<TValue> actionIfSome)
        {
        }

        public override Task IfSome(Func<TValue, Task> actionIfSome)
        {
            return Task.CompletedTask;
        }

        public override void IfNone(Action actionIfNone)
        {
            actionIfNone();
        }

        public override Task IfNone(Task actionIfNone)
        {
            return actionIfNone;
        }
    }
}
