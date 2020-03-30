using LittleGarden.Core.Bus.Events;
using System;

namespace LittleGarden.Core.Bus
{
    public interface IBus
    {
         void Publish<T>(T data) where T : Event;

         void Subscribe<T>(Action<T> callback) where T : Event;
    }
}