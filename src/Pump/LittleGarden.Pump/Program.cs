using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LittleGarden.Core;
using LittleGarden.Data;
using LittleGarden.Core.Extensions;
using LittleGarden.Core.Bus;
using LittleGarden.Core.Entities;
using LittleGarden.Core.Bus.Events;

namespace LittleGarden.Pump
{
    class Program
    {

        public static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
                .AddSingleton<IBus, Bus>()
                .AddSingleton(typeof(IDataContext<>), typeof(DataContext<>))
                .AddPumps()
                .BuildServiceProvider();
            
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            //do the actual work here
            var bus = serviceProvider.GetService<IBus>();
            var seedlingContext = serviceProvider.GetService<IDataContext<Seedling>>();
            bus.Subscribe<EntityUpdated<Seedling>>(async e => await seedlingContext.Save(e.Entity));
            bus.Subscribe<EntityUpdated<Seedling>>(e => logger.LogInformation($"Star {e.Entity.Name} is saved"));

            serviceProvider.GetServices<IPump>().ForEach(p=>p.Run());

            logger.LogDebug("All done!");
            Console.ReadLine();
        }
    }

    public static class PumpExtensions
    {
        public static IServiceCollection AddPumps(this IServiceCollection serviceCollection)
        {
            var result = new List<IPump>();
            var assemblies = new string[] {
                @"C:\git\little-garden\src\Pump\Pumps\PumpComptoirDesGraines\bin\Debug\netstandard2.0\PumpComptoirDesGraines.dll"
            };
            foreach (var assembly in assemblies)
            {
                Assembly.LoadFile(assembly)
                    .GetTypes()
                    .Where(t => null != t.GetInterface("Pump.Core.IPump"))
                    .ForEach(t => serviceCollection.AddSingleton(typeof(IPump),t));
            }
            return serviceCollection;
        }
    }
}
