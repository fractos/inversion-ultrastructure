using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using log4net;

using Inversion.Collections;
using Inversion.Extensibility.Extensions;
using Inversion.Messaging.Model;
using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Ultrastructure.Transport;

namespace Inversion.Ultrastructure.Application.Behaviour
{
    /// <summary>
    /// Publish an event (MessagingEventWithControlState) that has specified items copied into its control state from the context using the specified IPubSubClient transport
    /// </summary>
    public class PublishEventWithControlStateBehaviour : PrototypedBehaviour
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PublishEventWithControlStateBehaviour(string respondsTo) : base(respondsTo) {}
        public PublishEventWithControlStateBehaviour(string respondsTo, IPrototype prototype) : base(respondsTo, prototype) {}
        public PublishEventWithControlStateBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config) : base(respondsTo, config) {}

        public override void Action(IEvent ev, IProcessContext context)
        {
            string transport = this.Configuration.GetNameWithAssert("config", "transport");
            string message = this.Configuration.GetNameWithAssert("config", "message");
            string controlStateKeys = this.Configuration.GetNameWithAssert("config", "control-state-keys");

            _log.Debug(String.Format("about to publish event {0}\r\n----\r\n", message));

            List<string> whitelist = controlStateKeys.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

            DataDictionary<object> controlState =
                new DataDictionary<object>(context.ControlState.Where(c => whitelist.Any(wl => wl == c.Key)));

            MessagingEventWithControlState publishEvent = new MessagingEventWithControlState(context, message, DateTime.Now, context.Params, controlState);

            using (IPubSubClient pubSubClient = context.Services.GetService<IPubSubClient>(transport))
            {
                pubSubClient.Start();

                pubSubClient.Publish(publishEvent);
            }
        }
    }
}