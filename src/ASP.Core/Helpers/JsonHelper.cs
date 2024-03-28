using ErrorOr;
using Newtonsoft.Json;

namespace ASP.Core.Helpers
{
    public static class JsonHelper
    {
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string SerializeIndented(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static ErrorOr<T> Deserialize<T>(string json)
        {
            T? item;

            string? error = null;
                item = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                    Error = (object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) =>
                    {
                        error = args.ErrorContext.Error.Message;
                        args.ErrorContext.Handled = true;
                    }
                });

            if(error != null) 
            { 
                return Error.Unexpected(description: $"Error occurred deserializing object of type {typeof(T)}: {error}. Object: {Environment.NewLine}{json}");
            }

            if (item == null)
            {
                return Error.Unexpected(description: $"Item deserialized to null when deserializing type {typeof(T)}, serialized value: {Environment.NewLine}{json}");
            }

            return item!;
        }

        public static ErrorOr<T> DeserializeIgnoringMissingMembers<T>(string json)
        {
            T? item;

            string? error = null;
            item = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Error = (object? sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) =>
                {
                    error = args.ErrorContext.Error.Message;
                    args.ErrorContext.Handled = true;
                }
            });

            if (error != null)
            {
                return Error.Unexpected(description: $"Error occurred deserializing object of type {typeof(T)}: {error}. Object: {Environment.NewLine}{json}");
            }
            
            if (item == null)
            {
                return Error.Unexpected(description: $"Item deserialized to null when deserializing type {typeof(T)}, serialized value: {Environment.NewLine}{json}");
            }

            return item!;
        }

    }
}
