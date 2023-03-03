using System.Diagnostics;
using TeaTime;

namespace TeaFiles.Net.Test
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        private int _counter = 0;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            long write = 0;
            long read = 0;

            using var tf = TeaFile<Data>.OpenWrite("acme.tea");
            var stopwatch = new Stopwatch();
            while (!stoppingToken.IsCancellationRequested)
            {
                var x = new Random().NextDouble() * new Random().Next(10000);
                var y = new Random().NextDouble() * new Random().Next(10000);

                var data = new Data()
                {
                    X = x,
                    Y = y,
                    Id = Interlocked.Increment(ref _counter),
                    Time = DateTime.UtcNow
                };

                stopwatch.Start();
                tf.Write(data);


                stopwatch.Stop();

                if (_counter == 100000)
                    break;
            }


            write = stopwatch.ElapsedMilliseconds;

            var watch = new Stopwatch();
            watch.Start();

            using var reader = TeaFile<Data>.OpenRead("acme.tea");


            var range = reader.Items.Where(data => data.X > data.Y)
                .Count();
            watch.Stop();

            Console.WriteLine("Writing took: " + write + "ms");
            Console.WriteLine("reading took: " + watch.ElapsedMilliseconds + "ms");

        }
    }
}