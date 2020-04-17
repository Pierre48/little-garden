using System;
using System.Threading.Tasks;
using LittleGarden.Core.Bus.Events;

namespace LittleGarden.Core.Bus
{
    public interface IBus
    {
        Task Publish<T>(T data) where T : IEvent;

        Task Subscribe<T>(Action<T> callback) where T : IEvent;
    }
}