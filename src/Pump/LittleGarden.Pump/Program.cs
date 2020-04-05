using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LittleGarden.Data;
using Ppl.Core.Extensions;
using LittleGarden.Core.Bus;
using LittleGarden.Core.Entities;
using LittleGarden.Core.Bus.Events;
using System.IO;
using Newtonsoft.Json;
using Pump.Core;
using Pump.Core.Metrics;

namespace LittleGarden.Pump
{
    class Program
    {

        public  static void Main(string[] args)
        {

            var bootstrap = new Boostrap();
            bootstrap.Start().GetAwaiter();
            Console.ReadLine();
        }
    }
}
