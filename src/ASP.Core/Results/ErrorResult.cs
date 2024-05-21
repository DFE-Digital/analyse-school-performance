namespace ASP.Core.Results
{
    /// <summary>
    /// Represents the failed result of an operation. See <see cref="Result{TValue}"/>
    /// </summary>
    /// <typeparam name="TValue">Type of the successful result of the operation</typeparam>
    public sealed class ErrorResult<TValue> : Result<TValue>
    {
        public Error Error { get; }

        public ErrorResult(Error error)
        {
            Error = error;
        }
        
        public override Result<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)
        {
            return new ErrorResult<TNextValue>(Error);
        }

        public override Task<Result<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        {
            return Task.FromResult((Result<TNextValue>)new ErrorResult<TNextValue>(Error));
        }

        public override Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
        {
            return new ErrorResult<TNextValue>(Error);
        }

        public override Task<Result<TNextValue>> Then<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            return Task.FromResult((Result<TNextValue>)new ErrorResult<TNextValue>(Error));
        }

        public override Result<TValue> MapError(Func<Error, Error> onError)
        {
            return onError(Error);
        }

        public override async Task<Result<TValue>> MapError(Func<Error, Task<Error>> onError)
        {
            var error = await onError(Error);

            return error;
        }

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
        {
            return onError(Error);
        }

        public override Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
        {
            return onError(Error);
        }

        public override void Switch(Action<TValue> onSuccess, Action<Error> onError)
        {
            onError(Error);
        }

        public override Task Switch(Func<TValue, Task> onSuccess, Func<Error, Task> onError)
        {
            return onError(Error);
        }

        public override TValue GetValueOrDefault(TValue defaultValue)
        {
            return defaultValue;
        }

        public override SuccessResult<TValue> DefaultIfError(TValue defaultValue)
        {
            return (SuccessResult<TValue>)defaultValue;
        }

        public override Result<TValue> DefaultIf(Func<Error, bool> predicate, TValue defaultValue)
        {
            return predicate(Error) ? defaultValue : this;
        }

        public override Result<TValue> ErrorIf(Func<TValue, bool> predicate, Error error)
        {
            return this;
        }

        public override Result<TNextValue> Convert<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess, Func<Error, Result<TNextValue>> onError)
        {
            return onError(Error);
        }
    }
}
