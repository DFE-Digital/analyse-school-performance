namespace ASP.Core.Results
{
    /// <summary>
    /// Represents the successful result of an operation. See <see cref="Result{TValue}"/>
    /// </summary>
    /// <typeparam name="TValue">Type of the successful result of the operation</typeparam>
    public sealed class SuccessResult<TValue> : Result<TValue>
    {
        public TValue Value { get; }

        public SuccessResult(TValue value)
        {
            Value = value;
        }

        public override Result<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        {
            return new SuccessResult<TNextValue>(mapFunction(Value));
        }

        public override async Task<Result<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        {
            var nextResult = await mapFunction(Value);

            return new SuccessResult<TNextValue>(nextResult);
        }

        public override Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
        {
            return onSuccess(Value);
        }

        public override Task<Result<TNextValue>> Then<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            return onSuccess(Value);
        }

        public override Result<TValue> MapError(Func<Error, Error> onError)
        {
            return this;
        }

        public override Task<Result<TValue>> MapError(Func<Error, Task<Error>> onError)
        {
            return Task.FromResult((Result<TValue>)this);
        }

        public override Result<TValue> MapErrorMessage(Func<string, string> mapFunction)
        {
            return this;
        }

        public override Task<Result<TValue>> MapErrorMessage(Func<string, Task<string>> mapFunction)
        {
            return Task.FromResult((Result<TValue>)this);
        }

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
        {
            return onSuccess(Value);
        }

        public override Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
        {
            return onSuccess(Value);
        }

        public override void Switch(Action<TValue> onSuccess, Action<Error> onError)
        {
            onSuccess(Value);
        }

        public override Task Switch(Func<TValue, Task> onSuccess, Func<Error, Task> onError)
        {
            return onSuccess(Value);
        }

        public override Result<TValue> OnSuccess(Action<TValue> onSuccess)
        {
            onSuccess(Value);
            return this;
        }

        public override async Task<Result<TValue>> OnSuccess(Func<TValue, Task> onSuccess)
        {
            await onSuccess(Value);
            return this;
        }

        public override Result<TValue> OnError(Action<Error> onError)
        {
            return this;
        }

        public override Task<Result<TValue>> OnError(Func<Error, Task> onError)
        {
            return Task.FromResult((Result<TValue>)this);
        }

        public override TValue GetValueOrDefault(TValue defaultValue)
        {
            return Value;
        }

        public override SuccessResult<TValue> DefaultIfError(TValue defaultValue)
        {
            return this;
        }

        public override Result<TValue> DefaultIf(Func<Error, bool> predicate, TValue defaultValue)
        {
            return this;
        }

        public override Result<TValue> ErrorIf(Func<TValue, bool> predicate, Error error)
        {
            return predicate(Value) ? error : this;
        }

        public override Result<TValue> IfErrorThen(Func<Error, bool> predicate, Func<Result<TValue>> onError)
        {
            return this;
        }

        public override Task<Result<TValue>> IfErrorThen(Func<Error, bool> predicate, Func<Task<Result<TValue>>> onError)
        {
            return Task.FromResult((Result<TValue>)this);
        }

        public override Result<TNextValue> Convert<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess, Func<Error, Result<TNextValue>> onError)
        {
            return onSuccess(Value);
        }

        public override async Task<Result<TNextValue>> Convert<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess, Func<Error, Task<Result<TNextValue>>> onError)
        {
            return await onSuccess(Value);
        }
    }
}
