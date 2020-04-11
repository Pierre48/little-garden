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
using System.Threading;

namespace LittleGarden.Pump
{
    class Program
    {

        public  static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"File exist : {File.Exists("librocksdb.so")}");
                var bootstrap = new Boostrap();
                bootstrap.Start();
                while (true)
                {
                    //TODO Not the best way ?
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetFullMessage());
            }
        }
    }
}
