using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Exceptions;

namespace Step_01_MediatorPattern
{
    class Program
    {
        static async Task Main(string[] args)
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
                //.WriteTo.Console(formatter: new JsonFormatter())
                .WriteTo.Console()
                .CreateLogger();

            Thread.CurrentThread.Name = "Main Thread";

            //
            // IoC 컨테이너 생성 및 의존성 등록
            //
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(ILogger), Log.Logger);
            services.AddMediatR(typeof(Program));

            IServiceProvider provider = services.BuildServiceProvider();
            IMediator mediator = provider.GetRequiredService<IMediator>();

            //
            // 간접 호출 : Objects no longer communicate directly with each other, but instead communicate through the mediator. 
            //
            await mediator.Send(new Divide(2021, 100));

            Log.CloseAndFlush();
        }
    }
}
