using System;
using System.Collections.Generic;
using System.Reflection;

using log4net;

using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Extensibility.Extensions;

namespace Ultrastructure.Demo1.Code.Behaviour
{
    public class HelloWorldBehaviour : PrototypedBehaviour
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HelloWorldBehaviour(string respondsTo) : base(respondsTo) {}
        public HelloWorldBehaviour(string respondsTo, IPrototype prototype) : base(respondsTo, prototype) {}
        public HelloWorldBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config) : base(respondsTo, config) {}

        public override void Action(IEvent ev, IProcessContext context)
        {
            string message = this.Configuration.GetNameWithAssert("config", "message");

            _log.Info(message);
        }
    }
}