using System.Threading.Tasks;

namespace LittleGarden.Core
{
    public interface IPump
    {
         int PumpDelayInSeconds {get;}
         Task Run();
    }
}   