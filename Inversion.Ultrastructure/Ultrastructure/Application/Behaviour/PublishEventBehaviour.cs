using System;
using System.Collections.Generic;
using System.Reflection;

using log4net;

using Inversion.Extensibility.Extensions;
using Inversion.Messaging.Model;
using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Ultrastructure.Transport;

namespace Inversion.Ultrastructure.Application.Behaviour
{
    /// <summary>
    /// Publish an event (MessagingEvent) using the specified IPubSubClient transport
    /// </summary>
    public class PublishEventBehaviour : PrototypedBehaviour
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PublishEventBehaviour(string respondsTo) : base(respondsTo) {}
        public PublishEventBehaviour(string respondsTo, IPrototype prototype) : base(respondsTo, prototype) {}
        public PublishEventBehaviour(string respondsTo, IEnumerable<IConfigurationElement> config) : base(respondsTo, config) {}

        public override void Action(IEvent ev, IProcessContext context)
        {
            string transport = this.Configuration.GetNameWithAssert("config", "transport");
            string message = this.Configuration.GetNameWithAssert("config", "message");

            _log.Debug(String.Format("about to publish event {0}\r\n----\r\n", message));

            MessagingEvent publishEvent = new MessagingEvent(context, message, DateTime.Now, context.Params);

            using (IPubSubClient pubSubClient = context.Services.GetService<IPubSubClient>(transport))
            {
                pubSubClient.Start();

                pubSubClient.Publish(publishEvent);
            }
        }
    }
}