namespace ASP.Core.Results
{
    public class ErrorResult<TValue> : Result<TValue>
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

        public override Task<Result<TNextValue>> MapAsync<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)
        {
            return Task.FromResult((Result<TNextValue>)new ErrorResult<TNextValue>(Error));
        }

        public override Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
        {
            return new ErrorResult<TNextValue>(Error);
        }

        public override Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            return Task.FromResult((Result<TNextValue>)new ErrorResult<TNextValue>(Error));
        }

        public override Result<TValue> MapError(Func<Error, Error> onError)
        {
            return onError(Error);
        }

        public override async Task<Result<TValue>> MapErrorAsync(Func<Error, Task<Error>> onError)
        {
            var error = await onError(Error);

            return error;
        }

        public override TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
        {
            return onError(Error);
        }

        public override Task<TNextValue> MatchAsync<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
        {
            return onError(Error);
        }

        public override void Switch(Action<TValue> onSuccess, Action<Error> onError)
        {
            onError(Error);
        }

        public override Task SwitchAsync(Func<TValue, Task> onSuccess, Func<Error, Task> onError)
        {
            return onError(Error);
        }
    }
}
