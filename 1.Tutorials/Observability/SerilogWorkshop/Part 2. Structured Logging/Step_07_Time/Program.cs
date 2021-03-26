using Serilog;
using Serilog.Formatting.Json;
using System;
using Serilog.Exceptions;
using System.Threading;
using SerilogTimings;

namespace Step_07_Time
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
                //.WriteTo.Console()
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
            // MessageTemplate : messageTemplate + " {Outcome} in {Elapsed:0.0} ms"
            //  Outcome 값
            //   - completed : Information
            //   - abandoned : Warning
            using (var op = Operation.Begin("{ClassName}", nameof(Hello)))
            {
                _logger.Information("{Message} Mirero.", message);

                // 완료
                op.Complete();
                //op.Complete(resultPropertyName: "키", result: 값, destructureObjects: true);
                //
                // 비정상 완료
                //op.Abandon(exp);
                //
                // 취소
                //op.Cancel();
            }
        }
    }
}
