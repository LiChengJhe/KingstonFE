using EAP_Library.Models;
using EAP_Simulation.Emulator;
using EAP_Simulation.ServiceBus;
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
        private const string _ConnectionString = "Endpoint=sb://chengjhe.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=7ibljcV9Y2xAvNCM8ekONHynKGtMZ56jIdpoT6LTgeg=";
        private const string _QueueName = "EAP_Queue";
        private static List<string> _Eqps = new List<string> { "MT001","MT002","MT003" };
        private static EqpType _EqpType = EqpType.Cutting;
        private static QueueHelper _QueueHelper = new QueueHelper(_ConnectionString, _QueueName);
        static void Main(string[] args)
        {

            SimulateEAP(_Eqps);
            ReceiveEqpInfo();
            Console.ReadKey();
        }
        static void ReceiveEqpInfo()
        {
            _QueueHelper.ReceiveMessages<EqpInfo>((i) =>
            {
                Console.WriteLine(JsonSerializer.Serialize(i));
            });
        }
        static void SimulateEAP(List<string> eqps)
        {


            Dictionary<string, CutterEqpEmulator> emulator = new Dictionary<string, CutterEqpEmulator>();
            eqps.ForEach((eqp) =>
            {
                emulator.Add(eqp, new CutterEqpEmulator(eqp));
                Observable.Interval(TimeSpan.FromSeconds(1)).Select(o => eqp)
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(async i =>
                {

                    await _QueueHelper.SendMessagesAsync(emulator[i].GetLatestEqpInfo());

                });
            });


        }




    }
}
