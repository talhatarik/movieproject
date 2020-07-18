using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using restapi.Models;
using restapi.Types;
using RestSharp;


namespace restapi.Controllers
{
    [Route("taskRun")]
    public class TaskController : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer _timer;

        // GET api
        [HttpGet]
        public Task StartAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {

            var response = ExecuteTask();

        }


        public Task StopAsync(CancellationToken stoppingToken)
        {

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task<IActionResult> ExecuteTask()
        {
            var client = new RestClient($"themoviedb.org"); //API tokenları ve endpointleri burada setlenecek.
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            //Dönen response


            OkResult ok = new OkResult();
            return ok;
        }
    }

}
