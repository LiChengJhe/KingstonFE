using EAP_Library.DTO;
using EAP_Library.Configs;
using EAP_Simulation.Emulators;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EAP_Simulation
{
    class Program
    {
        private static string _ConnectionString = "Endpoint=sb://kingston.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=77IZBQxwsp4/KiJrVejqgVZzlaZL0QVK6cI72cep94I=";
        private static string _RepositoryQueueName = "Repository";
        private static string _DashboardQueueName = "Dashboard";
        private static QueueHelper _DashboardQueueHelper = new QueueHelper(new QueueConfig { ConnectionString=_ConnectionString, QueueName= _DashboardQueueName });
        private static QueueHelper _RepositoryQueueHelper = new QueueHelper(new QueueConfig { ConnectionString = _ConnectionString, QueueName = _RepositoryQueueName });
        private static Random _Random = new Random();
        static void Main(string[] args)
        {

            SimulateEAP(CreateEqps(30, nameof(EqpType.Cutting)));
            SimulateEAP(CreateEqps(30, nameof(EqpType.Tape)));
            SimulateEAP(CreateEqps(30, nameof(EqpType.Grinding)));
            while (true);
        }
        static List<KeyValuePair<string, string>> CreateEqps(int count, string type)
        {
            List<KeyValuePair< string, string>> eqps = new List<KeyValuePair<string, string>>();
            for (int i=0;i<count ;i++) {
                eqps.Add(new KeyValuePair<string, string>($"{type.Substring(0,3).ToUpper()}{i:000}", type));
            }
            return eqps;
        }
            static void SimulateEAP(List<KeyValuePair<string, string>> eqps)
        {

            Dictionary<string, EqpEmulator> emulator = new Dictionary<string, EqpEmulator>();
            eqps.ForEach((eqp) =>
            {
                switch (eqp.Value) {
                    case nameof(EqpType.Cutting):
                        emulator.Add(eqp.Key, new CuttingEqpEmulator(eqp.Key));
                        break;
                    case nameof(EqpType.Tape):
                        emulator.Add(eqp.Key, new TapeEqpEmulator(eqp.Key));
                        break;
                    case nameof(EqpType.Grinding):
                        emulator.Add(eqp.Key, new GrindingEqpEmulator(eqp.Key));
                        break;
                    default:
                        break;
                }
               
          
                Observable.Interval(TimeSpan.FromSeconds(_Random.Next(60,120)))
                .Select(o => eqp.Key)
                //.ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(async i =>
                {
                    EqpInfoDTO data = emulator[i].GetNewEqpInfo();
                    await _DashboardQueueHelper.SendMessagesAsync(data);
                    await _RepositoryQueueHelper.SendMessagesAsync(data);
                    Console.WriteLine($"Send data of {i}...");

                });
            });


        }




    }
}
