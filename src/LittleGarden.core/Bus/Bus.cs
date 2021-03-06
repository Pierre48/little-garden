using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LittleGarden.Core.Bus.Events;
using Microsoft.Extensions.Logging;
using Ppl.Core.Extensions;

namespace LittleGarden.Core.Bus
{
    public class Bus : IBus
    {
        private readonly Dictionary<Type, List<Call>> _callbacks = new Dictionary<Type, List<Call>>();

        public Bus(ILogger<Bus> logger)
        {
            Logger = logger;
        }

        public ILogger<Bus> Logger { get; }

        public async Task Publish<T>(T data) where T : IEvent
        {
            if (_callbacks.ContainsKey(typeof(T)))
                try
                {
                    _callbacks[typeof(T)].ForEach(x => x(data));
                }
                catch (Exception e)
                {
                    Logger.LogError($"Event {data} was not processed \n\a" + e.GetFullMessage());
                }
        }

        public async Task Subscribe<T>(Action<T> callback) where T : IEvent
        {
            lock (_callbacks)
            {
                if (!_callbacks.ContainsKey(typeof(T))) _callbacks.Add(typeof(T), new List<Call>());
            }

            var list = _callbacks[typeof(T)];
            list.Add(t => callback((T) t));
        }

        private delegate void Call(object o);
    }
}