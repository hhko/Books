using Serilog;
using Serilog.Formatting.Json;
using System;

namespace Step_01_StructuredLogging
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // 출력 형식을 Json으로 지정한다.
            //
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(formatter: new JsonFormatter())
                .CreateLogger();

            var hello = new Hello(Log.Logger);
            hello.Greeting("Hi");

            // 예외 발생 -> 장애 처리 전략 : 호출자 책임
            try
            {
                hello.Divide(2021, 10);
            }
            catch (DivideByZeroException exp)
            {
                Log.Error(exp, "분모 Y값 확인이 필요하다.");
            }

            hello.Greeting("Hello");

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
            _logger.Information("{Message} Mirero. {MyKey}", message, "MyValue");
        }

        public void Divide(int x, int y)
        {
            var result = x / y;

            _logger.Information("Divide is {Result}", result);
        }
    }
}
