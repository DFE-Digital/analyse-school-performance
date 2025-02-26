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
            => new ErrorResult<TNextValue>(Error);

        public override Task<Result<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
            => Task.FromResult((Result<TNextValue>)new ErrorResult<TNextValue>(Error));

        public override Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
            => new ErrorResult<TNextValue>(Error);

        public override Task<Result<TNextValue>> Then<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess)
            => Task.FromResult((Result<TNextValue>)new ErrorResult<TNextValue>(Error));

        public override Result<TValue> MapError(Func<Error, Error> onError)
            => onError(Error);

        public override async Task<Result<TValue>> MapError(Func<Error, Task<Error>> onError)
            => await onError(Error);

        public override Result<TValue> MapErrorIf(Func<Error, bool> predicate, Error error)
            => predicate(Error) ? error : Error;

        public override async Task<Result<TValue>> MapErrorIf(Func<Error, Task<bool>> predicate, Error error)
        {
            if (await predicate(Error))
            {
                return error;
            }

            return Error;
        }

        public override Result<TValue> MapErrorIf(Func<Error, bool> predicate, Func<string, Error> errorFunction)
            => predicate(Error) ? errorFunction(Error.Message) : Error;

        public override async Task<Result<TValue>> MapErrorIf(Func<Error, Task<bool>> predicate, Func<string, Task<Error>> errorFunction)
        {
            if(await predicate(Error))
            {
                var error = await errorFunction(Error.Message);
                return error;
            }

            return Error;
        }

        public override Result<TValue> MapErrorMessage(Func<string, string> mapFunction)
            => new ErrorResult<TValue>(Error.MapMessage(mapFunction));

        public override async Task<Result<TValue>> MapErrorMessage(Func<string, Task<string>> mapFunction)
        {
            var error = await Error.MapMessage(mapFunction);
            return new ErrorResult<TValue>(error);
        }

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
            => onError(Error);

        public override Task<TNextValue> Match<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
            => onError(Error);

        public override void Switch(Action<TValue> onSuccess, Action<Error> onError)
            => onError(Error);

        public override Task Switch(Func<TValue, Task> onSuccess, Func<Error, Task> onError)
            => onError(Error);

        public override Result<TValue> OnSuccess(Action<TValue> onSuccess)
            => this;

        public override Task<Result<TValue>> OnSuccess(Func<TValue, Task> onSuccess)
            => Task.FromResult((Result<TValue>)this);

        public override Result<TValue> OnError(Action<Error> onError)
        {
            onError(Error);
            return this;
        }

        public override async Task<Result<TValue>> OnError(Func<Error, Task> onError)
        {
            await onError(Error);
            return this;
        }

        public override TValue GetValueOrDefault(TValue defaultValue)
            => defaultValue;

        public override SuccessResult<TValue> DefaultIfError(TValue defaultValue)
            => (SuccessResult<TValue>)defaultValue;

        public override Result<TValue> DefaultIf(Func<Error, bool> predicate, TValue defaultValue)
            => predicate(Error) ? defaultValue : this;

        public override Result<TValue> ErrorIf(Func<TValue, bool> predicate, Error error)
            => this;

        public override Result<TValue> IfErrorThen(Func<Error, bool> predicate, Func<Result<TValue>> onError)
        {
            if(predicate(Error))
            {
                return onError();
            }

            return this;
        }

        public override async Task<Result<TValue>> IfErrorThen(Func<Error, bool> predicate, Func<Task<Result<TValue>>> onError)
        {
            if (predicate(Error))
            {
                return await onError();
            }

            return this;
        }

        public override Result<TNextValue> Convert<TNextValue>(
            Func<TValue, Result<TNextValue>> onSuccess, 
            Func<Error, Result<TNextValue>> onError
        )   => onError(Error);

        public override async Task<Result<TNextValue>> Convert<TNextValue>(
            Func<TValue, Task<Result<TNextValue>>> onSuccess, 
            Func<Error, Task<Result<TNextValue>>> onError
        )   => await onError(Error);

        protected override bool Equals(Result<TValue> other)
            => other is ErrorResult<TValue> error && error.Error.Equals(Error);

        public override int GetHashCode()
            => HashCode.Combine(Error);

        public override string ToString()
            => $"Error: {Error}";
    }
}
