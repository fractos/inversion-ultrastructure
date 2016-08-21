using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Inversion.Process;
using Inversion.Collections;

namespace Inversion.Messaging.Model
{
    public class MessagingEventWithControlState : MessagingEvent
    {
        private readonly DataDictionary<object> _controlState = new DataDictionary<object>();

        public IDataDictionary<object> ControlState
        {
            get
            {
                return _controlState;
            }
        }

        public MessagingEventWithControlState(IProcessContext context, string message, DateTime created,
            IDictionary<string, string> parameters, IDictionary<string, object> controlState) : this(context, message, created, parameters)
        {
            _controlState = new DataDictionary<object>(controlState);
        }

        /// <summary>
        /// Instantiates a new event bound to a context.
        /// </summary>
        /// <param name="context">The context to which the event is bound.</param>
        /// <param name="message">The simple message the event represents.</param>
        /// <param name="created">The temporal part of the event.</param>
        /// <param name="parameters">The parameters of the event.</param>
        public MessagingEventWithControlState(IProcessContext context, string message, DateTime created, IDictionary<string, string> parameters)
            : this(context, message, null, created, parameters) {}

        /// <summary>
        /// Instantiates a new event bound to a context.
        /// </summary>
        /// <param name="context">The context to which the event is bound.</param>
        /// <param name="message">The simple message the event represents.</param>
        /// <param name="obj">An object being carried by the event.</param>
        /// <param name="created">The temporal part of the event.</param>
        /// <param name="parameters">The parameters of the event.</param>
        public MessagingEventWithControlState(IProcessContext context, string message, IData obj, DateTime created,
            IDictionary<string, string> parameters)
            : base(context, message, obj, created, parameters) {}

		/// <summary>
		/// Instantiates a new event bound to a context.
		/// </summary>
		/// <param name="context">The context to which the event is bound.</param>
		/// <param name="message">The simple message the event represents.</param>
        /// <param name="created">The temporal part of the event.</param>
		/// <param name="parms">
		/// A sequnce of context parameter names that should be copied from the context
		/// to this event.
		/// </param>
		public MessagingEventWithControlState(IProcessContext context, string message, DateTime created, params string[] parms) : base(context, message, null, created, parms) { }

        /// <summary>
        /// Instantiates a new event bound to a context.
        /// </summary>
        /// <param name="context">The context to which the event is bound.</param>
        /// <param name="message">The simple message the event represents.</param>
        /// <param name="obj">An object being carried by the event.</param>
        /// <param name="created">The temporal part of the event.</param>
        /// <param name="parms">
        /// A sequnce of context parameter names that should be copied from the context
        /// to this event.
        /// </param>
        public MessagingEventWithControlState(IProcessContext context, string message, IData obj, DateTime created,
            params string[] parms) : base(context, message, obj, created, parms) {}

		/// <summary>
		/// Instantiates a new event as a copy of the event provided.
		/// </summary>
		/// <param name="ev">The event to copy for this new instance.</param>
        public MessagingEventWithControlState(IEvent ev) : base(ev) {}

        /// <summary>
        /// Instantiates a new event as a copy of the event provided and also allows setting of Created value.
        /// </summary>
        /// <param name="ev">The event to copy for this new instance.</param>
        /// <param name="created">DateTime to set Creatd field to.</param>
        public MessagingEventWithControlState(IEvent ev, DateTime created) : base(ev, created) {}

        public MessagingEventWithControlState(MessagingEvent ev) : base(ev) {}

        /// <summary>
        /// Creates a string representation of the event.
        /// </summary>
        /// <returns>Returns a new string representing the event.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("(event @message {0}:{1:o}\n", this.Message, this.Created);
            foreach (KeyValuePair<string, string> entry in this.Params)
            {
                sb.AppendFormat("   ({0} -{1})\n", entry.Key, entry.Value);
            }
            sb.AppendLine(")");

            return sb.ToString();
        }

        /// <summary>
        /// Produces an xml representation of the model.
        /// </summary>
        /// <param name="xml">The writer to used to write the xml to. </param>
        public override void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("event");

            xml.WriteAttributeString("message", this.Message);
            xml.WriteAttributeString("created", this.Created.ToString("o"));

            xml.WriteStartElement("params");
            foreach (KeyValuePair<string, string> entry in this.Params)
            {
                xml.WriteStartElement("item");
                xml.WriteAttributeString("name", entry.Key);
                xml.WriteAttributeString("value", entry.Value);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            xml.WriteStartElement("control-state");
            foreach (KeyValuePair<string, object> kvp in this.ControlState)
            {
                IData item = kvp.Value as IData;
                if (item != null)
                {
                    xml.WriteStartElement("item");
                    xml.WriteAttributeString("key", kvp.Key);
                    xml.WriteAttributeString("type", item.GetType().AssemblyQualifiedName);
                    item.ToXml(xml);
                    xml.WriteEndElement();
                }
            }
            xml.WriteEndElement();

            xml.WriteEndElement();
        }

