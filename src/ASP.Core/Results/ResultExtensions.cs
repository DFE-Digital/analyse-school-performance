namespace ASP.Core.Results
{
    public static class ResultExtensions
    {
        public static async Task<Result<TNextValue>> Map<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, TNextValue> mapFunction)
        {
            var result = await resultTask;

            var newResult = result.Map(mapFunction);

            return newResult;
        }

        public static async Task<Result<TNextValue>> MapAsync<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TNextValue>> mapFunction)
        {
            var result = await resultTask;

            var newResult = await result.MapAsync(mapFunction);

            return newResult;
        }

        public static async Task<Result<TNextValue>> Then<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Result<TNextValue>> onSuccess)
        {
            var result = await resultTask;

            var newResult = result.Then(onSuccess);

            return newResult;
        }

        public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            var result = await resultTask;

            var newResult = await result.ThenAsync(onSuccess);

            return newResult;
        }

        public static async Task<TNextValue> Match<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
        {
            var result = await resultTask;

            var newResult = result.Match(onSuccess, onError);

            return newResult;
        }

        public static async Task<TNextValue> MatchAsync<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
        {
            var result = await resultTask;

            var newResult = await result.MatchAsync(onSuccess, onError);

            return newResult;
        }

        public static async Task Switch<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> onSuccess, Action<Error> onError)
        {
            var result = await resultTask;

            result.Switch(onSuccess, onError);
        }

        public static async Task SwitchAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> onSuccess, Func<Error, Task> onError)
        {
            var result = await resultTask;

            await result.SwitchAsync(onSuccess, onError);
        }

        public static Result<TValue> ToResult<TValue>(this TValue value)
        {
            return Result.Success(value);
        }

        public static Result<IEnumerable<TValue>> Combine<TValue>(this IEnumerable<Result<TValue>> results)
        {
            var error = results.OfType<ErrorResult<TValue>>().FirstOrDefault();

            if(error != null)
            {
                return error.Error;
            }

            var values = results.OfType<SuccessResult<TValue>>().Select(r => r.Value);

            return (Result<IEnumerable<TValue>>) values;
        }
    }
}
