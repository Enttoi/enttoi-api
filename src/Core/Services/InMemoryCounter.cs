using Core.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public class InMemoryCounter : IDistributedCounter
    {
        private ConcurrentDictionary<string, byte> _store;
        private BlockingCollection<Func<int, Task>> _callbacks;

        public InMemoryCounter()
        {
            _store = new ConcurrentDictionary<string, byte>();
            _callbacks = new BlockingCollection<Func<int, Task>>();
        }

        public Task Add(string requestId)
        {
            if (String.IsNullOrEmpty(requestId)) throw new ArgumentNullException(nameof(requestId));
            
            _store.AddOrUpdate(
                requestId,
                byte.MinValue,
                (key, value) => value);
            return notifyOnChange(_store.Count);
        }

        public Task Subtruct(string requestId)
        {
            if (String.IsNullOrEmpty(requestId)) throw new ArgumentNullException(nameof(requestId));
            
            byte dummyValue;
            _store.TryRemove(requestId, out dummyValue);
            return notifyOnChange(_store.Count);
        }

        public Task Reset()
        {
            _store.Clear();
            return notifyOnChange(_store.Count);
        }

        public Task<int> GetCurrent()
        {
            return Task.FromResult(_store.Count);
        }

        public void OnCounterChangedAsync(Func<int, Task> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            _callbacks.Add(callback);
        }

        private Task notifyOnChange(int value)
        {
            return Task.WhenAll(
                _callbacks.Select(callback => callback(value)));
        }
    }
}
