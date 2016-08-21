using System;
using System.Collections.Generic;

using Inversion.Extensibility.Extensions;
using Inversion.Process;
using Inversion.Process.Behaviour;

using Ultrastructure.Demo3.Code.Model;

namespace Ultrastructure.Demo3.Code.Behaviour
{
    public class CreateHelloWorldTransportableDataBehaviour : PrototypedBehaviour
    {
        public CreateHelloWorldTransportableDataBehaviour(string respondsTo) : base(respondsTo) {}
        public CreateHelloWorldTransportableDataBehaviour(string respondsTo, IPrototype prototype) : base(respondsTo, prototype) {}
        public CreateHelloWorldTransportableDataBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config) : base(respondsTo, config) {}

        public override void Action(IEvent ev, IProcessContext context)
        {
            string outputKey = this.Configuration.GetNameWithAssert("config", "output-key");
            string message = this.Configuration.GetNameWithAssert("config", "message");

            TransportableNamedTextData data = new TransportableNamedTextData(outputKey, message);

            context.ControlState[outputKey] = data;
        }
    }
}