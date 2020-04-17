using System;
using System.Net;
using System.Reflection;
using Confluent.Kafka;
using LittleGarden.Core.Bus.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Ppl.Core.Container;
using Ppl.Core.Extensions;

namespace LittleGarden.Core.Bus
{
    public class AvroKafkaBus : IBus
    {
        private readonly ContainerParameters _parameters;
        private readonly ILogger<JSonKafkaBus> _logger;

        public AvroKafkaBus(ILogger<JSonKafkaBus> logger,ContainerParameters parameters)
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
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = _parameters.GetStringParameter("KAFKA_SCHEMA_REGISTRY_URLS")
            };

            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            using (var p = new ProducerBuilder<Null, T>(config)
                .SetValueSerializer(new AvroSerializer<T>(schemaRegistry).AsSyncOverAsync())
                .SetErrorHandler((_, e) => _logger.LogError($"Exception has occured when trying to publish an event : {e.Reason}"))
                .Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(GetTopicName<T>(), new Message<Null, T> { Value=data });
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
            return $"busavro-{typeof(T).FullName}".ToLower();
        }

        public async Task Subscribe<T>(Action<T> callback) where T : IEvent
        {
            var conf = new ConsumerConfig
            { 
                GroupId = $"{Dns.GetHostName()}-{Assembly.GetExecutingAssembly()}",
                BootstrapServers = _parameters.GetStringParameter("KAFKA_BOOTSTRAP_SERVERS"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = _parameters.GetStringParameter("KAFKA_SCHEMA_REGISTRY_URLS")
            };
            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            using (var c = new ConsumerBuilder<Ignore, T>(conf)
                .SetValueDeserializer(new AvroDeserializer<T>(schemaRegistry).AsSyncOverAsync())
                .SetErrorHandler((_, e) => _logger.LogError($"Exception has occured when trying to consume an event : {e.Reason}"))
                .Build())
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
                                if (cr.Message.Value == null) continue;
                                _logger.LogDebug($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                                try
                                {
                                    callback(cr.Message.Value);
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