using ASP.Core;
using ASP.Core.Helpers;
using ErrorOr;
using System.Runtime.InteropServices.JavaScript;

namespace ASP.Test.Core
{

    public class MemoryStore
    {
        private Dictionary<string, Dictionary<(string, string), string>> _store = new();
        public void Clear()
        {
            _store = new();
        }

        public void Set(string containerKey, string id, string partitionKeyValue, object document)
        {
            Set(containerKey, id, partitionKeyValue, JsonHelper.Serialize(document));
        }

        public void Set(string containerKey, string id, string partitionKeyValue, string document)
        {
            if (!_store.ContainsKey(containerKey))
            {
                _store[containerKey] = new();
            }
            _store[containerKey][(id, partitionKeyValue)] = document;
        }

        public ErrorOr<string> Get(string container, string id, string partitionKeyValue)
        {
            if (!_store.ContainsKey(container))
            {
                return Error.NotFound(description: $@"Container ""{container}"" does not exist.");
            }

            if (!_store[container].ContainsKey((id, partitionKeyValue)))
            {
                return Error.NotFound(description: $@"Document with id ""{id}"" and partition key ""{partitionKeyValue}"" does not exist in container ""{container}"".");
            }

            return _store[container][(id, partitionKeyValue)];
        }

        public ErrorOr<TItem> Get<TItem>(string container, string id, string partitionKeyValue) where TItem : class
        {
            return Get(container, id, partitionKeyValue)
                .Then(JsonHelper.DeserializeIgnoringMissingMembers<TItem>);
        }

        public ErrorOr<IEnumerable<string>> GetAll(string container)
        {
            if (!_store.ContainsKey(container))
            {
                return Error.NotFound(description: $@"Container ""{container}"" does not exist.");
            }

            return _store[container].Values.AsEnumerable().ToErrorOr();
        }

        public ErrorOr<IEnumerable<TItem>> GetAll<TItem>(string container) where TItem : class
        {
            var result = GetAll(container);

            return result.Then(r => ConvertAll(r.Select(JsonHelper.DeserializeIgnoringMissingMembers<TItem>)));
        }

        private ErrorOr<IEnumerable<TItem>> ConvertAll<TItem>(IEnumerable<ErrorOr<TItem>> items)
        {
            var deseralized = items.ToList();
            var errors = deseralized.SelectMany(e => e.Errors).ToList();
            if (errors.Any())
            {
                return errors;
            }

            return deseralized.Select(r => r.Value).ToErrorOr();
        }
    }
}
