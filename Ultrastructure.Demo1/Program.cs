using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;

using Inversion.Data;
using Inversion.Messaging.Model;
using Inversion.Naiad;
using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;
using Inversion.Ultrastructure.Transport;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Ultrastructure.Demo1
{
    public class Program

    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<string, ServiceContainer> _pipelines = new Dictionary<string, ServiceContainer>();
        private static bool _requestToQuit = false;

        static void Lifecycle(IProcessContext context, Func<bool> timeToGo)
        {
            // register to a signal receiver so that we can quit on command
            // sub using adaptor
            // - pass fully registered context

            using (IPubSubClient pubSubClient = context.Services.GetService<IPubSubClient>("pubsub"))
            {
                pubSubClient.Start();

                pubSubClient.Subscribe(context, timeToGo, (eventChannel, eventValue) =>
                {
                    _log.Debug(String.Format("received {0}\r\n----\r\n", eventValue));

                    IEvent ev = MessagingEvent.FromJson(context, eventValue);

                    context.Fire(ev);
                });
            }
        }

        static IProcessContext CreatePipeline(string name)
        {
            IDictionary<string, string> settings = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                settings.Add(key, ConfigurationManager.AppSettings[key]);
            }

            ServiceContainer instance = new ServiceContainer();

            PipelineConfigurationHelper.Configure(instance, settings, new List<string>
            {
                settings["prototype-provider"],
                settings[name],
                settings["storage-provider"]
            });

            _pipelines.Add(name, instance);

            IProcessContext context = new ProcessContext(instance, FileSystemResourceAdapter.Instance);

            context.Register(
                context.Services.GetService<IList<IProcessBehaviour>>("application-behaviours"));

            return context;
        }

        static Task PubSubLifecycle(TaskFactory taskFactory, IProcessContext context)
        {
            return taskFactory.StartNew(() => Lifecycle(context, TimeToGo), TaskCreationOptions.LongRunning);
        }

        static bool TimeToGo()
        {
            return _requestToQuit;
        }

        static void Main(string[] args)
        {
            TaskFactory taskFactory = new TaskFactory();

            List<Task> pipelines = new List<Task>
            {
                //taskFactory.StartNew(() => { while(true) { } })
                PubSubLifecycle(taskFactory, CreatePipeline("pipeline1")),
                PubSubLifecycle(taskFactory, CreatePipeline("pipeline2"))
            };

            Task primaryPipeline = taskFactory.StartNew(() =>
            {
                IProcessContext context = CreatePipeline("pipeline0");

                while (!_requestToQuit)
                {
                    context.Fire("process-request");
                    System.Threading.Thread.Sleep(1);
                }
            }, TaskCreationOptions.LongRunning);

            Task requestToQuit = taskFactory.StartNew(() =>
            {
                Console.WriteLine("Press ENTER to quit.");
                Console.ReadLine();
                _requestToQuit = true;
            }, TaskCreationOptions.LongRunning);

            Task.WaitAll(pipelines.ToArray());
        }
    }
}