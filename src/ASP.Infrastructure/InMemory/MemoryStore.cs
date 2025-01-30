using ASP.Core.Results;
using ASP.Core.Text;

namespace ASP.Infrastructure.InMemory
{
    public class MemoryStore<TKey> 
        where TKey : notnull
    {
        private Dictionary<string, Dictionary<TKey, MemoryStoreItem<TKey>>> _store = new();

        public void Clear()
        {
            _store = new();
        }

        public void ClearContainer(string containerKey)
        {
            _store[containerKey] = new();
        }

        public void Set(string containerKey, TKey key, object document)
        {
            Set(containerKey, key, JsonHelper.Serialize(document));
        }

        public void Set(string containerKey, TKey key, string document)
        {
            EnsureContainer(containerKey);

            _store[containerKey][key] = new(key, document);
        }

        public Result<MemoryStoreItem<TKey>> Get(string containerKey, TKey key)
        {
            EnsureContainer(containerKey);

            if (!_store[containerKey].ContainsKey(key))
            {
                return Error.NotFound($@"Could not find the object with key ""{key}"" in container ""{containerKey}"".");
            }

            return _store[containerKey][key];
        }

        public Result<IEnumerable<MemoryStoreItem<TKey>>> GetAll(string containerKey)
        {
            EnsureContainer(containerKey);

            var values = _store[containerKey].Values;

            return Result.Success(values.AsEnumerable());
        }

        private void EnsureContainer(string containerKey)
        {
            if (!_store.ContainsKey(containerKey))
            {
                _store[containerKey] = new();
            }
        }
    }
}
