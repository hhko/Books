using Serilog;
using Serilog.Formatting.Json;
using System;
using Serilog.Exceptions;
using System.Threading;

namespace Step_06_DestructureByTransforming
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

                //
                // Destructuring operator을 재정의한다.
                //
                .Destructure.ByTransforming<Person>(x => new { x.Name })
                .WriteTo.Console(formatter: new JsonFormatter())
                .CreateLogger();

            Thread.CurrentThread.Name = "Main Thread";

            Person person = new Person("고길동", 2021);

            //
            // 객체를 Json 형식으로 출력한다.
            //
            Log.Information("Who are {@Person}?", person);

            Log.CloseAndFlush();
        }
    }

    public class Person
    {
        public string Name { get; private set; }

        public int Age { get; private set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
