using System;
using Microsoft.Extensions.Logging;

namespace Ppl.Core.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogException(this ILogger logger, Exception e)
        {
            logger.LogError($"An exception has occured : {e.GetFullMessage()}");
        }
    }
}