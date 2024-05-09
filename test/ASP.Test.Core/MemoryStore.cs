using ASP.Core.Helpers;
using ASP.Core.Results;

namespace ASP.Test.Core
{

    public class MemoryStore
    {
        private Dictionary<string, Dictionary<(string, string), string>> _store = new();

        public void Clear()
        {
            _store = new();
        }

        public void ClearContainer(string containerKey)
        {
            _store[containerKey] = new();
        }

        public void EnsureContainer(string containerKey)
        {
            if (!_store.ContainsKey(containerKey))
            {
                _store[containerKey] = new();
            }
        }

        public void Set(string containerKey, string id, string partitionKeyValue, object document)
        {
            Set(containerKey, id, partitionKeyValue, JsonHelper.Serialize(document));
        }

        public void Set(string containerKey, string id, string partitionKeyValue, string document)
        {
            EnsureContainer(containerKey);
            _store[containerKey][(id, partitionKeyValue)] = document;
        }

        public Result<string> Get(string container, string id, string partitionKeyValue)
        {
            if (!_store.ContainsKey(container))
            {
                return Error.NotFound($@"Container ""{container}"" does not exist.");
            }

            if (!_store[container].ContainsKey((id, partitionKeyValue)))
            {
                return Error.NotFound($@"404 Error: Could not find object with id ""{id}"" and partition key ""{partitionKeyValue}"" in container ""{container}"".");
            }

            return _store[container][(id, partitionKeyValue)];
        }

        public Result<TItem> Get<TItem>(string container, string id, string partitionKeyValue) where TItem : class
        {
            return Get(container, id, partitionKeyValue)
                .Then(JsonHelper.DeserializeIgnoringMissingMembers<TItem>);
        }

        public Result<IEnumerable<string>> GetAll(string container)
        {
            if (!_store.ContainsKey(container))
            {
                return Error.NotFound($@"Container ""{container}"" does not exist.");
            }

            return (Result<IEnumerable<string>>) _store[container].Values.AsEnumerable();
        }

        public Result<IEnumerable<TItem>> GetAll<TItem>(string container) where TItem : class
        {
            var result = GetAll(container);

            return result.Then(r => r.Select(JsonHelper.DeserializeIgnoringMissingMembers<TItem>).Combine());
        }
    }
}
