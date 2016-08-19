using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;

using Ultrastructure.Demo1.Code.Behaviour;

namespace Ultrastructure.Demo1.Code
{
    public class Pipeline2 : IPipelineProvider
    {
        public void Register(IServiceContainerRegistrar registrar, IDictionary<string, string> settings)
        {
            registrar.RegisterService("application-behaviours",
                container => new List<IProcessBehaviour>
                {
                    new HelloWorldBehaviour("hello-from-pipeline1",
                        new Configuration.Builder
                        {
                            {"config", "message", "hello from pipeline2" }
                        })
                });
        }
    }
}