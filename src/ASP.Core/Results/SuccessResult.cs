namespace ASP.Core.Results
{
    public class SuccessResult<TValue> : Result<TValue>
    {
        public override TValue Value { get; }

        public SuccessResult(TValue value)
        {
            Value = value;
        }
        
        public override bool IsSuccess => true;
        public override bool IsError => false;

        public override Result<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        {
            return new SuccessResult<TNextValue>(mapFunction(Value));
        }

        public override async Task<Result<TNextValue>> MapAsync<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        {
            var nextResult = await mapFunction(Value);

            return new SuccessResult<TNextValue>(nextResult);
        }

        public override Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
        {
            return onSuccess(Value);
        }

        public override Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            return onSuccess(Value);
        }

        public override Result<TValue> MapError(Func<Error, Error> onError)
        {
            return this;
        }

        public override Task<Result<TValue>> MapErrorAsync(Func<Error, Task<Error>> onError)
        {
            return Task.FromResult((Result<TValue>)this);
        }

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
        {
            return onSuccess(Value);
        }

        public override Task<TNextValue> MatchAsync<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
        {
            return onSuccess(Value);
        }

        public override void Switch(Action<TValue> onSuccess, Action<Error> onError)
        {
            onSuccess(Value);
        }

        public override Task SwitchAsync(Func<TValue, Task> onSuccess, Func<Error, Task> onError)
        {
            return onSuccess(Value);
        }
    }
}
