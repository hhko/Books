using Microsoft.Extensions.DependencyInjection;
using System;

namespace Step_01_DependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            //
            // <서비스, 구현> : 서비스와 구현을 등록한다.
            //                 서비스(인터페이스)와 구현이 분리된 경우
            // IServiceCollection AddTransient<TService, TImplementation>()
            //
            services.AddTransient<IGreeting, Hello>();
            
            //
            // <서비스> : 서비스를 등록한다
            //           서비스(인터페이스)와 구현이 분리되어 있지 않을 경우
            // IServiceCollection AddTransient<TService>()
            //
            services.AddTransient<ConsoleApp>();

            var provider =  services.BuildServiceProvider();

            // 
            // 요청한 객체의 생성자 매개변수를 전달(주입)하여 생성한다.
            //
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
