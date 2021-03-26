using Serilog;
using Serilog.Formatting.Json;
using System;

namespace Step_02_ConventionalLoggingBySerilog
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Serilog 패키지는 Fluent Intface 패턴으로 구조화를 제공한다.
            //   - Fluent Intface 패턴 : 문맥 중심으로 메서드를 호출한다.
            //   - 구조화 : 출력은 ".WriteTo", 필터는 ".Filter", ... 등으로 구분되어 있다.
            //
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var hello = new Hello(Log.Logger);
            hello.Greeting("Hi");

            // 예외 발생 -> 장애 처리 전략 : 호출자 책임
            try
            {
                hello.Divide(2021, 0);
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
            _logger.Information("{Message} Mirero.", message);
        }

        public void Divide(int x, int y)
        {
            var result = x / y;

            _logger.Information("Divide is {Result}", result);
        }
    }
}
