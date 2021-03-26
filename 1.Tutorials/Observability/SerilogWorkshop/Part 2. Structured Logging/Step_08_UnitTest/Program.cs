using Serilog;
using Serilog.Formatting.Json;
using System;
using Serilog.Exceptions;
using System.Threading;

namespace Step_08_UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .WriteTo.Console(formatter: new JsonFormatter())
                .CreateLogger();

            Thread.CurrentThread.Name = "Main Thread";

            var hello = new Hello(Log.Logger);
            hello.Greeting("Hi");

            Log.CloseAndFlush();
        }
    }

    public class Hello
    {
        private ILogger _logger;

        public Hello(ILogger logger)
        {
            _logger = logger;
        }

        public void Greeting(string message)
        {
            _logger.Information("{Message} Mirero.", message);
        }
    }
}
