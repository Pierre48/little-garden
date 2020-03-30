using LittleGarden.Core.Bus.Events;
using System;
using System.Collections.Generic;

namespace LittleGarden.Core.Bus
{
    public class Bus : IBus
    {
        readonly Dictionary<Type, List<Call>> _callbacks = new Dictionary<Type, List<Call>>();
        public void Publish<T>(T data) where T : Event
        {
            if (_callbacks.ContainsKey(typeof(T)))
            {
                _callbacks[typeof(T)].ForEach(x => x(data));
            }
        }
        delegate void Call(object o);
        public void Subscribe<T>(Action<T> callback) where T:Event
        {
            lock (_callbacks)
            {
                if (!_callbacks.ContainsKey(typeof(T)))
                {
                    _callbacks.Add(typeof(T), new List<Call>());
                }
            }
            var list = _callbacks[typeof(T)];
            list.Add(t => callback((T)t));
        }
    }
}