using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Step_01_Helloworld
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddMediatR(typeof(Program));
            var provider = services.BuildServiceProvider();

            //
            // public interface IMediator : ISender, IPublisher
            //
            var sender = provider.GetService<ISender>();
            var pong = await sender.Send(new Ping { Message = "Ping" });

            Console.WriteLine($"{pong.Message}");
        }
    }
}
