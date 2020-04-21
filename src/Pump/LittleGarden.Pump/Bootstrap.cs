using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using LittleGarden.Core.Bus;
using LittleGarden.Core.Bus.Events;
using LittleGarden.Core.Entities;
using LittleGarden.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ppl.Core.Container;
using Ppl.Core.Extensions;
using Pump.Core;
using Pump.Core.Metrics;

namespace LittleGarden.Pump
{
    internal class Boostrap
    {
        public void Start()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .DefineInputParameters("MetricsPort", 9999)
                .DefineInputParameters("MongoDBConnectionString", "mongodb://root:example@localhost/")
                .DefineInputParameters("KAFKA_BOOTSTRAP_SERVERS","localhost:9092")
                .DefineInputParameters("KAFKA_SCHEMA_REGISTRY_URLS","http://localhost:8082/")
                .AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
                .AddSingleton<IHttpExtractor, HttpExtractor>()
                .AddSingleton<IBus, Bus>()
                .AddSingleton<IMetricsServer, MetricsServer>()
                .AddSingleton(typeof(IDataContext<>), typeof(DataContext<>))
                .AddAutoMapper(typeof(AutoMapperEventsProfile))
                .AddPumps()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogDebug("Starting application");

            //do the actual work here
            var parameters = serviceProvider.GetService<ContainerParameters>();
            var mapper = serviceProvider.GetService<IMapper>();
            parameters.LogParameters();
            var metricsServer = serviceProvider.GetService<IMetricsServer>();
            metricsServer.Open();
            var bus = serviceProvider.GetService<IBus>();
            var seedlingContext = serviceProvider.GetService<IDataContext<Seedling>>();
            var imageContext = serviceProvider.GetService<IDataContext<Image>>();
            var interestContext = serviceProvider.GetService<IDataContext<Interest>>();
            bus.Subscribe<ImageEvent>(async e =>
            {
                var created = await imageContext.Create(mapper.Map<Image>(e), x => x.Name == e.Name && x.Hash == e.Hash);
                if(created) logger.LogInformation($"Image for {e.Name} is saved ");
            });
            bus.Subscribe<SeedlingEvent>(async e =>
            {
                await seedlingContext.Save(mapper.Map<Seedling>(e), x=>x.Name == e.Name);
                logger.LogInformation($"Seedling {e.Name} is saved");

                e.Interet?.Split(",").ForEach(i => bus.Publish(new InterestEvent {Name = i.Trim()}));
            });
            bus.Subscribe<InterestEvent>(async e =>
            {
                var created = await interestContext.Create(mapper.Map<Interest>(e), x=>x.Name == e.Name);
                if(created) logger.LogInformation($"Interest {e.Name} is saved");
            });
            bus.Subscribe<ErrorEvent>(e =>
            {
                logger.LogError($"Error has occured for {e.Name}\r\n {e.Exception}\r\n{e.StackTrace}");
                if (!Directory.Exists("Errors")) Directory.CreateDirectory("Errors");
                var fileName = e.Name.Replace(@"\", "_").Replace(@"/", "_");
                File.WriteAllText($"Errors/{fileName}.json", JsonConvert.SerializeObject(e, Formatting.Indented));
            });

            serviceProvider.GetServices<IPump>().ForEach(p =>
            {
                logger.LogInformation($"Starting {p}");
                p.Run().ContinueWith(t => { logger.LogError(t.Exception.GetFullMessage()); },
                    TaskContinuationOptions.OnlyOnFaulted);
            });

            logger.LogDebug("All done!");
            Console.ReadLine();
        }
    }

    public static class PumpExtensions
    {
        public static IServiceCollection AddPumps(this IServiceCollection serviceCollection)
        {
            var location = Assembly.GetEntryAssembly()?.Location;
            var directory = Path.GetDirectoryName(location);
            var assemblies = Directory.GetFiles(directory, "Pump*.dll");

            foreach (var assembly in assemblies)
                Assembly.LoadFile(assembly)
                    .GetTypes()
                    .Where(t => null != t.GetInterface("IPump"))
                    .ForEach(t => serviceCollection.AddSingleton(typeof(IPump), t));
            return serviceCollection;
        }
    }
}