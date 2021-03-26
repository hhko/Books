using Serilog;
using Serilog.Formatting.Json;
using System;
using Serilog.Exceptions;
using System.Threading;
using Step_04_MessageIdEnricher;

namespace Step_04_MessageIdEnricherApp
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

                //
                // MessageTemplate 값으로 MessageId을 생성한다.
                //
                .Enrich.WithMessasgeId()
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
            //
            // Information("{키}", 값); 형식으로 구현한다.
            //
            _logger.Information("{Message} Mirero.", message);
        }
    }
}
