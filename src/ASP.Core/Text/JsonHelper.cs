using ASP.Core.Results;
using Newtonsoft.Json;

namespace ASP.Core.Text
{
    public static class JsonHelper
    {
        public static string Serialize(object? obj)
        {
            return obj switch {
                null => "null",
                _ => JsonConvert.SerializeObject(obj)
            };
        }

        public static string SerializeIndented(object? obj)
        {
            return obj switch {
                null => "null",
                _ => JsonConvert.SerializeObject(obj, Formatting.Indented)
            };
        }

        public static Result<T?> DeserializeOrNull<T>(string json, bool ignoreNullValues = false, bool ignoreMissingMembers = false) where T : notnull
        {
            T? item;

            string? error = null;
            item = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                NullValueHandling = ignoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include,
                MissingMemberHandling = ignoreMissingMembers ? MissingMemberHandling.Ignore : MissingMemberHandling.Error,
                Error = (sender, args) =>
                {
                    error = args.ErrorContext.Error.Message;
                    args.ErrorContext.Handled = true;
                }
            });

            if (error != null)
            {
                return Error.Unexpected($"Error occurred deserializing object of type {typeof(T)}: {error}. Object: {Environment.NewLine}{json}", null);
            }

            return item;
        }

        public static Result<T> DeserializeNotNull<T>(string json, bool ignoreNullValues = false, bool ignoreMissingMembers = false) where T : notnull
        {
            T? item;

            string? error = null;
            item = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                NullValueHandling = ignoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include,
                MissingMemberHandling = ignoreMissingMembers ? MissingMemberHandling.Ignore : MissingMemberHandling.Error,
                Error = (sender, args) =>
                {
                    error = args.ErrorContext.Error.Message;
                    args.ErrorContext.Handled = true;
                }
            });

            if (error != null)
            {
                return Error.Unexpected($"Error occurred deserializing object of type {typeof(T)}: {error}. Object: {Environment.NewLine}{json}", null);
            }

            if (item == null)
            {
                return Error.Unexpected($"Item deserialized to null when deserializing type {typeof(T)}, serialized value: {Environment.NewLine}{json}", null);
            }

            return item;
        }

        public static string Normalize(string json)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(json));
        }
    }
}
