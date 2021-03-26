using Serilog;
using Serilog.Formatting.Json;
using System;
using Serilog.Exceptions;
using System.Threading;

namespace Step_03_Enrichers
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()

                // 컴퓨터 이름
                .Enrich.WithMachineName()

                // 로그인 사용자 이름
                .Enrich.WithEnvironmentUserName()

                // 어셈블리 이름
                .Enrich.WithAssemblyName()

                // 어셈블리 버전
                .Enrich.WithAssemblyVersion()

                // 프로세스 Id
                .Enrich.WithProcessId()

                // 프로세스 이름
                .Enrich.WithProcessName()

                // 스레드 Id
                .Enrich.WithThreadId()

                // 스레드 이름
                .Enrich.WithThreadName()
                .WriteTo.Console(formatter: new JsonFormatter())
                .CreateLogger();

            // 메인 스레드 이름(기본 값)은 NULL이기 때문에 메인 스레드 이름은 명시적으로 지정해야 한다.
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
