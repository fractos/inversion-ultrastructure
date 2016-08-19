using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;
using Inversion.Ultrastructure.Application.Behaviour;

using Ultrastructure.Demo1.Code.Behaviour;

namespace Ultrastructure.Demo1.Code
{
    public class Pipeline1 : IPipelineProvider
    {
        public void Register(IServiceContainerRegistrar registrar, IDictionary<string, string> settings)
        {
            registrar.RegisterService("application-behaviours",
                container => new List<IProcessBehaviour>
                {
                    new HelloWorldBehaviour("hello-from-pipeline0",
                        new Configuration.Builder
                        {
                            {"config", "message", "hello from pipeline1" }
                        }),

                    new SetContextItemsBehaviour("hello-from-pipeline0",
                        new Configuration.Builder
                        {
                            {"context", "set", "source", "pipeline1" }
                        }),

                    new PublishEventBehaviour("hello-from-pipeline0",
                        new Configuration.Builder
                        {
                            {"config", "transport", "pubsub" },
                            {"config", "message", "hello-from-pipeline1" }
                        })
                });
        }
    }
}