        /// <summary>
        /// Produces a json respresentation of the model.
        /// </summary>
        /// <param name="json">The writer to use for producing json.</param>
        public override void ToJson(JsonWriter json)
        {
            json.WriteStartObject();

            json.WritePropertyName("_type");
            json.WriteValue("event");
            json.WritePropertyName("_created");
            json.WriteValue(this.Created.ToString("o"));
            json.WritePropertyName("message");
            json.WriteValue(this.Message);

            json.WritePropertyName("params");
            json.WriteStartObject();
            foreach (KeyValuePair<string, string> kvp in this.Params)
            {
                json.WritePropertyName(kvp.Key);
                json.WriteValue(kvp.Value);
            }
            json.WriteEndObject();

            json.WritePropertyName("control-state");
            json.WriteStartArray();
            foreach (KeyValuePair<string, object> kvp in this.ControlState)
            {
                IData item = kvp.Value as IData;
                if (item != null)
                {
                    json.WriteStartObject();
                    json.WritePropertyName("key");
                    json.WriteValue(kvp.Key);
                    json.WritePropertyName("type");
                    json.WriteValue(item.GetType().AssemblyQualifiedName);
                    json.WritePropertyName("object");
                    item.ToJson(json);
                    json.WriteEndObject();
                }
            }
            json.WriteEndArray();

            json.WriteEndObject();
        }

        /// <summary>
        /// Creates a new event from an xml representation.
        /// </summary>
        /// <param name="context">The context to which the new event will be bound.</param>
        /// <param name="xml">The xml representation of an event.</param>
        /// <returns>Returns a new event.</returns>
        public new static Event FromXml(IProcessContext context, string xml)
        {
            try
            {
                XElement ev = XElement.Parse(xml);
                if (ev.Name == "event")
                {
                    return new MessagingEventWithControlState(
                        context,
                        ev.Attribute("message").Value,
                        DateTime.Parse(ev.Attribute("created").Value),
                        ev.Elements().ToDictionary(el => el.Attribute("name").Value, el => el.Attribute("value").Value)
                    );
                }
                else
                {
                    throw new ParseException("The expressed type of the json provided does not appear to be an event.");
                }
            }
            catch (Exception err)
            {
                throw new ParseException("An unexpected error was encoutered parsing the provided json into an event object.", err);
            }
        }

        /// <summary>
        /// Creates a new event from an json representation.
        /// </summary>
        /// <param name="context">The context to which the new event will be bound.</param>
        /// <param name="json">The json representation of an event.</param>
        /// <returns>Returns a new event.</returns>
        public new static Event FromJson(IProcessContext context, string json)
        {
            try
            {
                using (JsonReader reader = new JsonTextReader(new StringReader(json)))
                {
                    reader.DateParseHandling = DateParseHandling.None;
                    JObject job = JObject.Load(reader);

                    if (job.Value<string>("_type") == "event")
                    {
                        DataDictionary<object> controlState = new DataDictionary<object>();
                        if (job["control-state"] != null)
                        {
                            foreach (JToken item in job.Value<JArray>("control-state").Children())
                            {
                                string key = item.Value<string>("key");
                                string typeName = item.Value<string>("type");
                                JToken token = item.Value<JObject>("object");

                                Type type = Type.GetType(typeName);
                                if (type == null)
                                {
                                    continue;
                                }
                                object transportableObject = Activator.CreateInstance(type);

                                if (transportableObject != null)
                                {
                                    MethodInfo method = type.GetMethod("FromJToken");
                                    var result = method.Invoke(transportableObject, new object[] {token});

                                    if (result != null)
                                    {
                                        controlState.Add(key, result);
                                    }
                                }
                            }
                        }
                        return new MessagingEventWithControlState(
                            context: context,
                            message: job.Value<string>("message"),
                            created: job.Value<string>("_created") != null
                                ? DateTime.Parse(job.Value<string>("_created"))
                                : DateTime.MinValue,
                            parameters: job.Value<JObject>("params").HasValues
                                ? job.Value<JObject>("params").Properties().ToDictionary(p => p.Name, p => p.Value.ToString())
                                : new Dictionary<string, string>(),
                            controlState: controlState
                        );
                    }
                    throw new ParseException("The expressed type of the json provided does not appear to be an event.");
                }
            }
            catch (Exception err)
            {
                throw new ParseException("An unexpected error was encountered parsing the provided json into an event object.", err);
            }
        }
    }
}