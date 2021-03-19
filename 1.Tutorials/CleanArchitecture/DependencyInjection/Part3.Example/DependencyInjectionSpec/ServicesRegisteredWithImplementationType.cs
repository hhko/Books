using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace DependencyInjectionSpec
{
    //
    // "인터페이스가 있는 구체 클래스" 의존성 주입하기
    //
    public class ServicesRegisteredWithImplementationType
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            // ServiceProvider BuildServiceProvider();
            // ServiceProvider BuildServiceProvider(bool validateScopes);
            // ServiceProvider BuildServiceProvider(ServiceProviderOptions options);
            //      public class ServiceProviderOptions
            //      {
            //          bool ValidateScopes { get; set; }
            //          bool ValidateOnBuild { get; set; }
            //      }
            return services.BuildServiceProvider();
        }

        //
        // 의존성을 등록하여 의존성을 꺼낸다.
        //
        [Fact]
        public void CanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var service = provider.GetService<IFakeService>();

            // Assert
            Assert.NotNull(service);
            Assert.IsType<FakeService>(service);
        }

        //
        // Transient는 개별 인스턴스로 의존성을 주입한다.
        // 
        [Fact]
        public void ReturnDifferentInstances_PerResolution_ForTransientServices()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.IsType<FakeService>(service1);
            Assert.IsType<FakeService>(service2);
            Assert.NotSame(service1, service2);
        }

        //
        // Singleton는 인스턴스 하나로 의존성을 주입한다.
        //
        [Fact]
        public void ReturnSameInstances_PerResolution_ForSingletons()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddSingleton(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.IsType<FakeService>(service1);
            Assert.IsType<FakeService>(service2);
            Assert.Same(service1, service2);
        }
    }
}
