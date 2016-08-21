using System;
using System.Threading.Tasks;

using StackExchange.Redis;

using Inversion.Data.Redis;
using Inversion.Process;

namespace Inversion.Ultrastructure.Transport
{
    public class RedisPubSubClient : RedisStore, IPubSubClient
    {
        private readonly RedisChannel _channel;

        private readonly int _cancellationCycleTimeMS;

        public RedisPubSubClient(string channel, string connections, int databaseNumber, int cancellationCycleTimeMS = 100)
            : base(connections, databaseNumber)
        {
            _channel = new RedisChannel(channel, RedisChannel.PatternMode.Pattern);
            _cancellationCycleTimeMS = cancellationCycleTimeMS;
        }

        /// <summary>
        /// Use Redis Subscriber mechanism to subscribe to a particular channel.
        /// Call the passed handler on message arrival.
        /// Unsubscribe from the channel when the passed timeToGo function returns true.
        /// </summary>
        /// <param name="timeToGo">function that returns true when the subscription should be detached</param>
        /// <param name="handler">function that will have the eventChannel and eventValue strings passed to it when a message is received</param>
        public void Subscribe(Func<bool> timeToGo, Action<string, string> handler)
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

        /// <summary>
        /// Publish the passed event in JSON format on the store's channel.
        /// </summary>
        /// <param name="ev"></param>
        public void Publish(IEvent ev)
        {
            this.Database.Publish(_channel, ev.ToJson());
        }
    }
}