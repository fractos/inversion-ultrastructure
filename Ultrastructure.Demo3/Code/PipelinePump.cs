using System.Collections.Generic;
using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;
using Inversion.Ultrastructure.Application.Behaviour;
using Ultrastructure.Demo3.Code.Behaviour;

namespace Ultrastructure.Demo3.Code
{
    public class PipelinePump : IPipelineProvider
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

                    new CreateHelloWorldTransportableDataBehaviour("process-request",
                         new Configuration.Builder
                         {
                             {"config", "output-key", "hello" },
                             {"config", "message", "world" }
                         }),

                    new PublishEventWithControlStateBehaviour("process-request",
                        new Configuration.Builder
                        {
                            {"config", "transport", "pubsub" },
                            {"config", "message", "hello-from-pump" },
                            {"config", "control-state-keys", "hello" }
                        })
                });
        }
    }
}