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

        [Fact]
        public void ServiceInstance_CanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            var instance = new FakeService();
            collection.AddSingleton(typeof(IFakeServiceInstance), instance);
            var provider = CreateServiceProvider(collection);

            // Act
            var service = provider.GetService<IFakeServiceInstance>();

            // Assert
            Assert.Same(instance, service);
        }
    }
}
