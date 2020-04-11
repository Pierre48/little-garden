using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LittleGarden.Data;
using Ppl.Core.Extensions;
using Ppl.Core.Docker;
using LittleGarden.Core.Bus;
using LittleGarden.Core.Entities;
using LittleGarden.Core.Bus.Events;
using System.IO;
using Newtonsoft.Json;
using Pump.Core;
using Pump.Core.Metrics;
using System.Threading.Tasks;

namespace LittleGarden.Pump
{
    class Boostrap
    {

        public void Start()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .DefineInputParameters("MetricsPort", 9999)
                .DefineInputParameters("MongoDBConnectionString", "mongodb://root:example@localhost/")
                .AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
                .AddSingleton<IHttpExtractor, HttpExtractor>()
                .AddSingleton<IBus, Bus>()
                .AddSingleton<IMetricsServer, MetricsServer>()
                .AddSingleton(typeof(IDataContext<>), typeof(DataContext<>))
                .AddPumps()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogDebug("Starting application");

            //do the actual work here
            var parameters = serviceProvider.GetService<ContainerParameters>();
            parameters.LogParameters();
            var metricsServer = serviceProvider.GetService<IMetricsServer>();
             metricsServer.Open();
            var bus = serviceProvider.GetService<IBus>();
            var seedlingContext = serviceProvider.GetService<IDataContext<Seedling>>();
            bus.Subscribe<EntityUpdated<Seedling>>(async e => await seedlingContext.Save(e.Entity));
            bus.Subscribe<EntityUpdated<Seedling>>(e => logger.LogInformation($"Seedling {e.Entity.NomVernaculaire} is saved"));
            bus.Subscribe<Error>(e =>
            {
                logger.LogError($"Error has occured for {e.Name}\r\n {e.Exception}\r\n{e.StackTrace}");
                if (!Directory.Exists("Errors")) Directory.CreateDirectory("Errors");
                var fileName = e.Name.Replace(@"\", "_").Replace(@"/", "_");
                File.WriteAllText($"Errors/{fileName}.json", JsonConvert.SerializeObject(e, Formatting.Indented));
            });

            serviceProvider.GetServices<IPump>().ForEach(p=>
            {
                logger.LogInformation($"Starting {p}");
                p.Run().ContinueWith(t =>
                {
                    logger.LogError(t.Exception.GetFullMessage());
                },TaskContinuationOptions.OnlyOnFaulted);
                });

            logger.LogDebug("All done!");
            Console.ReadLine();
        }
    }

    public static class PumpExtensions
    {
        public static IServiceCollection AddPumps(this IServiceCollection serviceCollection)
        {
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(location);
            var result = new List<IPump>();
            var assemblies = Directory.GetFiles(directory, "Pump*.dll");

            foreach (var assembly in assemblies)
            {
                Assembly.LoadFile(assembly)
                    .GetTypes()
                    .Where(t => null != t.GetInterface("IPump"))
                    .ForEach(t => serviceCollection.AddSingleton(typeof(IPump),t));
            }
            return serviceCollection;
        }
    }
}
