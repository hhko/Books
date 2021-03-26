using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using System;
using Xunit;

namespace Step_08_UnitTest.Tests
{
    public class HelloSpec
    {
        public HelloSpec()
        {
            Log.Logger = new LoggerConfiguration()

                //
                // 메모리에 로그를 출력한다.
                //
                .WriteTo.InMemory()
                .CreateLogger();
        }

        [Fact]
        public void PrintInfoLog()
        {
            // Arrange
            var hello = new Hello(Log.Logger);

            // Act
            hello.Greeting("Hello");

            // Assert
            InMemorySink.Instance.Should()
                .HaveMessage("{Message} Mirero.")
                .Once()
                .WithLevel(LogEventLevel.Information)
                .WithProperty("Message").WithValue("Hello");
        }
    }
}
