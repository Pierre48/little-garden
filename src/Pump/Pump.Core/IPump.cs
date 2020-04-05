using System.Threading.Tasks;

namespace Pump.Core
{
    public interface IPump
    {
         int PumpDelayInSeconds {get;}
         Task Run();
    }
}   