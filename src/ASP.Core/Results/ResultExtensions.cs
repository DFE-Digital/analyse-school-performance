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
        
       
        /// <summary>
        /// Attempts to combine two results of different types into a single result of a new type,
        /// using a specified combining function. If both results are successful and non-null,
        /// it combines their values using the provided function and returns a successful Result.
        /// If either result is an error or contains a null value, it returns an error Result with appropriate error details.
        /// </summary>
        /// <param name="result1">The first result to combine. It must be a successful result of type T1 for the combination to proceed.</param>
        /// <param name="result2">The second result to combine. It must be a successful result of type T2 for the combination to proceed.</param>
        /// <param name="combineFunc">A function that defines how to combine the values of the two results if both are successful.
        /// This function must handle possible null values if T1 or T2 are nullable types.</param>
        /// <typeparam name="T1">The type of the value contained in the first result.</typeparam>
        /// <typeparam name="T2">The type of the value contained in the second result.</typeparam>
        /// <typeparam name="TOut">The type of the value to be contained in the resulting combined result.</typeparam>
        /// <returns>A Result of type TOut which can either be a success result containing the combined value,
        /// or an error result if either input result is an error or if any input value is null. The error result contains an error message specifying the issue.</returns>
        /// <remarks>
        /// This method first checks the success state of each input result:
        /// - If both results are successful, it checks for non-null values and then uses the provided combineFunc to combine these values.
        /// - If either result is in an error state or contains a null value, the error from the erroneous result is propagated in a new Result of type TOut.
        /// </remarks>
        /// <example>
        /// Example usage:
        /// <code>
        /// var result1 = Result.Success(5);
        /// var result2 = Result.Success(3);
        /// var combinedResult = result1.Combine(result2, (a, b) => a + b);
        /// // combinedResult would now be a successful Result containing 8
        /// </code>
        /// </example>
        public static Result<TOut> Combine<T1, T2, TOut>(
            this Result<T1> result1, Result<T2> result2, Func<T1, T2, TOut> combineFunc)
        {
            if (result1.IsSuccess && result2.IsSuccess)
            {
                // Check for null values before invoking combineFunc
                if (result1.Value != null && result2.Value != null)
                {
                    return Result.Success(combineFunc(result1.Value, result2.Value));
                }
                else
                {
                    // Handle the case where either value is null, potentially returning an error result
                    var error = Error.NotFound($"One of the input values {nameof(result1)} or {nameof(result2)} is null.");
                    return new ErrorResult<TOut>(error);
                }
            }
            else if (result1.IsError)
            {
                return ConvertError<T1, TOut>(result1);
            }
            else
            {
                return ConvertError<T2, TOut>(result2);
            }
        }

        /// <summary>
        /// Converts an error result from one type to another, preserving the error details while changing the result type.
        /// This method is useful for propagating errors across different layers or types in an application.
        /// </summary>
        /// <param name="result">The result to convert, which must be an error result of type TIn.</param>
        /// <typeparam name="TIn">The type of the input result's value, which should be an error.</typeparam>
        /// <typeparam name="TOut">The type of the output result's value. This type indicates the type of the new result that will be returned with the same error.</typeparam>
        /// <returns>Returns a new error result of type TOut. If the input is not an error result,
        /// the method returns a default value of TOut, which is null for reference types or the default for value types.</returns>
        /// <remarks>
        /// The method checks if the provided result is an error result (i.e., an instance of ErrorResult<TIn>):
        /// - If true, it constructs a new ErrorResult<TOut> using the error from the input, thus preserving the error details while changing the result's value type.
        /// - If the input result is not an error result, this method defaults to returning a default value of TOut.
        /// If TOut is a non-nullable type and default values are not acceptable, modifications may be necessary to handle this scenario appropriately.
        /// </remarks>
        private static Result<TOut> ConvertError<TIn, TOut>(Result<TIn> result)
        {
            if (result is ErrorResult<TIn> errorResult)
            {
                return new ErrorResult<TOut>(errorResult.Error);
            }

            // Consider what to return if TOut is a non-nullable type
            return default!; // or handle differently if default(TOut) is not acceptable
        }

    }
}
