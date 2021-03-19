using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Step_03_DuplicatedTryAdd
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<IGreeting, Hello>();

            //
            // "TryAdd..." 이미 등록된 "서비스"가 있으면 추가하지 않는다.
            //
            services.TryAddTransient<IGreeting, Hi>();

            services.AddTransient<ConsoleApp>();
            var provider = services.BuildServiceProvider();

            var app = provider.GetRequiredService<ConsoleApp>();
            app.Print();
        }
    }

    public interface IGreeting
    {
        string Message { get; }
    }

    public class Hello : IGreeting
    {
        public string Message => "Hello";
    }

    public class Hi : IGreeting
    {
        public string Message => "Hi";
    }

    public class ConsoleApp
    {
        private readonly IGreeting _greeting;

        public ConsoleApp(IGreeting greeting)
        {
            _greeting = greeting;
        }

        public void Print()
        {
            Console.WriteLine(_greeting.Message);
        }
    }
}