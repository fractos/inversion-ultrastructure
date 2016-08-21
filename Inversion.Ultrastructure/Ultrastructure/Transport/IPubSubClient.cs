using System;

using Inversion.Data;
using Inversion.Process;

namespace Inversion.Ultrastructure.Transport
{
    public interface IPubSubClient : IStoreHealth
    {
        void Subscribe(Func<bool> timeToGo, Action<string, string> handler);
        void Publish(IEvent ev);
    }
}