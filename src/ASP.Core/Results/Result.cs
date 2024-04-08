namespace ASP.Core.Results
{
    public static class Result
    {
        private static readonly Done _done = new Done();
        public static Done Done => _done;

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new SuccessResult<TValue>(value);
        }
    }

    public abstract class Result<TValue>
    {
        public abstract Result<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction);
        public abstract Task<Result<TNextValue>> MapAsync<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction);

        public abstract Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess);
        public abstract Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess);

        public abstract Result<TValue> MapError(Func<Error, Error> onError);
        public abstract Task<Result<TValue>> MapErrorAsync(Func<Error, Task<Error>> onError);

        public abstract TNextValue Match<TNextValue>(Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError);
        public abstract Task<TNextValue> MatchAsync<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError);

        public abstract void Switch(Action<TValue> onSuccess, Action<Error> onError);
        public abstract Task SwitchAsync(Func<TValue, Task> onSuccess, Func<Error, Task> onError);

        public static implicit operator Result<TValue>(TValue value)
        {
            return new SuccessResult<TValue>(value);
        }

        public static implicit operator Result<TValue>(Error error)
        {
            return new ErrorResult<TValue>(error);
        }
    }
}
