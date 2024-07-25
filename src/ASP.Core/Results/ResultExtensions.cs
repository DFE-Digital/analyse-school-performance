namespace ASP.Core.Results
{
    public static class ResultExtensions
    {
        public static async Task<Result<TNextValue>> Map<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, TNextValue> mapFunction)
        {
            var result = await resultTask;

            return result.Map(mapFunction);
        }

        public static async Task<Result<TNextValue>> Map<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TNextValue>> mapFunction)
        {
            var result = await resultTask;

            return await result.Map(mapFunction);
        }

        public static async Task<Result<TValue>> MapError<TValue>(this Task<Result<TValue>> resultTask, Func<Error, Error> mapFunction)
        {
            var result = await resultTask;

            return result.MapError(mapFunction);
        }

        public static async Task<Result<TValue>> MapError<TValue>(this Task<Result<TValue>> resultTask, Func<Error, Task<Error>> mapFunction)
        {
            var result = await resultTask;

            return await result.MapError(mapFunction);
        }

        public static async Task<Result<TValue>> MapErrorMessage<TValue>(this Task<Result<TValue>> resultTask, Func<string, string> mapFunction)
        {
            var result = await resultTask;

            return result.MapErrorMessage(mapFunction);
        }

        public static async Task<Result<TValue>> MapErrorMessage<TValue>(this Task<Result<TValue>> resultTask, Func<string, Task<string>> mapFunction)
        {
            var result = await resultTask;

            return await result.MapErrorMessage(mapFunction);
        }

        public static async Task<Result<TNextValue>> Then<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Result<TNextValue>> onSuccess)
        {
            var result = await resultTask;

            return result.Then(onSuccess);
        }

        public static async Task<Result<TNextValue>> Then<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            var result = await resultTask;

            return await result.Then(onSuccess);
        }

        public static async Task<TNextValue> Match<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, TNextValue> onSuccess, Func<Error, TNextValue> onError)
        {
            var result = await resultTask;

            return result.Match(onSuccess, onError);
        }

        public static async Task<TNextValue> Match<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TNextValue>> onSuccess, Func<Error, Task<TNextValue>> onError)
        {
            var result = await resultTask;

            return await result.Match(onSuccess, onError);
        }

        public static async Task Switch<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> onSuccess, Action<Error> onError)
        {
            var result = await resultTask;

            result.Switch(onSuccess, onError);
        }

        public static async Task Switch<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> onSuccess, Func<Error, Task> onError)
        {
            var result = await resultTask;

            await result.Switch(onSuccess, onError);
        }

        public static async Task<Result<TValue>> OnSuccess<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> onSuccess)
        {
            var result = await resultTask;

            return result.OnSuccess(onSuccess);
        }

        public static async Task<Result<TValue>> OnSuccess<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> onSuccess)
        {
            var result = await resultTask;

            return await result.OnSuccess(onSuccess);
        }

        public static async Task<Result<TValue>> OnError<TValue>(this Task<Result<TValue>> resultTask, Action<Error> onError)
        {
            var result = await resultTask;

            return result.OnError(onError);
        }

        public static async Task<Result<TValue>> OnError<TValue>(this Task<Result<TValue>> resultTask, Func<Error, Task> onError)
        {
            var result = await resultTask;

            return await result.OnError(onError);
        }

        public static async Task<TValue> GetValueOrDefault<TValue>(this Task<Result<TValue>> resultTask, TValue defaultValue)
        {
            var result = await resultTask;

            return result.GetValueOrDefault(defaultValue);
        }

        public static async Task<Result<TValue>> DefaultIfError<TValue>(this Task<Result<TValue>> resultTask, TValue defaultValue)
        {
            var result = await resultTask;

            return result.DefaultIfError(defaultValue);
        }

        public static async Task<Result<TValue>> DefaultIf<TValue>(this Task<Result<TValue>> resultTask, Func<Error, bool> predicate, TValue defaultValue)
        {
            var result = await resultTask;

            return result.DefaultIf(predicate, defaultValue);
        }

        public static async Task<Result<TValue>> ErrorIf<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Error error)
        {
            var result = await resultTask;

            return result.ErrorIf(predicate, error);
        }

        public static async Task<Result<TNewValue>> Combine<TValue, TOtherValue, TNewValue>(this Task<Result<TValue>> resultTask, Result<TOtherValue> otherResult, Func<TValue, TOtherValue, TNewValue> combineFunction)
        {
            var result = await resultTask;

            return result.Combine(otherResult, combineFunction);
        }

        public static async Task<Result<TNewValue>> Combine<TValue, TValue1, TValue2, TNewValue>(this Task<Result<TValue>> resultTask, Result<TValue1> result1, Result<TValue2> result2, Func<TValue, TValue1, TValue2, TNewValue> combineFunction)
        {
            var result = await resultTask;

            return result.Combine(result1, result2, combineFunction);
        }

        public static async Task<Result<TNewValue>> Combine<TValue, TValue1, TValue2, TValue3, TNewValue>(this Task<Result<TValue>> resultTask, Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Func<TValue, TValue1, TValue2, TValue3, TNewValue> combineFunction)
        {
            var result = await resultTask;

            return result.Combine(result1, result2, result3, combineFunction);
        }

        public static async Task<Result<TNewValue>> Combine<TValue, TValue1, TValue2, TValue3, TValue4, TNewValue>(this Task<Result<TValue>> resultTask, Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Result<TValue4> result4, Func<TValue, TValue1, TValue2, TValue3, TValue4, TNewValue> combineFunction)
        {
            var result = await resultTask;

            return result.Combine(result1, result2, result3, result4, combineFunction);
        }

        public static async Task<Result<TNextValue>> Convert<TValue, TNextValue>(this Task<Result<TValue>> resultTask, Func<TValue, Result<TNextValue>> onSuccess, Func<Error, Result<TNextValue>> onError)
        {
            var result = await resultTask;

            return result.Convert(onSuccess, onError);
        }

        public static Result<TValue> ToResult<TValue>(this TValue value)
        {
            return Result.Success(value);
        }

        public static Result<IEnumerable<TValue>> Combine<TValue>(this IEnumerable<Result<TValue>> results)
        {
            var error = results.OfType<ErrorResult<TValue>>().FirstOrDefault();

            if (error != null)
            {
                return error.Error;
            }

            var values = results.OfType<SuccessResult<TValue>>().Select(r => r.Value);

            return Result.Success(values);
        }
    }
}
