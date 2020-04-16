using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ppl.Core.Extensions;

namespace Ppl.Core.Docker
{
    public static class ContainerParametersExtensions
    {
        internal static ContainerParameters Container = null;

        public static IServiceCollection DefineInputParameters<T>(this IServiceCollection services, string name,
            T value)
        {
            lock (services)
            {
                if (Container == null)
                {
                    services.AddSingleton<ContainerParameters>();
                    ContainerParameters.parameters[name] = value;
                }
            }

            return services;
        }
    }

    public class ContainerParameters
    {
        internal static readonly Dictionary<string, object> parameters = new Dictionary<string, object>();
        internal readonly Dictionary<string, object> defaultParameters = new Dictionary<string, object>();

        public ContainerParameters(ILogger<ContainerParameters> logger)
        {
            parameters.ForEach(kv => defaultParameters[kv.Key] = kv.Value);
            foreach (string k in Environment.GetEnvironmentVariables().Keys)
                parameters[k] = Environment.GetEnvironmentVariable(k);
            Logger = logger;
        }

        private ILogger<ContainerParameters> Logger { get; }

        public void LogParameters()
        {
            var strb = new StringBuilder();
            parameters.ForEach(kv =>
                {
                    strb.AppendLine($" - Parameter {kv.Key} = {kv.Value}.");
                    if (defaultParameters.ContainsKey(kv.Key))
                        strb.AppendLine($"   Default value was {defaultParameters[kv.Key]}.");
                }
            );
            Logger.LogDebug(strb.ToString());
        }

        public int GetIntParameter(string name)
        {
            object value;
            if (!parameters.TryGetValue(name, out value)) throw new ArgumentException($"Unknown parameter : {name}");
            Logger.LogDebug($"Get value {name} : {value}");
            return value == null ? 0 : int.Parse(value.ToString());
        }

        public string GetStringParameter(string name)
        {
            object value;
            if (!parameters.TryGetValue(name, out value)) throw new ArgumentException($"Unknown parameter : {name}");
            return value?.ToString();
        }
    }
}