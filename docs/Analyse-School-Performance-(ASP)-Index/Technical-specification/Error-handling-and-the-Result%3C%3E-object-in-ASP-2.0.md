A common source of errors in code is not catering for all possible outcomes of a particular event or action. For example looking up an object from a database could result in:

* the object being found in the database and returned successfully (the happy path)
* the object not being found in the database 
* an error thrown by the database (maybe a syntax error in the database query)
* the database being down
* the database not being reachable due to a networking problem
* the response from the database being successful but due to a networking problem it never reaches the application server

... and many other possible failure cases. A lot of code only really caters for the happy path, and if any  thought is given to the failure cases, these are lumped together in the "something went wrong" category. In reality not every failure case should be handled in the same way. We can divide these cases into three broad categories:

* Success case (happy path)
* Expected failure (this is a likely case or a logical outcome of interacting with the system in a certain way). Examples:
  * Page not found
  * Attempting to interact with a deleted resource/entity
  * Authorization issue - trying to access a restricted area/resource
* Unexpected error (this shouldn't happen and we need to fix it)
  * Database down
  * Networking issue
  * Server error

What is deemed expected vs. unexpected is down to the problem domain. For example for the Azure portal software, a database being down or a network issue would probably come under expected failure cases and would need specific logic to handle.

Another issue is that once we try to handle all the possible failure cases the code quickly becomes very messy and confusing. Here's an example of quite a simple controller action for the school landing page which needs to do a few things:
1. Fetch an EstablishmentDetails object using the `urn` parameter
2. Fetch a ContentTemplate object using the constant `CONTENT_TEMPLATE_ID` (to render the landing page cards)
3. Compose a view model from these two objects and pass it to the view

This is complicated by needing to handle failure cases:
1. If an unexpected error happens while fetching the EstablishmentDetails or ContentTemplate, show a 500 error page
2. If the EstablishmentDetails with the provided urn doesn't exist, show a 404 error page
3. If the EstablishmentDetails exists in the database but has isDeleted = true, show a 404 error page
4. If the ContentTemplate doesn't exist, don't show an error but just silently fail, showing an empty list of cards

Here's how we might handle this using exceptions:

``` csharp
[HttpGet("{urn}")]
public async Task<IActionResult> Index(string urn)
{
    var defaultIfNotFound = new ContentTemplateViewModel {
        Views = []
    };

    try {
        var estab = await _useCase.HandleRequest(new GetEstablishmentDetailsUseCaseRequest(urn));
        
        // do we need to check if estab is null here?
        
        if(estab.IsDeleted) {
            return new ObjectResult($"Establishment {urn} has been deleted.") { StatusCode = StatusCodes.Status404NotFound };
        }
        var establishmentDetailsModel = EstablishmentDetailsViewModel.FromEstablishmentDetails(estab);
        
        ContentTemplateViewModel contentTemplateModel;
        
        try {
            var template = await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(CONTENT_TEMPLATE_ID));
            
            // do we need to check if template is null here?
            
            contentTemplateModel = ContentTemplateViewModel.FromTemplate(CONTENT_TEMPLATE_ID, template);
        } catch(NotFoundException ex) {
            contentTemplateModel = defaultIfNotFound;
        }

        // any other exceptions bubble up to outside try/catch block

        var model = new SchoolViewModel {
            EstablishmentDetails = establishmentDetailsModel,
            ContentTemplate = contentTemplateModel
        };

        return View(model);
    } catch(NotFoundException ex) {
        return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status404NotFound };
    } catch(Exception ex) {
        return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError }
    }
}
```

It becomes quite difficult to understand this logic with all the explicit error handling code, and this will get more complicated the more cases we need to handle.

We would like a way to 
1. explicitly mark failure cases
2. force each failure case to be explicitly handled
3. be able to see the happy path by inspecting the code
4. encapsulate each type of response to a success/failure into self-contained understandable "chunks" of logic

Have a look at this post that describes "Railway-oriented programming" (I recommend watching Scott Wlaschin's talk on this if you haven't seen it already - linked in the post)
https://blog.logrocket.com/what-is-railway-oriented-programming/

In ASP 2.0 we have the concept of a `Result<TValue>` which is an abstract class with two implementations: `
``` csharp
public abstract class Result<TValue> {
}

public sealed class SuccessResult<TValue> : Result<TValue> {
    public TValue Value { get; }
}

public sealed class ErrorResult<TValue>` :  Result<TValue> {
    public Error Error { get; }
}
```

`Error` is another abstract class which has several implementations (and will have more in the future as we identify them, e.g. `AccessDeniedError`:

``` csharp
public abstract class Error {
    public string Message { get; }
}

public sealed class NotFoundError : Error { }
public sealed class UnexpectedError : Error { }
public sealed class ValidationError : Error { }
```
We return a `Result<TValue>` from any method that has a possibility of failure. We can then chain these result-returning operations together so that the next step in the happy path is only ever called if all the preceding steps have been successful, and all the error cases fall through to the end where all the types of error are handled explicitly. So for example:

``` csharp
var result1 = DoSomeOperationThatMightFail();

// Call SomeOtherOperationThatMightFail() with the value of result1 only if it was successful
var result2 = result1.Then(r => SomeOtherOperationThatMightFail(r));

// Call AFurtherOperationThatMightFail() with the value of result2 only if it was successful
var result3 = result2.Then(r => AFurtherOperationThatMightFail(r));

// Handle the success/error cases:
result3.Switch(
    value => Console.WriteLine("Success! Result was: " + value),
    error => {
        switch(error) {
            case NotFoundError _:
                Console.WriteLine("Item not found: " + error.Message);
                break;
            case ValidationError _:
                Console.WriteLine("The following validation error occurred: " + error.Message);
                break;
            default:
                Console.WriteLine("An unexpected error occurred: " + error.Message);
                break;
        }
    }
);
```
This can be simplified into a fluent-style method chain:
``` csharp
DoSomeOperationThatMightFail()
    .Then(SomeOtherOperationThatMightFail)
    .Then(AFurtherOperationThatMightFail);
    .Switch(
        value => Console.WriteLine("Success! Result was: " + value),
        error => {
            switch(error) {
                case NotFoundError _:
                    Console.WriteLine("Item not found: " + error.Message);
                    break;
                case ValidationError _:
                    Console.WriteLine("The following validation error occurred: " + error.Message);
                    break;
                default:
                    Console.WriteLine("An unexpected error occurred: " + error.Message);
                    break;
            }
        }
    );

```

The `Then()` and `Switch()` methods are abstract methods on `Result<TValue>` that have an implementation for both the success and error cases. The magic is in the polymorphism of the `Result<TValue>` type so that the success case propogates the success result through the method chain, and the error case propogates the error result. Any operation in the chain might fail, in which case the success result becomes an error result which gets passed through the chain to be ignored by the happy path and dealt with at the end.

Here are the abstract methods on `Result<TValue>`:

``` cs
public abstract class Result<TValue>
{
    // Pass the successful result value to an operation that changes the value type (and can't fail)
    public abstract Result<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction);
    
    // If this is an error result, change the error from one type to another
    public abstract Result<TValue> MapError(Func<Error, Error> mapFunction);
    
    // Pass the successful result value to an operation that changes the value type 
    // (but could fail in the process)
    public abstract Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess);
    
    // Handle the success and error cases by converting each case into the same return type
    public abstract TReturnValue Match<TReturnValue>(Func<TValue, TReturnValue> onSuccess, Func<Error, TReturnValue> onError);

    // Handle the success and error cases by performing an action for each case
    public abstract void Switch(Action<TValue> onSuccess, Action<Error> onError);

    // Gets the successful result value, or a default value in case of any error
    public abstract TValue GetValueOrDefault(TValue defaultValue);

    // Make sure the result always succeeds, by replacing any error with a default value
    public abstract SuccessResult<TValue> DefaultIfError(TValue defaultValue);

    // Replace an error with a success using a default value, but only if the error matches a predicate
    public abstract Result<TValue> DefaultIf(Func<Error, bool> predicate, TValue defaultValue);

    // Replace an success with an error, but only if the success value matches a predicate
    public abstract Result<TValue> ErrorIf(Func<TValue, bool> predicate, Error error);

    // Convert the result into a result of a different type, handling the success and error cases
    public abstract Result<TNextValue> Convert(Func<TValue, Result<TNextValue>> onSuccess, Func<TValue, Result<TNextValue>> onError);

    // Combine the result with another result, using a function that takes the two result values and returns
    // a different value
    public Result<TNewValue> Combine<TOtherValue, TNewValue>(Result<TOtherValue> otherResult, Func<TValue, TOtherValue, TNewValue> combineFunction)
}
```
In addition some methods have overloads to handle asynchronous operations:
``` csharp
public abstract class Result<TValue>
{
    public abstract Task<Result<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction);
    public abstract Task<Result<TValue>> MapError(Func<Error, Task<Error>> mapFunction);
    public abstract Task<Result<TNextValue>> Then<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess);
    public abstract Task<TReturnValue> Match<TReturnValue>(Func<TValue, Task<TReturnValue>> onSuccess, Func<Error, Task<TReturnValue>> onError);
    public abstract Task Switch(Func<TValue, Task> onSuccess, Func<Error, Task> onError);
}
```
The `Combine()` function also has overloads to combine more than two results at once:
``` csharp
public Result<TNewValue> Combine<TValue1, TValue2, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Func<TValue, TValue1, TValue2, TNewValue> combineFunction)
public Result<TNewValue> Combine<TValue1, TValue2, TValue3, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Func<TValue, TValue1, TValue2, TValue3, TNewValue> combineFunction)
public Result<TNewValue> Combine<TValue1, TValue2, TValue3, TValue4, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Result<TValue4> result4, Func<TValue, TValue1, TValue2, TValue3, TValue4, TNewValue> combineFunction)
```
Here's how we might refactor the above controller action using result objects:
``` csharp
[HttpGet("{urn}")]
public async Task<IActionResult> Index(string urn)
{
    var defaultIfNotFound = new ContentTemplateViewModel {
        Views = []
    };

    // Get the establishment details (this could fail with a NotFoundError or UnexpectedError)
    return await _useCase.HandleRequest(new GetEstablishmentDetailsUseCaseRequest(urn))
        
        // If the establishment is deleted, change the result to a NotFoundError
        .ErrorIf(estab => estab.IsDeleted, Error.NotFound($"Establishment {urn} has been deleted."))

        // Convert the EstablishmentDetails to view model
        .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails)

        // Pass the view model along to the next operation (get content template - this could also fail with a NotFoundError or UnexpectedError)
        .Then(estabModel =>  await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(CONTENT_TEMPLATE_ID))

            // Convert the ContentTemplate to view model
            .Map(template => ContentTemplateViewModel.FromTemplate(CONTENT_TEMPLATE_ID, template))

            // Ignore NotFoundError, substituting with the default view model
            .DefaultIf(error => error is NotFoundError, defaultIfNotFound)

            // Construct the main view model from the estabModel passed to Then() and the templateModel
            .Then(templateModel => new SchoolViewModel {
                 EstablishmentDetails = estabModel,
                 ContentTemplate = templateModel
             })

        // Convert to IActionResult, handling both the success and error cases
        .Match(
            viewModel => (IActionResult)View(viewModel),
            error => error switch {
                NotFoundError _ => new ObjectResult(error.Message) { 
                    StatusCode = StatusCodes.Status404NotFound 
                },
                _ => new ObjectResult(error.Message) { 
                    StatusCode = StatusCodes.Status500InternalServerError 
                }
            });
}
```
There are a number of different ways to achieve the same result (pun intended) - we could create the establishment result and template result separately and then combine them together:
``` csharp
[HttpGet("{urn}")]
public async Task<IActionResult> Index(string urn)
{
    // 1. Get the establishment details (this could fail with a NotFoundError or UnexpectedError)
    var estabModelResult = await _useCase.HandleRequest(new GetEstablishmentDetailsUseCaseRequest(urn)) 
       .ErrorIf(estab => estab.IsDeleted, Error.NotFound($"Establishment {urn} has been deleted."))
       .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails);

    // 2. Get the content template (this could fail with a NotFoundError or UnexpectedError)
    var templateModelResult = await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(CONTENT_TEMPLATE_ID))
        .Map(template => ContentTemplateViewModel.FromTemplate(CONTENT_TEMPLATE_ID, template))
        .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel {
            Views = []
        });
    
    // Construct the main view model by combining the estabModelResult and the templateModelResult
    var viewModelResult = estabModelResult.Combine(templateModelResult, (estabModel, templateModel) => 
       new SchoolViewModel {
            EstablishmentDetails = estabModel,
            ContentTemplate = templateModel
        }
    );

    return viewModelResult.Match(
        viewModel => (IActionResult)View(viewModel),
        error => error switch {
            NotFoundError _ => new ObjectResult(error.Message) { 
                StatusCode = StatusCodes.Status404NotFound 
            },
            _ => new ObjectResult(error.Message) { 
                StatusCode = StatusCodes.Status500InternalServerError 
            }
        });
}
```

# Result\<TValue> Methods
Here is more in-depth documentation for each method on `Result<TValue>`. 

**TODO:** find some way to generate this automatically from code comments.

---
## Map\<TNextValue>(Func<TValue, TNextValue>)
`public abstract Result<TNextValue> Map<TNextValue>(Func<TValue, TNextValue> mapFunction)`

Converts the current result from a `Result<TValue>` to a `Result<TNextValue>`, using the map function provided to convert the result value from a `TValue` to a TNextValue if the result is a `SuccessResult`, otherwise changes the type while preserving the error. For example:

```` csharp
var a = Result.Success(3)
    .Map(v => v + 1);
    
var b = Result.NotFound<int>("Not found")
    .Map(v => v + 1); 
````
results in `a` having the value `4` and `b` having the error `NotFoundError: "Not found"`.

### Type parameters
*TNextValue*: Type of the result value once converted

### Parameters
*mapFunction*: Function to convert the result value if the current result is a `SuccessResult`

### Return value
A result object of type `Result<TNextValue>`

---
## Map\<TNextValue>(Func<TValue, Task\<TNextValue>>)
`public abstract Task<Result<TNextValue>> Map<TNextValue>(Func<TValue, Task<TNextValue>> mapFunction)`

Asynchronously converts the current result from a `Result<TValue>` to a `Result<TNextValue>`, using the map function provided to convert the result value from a `TValue` to a TNextValue if the current result is a `SuccessResult`, otherwise changes the type while preserving the error. For example:

``` csharp
async Task<int> wait1SecThenAdd1(int v) 
{
    await Task.Delay(1000);
    return v + 1;
}

var a = await Result.Success(3)
    .Map(wait1SecThenAdd1);
    
var b = await Result.NotFound<int>("Not found")
    .Map(wait1SecThenAdd1);
```
results in `a` having the value `4` and `b` having the error `NotFoundError: "Not found"`.

### Type parameters
*TNextValue*: Type of the result value once converted

### Parameters

*mapFunction*: Asynchronous function to convert the result value if the current result is a `SuccessResult`

### Return value

The task object representing the asynchronous operation returning a `Result<TNextValue>`

---
## MapError(Func<Error, Error>)
`public abstract Result<TValue> MapError(Func<Error, Error> mapFunction)`

If the current result is an `ErrorResult`, uses the map function provided to change the error, otherwise returns the current result unchanged. For example:

``` csharp
Func<Error, Error> changeError = e => Error.Unexpected("ERROR: " + e.Message);

var a = Result.Success(3)
    .MapError(changeError);
    
var b = Result.NotFound<int>("Not found")
    .MapError(changeError);
```
results in `a` having the value `3` and `b` having the error `UnexpectedError: "ERROR: Not found"`.

### Parameters

*mapFunction*: Function to convert the error if the current result is an `ErrorResult`

### Return value

A result object of type `Result<TValue>`

---
## MapError(Func<Error, Task\<Error>>)
`public abstract Task<Result<TValue>> MapError(Func<Error, Task<Error>> mapFunction)`

If the current result is an `ErrorResult`, asynchronously uses the map function provided to change the error, otherwise returns the current result unchanged. For example:

``` csharp
async Task<Error> wait1SecThenChangeError(Error e) 
{ 
    await Task.Delay(1000); 
    return Error.Unexpected("ERROR: " + e.Message); 
}

var a = await Result.Success(3)
    .MapError(wait1SecThenChangeError);
    
var b = await Result.NotFound<int>("Not found")
    .MapError(wait1SecThenChangeError);
```
results in `a` having the value `3` and `b` having the error `UnexpectedError: "ERROR: Not found"`.

### Parameters

*mapFunction*: Asynchronous function to convert the error if the current result is an `ErrorResult`

### Return value
The task object representing the asynchronous operation returning a `Result<TValue>`

---
## Then\<TNextValue>(Func<TValue, Result\<TNextValue>>)
`public abstract Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)`

Converts the current result from a `Result<TValue>` to a `Result<TNextValue>`, using the provided function. Unlike `Map` this function returns another result rather than just a value. This allows a pipeline to be created out of many operations, each of which might fail, producing an `ErrorResult` if any of the operations failed, or a `SuccessResult` if all the operations succeeded. For example:

``` csharp
Result<double> parseDouble(string str) =>
    double.TryParse(str, out var result) ? result : Error.Unexpected($"Could not parse value \"{str}\" as double!");
   
Result<double> divide1By(double x) =>
    x == 0 ? Error.Unexpected("Division by zero") : 1.0 / x;

var a = Result.Success("2")
    .Then(parseDouble)
    .Then(x => divide1By(x));
    
var b = Result.Success("0")
    .Then(parseDouble)
    .Then(x => divide1By(x));
    
var c = Result.Success("XYZ")
    .Then(parseDouble)
    .Then(x => divide1By(x));
    
var d = Result.NotFound<string>("Not found")
    .Then(parseDouble)
    .Then(x => divide1By(x));
```
results in `a` having the value `0.5`, `b` having the error `UnexpectedError: "Could not parse value \"XYZ\" as double!"`, `c` having the error `UnexpectedError: "Division by zero!"`, and `d` having the error `NotFoundError: "Not found"`.

### Type parameters

*TNextValue*: Type of the result value of the `onSuccess` operation

### Parameters

*onSuccess*: Operation to perform with the value of the current result (if current result is a `SuccessResult`)

### Return value
The result of the `onSuccess` function

---
## Then\<TNextValue>(Func<TValue, Task\<Result\<TNextValue>>>)
`public abstract Task<Result<TNextValue>> Then<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onSuccess)`

Asynchronously converts the current result from a `Result<TValue>` to a `Result<TNextValue>`, using the provided function. Unlike `Map` this function returns another result rather than just a value. This allows a pipeline to be created out of many operations, each of which might fail, producing an `ErrorResult` if any of the operations failed, or a `SuccessResult` if all the operations succeeded. For example:

``` csharp
async Task<Result<double>> wait1SecThenParseDouble(string str)
{
    await Task.Delay(1000);
    return double.TryParse(str, out var result) ? result : Error.Unexpected($"Could not parse value \"{str}\" as double!");
}
   
async Task<Result<double>> wait2SecsThenDivide1By(double x)
{
    await Task.Delay(2000);
    return x == 0 ? Error.Unexpected("Division by zero") : 1.0 / x;
}
   
var a = await Result.Success("2")
    .Then(wait1SecThenParseDouble)
    .Then(x => wait2SecsThenDivide1By(x));
     
var b = await Result.Success("0")
    .Then(wait1SecThenParseDouble)
    .Then(x => wait2SecsThenDivide1By(x));
 
var c = await Result.Success("XYZ")
    .Then(wait1SecThenParseDouble)
    .Then(x => wait2SecsThenDivide1By(x));

var d = await Result.NotFound<string>("Not found")
    .Then(wait1SecThenParseDouble)
    .Then(x => wait2SecsThenDivide1By(x));
```
results in `a` having the value `0.5`, `b` having the error `UnexpectedError: "Could not parse value \"XYZ\" as double!"`, 
`c` having the error `UnexpectedError: "Division by zero!"`, and `d` having the error `NotFoundError: "Not found"`.

### Type parameters

*TNextValue*: Type of the result value of the `onSuccess` operation

### Parameters

*onSuccess*: Asynchronous operation to perform with the value of the current result (if current result is a `SuccessResult`)

### Return value
The task object representing the asynchronous operation returning the result of the `onSuccess` function

---
## Match\<TReturnValue>(Func<TValue, TReturnValue>, Func<Error, TReturnValue>)
`public abstract TReturnValue Match<TReturnValue>(Func<TValue, TReturnValue> onSuccess, Func<Error, TReturnValue> onError);`

Converts the current result from a `Result<TValue>` to a value of type `TReturnValue`, by way of the `onSuccess` and `onError` functions. This is useful if the success/error state of the result needs to be converted into another representation (such as at an application boundary). For example:

``` csharp
string toString(Result<double> result) => result.Match(
    value => value.ToString(),
    error => $"{error.GetType().Name.ToUpper()}: {error.Message}"
);
   
var a = toString(Result.Success(2.0));
     
var b = toString(Result.NotFound<double>("Not found"));

var c = toString(Result.Unexpected<double>("Unexpected"));
```
results in `a` having the value `"2.0"`, `b` having the value `"NOTFOUNDERROR: Not found"` and `c` having the value `"UNEXPECTEDERROR: Unexpected"`.


``` csharp
public class Response 
{
    public int StatusCode { get; set; }
    public string Body { get; set; }
}

Response toResponse(Result<double> result) => result.Match(
    value => new Response { StatusCode = 200, Body = value.ToString() },
    error => {
        int statusCode = error switch {
            ValidationError _ => 400,
            NotFoundError _ => 404,
            _ => 500
        };
        return new Response { StatusCode = statusCode, Body = error.Message };
    }
);
   
var a = toResponse(Result.Success(2.0));
     
var b = toResponse(Result.NotFound<double>("Not found"));

var c = toResponse(Result.Unexpected<double>("Unexpected"));
```
results in `a` having the value `{ StatusCode: 200, Body: "2.0" }`, `b` having the value `{ StatusCode: 404, Body: "Not found" }` and `c` having the value `{ StatusCode: 500, Body: "Unexpected" }`.

### Type parameters

*TReturnValue*: The type of the value the current result will be converted into

### Parameters

*onSuccess*: Function to convert a `SuccessResult` to the return value

*onError*: Function to convert an `ErrorResult` to the return value

### Return value
An value of type `TReturnValue`

---
## Match\<TReturnValue>(Func<TValue, Task\<TReturnValue>>, Func<Error, Task\<TReturnValue>>)
`public abstract Task<TReturnValue> Match<TReturnValue>(Func<TValue, Task<TReturnValue>> onSuccess, Func<Error, Task<TReturnValue>> onError)`

Asynchronously converts the current result from a `Result<TValue>` to a value of type `TReturnValue`, by way of the `onSuccess` and `onError` functions. This is useful if the success/error state of the result needs to be converted into another representation (such as at an application boundary). For example:

``` csharp
async Task<string> waitThenToString(int delayMs, Result<double> result) => await result.Match(
    async value => { await Task.Delay(delayMs); return value.ToString(); },
    async error => { await Task.Delay(delayMs); return $"{error.GetType().Name.ToUpper()}: {error.Message}"; }
);

var a = await waitThenToString(100, Result.Success(2.0));
     
var b = await waitThenToString(1000, Result.NotFound<double>("Not found"));

var c = await waitThenToString(2000, Result.Unexpected<double>("Unexpected"));
```
results (eventually) in `a` having the value `"2.0"`, `b` having the value `"NOTFOUNDERROR: Not found"` and `c` having the value `"UNEXPECTEDERROR: Unexpected"`.

``` csharp
public class Response 
{
    public int StatusCode { get; set; }
    public string Body { get; set; }
}

async Task<Response> waitThenToResponse(int delayMs, Result<double> result) => result.Match(
    async value => { 
        await Task.Delay(delayMs); 
        return new Response { StatusCode = 200, Body = value.ToString() }; 
    },
    async error => {
        await Task.Delay(delayMs); 
        int statusCode = error switch {
            ValidationError _ => 400,
            NotFoundError _ => 404,
            _ => 500
        };
        return new Response { StatusCode = statusCode, Body = error.Message };
    }
);
   
var a = waitThenToResponse(100, Result.Success(2.0));
     
var b = waitThenToResponse(200, Result.NotFound<double>("Not found"));

var c = waitThenToResponse(300, Result.Unexpected<double>("Unexpected"));
```
results (eventually) in `a` having the value `{ StatusCode: 200, Body: "2.0" }`, `b` having the value `{ StatusCode: 404, Body: "Not found" }` and `c` having the value `{ StatusCode: 500, Body: "Unexpected" }`.

### Type parameters

*TReturnValue*: The type of the value the current result will be converted into

### Parameters

*onSuccess*: Asynchronous function to convert a `SuccessResult` to the return value

*onError*: Asynchronous function to convert an `ErrorResult` to the return value

### Return value
The task object representing the asynchronous operation returning a value of type `TReturnValue`

---
## Switch(Action\<TValue>, Action\<Error>)
`public abstract void Switch(Action<TValue> onSuccess, Action<Error> onError)`

Carries out an action based on the current result, by way of the `onSuccess` and `onError` functions. This is useful if the success/error state of the result needs to be handled directly by the application (such as at an application boundary). For example:

``` csharp
void handleResult(Result<double> result) => result.Switch(
    value => Console.WriteLine(value),
    error => {
        if(error is NotFoundError) {
            Console.WriteLine(error.Message);
        } else {
            throw new ApplicationException("An error occurred: " + error.Message);
        }
    }
);
   
handleResult(Result.Success(2.0));
     
handleResult(Result.NotFound<double>("Not found"));

handleResult(Result.Unexpected<double>("Unexpected"));
````
results in the following being written to the console: 

```
2.0
Not found
```
and then an `ApplicationException` being thrown with message `"An error occurred: Unexpected"`

### Parameters
*onSuccess*: Action to execute if current result is a `SuccessResult`

*onError*: Action to execute if current result is an `ErrorResult`

---
## Switch(Func<TValue, Task>, Func<Error, Task>)
`public abstract Task Switch(Func<TValue, Task> onSuccess, Func<Error, Task> onError)`

Carries out an action based on the current result, by way of the `onSuccess` and `onError` functions. This is useful if the success/error state of the result needs to be handled directly by the application (such as at an application boundary). For example:


``` csharp
async Task waitThenHandleResult(int delayMs, Result<double> result) => await result.Switch(
    async value => { 
        await Task.Delay(delayMs); 
        Console.WriteLine(value); 
    },
    async error => {
        await Task.Delay(delayMs); 
        if(error is NotFoundError) {
            Console.WriteLine(error.Message);
        } else {
            throw new ApplicationException("An error occurred: " + error.Message);
        }
    }
);
   
await waitThenHandleResult(200, Result.Success(2.0));
     
await waitThenHandleResult(100, Result.NotFound<double>("Not found"));

await waitThenHandleResult(300, handleResult(Result.Unexpected<double>("Unexpected"));
```
results in the following being written to the console: 

```
Not found
2.0
```
and then an `ApplicationException` being thrown with message `"An error occurred: Unexpected"`

### Parameters

*onSuccess*: Asynchronous action to execute if current result is a `SuccessResult`

*onError*: Asynchronous action to execute if current result is an `ErrorResult`

### Return value
The task object representing the asynchronous operation

---
## GetValueOrDefault(TValue)
`public abstract TValue GetValueOrDefault(TValue defaultValue)`

Gets the value of the current result if it is a `SuccessResult`, or the `defaultValue` provided if it is an `ErrorResult`. For example:


``` csharp
var a = Result.Success(2).GetValueOrDefault(123);
var b = Result.NotFound("Not found").GetValueOrDefault(123);
```
results in `a` having the value `2` and `b` having the value `123`.


`defaultValue`: Value to return if the current result is an `ErrorResult`
### Return value
The value of the current result if it is a `SuccessResult`, or `defaultValue`
if it is an `ErrorResult`

## DefaultIfError(TValue defaultValue)
`public abstract SuccessResult<TValue> DefaultIfError(TValue defaultValue)`

If the current result is an `ErrorResult`, returns a `SuccessResult` with the provided `defaultValue` as its value, otherwise returns the current result unchanged. This ensures the result returned is always a `SuccessResult`. For example:

````
var a = Result.Success(2).DefaultIfError(123);
var b = Result.NotFound("Not found").DefaultIfError(123);
````
results in `a` being a `SuccessResult` with value `2` and `b` being a `SuccessResult` with value `123`.

### Parameters

*defaultValue*: Value to use if the current result is an `ErrorResult`

### Return value
A result object of type `SuccessResult<TValue>`

---
## DefaultIf(Func<Error, bool>, TValue)
`public abstract Result<TValue> DefaultIf(Func<Error, bool> predicate, TValue defaultValue)`

If the current result is an `ErrorResult` and its `Error` matches the `predicate` provided, returns a `SuccessResult`  with the provided `defaultValue` as its `Value`, otherwise returns the current result unchanged. For example:

``` csharp
var a = Result.Success(2).DefaultIf(e => e is NotFoundError, 123);
var b = Result.NotFound("Not found").DefaultIf(e => e is NotFoundError, 123);
var b = Result.Unexpected("Unexpected").DefaultIf(e => e is NotFoundError, 123);
```
results in `a` being a `SuccessResult` with value `2`, `b` being a `SuccessResult` with value `123`, and `c` being
an `ErrorResult` with error `UnexpectedError: "Unexpected"`.

### Parameters
*predicate*: Predicate which should evaluates to true if the current result is an `ErrorResult` and should be converted to a 
*SuccessResult* using the provided `defaultValue`

*defaultValue*: Value to use if the current result is an `ErrorResult` and its `Error` matches the `predicate`
### Return value
A result object of type `Result<TValue>`

---
## ErrorIf(Func<TValue, bool>, Error error)
`public abstract Result<TValue> ErrorIf(Func<TValue, bool> predicate, Error error)`

If the current result is a `SuccessResult` and its `Value` matches the `predicate` provided, returns an `ErrorResult` with the provided `error` as its `Error`, otherwise returns the current result unchanged. For example:

``` csharp
var a = Result.Success(2).ErrorIf(v => v > 10, Error.Unexpected("Value was greater than 10!"));
var b = Result.Success(22).ErrorIf(v => v > 10, Error.Unexpected("Value was greater than 10!"));
```
results in `a` being a `SuccessResult` with value `2` and `b` being an `ErrorResult` with error `UnexpectedError: "Value was greater than 10!"`.


### Parameters

*predicate*: Predicate which should evaluates to true if the current result is a `SuccessResult` and should be converted to an 
`ErrorResult` using the provided `error`

*error*: Error object to use if the current result is a `SuccessResult` and its `Value` matches the `predicate`

### Return value
A result object of type `Result<TValue>`

---
## Combine<TOtherValue, TNewValue>(Result<TOtherValue>, Func<TValue, TOtherValue, TNewValue>)
`public Result<TNewValue> Combine<TOtherValue, TNewValue>(Result<TOtherValue> otherResult, Func<TValue, TOtherValue, TNewValue> combineFunction)`

Combines the current result with another result using the `combineFunction` provided to create a new value from the values of both results if both are `SuccessResult`s.

### Type parameters
*TOtherValue*: Type of the result's value to combine with the current result

*TNewValue*: Type of the new result created by using the `combineFunction` on the values of both results
### Parameters
*otherResult*: Result to combine with the current result

*combineFunction*: Function to use to combine the values of both results
### Return value
A result object of type `Result<TNewValue>`

---
## Combine<TValue1, TValue2, TNewValue>(Result<TValue1>, Result<TValue2>, Func<TValue, TValue1, TValue2, TNewValue>)
`public Result<TNewValue> Combine<TValue1, TValue2, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Func<TValue, TValue1, TValue2, TNewValue> combineFunction)`

Combines the current result with two other results using the `combineFunction` provided to create a new value from the values of all results if all are `SuccessResult`s.

### Type parameters
*TValue1*: Type of the first result's value to combine with the current result

*TValue2*: Type of the second result's value to combine with the current result

*TNewValue*: Type of the new result created by using the `combineFunction` on the values of all results
### Parameters
*result1*: First result to combine with the current result

*result2*: Second result to combine with the current result

*combineFunction*: Function to use to combine the values of all results
### Return value
A result object of type `Result<TNewValue>`

---
## Combine<TValue1, TValue2, TValue3, TNewValue>(Result<TValue1>, Result<TValue2>, Result<TValue3>, Func<TValue, TValue1, TValue2, TValue3, TNewValue>)
`public Result<TNewValue> Combine<TValue1, TValue2, TValue3, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Func<TValue, TValue1, TValue2, TValue3, TNewValue> combineFunction)`

Combines the current result with three other results using the `combineFunction` provided to create a new value from the values of all results if all are `SuccessResult`s.

### Type parameters
*TValue1*: Type of the first result's value to combine with the current result

*TValue2*: Type of the second result's value to combine with the current result

*TValue3*: Type of the third result's value to combine with the current result
*TNewValue*: 
### Parameters
*result1*: First result to combine with the current result

*result2*: Second result to combine with the current result

*result3*: Third result to combine with the current result

*combineFunction*: Function to use to combine the values of all results
### Return value
A result object of type `Result<TNewValue>`

---
## Combine<TValue1, TValue2, TValue3, TValue4, TNewValue>(Result<TValue1>, Result<TValue2>, Result<TValue3>, Result<TValue4>, Func<TValue, TValue1, TValue2, TValue3, TValue4, TNewValue>)
`public Result<TNewValue> Combine<TValue1, TValue2, TValue3, TValue4, TNewValue>(Result<TValue1> result1, Result<TValue2> result2, Result<TValue3> result3, Result<TValue4> result4, Func<TValue, TValue1, TValue2, TValue3, TValue4, TNewValue> combineFunction)`

Combines the current result with four other results using the `combineFunction` provided to create a new value from the values of all results if all are `SuccessResult`s.

### Type parameters
*TValue1*: Type of the first result's value to combine with the current result

*TValue2*: Type of the second result's value to combine with the current result

*TValue3*: Type of the third result's value to combine with the current result

*TValue4*: Type of the fourth result's value to combine with the current result
*TNewValue*: 
### Parameters
*result1*: First result to combine with the current result

*result2*: Second result to combine with the current result

*result3*: Third result to combine with the current result

*result4*: Fourth result to combine with the current result

*combineFunction*: Function to use to combine the values of all results
### Return value
A result object of type `Result<TNewValue>`

---
## Convert<TNextValue>(Func<TValue, Result<TNextValue>>, Func<Error, Result<TNextValue>>)
`public abstract Result<TNextValue> Convert<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess, Func<Error, Result<TNextValue>> onError)`

Converts the current result into a result of type `Result<TNextValue>`, using the provided `onSuccess` function if the current result is a 
`SuccessResult`, otherwise using the provided `onError` function.

### Type parameters
*TNextValue*: Type of the value of the new result
### Parameters
*onSuccess*: Function to use if the current result is a `SuccessResult`

*onError*: Function to use if the current result is an `ErrorResult`
### Return value
A result object of type `Result<TNewValue>`