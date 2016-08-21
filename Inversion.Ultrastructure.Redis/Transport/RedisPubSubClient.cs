using System;
using System.Reflection;
using System.Threading.Tasks;

using StackExchange.Redis;

using Inversion.Data.Redis;
using Inversion.Messaging.Model;
using Inversion.Process;
using log4net;

namespace Inversion.Ultrastructure.Transport
{
    public class RedisPubSubClient : RedisStore, IPubSubClient
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly StackExchange.Redis.RedisChannel _channel;

        private readonly int _cancellationCycleTimeMS;

        private readonly Guid _subscriberID = Guid.NewGuid();

        public RedisPubSubClient(string channel, string connections, int databaseNumber, int cancellationCycleTimeMS = 100)
            : base(connections, databaseNumber)
        {
            _channel =
                new StackExchange.Redis.RedisChannel(channel, StackExchange.Redis.RedisChannel.PatternMode.Pattern);
            _cancellationCycleTimeMS = cancellationCycleTimeMS;
        }

        public void Subscribe(IProcessContext context, Func<bool> timeToGo, Action<string, string> handler)
        {

            ISubscriber subscriber = this.ConnectionMultiplexer.GetSubscriber();

            TaskFactory taskFactory = new TaskFactory();

            Task cancellationTask = taskFactory.StartNew(() =>
            {
                while(!timeToGo())
                {
                    System.Threading.Thread.Sleep(_cancellationCycleTimeMS);
                }

                subscriber.Unsubscribe(_channel);
            }, TaskCreationOptions.LongRunning);

            subscriber.Subscribe(
                _channel,
                (eventChannel, eventValue) => handler(eventChannel, eventValue));

            cancellationTask.Wait();
        }

        public void Publish(IEvent ev, IProcessContext context)
        {
            _log.Debug(String.Format("{0} published {1}\r\n----\r\n", _subscriberID, ev.Message));

            this.Database.Publish(_channel, ev.ToJson());
        }
    }
}