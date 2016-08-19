﻿using System;

using Inversion.Data;
using Inversion.Process;

namespace Inversion.Ultrastructure.Transport
{
    public interface IPubSubClient : IStoreHealth
    {
        void Subscribe(IProcessContext context, Func<bool> timeToGo);
        void Publish(IEvent ev, IProcessContext context);
    }
}