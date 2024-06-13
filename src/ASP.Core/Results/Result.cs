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

        public static Result<TValue> NotFound<TValue>(string message)
        {
            return Results.Error.NotFound(message);
        }

        public static Result<TValue> Unexpected<TValue>(string message)
        {
            return Results.Error.Unexpected(message);
        }

        public static Result<TValue> Validation<TValue>(string message)
        {
            return Results.Error.Validation(message);
        }

        public static Result<TValue> NotAllowed<TValue>(string message)
        {
            return Results.Error.NotAllowed(message);
        }

        public static Result<TValue> Error<TValue>(Error error)
        {
            return new ErrorResult<TValue>(error);
        }
    }

    /// <summary>
    /// Represents the result of an operation that could fail. Derived classes represent the possible outcomes: <c>SuccessResult&lt;TValue&gt;</c> indicates
    /// the operation was successful and contains the value of the result of type <typeparamref name="TValue"></typeparam>. <c>ErrorResult&lt;TValue&gt;</c>
    /// indicates an error and contains an <c>Error</c> object
    /// </summary>
    /// <typeparam name="TValue">Type of the successful result of the operation</typeparam>
    public abstract class Result<TValue>
    {
        /// <summary>
        /// Converts the current result from a <c>Result&lt;<typeparamref name="TValue"/>&gt;</c> to a 
        /// <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c>, using the map function provided to convert the result value from a 
        /// <typeparamref name="TValue"/> to a <typeparamref name="TNextValue"/> if the result is a <c>SuccessResult</c>,
        /// otherwise changes the type while preserving the error. For example:
        /// <example>
        /// <code>
        /// var a = Result.Success(3)
        ///     .Map(v => v + 1);
        ///     
        /// var b = Result.NotFound&lt;int&gt;("Not found")
        ///     .Map(v => v + 1); 
        /// </code>
        /// results in <c>a</c> having the value <c>4</c> and <c>b</c> having the error <c>NotFoundError</c>: <c>"Not found"</c>.
        /// </example>
        /// </summary>
        /// <typeparam name="TNextValue">Type of the result value once converted</typeparam>
        /// <param name="mapFunction">Function to convert the result value if the current result is a <c>SuccessResult</c></param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c></returns>
        public abstract Result<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction);

        /// <summary>
        /// Asynchronously converts the current result from a <c>Result&lt;<typeparamref name="TValue"/>&gt;</c> to a 
        /// <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c>, using the map function provided to convert the result value from a 
        /// <typeparamref name="TValue"/> to a <typeparamref name="TNextValue"/> if the current result is a <c>SuccessResult</c>,
        /// otherwise changes the type while preserving the error. For example:
        /// <example>
        /// <code>
        /// async Task&lt;int&gt; wait1SecThenAdd1(int v) 
        /// {
        ///     await Task.Delay(1000);
        ///     return v + 1;
        /// }
        /// 
        /// var a = await Result.Success(3)
        ///     .Map(wait1SecThenAdd1);
        ///     
        /// var b = await Result.NotFound&lt;int&gt;("Not found")
        ///     .Map(wait1SecThenAdd1);
        /// </code>
        /// results in <c>a</c> having the value <c>4</c> and <c>b</c> having the error <c>NotFoundError</c>: <c>"Not found"</c>.
        /// </example>
        /// </summary>
        /// <typeparam name="TNextValue">Type of the result value once converted</typeparam>
        /// <param name="mapFunction">Asynchronous function to convert the result value if the current result is a <c>SuccessResult</c></param>
        /// <returns>The task object representing the asynchronous operation returning a <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c></returns>
        public abstract Task<Result<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction);

        /// <summary>
        /// If the current result is an <c>ErrorResult</c>, uses the map function provided to change the error, otherwise returns the current 
        /// result unchanged. For example:
        /// <example>
        /// <code>
        /// Func&lt;Error, Error&gt; changeError = e => Error.Unexpected("ERROR: " + e.Message);
        ///
        /// var a = Result.Success(3)
        ///     .MapError(changeError);
        ///     
        /// var b = Result.NotFound&lt;int&gt;("Not found")
        ///     .MapError(changeError);
        /// </code>
        /// results in <c>a</c> having the value <c>3</c> and <c>b</c> having the error <c>UnexpectedError</c>: <c>"ERROR: Not found"</c>.
        /// </example>
        /// </summary>
        /// <param name="mapFunction">Function to convert the error if the current result is an <c>ErrorResult</c></param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TValue"/>&gt;</c></returns>
        public abstract Result<TValue> MapError(Func<Error, Error> mapFunction);

        /// <summary>
        /// If the current result is an <c>ErrorResult</c>, asynchronously uses the map function provided to change the error, otherwise returns the 
        /// current result unchanged. For example:
        /// <example>
        /// <code>
        /// async Task&lt;Error&gt; wait1SecThenChangeError(Error e) 
        /// { 
        ///     await Task.Delay(1000); 
        ///     return Error.Unexpected("ERROR: " + e.Message); 
        /// }
        /// 
        /// var a = await Result.Success(3)
        ///     .MapError(wait1SecThenChangeError);
        ///     
        /// var b = await Result.NotFound&lt;int&gt;("Not found")
        ///     .MapError(wait1SecThenChangeError);
        /// </code>
        /// results in <c>a</c> having the value <c>3</c> and <c>b</c> having the error <c>UnexpectedError</c>: <c>"ERROR: Not found"</c>.
        /// </example>
        /// </summary>
        /// <param name="mapFunction">Asynchronous function to convert the error if the current result is an <c>ErrorResult</c></param>
        /// <returns>The task object representing the asynchronous operation returning a <c>Result&lt;<typeparamref name="TValue"/>&gt;</c></returns>
        public abstract Task<Result<TValue>> MapError(Func<Error, Task<Error>> mapFunction);

        /// <summary>
        /// Converts the current result from a <c>Result&lt;<typeparamref name="TValue"/>&gt;</c> to a <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c>, 
        /// using the provided function. Unlike <c>Map</c> this function returns another result rather than just a value. This allows a pipeline to be 
        /// created out of many operations, each of which might fail, producing an <c>ErrorResult</c> if any of the operations failed, or a <c>SuccessResult</c>
        /// if all the operations succeeded. For example:
        /// <example>
        /// <code>
        /// Result&lt;double&gt; parseDouble(string str) =>
        ///     double.TryParse(str, out var result) ? result : Error.Unexpected($"Could not parse value \"{str}\" as double!");
        ///    
        /// Result&lt;double&gt; divide1By(double x) =>
        ///     x == 0 ? Error.Unexpected("Division by zero") : 1.0 / x;
        /// 
        /// var a = Result.Success("2")
        ///     .Then(parseDouble)
        ///     .Then(x => divide1By(x));
        ///     
        /// var b = Result.Success("0")
        ///     .Then(parseDouble)
        ///     .Then(x => divide1By(x));
        ///     
        /// var c = Result.Success("XYZ")
        ///     .Then(parseDouble)
        ///     .Then(x => divide1By(x));
        ///     
        /// var d = Result.NotFound&lt;string&gt;("Not found")
        ///     .Then(parseDouble)
        ///     .Then(x => divide1By(x));
        /// </code>
        /// results in <c>a</c> having the value <c>0.5</c>, <c>b</c> having the error <c>UnexpectedError</c>: <c>"Could not parse value \"XYZ\" as double!"</c>, 
        /// <c>c</c> having the error <c>UnexpectedError</c>: <c>"Division by zero!"</c>, and <c>d</c> having the error <c>NotFoundError</c>: <c>"Not found"</c>.
        /// </example>
        /// </summary>
        /// <typeparam name="TNextValue">Type of the result value of the <c>onSuccess</c> operation</typeparam>
        /// <param name="onSuccess">Operation to perform with the value of the current result (if current result is a <c>SuccessResult</c>)</param>
        /// <returns>The result of the <c>onSuccess</c> function</returns>
        public abstract Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess);

        /// <summary>
        /// Asynchronously converts the current result from a <c>Result&lt;<typeparamref name="TValue"/>&gt;</c> to a <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c>, 
        /// using the provided function. Unlike <c>Map</c> this function returns another result rather than just a value. This allows a pipeline to be 
        /// created out of many operations, each of which might fail, producing an <c>ErrorResult</c> if any of the operations failed, or a <c>SuccessResult</c>
        /// if all the operations succeeded. For example:
        /// <example>
        /// <code>
        /// async Task&lt;Result&lt;double&gt;&gt; wait1SecThenParseDouble(string str)
        /// {
        ///     await Task.Delay(1000);
        ///     return double.TryParse(str, out var result) ? result : Error.Unexpected($"Could not parse value \"{str}\" as double!");
        /// }
        ///    
        /// async Task&lt;Result&lt;double&gt;&gt; wait2SecsThenDivide1By(double x)
        /// {
        ///     await Task.Delay(2000);
        ///     return x == 0 ? Error.Unexpected("Division by zero") : 1.0 / x;
        /// }
        ///    
        /// var a = await Result.Success("2")
        ///     .Then(wait1SecThenParseDouble)
        ///     .Then(x => wait2SecsThenDivide1By(x));
        ///      
        /// var b = await Result.Success("0")
        ///     .Then(wait1SecThenParseDouble)
        ///     .Then(x => wait2SecsThenDivide1By(x));
        ///  
        /// var c = await Result.Success("XYZ")
        ///     .Then(wait1SecThenParseDouble)
        ///     .Then(x => wait2SecsThenDivide1By(x));
        /// 
        /// var d = await Result.NotFound&lt;string&gt;("Not found")
        ///     .Then(wait1SecThenParseDouble)
        ///     .Then(x => wait2SecsThenDivide1By(x));
        /// </code>
        /// results in <c>a</c> having the value <c>0.5</c>, <c>b</c> having the error <c>UnexpectedError</c>: <c>"Could not parse value \"XYZ\" as double!"</c>, 
        /// <c>c</c> having the error <c>UnexpectedError</c>: <c>"Division by zero!"</c>, and <c>d</c> having the error <c>NotFoundError</c>: <c>"Not found"</c>.
        /// </example>
        /// </summary>
        /// <typeparam name="TNextValue">Type of the result value of the <c>onSuccess</c> operation</typeparam>
        /// <param name="onSuccess">Asynchronous operation to perform with the value of the current result (if current result is a <c>SuccessResult</c>)</param>
        /// <returns>The task object representing the asynchronous operation returning the result of the <c>onSuccess</c> function</returns>
        public abstract Task<Result<TNextValue>> Then<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess);

        /// <summary>
        /// Converts the current result from a <c>Result&lt;<typeparamref name="TValue"/>&gt;</c> to a value of type <typeparamref name="TReturnValue"/>, by way
        /// of the <paramref name="onSuccess"/> and <paramref name="onError"/> functions. This is useful if the success/error state of the result needs to be 
        /// converted into another representation (such as at an application boundary). For example:
        /// <example>
        /// <code>
        /// string toString(Result&lt;double&gt; result) => result.Match(
        ///     value => value.ToString(),
        ///     error => $"{error.GetType().Name.ToUpper()}: {error.Message}"
        /// );
        ///    
        /// var a = toString(Result.Success(2.0));
        ///      
        /// var b = toString(Result.NotFound&lt;double&gt;("Not found"));
        ///
        /// var c = toString(Result.Unexpected&lt;double&gt;("Unexpected"));
        /// </code>
        /// results in <c>a</c> having the value <c>"2.0"</c>, <c>b</c> having the value <c>"NOTFOUNDERROR: Not found"</c> and 
        /// <c>c</c> having the value <c>"UNEXPECTEDERROR: Unexpected"</c>.
        /// </example>
        /// <example>
        /// <code>
        /// public class Response 
        /// {
        ///     public int StatusCode { get; set; }
        ///     public string Body { get; set; }
        /// }
        /// 
        /// Response toResponse(Result&lt;double&gt; result) => result.Match(
        ///     value => new Response { StatusCode = 200, Body = value.ToString() },
        ///     error => {
        ///         int statusCode = error switch {
        ///             ValidationError _ => 400,
        ///             NotFoundError _ => 404,
        ///             _ => 500
        ///         };
        ///         return new Response { StatusCode = statusCode, Body = error.Message };
        ///     }
        /// );
        ///    
        /// var a = toResponse(Result.Success(2.0));
        ///      
        /// var b = toResponse(Result.NotFound&lt;double&gt;("Not found"));
        ///
        /// var c = toResponse(Result.Unexpected&lt;double&gt;("Unexpected"));
        /// </code>
        /// results in <c>a</c> having the value <c>{ StatusCode: 200, Body: "2.0" }</c>, <c>b</c> having the value <c>{ StatusCode: 404, Body: "Not found" }</c> and 
        /// <c>c</c> having the value <c>{ StatusCode: 500, Body: "Unexpected" }</c>.
        /// </example>
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the value the current result will be converted into</typeparam>
        /// <param name="onSuccess">Function to convert a <c>SuccessResult</c> to the return value</param>
        /// <param name="onError">Function to convert an <c>ErrorResult</c> to the return value</param>
        /// <returns>An value of type <typeparamref name="TReturnValue"/></returns>
        public abstract TReturnValue Match<TReturnValue>(Func<TValue, TReturnValue> onSuccess, Func<Error, TReturnValue> onError);

        /// <summary>
        /// Asynchronously converts the current result from a <c>Result&lt;<typeparamref name="TValue"/>&gt;</c> to a value of type <typeparamref name="TReturnValue"/>, by way
        /// of the <paramref name="onSuccess"/> and <paramref name="onError"/> functions. This is useful if the success/error state of the result needs to be 
        /// converted into another representation (such as at an application boundary). For example:
        /// <example>
        /// <code>
        /// async Task&lt;string&gt; waitThenToString(int delayMs, Result&lt;double&gt; result) => await result.Match(
        ///     async value => { await Task.Delay(delayMs); return value.ToString(); },
        ///     async error => { await Task.Delay(delayMs); return $"{error.GetType().Name.ToUpper()}: {error.Message}"; }
        /// );
        /// 
        /// var a = await waitThenToString(100, Result.Success(2.0));
        ///      
        /// var b = await waitThenToString(1000, Result.NotFound&lt;double&gt;("Not found"));
        ///
        /// var c = await waitThenToString(2000, Result.Unexpected&lt;double&gt;("Unexpected"));
        /// </code>
        /// results (eventually) in <c>a</c> having the value <c>"2.0"</c>, <c>b</c> having the value <c>"NOTFOUNDERROR: Not found"</c> and 
        /// <c>c</c> having the value <c>"UNEXPECTEDERROR: Unexpected"</c>.
        /// </example>
        /// <example>
        /// <code>
        /// public class Response 
        /// {
        ///     public int StatusCode { get; set; }
        ///     public string Body { get; set; }
        /// }
        /// 
        /// async Task&lt;Response&gt; waitThenToResponse(int delayMs, Result&lt;double&gt; result) => result.Match(
        ///     async value => { 
        ///         await Task.Delay(delayMs); 
        ///         return new Response { StatusCode = 200, Body = value.ToString() }; 
        ///     },
        ///     async error => {
        ///         await Task.Delay(delayMs); 
        ///         int statusCode = error switch {
        ///             ValidationError _ => 400,
        ///             NotFoundError _ => 404,
        ///             _ => 500
        ///         };
        ///         return new Response { StatusCode = statusCode, Body = error.Message };
        ///     }
        /// );
        ///    
        /// var a = waitThenToResponse(100, Result.Success(2.0));
        ///      
        /// var b = waitThenToResponse(200, Result.NotFound&lt;double&gt;("Not found"));
        ///
        /// var c = waitThenToResponse(300, Result.Unexpected&lt;double&gt;("Unexpected"));
        /// </code>
        /// results (eventually) in <c>a</c> having the value <c>{ StatusCode: 200, Body: "2.0" }</c>, <c>b</c> having the value <c>{ StatusCode: 404, Body: "Not found" }</c> 
        /// and <c>c</c> having the value <c>{ StatusCode: 500, Body: "Unexpected" }</c>.
        /// </example>
        /// </summary>
        /// <typeparam name="TReturnValue">The type of the value the current result will be converted into</typeparam>
        /// <param name="onSuccess">Asynchronous function to convert a <c>SuccessResult</c> to the return value</param>
        /// <param name="onError">Asynchronous function to convert an <c>ErrorResult</c> to the return value</param>
        /// <returns>The task object representing the asynchronous operation returning a value of type <typeparamref name="TReturnValue"/></returns>
        public abstract Task<TReturnValue> Match<TReturnValue>(Func<TValue, Task<TReturnValue>> onSuccess, Func<Error, Task<TReturnValue>> onError);

        /// <summary>
        /// Carries out an action based on the current result, by way of the <paramref name="onSuccess"/> and <paramref name="onError"/> functions. 
        /// This is useful if the success/error state of the result needs to be handled directly by the application (such as at an application boundary). 
        /// For example:
        /// <example>
        /// <code>
        /// void handleResult(Result&lt;double&gt; result) => result.Switch(
        ///     value => Console.WriteLine(value),
        ///     error => {
        ///         if(error is NotFoundError) {
        ///             Console.WriteLine(error.Message);
        ///         } else {
        ///             throw new ApplicationException("An error occurred: " + error.Message);
        ///         }
        ///     }
        /// );
        ///    
        /// handleResult(Result.Success(2.0));
        ///      
        /// handleResult(Result.NotFound&lt;double&gt;("Not found"));
        ///
        /// handleResult(Result.Unexpected&lt;double&gt;("Unexpected"));
        /// </code>
        /// results in the following being written to the console: 
        /// <code>
        /// 2.0
        /// Not found
        /// </code>
        /// and then an <c>ApplicationException</c> being thrown with message <c>"An error occurred: Unexpected"</c>
        /// </example>
        /// </summary>
        /// <param name="onSuccess">Action to execute if current result is a <c>SuccessResult</c></param>
        /// <param name="onError">Action to execute if current result is an <c>ErrorResult</c></param>
        public abstract void Switch(Action<TValue> onSuccess, Action<Error> onError);

        /// <summary>
        /// Carries out an action based on the current result, by way of the <paramref name="onSuccess"/> and <paramref name="onError"/> functions. 
        /// This is useful if the success/error state of the result needs to be handled directly by the application (such as at an application boundary). 
        /// For example:
        /// <example>
        /// <code>
        /// async Task waitThenHandleResult(int delayMs, Result&lt;double&gt; result) => await result.Switch(
        ///     async value => { 
        ///         await Task.Delay(delayMs); 
        ///         Console.WriteLine(value); 
        ///     },
        ///     async error => {
        ///         await Task.Delay(delayMs); 
        ///         if(error is NotFoundError) {
        ///             Console.WriteLine(error.Message);
        ///         } else {
        ///             throw new ApplicationException("An error occurred: " + error.Message);
        ///         }
        ///     }
        /// );
        ///    
        /// await waitThenHandleResult(200, Result.Success(2.0));
        ///      
        /// await waitThenHandleResult(100, Result.NotFound&lt;double&gt;("Not found"));
        ///
        /// await waitThenHandleResult(300, handleResult(Result.Unexpected&lt;double&gt;("Unexpected"));
        /// </code>
        /// results in the following being written to the console: 
        /// <code>
        /// Not found
        /// 2.0
        /// </code>
        /// and then an <c>ApplicationException</c> being thrown with message <c>"An error occurred: Unexpected"</c>
        /// </example>
        /// </summary>
        /// <param name="onSuccess">Asynchronous action to execute if current result is a <c>SuccessResult</c></param>
        /// <param name="onError">Asynchronous action to execute if current result is an <c>ErrorResult</c></param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public abstract Task Switch(Func<TValue, Task> onSuccess, Func<Error, Task> onError);

        /// <summary>
        /// Gets the value of the current result if it is a <c>SuccessResult</c>, or the <paramref name="defaultValue"/> provided
        /// if it is an <c>ErrorResult</c>
        /// For example:
        /// <example>
        /// <code>
        /// var a = Result.Success(2).GetValueOrDefault(123);
        /// var b = Result.NotFound("Not found").GetValueOrDefault(123);
        /// </code>
        /// results in <c>a</c> having the value <c>2</c> and <c>b</c> having the value <c>123</c>.
        /// </example>
        /// </summary>
        /// <param name="defaultValue">Value to return if the current result is an <c>ErrorResult</c></param>
        /// <returns>The value of the current result if it is a <c>SuccessResult</c>, or <paramref name="defaultValue"/>
        /// if it is an <c>ErrorResult</c></returns>
        public abstract TValue GetValueOrDefault(TValue defaultValue);

        /// <summary>
        /// If the current result is an <c>ErrorResult</c>, returns a <c>SuccessResult</c> with the provided <paramref name="defaultValue"/> as its
        /// value, otherwise returns the current result unchanged. This ensures the result returned is always a <c>SuccessResult</c>.
        /// For example:
        /// <example>
        /// <code>
        /// var a = Result.Success(2).DefaultIfError(123);
        /// var b = Result.NotFound("Not found").DefaultIfError(123);
        /// </code>
        /// results in <c>a</c> being a <c>SuccessResult</c> with value <c>2</c> and <c>b</c> being a <c>SuccessResult</c> with value <c>123</c>.
        /// </example>
        /// </summary>
        /// <param name="defaultValue">Value to use if the current result is an <c>ErrorResult</c></param>
        /// <returns>A result object of type <c>SuccessResult&lt;<typeparamref name="TValue"/>&gt;</c></returns>
        public abstract SuccessResult<TValue> DefaultIfError(TValue defaultValue);

        /// <summary>
        /// If the current result is an <c>ErrorResult</c> and its <c>Error</c> matches the <paramref name="predicate"/> provided, returns a <c>SuccessResult</c> 
        /// with the provided <paramref name="defaultValue"/> as its <c>Value</c>, otherwise returns the current result unchanged.
        /// For example:
        /// <example>
        /// <code>
        /// var a = Result.Success(2).DefaultIf(e => e is NotFoundError, 123);
        /// var b = Result.NotFound("Not found").DefaultIf(e => e is NotFoundError, 123);
        /// var b = Result.Unexpected("Unexpected").DefaultIf(e => e is NotFoundError, 123);
        /// </code>
        /// results in <c>a</c> being a <c>SuccessResult</c> with value <c>2</c>, <c>b</c> being a <c>SuccessResult</c> with value <c>123</c>, and <c>c</c> being
        /// an <c>ErrorResult</c> with error <c>UnexpectedError</c>: <c>"Unexpected"</c>.
        /// </example>
        /// </summary>
        /// <param name="predicate">Predicate which should evaluates to true if the current result is an <c>ErrorResult</c> and should be converted to a 
        /// <c>SuccessResult</c> using the provided <paramref name="defaultValue"/></param>
        /// <param name="defaultValue">Value to use if the current result is an <c>ErrorResult</c> and its <c>Error</c> matches the <paramref name="predicate"/></param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TValue"/>&gt;</c></returns>
        public abstract Result<TValue> DefaultIf(Func<Error, bool> predicate, TValue defaultValue);

        /// <summary>
        /// If the current result is a <c>SuccessResult</c> and its <c>Value</c> matches the <paramref name="predicate"/> provided, returns an <c>ErrorResult</c> 
        /// with the provided <paramref name="error"/> as its <c>Error</c>, otherwise returns the current result unchanged.
        /// For example:
        /// <example>
        /// <code>
        /// var a = Result.Success(2).ErrorIf(v => v > 10, Error.Unexpected("Value was greater than 10!"));
        /// var b = Result.Success(22).ErrorIf(v => v > 10, Error.Unexpected("Value was greater than 10!"));
        /// </code>
        /// results in <c>a</c> being a <c>SuccessResult</c> with value <c>2</c> and <c>b</c> being an <c>ErrorResult</c> with error <c>UnexpectedError</c>: <c>"Value was greater than 10!"</c>.
        /// </example>
        /// </summary>
        /// </summary>
        /// <param name="predicate">Predicate which should evaluates to true if the current result is a <c>SuccessResult</c> and should be converted to an 
        /// <c>ErrorResult</c> using the provided <paramref name="error"/></param>
        /// <param name="error">Error object to use if the current result is a <c>SuccessResult</c> and its <c>Value</c> matches the <paramref name="predicate"/></param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TValue"/>&gt;</c></returns>
        public abstract Result<TValue> ErrorIf(Func<TValue, bool> predicate, Error error);

        /// <summary>
        /// Combines the current result with another result using the <paramref name="combineFunction"/> provided to create a new value from the values of both results if both are <c>SuccessResult</c>s.
        /// </summary>
        /// <typeparam name="TOtherValue">Type of the result's value to combine with the current result</typeparam>
        /// <typeparam name="TNewValue">Type of the new result created by using the <paramref name="combineFunction"/> on the values of both results</typeparam>
        /// <param name="otherResult">Result to combine with the current result</param>
        /// <param name="combineFunction">Function to use to combine the values of both results</param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TNewValue"/>&gt;</c></returns>
        public Result<TNewValue> Combine<TOtherValue, TNewValue>(Result<TOtherValue> otherResult, Func<TValue, TOtherValue, TNewValue> combineFunction)
        {
            return this.Then(value => 
                otherResult.Map(otherValue => combineFunction(value, otherValue)));
        }

        /// <summary>
        /// Combines the current result with two other results using the <paramref name="combineFunction"/> provided to create a new value from the values of all results if all are <c>SuccessResult</c>s.
        /// </summary>
        /// <typeparam name="TValue1">Type of the first result's value to combine with the current result</typeparam>
        /// <typeparam name="TValue2">Type of the second result's value to combine with the current result</typeparam>
        /// <typeparam name="TNewValue">Type of the new result created by using the <paramref name="combineFunction"/> on the values of all results</typeparam>
        /// <param name="result1">First result to combine with the current result</param>
        /// <param name="result2">Second result to combine with the current result</param>
        /// <param name="combineFunction">Function to use to combine the values of all results</param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TNewValue"/>&gt;</c></returns>
        public Result<TNewValue> Combine<TValue1, TValue2, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Func<TValue, TValue1, TValue2, TNewValue> combineFunction)
        {
            return this.Then(value => 
                result1.Then(value1 => 
                    result2.Map(value2 => combineFunction(value, value1, value2))));
        }

        /// <summary>
        /// Combines the current result with three other results using the <paramref name="combineFunction"/> provided to create a new value from the values of all results if all are <c>SuccessResult</c>s.
        /// </summary>
        /// <typeparam name="TValue1">Type of the first result's value to combine with the current result</typeparam>
        /// <typeparam name="TValue2">Type of the second result's value to combine with the current result</typeparam>
        /// <typeparam name="TValue3">Type of the third result's value to combine with the current result</typeparam>
        /// <typeparam name="TNewValue"></typeparam>
        /// <param name="result1">First result to combine with the current result</param>
        /// <param name="result2">Second result to combine with the current result</param>
        /// <param name="result3">Third result to combine with the current result</param>
        /// <param name="combineFunction">Function to use to combine the values of all results</param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TNewValue"/>&gt;</c></returns>
        public Result<TNewValue> Combine<TValue1, TValue2, TValue3, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Func<TValue, TValue1, TValue2, TValue3, TNewValue> combineFunction)
        {
            return this.Then(value =>
                result1.Then(value1 =>
                    result2.Then(value2 => 
                        result3.Map(value3 => combineFunction(value, value1, value2, value3)))));
        }

        /// <summary>
        /// Combines the current result with four other results using the <paramref name="combineFunction"/> provided to create a new value from the values of all results if all are <c>SuccessResult</c>s.
        /// </summary>
        /// <typeparam name="TValue1">Type of the first result's value to combine with the current result</typeparam>
        /// <typeparam name="TValue2">Type of the second result's value to combine with the current result</typeparam>
        /// <typeparam name="TValue3">Type of the third result's value to combine with the current result</typeparam>
        /// <typeparam name="TValue4">Type of the fourth result's value to combine with the current result</typeparam>
        /// <typeparam name="TNewValue"></typeparam>
        /// <param name="result1">First result to combine with the current result</param>
        /// <param name="result2">Second result to combine with the current result</param>
        /// <param name="result3">Third result to combine with the current result</param>
        /// <param name="result4">Fourth result to combine with the current result</param>
        /// <param name="combineFunction">Function to use to combine the values of all results</param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TNewValue"/>&gt;</c></returns>
        public Result<TNewValue> Combine<TValue1, TValue2, TValue3, TValue4, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Result<TValue4> result4, Func<TValue, TValue1, TValue2, TValue3, TValue4, TNewValue> combineFunction)
        {
            return this.Then(value =>
                result1.Then(value1 =>
                    result2.Then(value2 =>
                        result3.Then(value3 =>
                            result4.Map(value4 => combineFunction(value, value1, value2, value3, value4))))));
        }

        /// <summary>
        /// Converts the current result into a result of type <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c>, using the provided <c>onSuccess</c> function if the current result is a 
        /// <c>SuccessResult</c>, otherwise using the provided <c>onError</c> function.
        /// </summary>
        /// <typeparam name="TNextValue">Type of the value of the new result</typeparam>
        /// <param name="onSuccess">Function to use if the current result is a <c>SuccessResult</c></param>
        /// <param name="onError">Function to use if the current result is an <c>ErrorResult</c></param>
        /// <returns>A result object of type <c>Result&lt;<typeparamref name="TNextValue"/>&gt;</c></returns>
        public abstract Result<TNextValue> Convert<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess, Func<Error, Result<TNextValue>> onError);

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
