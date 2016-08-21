using System;
using System.Linq;

using Newtonsoft.Json.Linq;

using Inversion;

namespace Ultrastructure.Demo3.Code.Model
{  
    public class TransportableNamedTextData : NamedTextData, ITransportableData<TransportableNamedTextData>
    {
        public TransportableNamedTextData() : base(String.Empty, String.Empty) { }

        public TransportableNamedTextData(NamedTextData text) : base(text) { }
        public TransportableNamedTextData(string name, string text) : base(name, text) { }

        public TransportableNamedTextData FromXml(string xml)
        {
            throw new NotImplementedException();
        }

        public TransportableNamedTextData FromJToken(JToken source)
        {
            JToken firstChild = source.Children().FirstOrDefault();
            if (firstChild != null)
            {
                string name = ((JProperty) firstChild).Name;
                return new TransportableNamedTextData(
                    name: name,
                    text: source[name].Value<string>()
                );
            }
            return null;
        }
    }
}