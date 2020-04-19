using System;
using System.IO;
using System.Threading;
using Ppl.Core.Extensions;

namespace LittleGarden.Pump
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"File exist : {File.Exists("librocksdb.so")}");
                var bootstrap = new Boostrap();
                bootstrap.Start();
                while (true)
                    ///TODO Not the best way ?
                    Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetFullMessage());
            }
        }
    }
}