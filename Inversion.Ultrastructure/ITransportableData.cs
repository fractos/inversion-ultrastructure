using System;

using Newtonsoft.Json.Linq;

namespace Inversion
{
    public interface ITransportableData<out T> : IData
    {
        T FromXml(string xml);
        T FromJToken(JToken source);
    }
}