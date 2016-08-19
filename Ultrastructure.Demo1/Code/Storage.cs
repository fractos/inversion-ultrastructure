using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Pipeline;
using Inversion.Ultrastructure.Transport;

namespace Ultrastructure.Demo1.Code
{
    public class Storage : IPipelineProvider
    {
        public void Register(IServiceContainerRegistrar registrar, IDictionary<string, string> settings)
        {
            string redisConnectionString = settings["redis"];
            int redisDatabaseNumber = Convert.ToInt32(settings["redis-pubsub-database"]);

            registrar.RegisterServiceNonSingleton<IPubSubClient>("pubsub",
                container => new RedisPubSubClient("global", redisConnectionString, redisDatabaseNumber));
        }
    }
}