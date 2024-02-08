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

            try
            {
                item = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                return Error.Unexpected("JsonHelper.Deserialize", $"Error occurred deserializing object of type {typeof(T)}: {ex.Message}. Object: {Environment.NewLine}{json}");
            }

            if (item == null)
            {
                return Error.Unexpected("JsonHelper.Deserialize", $"Item deserialized to null when deserializing type {typeof(T)}, serialized value: {Environment.NewLine}{json}");
            }

            return item!;
        }

        public static ErrorOr<T> DeserializeIgnoringMissingMembers<T>(string json)
        {
            T? item;

            try
            {
                item = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore });
            }
            catch (Exception ex)
            {
                return Error.Unexpected("JsonHelper.DeserializeIgnoringMissingMembers", $"Error occurred deserializing object of type {typeof(T)}: {ex.Message}");
            }

            if (item == null)
            {
                return Error.Unexpected("JsonHelper.DeserializeIgnoringMissingMembers", $"Item deserialized to null when deserializing type {typeof(T)}, serialized value: {Environment.NewLine}{json}");
            }

            return item!;
        }

    }
}
