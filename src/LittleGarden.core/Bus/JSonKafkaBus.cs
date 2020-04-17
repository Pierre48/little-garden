using System;
using System.Net;
using System.Reflection;
using Confluent.Kafka;
using LittleGarden.Core.Bus.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ppl.Core.Container;
using Ppl.Core.Extensions;

namespace LittleGarden.Core.Bus
{
    public class JSonKafkaBus : IBus
    {
        private readonly ContainerParameters _parameters;
        private readonly ILogger<JSonKafkaBus> _logger;

        public JSonKafkaBus(ILogger<JSonKafkaBus> logger,ContainerParameters parameters)
        {
            _parameters = parameters;
            _logger = logger;
        }


        public async Task Publish<T>(T data) where T : IEvent
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _parameters.GetStringParameter("KAFKA_BOOTSTRAP_SERVERS"),
               // Debug = "all"
            };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(GetTopicName<T>(), new Message<Null, string> { Value=JsonSerializer.Serialize(data) });
                    _logger.LogDebug($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    _logger.LogError($"Delivery failed: {e.Error.Reason}");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Delivery failed: {e.GetFullMessage()}");
                    throw;
                }
            }
        }

        private string GetTopicName<T>()
        {
            return $"bus-{typeof(T).FullName}".ToLower();
        }

        public async Task Subscribe<T>(Action<T> callback) where T : IEvent
        {
            var conf = new ConsumerConfig
            { 
                GroupId = $"{Dns.GetHostName()}-{Assembly.GetExecutingAssembly()}",
                BootstrapServers = _parameters.GetStringParameter("KAFKA_BOOTSTRAP_SERVERS"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(GetTopicName<T>());

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var cr = c.Consume(cts.Token);
                                if (cr?.Message.Value == null) continue;
                                _logger.LogDebug($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                                try
                                {
                                    T data = JsonSerializer.Deserialize<T>(cr.Message.Value);
                                    callback(data);
                                }
                                catch (Exception e) 
                                {
                                    _logger.LogError($"Event {cr.Message.Value} was not processed \n\a" + e.GetFullMessage());
                                }
                            }
                            catch (ConsumeException e)
                            {
                                _logger.LogError($"Error occured: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        c.Close();
                    }
                }, cts.Token);
            }
        }

    }
}