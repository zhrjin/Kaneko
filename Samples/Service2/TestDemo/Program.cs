using Kaneko.Server.Orleans.Services;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.Service;

namespace TestDemo
{
    class Program
    {
        static async Task Main()
        {
            await RunMainAsync();
        }
        private static async Task<int> RunMainAsync()
        {
            using var client = await StartClientWithRetries();
            var rd = new Random();
            var nnn = 3;
            while (nnn > 0)
            {
                try
                {
                    Console.WriteLine("Please enter the number of executions");
                    var times = 1;// rd.Next(1000);
                    var topupWatch = new Stopwatch();
                    topupWatch.Start();

                    Enumerable.Range(0, times).Select(x => client.GetGrain<IUtcUID>(1).NewLongID());
                    

                    topupWatch.Stop();

                    Console.WriteLine($"{1000 } completed, time-consuming:{topupWatch.ElapsedMilliseconds}ms");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                nnn--;
            }

            return 1;
        }
        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    var builder = new ClientBuilder()
                   .UseLocalhostClustering()
                   .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "ComConcrete";
                        options.ServiceId = "ComConcrete";
                    })
                   .ConfigureApplicationParts(parts =>
                   {
                       parts.AddApplicationPart(typeof(IScheduleTaskGrain).Assembly).WithReferences();
                   })
                   .ConfigureLogging(logging => logging.AddConsole());
                    client = builder.Build();
                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }
    }
}
