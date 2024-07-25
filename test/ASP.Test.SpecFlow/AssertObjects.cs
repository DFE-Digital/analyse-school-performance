using System.Text.RegularExpressions;
using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Test.Core;
using Xunit;

namespace ASP.Test.SpecFlow
{
    public class AssertObjects
    {
        public static void AreIdentical(string expected, string actual)
        {
            JsonHelper.DeserializeOrNull<object>(expected)
                .OnSuccess(obj => 
                {
                    var normalized = obj switch {
                        null => "null",
                        _ => JsonHelper.Serialize(obj)
                    };

                    Assert.Equal(normalized, actual);
                })
                .OnError(e => AssertWithMessage.Fail(e.ToString()));
        }

        public static void MatchProperties(string expected, string actual)
        {
            PruneTree(expected, actual, "")
                .Then(pruned => JsonHelper.DeserializeOrNull<object>(expected)
                    .OnSuccess(obj =>
                    {
                        var normalized = obj switch {
                            null => "null",
                            _ => JsonHelper.Serialize(obj)
                        };

                        Assert.Equal(normalized, pruned);
                    }))
                .OnError(e => AssertWithMessage.Fail(e.ToString()));
        }

        public static void MatchPropertiesExcludingNullValues(string expected, string actual)
        {
            JsonHelper.DeserializeNotNull<Dictionary<string, object>>(actual, ignoreNullValues: true)
                .Switch(
                    obj =>
                    {
                        var actualExcludingNullValues = JsonHelper.Serialize(obj);
                        MatchProperties(expected, actualExcludingNullValues);
                    },
                    e => JsonHelper.DeserializeNotNull<List<object>>(actual, ignoreNullValues: true)
                        .Switch(
                            list =>
                            {
                                var actualExcludingNullValues = JsonHelper.Serialize(list);
                                MatchProperties(expected, actualExcludingNullValues);
                            },
                            e => Assert.Fail($"Root element is not an object or an array:\n{actual}")
                        )
                );
        }

        public static void HavePropertyIdenticalTo(string propertyPath, string expected, string actual)
        {
            GetPropertyPathValue(propertyPath, actual, "")
                .Switch(
                    propertyValue => AreIdentical(expected, propertyValue),
                    e => Assert.Fail(e.ToString())
                );
        }

        public static void HavePropertyMatching(string propertyPath, string expected, string actual)
        {
            GetPropertyPathValue(propertyPath, actual, "")
                .Switch(
                    propertyValue => MatchProperties(expected, propertyValue),
                    e => Assert.Fail(e.ToString())
                );
        }

        protected static Result<string> GetPropertyPathValue(string propertyPath, string data, string path)
        {
            var parts = propertyPath.Split('.');
            var propertyName = parts[0];
            var parentElement = path.Any() ? $"Element at \"{path}\"" : "Root element";

            return JsonHelper.DeserializeNotNull<Dictionary<string, object>>(data)
                .MapErrorMessage(message => $"{parentElement} is not an object. {message}")
                .Then(dict =>
                {
                    var regex = new Regex(@"(.*)\[(\d+)\]");
                    var match = regex.Match(propertyName);

                    if (match.Success)
                    {
                        var arrayProperty = match.Groups[1].Value;
                        var arrayIndex = int.Parse(match.Groups[2].Value);
                        var currentPropertyPath = path.Any() ? (path + "." + arrayProperty) : arrayProperty;

                        if(!dict.ContainsKey(arrayProperty))
                        {
                            return Error.Unexpected($"Element at {currentPropertyPath} does not exist.", null);
                        }

                        var arrayValue = dict[arrayProperty];

                        return JsonHelper.DeserializeNotNull<object[]>(JsonHelper.Serialize(arrayValue))
                            .MapErrorMessage(message => $"Element at {currentPropertyPath} is not an array. {message}")
                            .Then(array =>
                            {
                                if (array.Length <= arrayIndex)
                                {
                                    return Error.Unexpected($"Array index {arrayIndex} does not exist on array \"{currentPropertyPath}\":\n{JsonHelper.Serialize(array!)}", null);
                                }

                                string value = JsonHelper.Serialize(array[arrayIndex]);
                                var restOfPath = string.Join(".", parts.Skip(1));
                                if (restOfPath.Length > 0)
                                {
                                    return GetPropertyPathValue(restOfPath, value, path.Any() ? path + "." + arrayProperty + "[" + arrayIndex + "]" : arrayProperty + "[" + arrayIndex + "]");
                                }

                                return value;
                            });
                    }
                    else
                    {
                        var currentPropertyPath = path.Any() ? (path + "." + propertyName) : propertyName;

                        if (!dict.ContainsKey(propertyName))
                        {
                            return Error.Unexpected($"Element at {currentPropertyPath} does not exist.", null);
                        }

                        var value = JsonHelper.Serialize(dict[propertyName]);
                        var restOfPath = string.Join(".", parts.Skip(1));
                        if (restOfPath.Length > 0)
                        {
                            return GetPropertyPathValue(restOfPath, value, path.Any() ? path + "." + propertyName : propertyName);
                        }
                        return value;
                    }
                })
               .MapErrorMessage(message => $"Error getting property value for \"{propertyPath}\": {message}");
        }

