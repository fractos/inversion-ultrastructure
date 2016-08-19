using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;

using Inversion.Ultrastructure.Application.Behaviour;

namespace Ultrastructure.Demo1.Code
{
    public class Pipeline0 : IPipelineProvider
    {
        public void Register(IServiceContainerRegistrar registrar, IDictionary<string, string> settings)
        {
            registrar.RegisterService("application-behaviours",
                container => new List<IProcessBehaviour>
                {
                    new SetContextItemsBehaviour("process-request",
                        new Configuration.Builder
                        {
                            {"context", "set", "source", "pipeline0" }
                        }),

                    new PublishEventBehaviour("process-request",
                        new Configuration.Builder
                        {
                            {"config", "transport", "pubsub" },
                            {"config", "message", "hello-from-pipeline0" }
                        })
                });
        }
    }
}