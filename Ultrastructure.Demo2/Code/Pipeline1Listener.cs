using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;
using Inversion.Ultrastructure.Application.Behaviour;

using Ultrastructure.Demo2.Code.Behaviour;

namespace Ultrastructure.Demo2.Code
{
    public class Pipeline1Listener : IPipelineProvider
    {
        public void Register(IServiceContainerRegistrar registrar, IDictionary<string, string> settings)
        {
            registrar.RegisterService("application-behaviours",
                container => new List<IProcessBehaviour>
                {
                    new HelloWorldBehaviour("hello-from-pump",
                        new Configuration.Builder
                        {
                            {"config", "message", "hello from listener1" }
                        }),

                    new SetContextItemsBehaviour("hello-from-pump",
                        new Configuration.Builder
                        {
                            {"context", "set", "source", "listener1" }
                        }),

                    new PublishEventBehaviour("hello-from-pump",
                        new Configuration.Builder
                        {
                            {"config", "transport", "pubsub" },
                            {"config", "message", "hello-from-listener1" }
                        })
                });
        }
    }
}