        private static Result<string> PruneTree(string expected, string actual, string path)
        {
            return JsonHelper.DeserializeOrNull<Dictionary<string, object?>>(expected)
                .Match(
                    expectedDict =>
                    {
                        if(expectedDict is null)
                        {
                            return "null";
                        }

                        return JsonHelper.DeserializeOrNull<Dictionary<string, object?>>(actual)
                            .Then<string>(
                                actualDict =>
                                {
                                    if(actualDict is null)
                                    {
                                        var element = path.Any() ? $"element at {path}" : "root element";
                                        return Error.Unexpected($"Expected {element} to exist:\n{actual}", null);
                                    }

                                    var resultDict = new Dictionary<string, object?>();

                                    foreach (var kvp in expectedDict)
                                    {
                                        if (!actualDict.ContainsKey(kvp.Key))
                                        {
                                            return Error.Unexpected($"Expected element at \"{path}.{kvp.Key}\" to exist:\n{actual}", null);
                                        }

                                        Error? error = null;
                                        PruneTree(JsonHelper.Serialize(kvp.Value), JsonHelper.Serialize(actualDict[kvp.Key]), path.Any() ? path + "." + kvp.Key : kvp.Key)
                                           .OnError(e => error = e)
                                           .OnSuccess(r => JsonHelper.DeserializeOrNull<object>(r)
                                                .OnSuccess(v => resultDict[kvp.Key] = v));

                                        if (error != null)
                                        {
                                            return error;
                                        }
                                    }

                                    return JsonHelper.Serialize(resultDict);
                                }
                            );
                    },
                    e =>
                    {
                        return JsonHelper.DeserializeOrNull<List<object?>>(expected)
                            .Match(
                                expectedList =>
                                {
                                    if (expectedList is null)
                                    {
                                        return "null";
                                    }

                                    return JsonHelper.DeserializeOrNull<List<object?>>(actual)
                                        .Then<string>(
                                            actualList =>
                                            {
                                                if (actualList is null)
                                                {
                                                    var element = path.Any() ? $"element at {path}" : "root element";
                                                    return Error.Unexpected($"Expected {element} to exist:\n{actual}", null);
                                                }

                                                var resultList = new List<object?>();

                                                for (var i = 0; i < expectedList.Count; i++)
                                                {
                                                    if (actualList.Count <= i)
                                                    {
                                                        return Error.Unexpected($"Array index {i} does not exist on array \"{path}\":\n{actual}", null);

                                                    }

                                                    Error? error = null;
                                                    PruneTree(JsonHelper.Serialize(expectedList[i]), JsonHelper.Serialize(actualList[i]), path + "[" + i + "]")
                                                        .OnError(e => error = e)
                                                        .OnSuccess(r => JsonHelper.DeserializeOrNull<object>(r)
                                                            .OnSuccess(v => resultList.Add(v!)));

                                                    if(error != null) 
                                                    {
                                                        return error;
                                                    }
                                                }

                                                return JsonHelper.Serialize(resultList);
                                            });
                                },
                                e => actual
                            );
                    }
            );
        }
    }
}
