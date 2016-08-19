using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;
using Inversion.Ultrastructure.Application.Behaviour;

using Ultrastructure.Demo2.Code.Behaviour;

namespace Ultrastructure.Demo2.Code
{
    public class Pipeline2Listener : IPipelineProvider
    {
        public void Register(IServiceContainerRegistrar registrar, IDictionary<string, string> settings)
        {
            registrar.RegisterService("application-behaviours",
                container => new List<IProcessBehaviour>
                {
                    new HelloWorldBehaviour("hello-from-listener1",
                        new Configuration.Builder
                        {
                            {"config", "message", "hello from listener2" }
                        })
                });
        }
    }
}