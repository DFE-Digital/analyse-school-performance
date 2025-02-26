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
            => new SuccessResult<TNextValue>(mapFunction(Value));

        public override async Task<Result<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        {
            var nextResult = await mapFunction(Value);

            return new SuccessResult<TNextValue>(nextResult);
        }

        public override Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
            => onSuccess(Value);

        public override Task<Result<TNextValue>> Then<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess)
            => onSuccess(Value);

        public override Result<TValue> MapError(Func<Error, Error> onError)
            => this;

        public override Task<Result<TValue>> MapError(Func<Error, Task<Error>> onError)
            => Task.FromResult((Result<TValue>)this);

        public override Result<TValue> MapErrorIf(Func<Error, bool> predicate, Error error)
            => this;

        public override Task<Result<TValue>> MapErrorIf(Func<Error, Task<bool>> predicate, Error error)
            => Task.FromResult((Result<TValue>)this);

        public override Result<TValue> MapErrorIf(Func<Error, bool> predicate, Func<string, Error> errorFunction)
            => this;

        public override Task<Result<TValue>> MapErrorIf(Func<Error, Task<bool>> predicate, Func<string, Task<Error>> errorFunction)
            => Task.FromResult((Result<TValue>)this);

        public override Result<TValue> MapErrorMessage(Func<string, string> mapFunction)
            => this;

        public override Task<Result<TValue>> MapErrorMessage(Func<string, Task<string>> mapFunction)
            => Task.FromResult((Result<TValue>)this);

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
            => onSuccess(Value);

        public override Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
            => onSuccess(Value);

        public override void Switch(Action<TValue> onSuccess, Action<Error> onError)
            => onSuccess(Value);

        public override Task Switch(Func<TValue, Task> onSuccess, Func<Error, Task> onError)
            => onSuccess(Value);

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
            => this;

        public override Task<Result<TValue>> OnError(Func<Error, Task> onError)
            => Task.FromResult((Result<TValue>)this);

        public override TValue GetValueOrDefault(TValue defaultValue)
            => Value;

        public override SuccessResult<TValue> DefaultIfError(TValue defaultValue)
            => this;

        public override Result<TValue> DefaultIf(Func<Error, bool> predicate, TValue defaultValue)
            => this;

        public override Result<TValue> ErrorIf(Func<TValue, bool> predicate, Error error)
            => predicate(Value) ? error : this;

        public override Result<TValue> IfErrorThen(Func<Error, bool> predicate, Func<Result<TValue>> onError)
            => this;

        public override Task<Result<TValue>> IfErrorThen(Func<Error, bool> predicate, Func<Task<Result<TValue>>> onError)
            => Task.FromResult((Result<TValue>)this);

        public override Result<TNextValue> Convert<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess, Func<Error, Result<TNextValue>> onError)
            => onSuccess(Value);

        public override async Task<Result<TNextValue>> Convert<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess, Func<Error, Task<Result<TNextValue>>> onError)
            => await onSuccess(Value);

        protected override bool Equals(Result<TValue> obj)
            => obj is SuccessResult<TValue> other && (
                Value is null && other.Value is null ||
                Value is not null && Value.Equals(other.Value)
            );

        public override int GetHashCode()
            => HashCode.Combine(Value);

        public override string ToString()
            => $"Success: {Value}";
    }
}