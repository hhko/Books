using NLog;
using System;

namespace Step_01_ConventionalLoggingByNLog
{
    class Program
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var hello = new Hello();
            hello.Greeting("Hi");

            // 예외 발생 -> 장애 처리 전략 : 호출자 책임
            try
            {
                hello.Divide(2021, 0);
            }
            catch (DivideByZeroException exp)
            {
                _logger.Error(exp, "분모 Y값 확인이 필요하다.");
            }

            hello.Greeting("Hello");
        }
    }

    public class Hello
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public void Greeting(string message)
        {
            _logger.Info($"{message} Mirero.");
        }

        public void Divide(int x, int y)
        {
            var result = x / y;

            _logger.Info($"Divide is {result}");
        }
    }
}

