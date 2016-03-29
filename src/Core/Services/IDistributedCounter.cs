using Core.Models;
using System;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IDistributedCounter
    {
        Task Add(string requestId);

        Task Subtruct(string requestId);

        Task Reset();

        Task<int> GetCurrent();

        void OnCounterChangedAsync(Func<int, Task> callback);
    }
}
